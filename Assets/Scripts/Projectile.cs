using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections;

public struct ProjectileHit {
	public Projectile projectile;
	public Collider2D collision;
	public bool ignoreCollision;
}

public class Projectile : MonoBehaviour {

	public int damage = 10;

	internal Rigidbody2D rigidBody;
	internal Vector3 direction;
	internal Transform target;

	private SpriteRenderer spriteRenderer;

	void Awake() {
		rigidBody = GetComponent<Rigidbody2D>();
		spriteRenderer = GetComponent<SpriteRenderer>();
	}

	void Update() {
	}


	private void OnTriggerEnter2D(Collider2D other) {
		var hit = new ProjectileHit();
		hit.projectile = this;
		hit.collision = other;

		var wizard = other.GetComponent<Wizard>();
		if (wizard) {
			Debug.Log("Wizard!");
			wizard.OnProjectileHit(hit);
		}

		hit.ignoreCollision = other.CompareTag("Barrier") || other.CompareTag("NavPoint") || other.GetComponent<Projectile>();

		if (!hit.ignoreCollision) {
			StartCoroutine(StopProjectile());
		}
	}

	IEnumerator StopProjectile() {
		GetComponent<Collider2D>().isTrigger = true;
		rigidBody.velocity = Vector2.zero;
		rigidBody.isKinematic = true;
		spriteRenderer.forceRenderingOff = true;

		var systems = GetComponentsInChildren<ParticleSystem>();
		foreach (var ps in systems) {
			ps.Stop(true, ParticleSystemStopBehavior.StopEmitting);
		}

		var trails = GetComponentsInChildren<TrailRenderer>();
		foreach (var trail in trails) {
			trail.emitting = false;
			trail.time = 0.2f;
		}

		var done = false;
		while (!done) {
			var hasParticles = false;
			foreach (var ps in systems) {
				if (ps.particleCount > 0) {
					hasParticles = true;
					break;
				}
			}
			done = !hasParticles;
			yield return null;
		}

		Destroy(gameObject);
		yield return null;
	}
}
