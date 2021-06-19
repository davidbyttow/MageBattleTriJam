using UnityEngine;
using System.Collections;

public static class VectorExtensions {
	public static Vector2 Delta(this Vector2 v, Vector2 target, float max) {
		var diff = target - v;
		if (diff.sqrMagnitude > max * max) {
			diff = diff.normalized * max;
		}
		return diff;
	}
}
