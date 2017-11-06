using UnityEngine;

public class HealthBar : MonoBehaviour {
	public GameObject HealthSphere;

	public void UpdateDisplay(int value) {
		foreach (Transform child in transform)
			Destroy(child.gameObject);

		var y = 1;

		for (var i = 0; i < value; i++) {
			var cube = Instantiate(HealthSphere, transform);
			cube.transform.Translate(new Vector3(0, y, 0));

			y += 1;
		}
	}
}
