using System.Collections;
using UnityEngine;

public class Wizard : MonoBehaviour {

	public GameObject projectileSpawnPoint;
	private Animator animator;

	void Awake() {
		animator = GetComponent<Animator>();
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

	public void OnProjectileHit(ProjectileHit hit) {

	}
}
