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
	private Vector2 movementDirection = Vector2.zero;
	private int nextNavPoint = 0;
	private bool moving;

	void Awake() {
		rigidBody = GetComponent<Rigidbody2D>();
		wizard = GetComponent<Wizard>();
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

	Vector3 GetNavTarget() {
		return navPoints[nextNavPoint].transform.position;
	}

	void GotoNextNavPoint() {
		moving = true;
		nextNavPoint = (nextNavPoint + 1) % navPoints.Length;
		//StartCoroutine(FireWhenReady());
	}

	IEnumerator FireWhenReady() {
		yield return new WaitForSeconds(3.0f);
		if (wizard.isDead) {
			yield return null;
		} else {
			FireMagicMissile();
			yield return FireWhenReady();
		}
	}

	void FireMagicMissile() {
		var toTarget = Player.inst.transform.position - transform.position;
		wizard.FireProjectile(magicMissile, toTarget.normalized);
	}
}