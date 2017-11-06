using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PointEntity : MonoBehaviour {
	public GameObject Explosion;

	[Range(5, 50)]
	public float MaxSpeed;

	[Range(0, 1)]
	public double Frequency;

	public int CollectedValuePoints;
	public int MissedValuePoints;

	public int CollectedHealthPoints;
	public int MissedHealthPoints;

	GameController gameController;

	void Start() {
		var gameControllerObject = GameObject.FindWithTag("GameController");

		if (gameControllerObject != null)
			gameController = gameControllerObject.GetComponent<GameController>();
		else
			Debug.Log("GameController not found");

		var maxModifier = gameController.Wave * Random.value;
		var minModifier = 3 + gameController.Wave * .01F;
		var velocity = minModifier + maxModifier;

		if (velocity > MaxSpeed)
			velocity = MaxSpeed;

		var rigidBody = gameObject.GetComponent<Rigidbody>();
		rigidBody.velocity = -transform.up * velocity;
	}

	void Update() {
		if (!gameController.Playing)
			Destroy(gameObject);
	}

	void OnTriggerEnter(Collider other) {
		if (other.CompareTag("Player"))
			Collected();

		if (other.CompareTag("Floor"))
			Missed();
	}

	void Collected() {
		gameController.PointsPopUp(CollectedValuePoints, gameObject.transform.position);

		gameController.UpdateScore(CollectedValuePoints);
		gameController.UpdateHealth(CollectedHealthPoints);

		var explosion = Instantiate(Explosion, transform.position, transform.rotation);
		Destroy(explosion, 1);
		Destroy(gameObject);
	}

	void Missed() {
		gameController.PointsPopUp(MissedValuePoints, gameObject.transform.position);

		gameController.UpdateScore(MissedValuePoints);
		gameController.UpdateHealth(MissedHealthPoints);

		var explosion = Instantiate(Explosion, transform.position, transform.rotation);
		Destroy(explosion, 1);
		Destroy(gameObject);
	}
}