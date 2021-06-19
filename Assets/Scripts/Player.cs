using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	public static Player inst;

	public Projectile magicMissilePrefab;

	private Rigidbody2D rigidBody;
	private Animator animator;
	private Wizard wizard;
	private float lastFireTime;
	private Vector2 movementDirection = Vector2.zero;
	private Vector2 fireDirection = Vector2.zero;
	private Vector2 dashDirection = Vector2.zero;

	private bool isDashing { get { return dashDirection != Vector2.zero; } }

	private Vector2 aimDirection {
		get {
			var screenPos = Camera.main.WorldToScreenPoint(transform.position);
			var cursorPos = Input.mousePosition;
			var dir = (cursorPos - screenPos).normalized;
			var dir2d = new Vector2(dir.x, dir.y);
			return dir2d;
		}
	}

	void Awake() {
		inst = this;
		rigidBody = GetComponent<Rigidbody2D>();
		animator = GetComponent<Animator>();
		wizard = GetComponent<Wizard>();
	}

	void Update() {
		ProcessInput();
	}

	void FixedUpdate() {
		UpdateMovement();
	}

	void LateUpdate() {
		Animate();
	}

	void ProcessInput() {
		if (!isDashing) {
			var moveX = Input.GetAxisRaw("Horizontal");
			var moveY = Input.GetAxisRaw("Vertical");
			movementDirection = new Vector2(moveX, moveY);
			movementDirection.Normalize();
		}

		if (!isDashing && Input.GetButtonDown("Fire1")) {
			fireDirection = aimDirection;
			lastFireTime = Time.realtimeSinceStartup;

			wizard.FireProjectile(magicMissilePrefab, aimDirection);
		}

		if (Input.GetButtonDown("Jump") && !isDashing) {
			Dash();
		}

		if (Time.realtimeSinceStartup - lastFireTime > 1f) {
			lastFireTime = 0;
			fireDirection = Vector2.zero;
		}
	}

	void UpdateMovement() {
		var vars = GameManager.inst.variables;

		Vector2 targetVelocity;
		if (isDashing) {
			targetVelocity = dashDirection * vars.playerSpeed * 3;
		} else {
			targetVelocity = movementDirection * vars.playerSpeed;
		}

		var diff = rigidBody.velocity.Delta(targetVelocity, vars.playerSpeed);
		rigidBody.velocity += diff;
	}

	void Animate() {
		animator.SetFloat("Speed", rigidBody.velocity.sqrMagnitude);
	}

	void Dash() {
		StartCoroutine(DashAsync());
	}

	IEnumerator DashAsync() {
		if (movementDirection.sqrMagnitude > 0) {
			dashDirection = movementDirection;
		} else {
			dashDirection = aimDirection;
		}

		yield return new WaitForSeconds(0.2f);

		dashDirection = Vector2.zero;
	}
}
