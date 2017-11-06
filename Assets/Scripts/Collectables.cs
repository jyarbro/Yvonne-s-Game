using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Collectables : MonoBehaviour {
	public float SpawnDelay;

	public List<Collectable> CollectableTypes;

	bool playing;
	int wave;

	#region Unity methods

	void Start() {
		StartPlaying();
	}

	#endregion

	public void StartPlaying() {
		if (playing)
			return;

		playing = true;

		wave = 1;

		StartCoroutine(SpawnWaves());
	}

	public void StopPlaying() {
		if (!playing)
			return;

		playing = false;

		foreach (Transform child in transform)
			Destroy(child.gameObject);
	}

	IEnumerator SpawnWaves() {
		while (playing) {
			for (var i = 0; i < wave; i++) {
				if (playing) {
					SpawnEntity();

					var delay = Random.value * SpawnDelay;
					yield return new WaitForSeconds(delay);
				}
			}

			wave = Mathf.CeilToInt(wave * 1.25F);
		}
	}

	void SpawnEntity() {
		var type = CollectableTypes.Last();

		foreach (var collectable in CollectableTypes) {
			var randomValue = Random.value;

			if (randomValue < collectable.Frequency) {
				type = collectable;
				break;
			}
		}

		var entity = Instantiate(type, transform);

		// TODO modify range to encompass transform x and scale

		var x = -4 + (8 * Random.value);
		var position = new Vector3(x, 0, 0);

		entity.transform.Translate(position);
		entity.SetVelocityBeforeStart(wave);
	}
}
