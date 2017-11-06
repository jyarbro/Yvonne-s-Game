using UnityEngine;

public class PlayerInput : MonoBehaviour {
	public GameController GameController;
	public GameObject LeftWall;
	public GameObject RightWall;

	public float MaxRotation;

	float maxLeft;
	float maxRight;

	void Start () {
		maxLeft = LeftWall.transform.position.x + (LeftWall.transform.localScale.x / 2);
		maxRight = RightWall.transform.position.x - (RightWall.transform.localScale.x / 2);
	}
	
	void FixedUpdate() {
		if (!GameController.Playing)
			return;

		var movement = Input.GetAxis("Mouse X");

		Slide(movement);
	}

	void Slide(float movement) {
		movement = movement * .35F;

		var halfWidth = transform.localScale.x / 2;
		var slideTarget = transform.position.x + movement;
		var slideTargetLeft = slideTarget - halfWidth;
		var slideTargetRight = slideTarget + halfWidth;

		if (slideTargetLeft <= maxLeft)
			movement = maxLeft - transform.position.x + halfWidth;
		else if (slideTargetRight >= maxRight)
			movement = maxRight - transform.position.x - halfWidth;

		transform.Translate(new Vector3(movement, 0, 0));
	}

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
