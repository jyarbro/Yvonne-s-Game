using UnityEngine;

public class Collectable : MonoBehaviour {
	public GameObject Explosion;

	[Range(1, 5)]
	public float MinSpeed;

	[Range(5, 50)]
	public float MaxSpeed;

	[Range(0, 1)]
	public double Frequency;

	public int CollectedValuePoints;
	public int MissedValuePoints;

	public int CollectedHealthPoints;
	public int MissedHealthPoints;

	float velocity;
	Player player;

	#region Unity methods

	void Start() {
		var rigidBody = gameObject.GetComponent<Rigidbody>();
		rigidBody.velocity = -transform.up * velocity;

		var playerInstance = GameObject.FindGameObjectWithTag("Player");
		player = playerInstance.GetComponent<Player>();
	} 

	void OnTriggerEnter(Collider other) {
		if (other.CompareTag("Player"))
			Collected();

		if (other.CompareTag("Floor"))
			Missed();
	}

	#endregion

	public void SetVelocityBeforeStart(int modifier) {
		var maxModifier = modifier * Random.value;
		var minModifier = modifier * .01F;

		velocity = MinSpeed + minModifier + maxModifier;

		if (velocity > MaxSpeed)
			velocity = MaxSpeed;
	}

	void Collected() {
		player.UpdateScore(CollectedValuePoints, gameObject.transform.position);
		player.UpdateHealth(CollectedHealthPoints);
		Explode();
	}

	void Missed() {
		player.UpdateScore(MissedValuePoints, gameObject.transform.position);
		player.UpdateHealth(MissedHealthPoints);
		Explode();
	}

	void Explode() {
		var explosion = Instantiate(Explosion, transform.position, transform.rotation);
		Destroy(explosion, 1);
		Destroy(gameObject);
	}
}