using System.Collections;
using UnityEngine;

public class Wizard : MonoBehaviour {

	public GameObject projectileSpawnPoint;

	public int health = 100;

	private Rigidbody2D rigidBody;
	private Animator animator;
	private bool isFacingRight;
	internal bool isDead;

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
		if (!isDead) {
			TakeDamage(hit.projectile.damage);
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
		Debug.Log($"HEALTH {health}");
		if (health <= 0) {
			Die();
		}
	}

	public void Die() {
		isDead = true;
		animator.SetTrigger("Die");
		rigidBody.isKinematic = true;
	}
}
