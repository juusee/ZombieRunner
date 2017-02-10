using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {
	[HideInInspector] public Transform Player;
	[HideInInspector] public IEnemyState CurrentState;
	[HideInInspector] public RunState RunState;
	[HideInInspector] public JumpState JumpUpState;
	[HideInInspector] public JumpState JumpForwardState;
	[HideInInspector] public JumpState JumpDownState;
	[HideInInspector] public Rigidbody RB;
	[HideInInspector] public Transform EnemyWall;

	[HideInInspector] public float MaxSpeed = PlayerMovement.MaxSpeed - 2f;
	[HideInInspector] public float CurrentFloor;
	[HideInInspector] public float JumpVelocity = 30f;
	[HideInInspector] public float LengthFromEnemyWallToJump = 15f;
	[HideInInspector] public Vector3 HoleTriggerPos;

	Vector3 InitialPosition;

	void Awake() {
		RunState = new RunState (this);
		JumpUpState = new JumpState (this, JumpMode.Up);
		JumpForwardState = new JumpState (this, JumpMode.Forward);
		JumpDownState = new JumpState (this, JumpMode.Down);
		InitialPosition = transform.position;
	}

	void Start () {
		RB = GetComponent<Rigidbody> ();
		CurrentState = RunState;
		EnemyWall = GameObject.FindGameObjectWithTag ("EnemyWall").transform;
	}

	void OnDisable() {
		transform.position = InitialPosition;
	}

	void Update () {
		CurrentState.UpdateState ();
	}

	void OnTriggerEnter(Collider other) {
		if (CurrentState != null) {
			CurrentState.OnTriggerEnter (other);
		}
	}
}

public interface IEnemyState {
	void UpdateState();
	void OnTriggerEnter (Collider other);
	void ToRunState();
	void ToJumpState(JumpMode jumpMode);
}

public class RunState : IEnemyState {
	readonly Enemy Enemy;
	bool FallingBack = false;
	float FallBackTime = 0.5f;
	float FallBackStartTime = 0f;
	float AttackCoolDownTime = 1f;
	float AttackTime = 0f;
	bool Attacked = false;

	public RunState (Enemy enemy) {
		Enemy = enemy;
	}

	public void UpdateState() {
		if (FallingBack) {
			FallBack ();
		} else {
			Run ();
		}
	}

	public void ToRunState() {}

	public void ToJumpState(JumpMode jumpMode) {
		if (jumpMode == JumpMode.Up) {
			Enemy.CurrentState = Enemy.JumpUpState;
		} else if (jumpMode == JumpMode.Forward) {
			Enemy.CurrentState = Enemy.JumpForwardState;
		} else if (jumpMode == JumpMode.Down) {
			Enemy.CurrentState = Enemy.JumpDownState;
		}
	}

	void Run() {
		if (Enemy.RB.velocity.z < Enemy.MaxSpeed) {			
			Enemy.RB.AddForce(new Vector3(0, -2f, 1f), ForceMode.VelocityChange);
		}
		
		if (Enemy.EnemyWall.transform.position.z - Enemy.transform.position.z > Enemy.LengthFromEnemyWallToJump) {
			Attack ();
		}
	}

	void Attack() {
		if (AttackTime == 0f) {
			AttackTime = Time.time;
		}
		if (!Attacked) {
			Attacked = true;
			Enemy.RB.velocity = Vector3.zero;
			Vector3 direction = new Vector3 (0, 0.9f, 1f);
			Enemy.RB.AddForce (direction * Enemy.JumpVelocity, ForceMode.VelocityChange);
		}
		if (Time.time - AttackTime > AttackCoolDownTime) {
			Attacked = false;
			AttackTime = 0f;
		}
	}

	void FallBack() {
		if (FallBackStartTime == 0f) {
			FallBackStartTime = Time.time;
		}
		//Enemy.RB.velocity = Enemy.RB.velocity * 0.9f;
		Enemy.RB.velocity = Vector3.zero;
		if (Time.time - FallBackStartTime > FallBackTime) {
			FallingBack = false;
		}
	}

	void CheckDistanceToEnemyWall() {
		if (Enemy.transform.position.z > Enemy.EnemyWall.transform.position.z) {
			FallingBack = true;
		}
	}
	
	public void OnTriggerEnter (Collider other) {
		if (other.gameObject.CompareTag ("PlatformTop") && Enemy.CurrentFloor != other.transform.parent.position.y) {
			Enemy.CurrentFloor = other.transform.parent.position.y;
		}
		if (other.gameObject.CompareTag ("EnemyWall")) {
			FallingBack = true;
		}
		if (other.gameObject.CompareTag ("HoleTrigger")) {
			Vector3 holeTriggerPos = other.transform.position;
			Enemy.HoleTriggerPos = holeTriggerPos;
			if (PlayerMovement.CurrentFloor > Enemy.CurrentFloor && holeTriggerPos.y > Enemy.CurrentFloor) {
				ToJumpState (JumpMode.Up);
			} else if (PlayerMovement.CurrentFloor >= Enemy.CurrentFloor && holeTriggerPos.y == Enemy.CurrentFloor) {
				ToJumpState (JumpMode.Forward);
			} else if (PlayerMovement.CurrentFloor < Enemy.CurrentFloor && holeTriggerPos.y == Enemy.CurrentFloor) {
				ToJumpState (JumpMode.Down);
			}
		}
	}
}

public enum JumpMode {
	Forward, Up, Down
}

public class JumpState : IEnemyState {

	readonly Enemy Enemy;
	JumpMode JumpMode;
	float JumpTime = 1f;
	float JumpStartTime = 0f;

	public JumpState (Enemy enemy, JumpMode jumpMode) {
		this.Enemy = enemy;
		JumpMode = jumpMode;
	}

	public void UpdateState() {
		Jump ();
		if (JumpStartTime == 0f || Time.time - JumpStartTime > JumpTime) {
			ToRunState ();
		}
	}

	public void ToRunState() {
		Enemy.CurrentState = Enemy.RunState;
		JumpStartTime = 0f;
	}

	public void ToJumpState(JumpMode jumpMode) {}
	public void OnTriggerEnter(Collider other) {}

	void Jump() {
		Enemy.RB.velocity = Vector3.zero;
		Vector3 direction;
		if (JumpStartTime == 0f) {
			JumpStartTime = Time.time;
		}
		if (JumpMode == JumpMode.Up) {
			direction = new Vector3 (0, 1.3f, 0.2f);
			if (Enemy.transform.position.y > Enemy.HoleTriggerPos.y) {
				ToRunState ();
			}
		} else if (JumpMode == JumpMode.Forward) {
			direction = new Vector3 (0, 0.5f, 1f);
			if (Enemy.transform.position.z > Enemy.HoleTriggerPos.z) {
				ToRunState ();
			}
		} else {
			// Down
			direction = new Vector3 (0, -1f, 0.2f);
			if (Enemy.transform.position.y < Enemy.HoleTriggerPos.y) {
				ToRunState ();
			}
		}
		if (Enemy.RB.velocity.z < 40f && Mathf.Abs(Enemy.RB.velocity.y) < 30f) {
			Enemy.RB.AddForce (direction * Enemy.JumpVelocity, ForceMode.VelocityChange);
			//Enemy.RB.velocity = direction * 10f;
		}
	}
}
