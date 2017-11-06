using UnityEngine;

public class HealthIndicator : MonoBehaviour {
	public GameObject HealthIndicatorType;

	public void Update(int value) {
		foreach (Transform child in transform)
			Destroy(child.gameObject);

		var y = 1;

		for (var i = 0; i < value; i++) {
			var position = new Vector3(0, y, 0);

			var indicatorInstance = Instantiate(HealthIndicatorType, transform);
			indicatorInstance.transform.Translate(position);

			y += 1;
		}
	}
}
