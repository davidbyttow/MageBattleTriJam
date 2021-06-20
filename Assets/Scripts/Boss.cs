using System.Collections;
using UnityEngine;

static class NavPoints {
	public static int center = 0;
	public static int ul = 1;
	public static int ur = 2;
	public static int ll = 3;
	public static int lr = 4;
}

public class Boss : MonoBehaviour {

	public GameObject[] navPoints;
	public Projectile magicMissile;

	private Wizard wizard;
	private Rigidbody2D rigidBody;
	private Animator animator;
	private int nextNavPoint = 0;
	private bool moving;
	private bool respawning;
	internal int respawnCount = 0;

	void Awake() {
		rigidBody = GetComponent<Rigidbody2D>();
		wizard = GetComponent<Wizard>();
		animator = GetComponent<Animator>();
		wizard.SetFacing(false);
	}

	void Start() {
		StartCoroutine(FireWhenReady());	
	}

	void Update() {
		var dead = wizard.isDead;
		if (dead) {
			moving = false;
			rigidBody.velocity = Vector2.zero;
			if (!respawning) {
				StartCoroutine(StartRespawn());
			}
		} else if (!moving) {
			GotoNextNavPoint();
		}
	}

	void FixedUpdate() {
		var vars = GameManager.inst.variables;

		Vector2 targetVelocity = Vector2.zero;

		if (moving && !wizard.isDead) {
			var targetPosition = GetNavTarget();
			var toTarget = targetPosition - transform.position;
			if (toTarget.magnitude <= 0.2f) {
				moving = false;
				GotoNextNavPoint();
			} else {
				targetVelocity = toTarget.normalized * vars.bossSpeed;
			}
		}

		var diff = rigidBody.velocity.Delta(targetVelocity, vars.playerSpeed);
		rigidBody.velocity += diff;
	}

	IEnumerator StartRespawn() {
		respawning = true;
		yield return new WaitForSeconds(2.5f);
		animator.SetTrigger("Respawn");
		yield return new WaitForSeconds(0.2f);

		Respawn();
		animator.ResetTrigger("Respawn");

		respawning = false;
	}

	void Respawn() {
		respawnCount++;
		wizard.health = 100;
		animator.SetTrigger("Respawn");
		rigidBody.isKinematic = false;
		StartCoroutine(FireWhenReady());
	}

	Vector3 GetNavTarget() {
		return navPoints[nextNavPoint].transform.position;
	}

	void GotoNextNavPoint() {
		moving = true;
		nextNavPoint = Random.Range(0, navPoints.Length - 1);
//		nextNavPoint = (nextNavPoint + 1) % navPoints.Length;
		//StartCoroutine(FireWhenReady());
	}

	IEnumerator FireWhenReady() {
		var startFireTime = 1.5f;
		var t = (1f - (respawnCount / 10.0f));
		var maxTime = Mathf.Clamp(t * startFireTime, 0, startFireTime);
		yield return new WaitForSeconds(maxTime + Random.Range(-0.3f, 0.3f));
		if (wizard.isDead) {
			yield return null;
		} else {
			FireMagicMissile();
			yield return FireWhenReady();
		}
	}

	void FireMagicMissile() {
		var toTarget = Player.inst.transform.position - transform.position;
		var missile = wizard.FireProjectile(magicMissile, toTarget.normalized);
		var scale = Mathf.Lerp(0.3f, 3f, Mathf.Clamp(respawnCount / 10.0f, 0f, 1f));
		Debug.Log(scale);
		missile.transform.localScale = new Vector3(scale, scale, 0);
	}
}