using System.Collections;
using UnityEngine;

public class MagicMissile : MonoBehaviour {

	private Projectile projectile;

	private void Awake() {
		projectile = GetComponent<Projectile>();
	}

	private void FixedUpdate() {
		var speed = GameManager.inst.variables.magicMissileSpeed;
		transform.position = transform.position + projectile.direction * speed * Time.fixedDeltaTime;
	}
}