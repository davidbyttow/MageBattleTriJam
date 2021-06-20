using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "Data", menuName = "MageReturnal/EnvironmentVariables")]
public class EnvironmentVariables : ScriptableObject {
	public float playerSpeed = 5;
	public float bossSpeed = 4;
	public float playerDashSpeed = 15;
	public float magicMissileSpeed = 10;
}
