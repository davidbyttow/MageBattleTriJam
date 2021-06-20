using System.Collections;
using UnityEngine;

public class MagicMissile : MonoBehaviour {

	private Projectile projectile;
	private Rigidbody2D rigidBody;
	private AudioSource fireSound;

	private void Awake() {
		projectile = GetComponent<Projectile>();
		rigidBody = GetComponent<Rigidbody2D>();
		fireSound = GetComponent<AudioSource>();
		fireSound.pitch = Random.Range(0.7f, 1.1f);
	}

	private void FixedUpdate() {
		var speed = GameManager.inst.variables.magicMissileSpeed;
		var pos = transform.position + projectile.direction * speed * Time.fixedDeltaTime;
		rigidBody.MovePosition(pos);
	}
}