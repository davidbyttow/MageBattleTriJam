using System.Collections;
using UnityEngine;

public class MagicMissile : MonoBehaviour {

	private Projectile projectile;
	private Rigidbody2D rigidBody;

	private void Awake() {
		projectile = GetComponent<Projectile>();
		rigidBody = GetComponent<Rigidbody2D>();
	}

	private void FixedUpdate() {
		var speed = GameManager.inst.variables.magicMissileSpeed;
		var pos = transform.position + projectile.direction * speed * Time.fixedDeltaTime;
		rigidBody.MovePosition(pos);
	}
}