using System;
using UnityEngine;

public class Player : MonoBehaviour {
	public int StartHealth;
	public float MaxRotation;

	public GameObject LeftWall;
	public GameObject RightWall;
	public GameObject ScoreInstance;
	public GameObject HealthIndicatorInstance;

	bool playing;
	int health;
	float leftMoveLimit;
	float rightMoveLimit;

	HealthIndicator healthIndicator;
	Score score;

	#region Unity Methods

	void Start() {
		leftMoveLimit = LeftWall.transform.position.x + (LeftWall.transform.localScale.x / 2);
		rightMoveLimit = RightWall.transform.position.x - (RightWall.transform.localScale.x / 2);

		score = ScoreInstance.GetComponent<Score>();

		health = StartHealth;
		healthIndicator = HealthIndicatorInstance.GetComponent<HealthIndicator>();
		healthIndicator.UpdateHealth(health);

		StartPlaying();
	}

	void FixedUpdate() {
		if (!playing)
			return;

		CheckInput();
	}

	#endregion

	public void StartPlaying() {
		if (playing)
			return;

		if (StartHealth == 0)
			throw new Exception("StartHealth must be greater than 0");

		health = StartHealth;
		playing = true;

		if (healthIndicator != null)
			healthIndicator.UpdateHealth(health);

		if (score != null)
			score.StartPlaying();
	}

	public void StopPlaying() {
		if (!playing)
			return;

		playing = false;

		if (score != null)
			score.StopPlaying();
	}

	public bool IsAlive() {
		return health > 0;
	}

	public void UpdateScore(int value, Vector3 location) {
		score.UpdateScore(value, location);
	}

	public void UpdateHealth(int value) {
		health += value;

		if (health > StartHealth)
			health = StartHealth;

		if (health < 0)
			health = 0;

		healthIndicator.UpdateHealth(health);
	}

	void CheckInput() {
		var movement = Input.GetAxis("Mouse X");
		Slide(movement);
	}

	/// <summary>
	/// Slides the gameObject left and right. Useful for direct control.
	/// </summary>
	void Slide(float movement) {
		movement = movement * .35F;

		var halfWidth = transform.localScale.x / 2;
		var slideTarget = transform.position.x + movement;
		var slideTargetLeft = slideTarget - halfWidth;
		var slideTargetRight = slideTarget + halfWidth;

		if (slideTargetLeft <= leftMoveLimit)
			movement = leftMoveLimit - transform.position.x + halfWidth;
		else if (slideTargetRight >= rightMoveLimit)
			movement = rightMoveLimit - transform.position.x - halfWidth;

		transform.Translate(new Vector3(movement, 0, 0));
	}

	/// <summary>
	/// Rotates the gameObject around a fixed point. Useful for indirect control.
	/// </summary>
	void Rotate(float movement) {
		var currentRotation = transform.rotation.eulerAngles.z;
		var rotationTarget = currentRotation + movement;

		var outsideRight = currentRotation < 180
						&& rotationTarget > MaxRotation;

		var outsideLeft = currentRotation > 180
						&& rotationTarget < 360 - MaxRotation;

		if (outsideLeft)
			movement = 360 - MaxRotation - transform.rotation.eulerAngles.z;
		else if (outsideRight)
			movement = MaxRotation - transform.rotation.eulerAngles.z;

		var rotation = new Vector3(0, 0, movement);

		transform.Rotate(rotation);
	}
}
