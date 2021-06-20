using System.Collections;
using UnityEngine;

public class Wizard : MonoBehaviour {

	public GameObject projectileSpawnPoint;

	public int health = 100;

	private Rigidbody2D rigidBody;
	private Animator animator;
	private bool isFacingRight;
	public bool isDead { get { return health <= 0; } }
	internal bool ignoreProjectiles;

	void Awake() {
		animator = GetComponent<Animator>();
		rigidBody = GetComponent<Rigidbody2D>();
	}

	public Projectile FireProjectile(Projectile prefab, Vector2 dir) {
		Projectile proj = Instantiate(prefab, projectileSpawnPoint.transform.position, Quaternion.identity);
		Physics2D.IgnoreCollision(GetComponent<Collider2D>(), proj.GetComponent<Collider2D>());
		proj.direction = dir;

		if (animator) {
			animator.SetTrigger("Fire");
		}

		return proj;
	}

	private void Update() {
	}

	void LateUpdate() {
		animator.SetFloat("Speed", rigidBody.velocity.sqrMagnitude);
	}


	public void OnProjectileHit(ProjectileHit hit) {
		if (ignoreProjectiles) {
			hit.ignoreCollision = true;
			return;
		}
		if (!isDead) {
			TakeDamage(hit.projectile.damage);
		} else {
			hit.ignoreCollision = true;
		}
	}

	public void SetFacing(bool faceRight) {
		isFacingRight = faceRight;

		if (!isFacingRight) {
			transform.rotation = Quaternion.Euler(0, 180, 0);
		} else {
			transform.rotation = Quaternion.Euler(0, 0, 0);
		}
	}

	public void TakeDamage(int damage) {
		health -= damage;
		if (health <= 0) {
			Die();
		}
	}

	public void Die() {
		health = 0;
		animator.SetTrigger("Die");
		rigidBody.isKinematic = true;
	}
}
