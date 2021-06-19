using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	public EnvironmentVariables variables;

	private void Awake() {
		Debug.Assert(inst == null, "only one instance allowed per scene");
		inst = this;
	}

	public static GameManager inst { get; private set; }
}
