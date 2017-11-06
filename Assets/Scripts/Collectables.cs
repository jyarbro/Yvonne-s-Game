using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Collectables : MonoBehaviour {
	public float SpawnDelay;

	public List<Collectable> CollectableTypes;

	bool playing;
	int wave;

	public void StartPlaying() {
		playing = true;

		StartCoroutine(SpawnWaves());
	}

	public void StopPlaying() {
		playing = false;

		foreach (Transform child in transform)
			Destroy(child.gameObject);
	}

	IEnumerator SpawnWaves() {
		while (playing) {
			for (var i = 0; i < wave; i++) {
				SpawnEntity();

				var delay = Random.value * SpawnDelay;
				yield return new WaitForSeconds(delay);
			}

			wave = Mathf.CeilToInt(wave * 1.25F);
		}
	}

	void SpawnEntity() {
		var type = CollectableTypes.Last();

		foreach (var collectable in CollectableTypes) {
			if (Random.value < collectable.Frequency)
				type = collectable;
		}

		var entity = Instantiate(type, transform);

		// TODO modify range to encompass transform x and scale

		var x = -4 + (8 * Random.value);
		var position = new Vector3(x, 0, 0);

		entity.transform.Translate(position);
		entity.AdjustVelocity(wave);
	}
}
