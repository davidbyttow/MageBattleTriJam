using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour {

	public EnvironmentVariables variables;
	public Wizard player;
	public Wizard boss;
	public RectTransform playerHealthBar;
	public RectTransform bossHealthBar;
	public Text respawnCounter;

	private float playerBarWidth;
	private float bossBarWidth;

	private void Awake() {
		Debug.Assert(inst == null, "only one instance allowed per scene");
		inst = this;
	}

	private void Start() {
		playerBarWidth = playerHealthBar.rect.width;
		bossBarWidth = bossHealthBar.rect.width;
	}

	private void Update() {
		UpdateHealthBar(playerHealthBar, playerBarWidth * (player.health / 100.0f));
		UpdateHealthBar(bossHealthBar, bossBarWidth * (boss.health / 100.0f));
		respawnCounter.text = $"{boss.GetComponent<Boss>().respawnCount + 1}";
	}

	private void UpdateHealthBar(RectTransform r, float width) {
		var height = r.rect.height;
		r.sizeDelta = new Vector2(width, height);
	}

	public static GameManager inst { get; private set; }
}
