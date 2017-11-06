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

	Rigidbody rigidBody;
	Player player;

	#region Unity methods

	void Start() {
		rigidBody = GetComponent<Rigidbody>();

		var playerInstance = GameObject.FindGameObjectWithTag("Player");
		player = playerInstance.GetComponent<Player>();
	} 

	#endregion

	void OnTriggerEnter(Collider other) {
		if (other.CompareTag("Player"))
			Collected();

		if (other.CompareTag("Floor"))
			Missed();
	}

	public void AdjustVelocity(int modifier) {
		var maxModifier = modifier * Random.value;
		var minModifier = modifier * .01F;

		var velocity = MinSpeed + minModifier + maxModifier;

		if (velocity > MaxSpeed)
			velocity = MaxSpeed;

		var rigidBody = GetComponent<Rigidbody>();
		rigidBody.velocity = -transform.up * velocity;
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