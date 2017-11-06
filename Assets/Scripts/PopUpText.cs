using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PopUpText : MonoBehaviour {
	public float Duration;

	float durationInverse;
	float timer;

	public void PopUp(string textBody, Vector3 position, Canvas canvas) {
		var text = GetComponent<Text>();
		text.text = textBody;

		var popupRectTransform = gameObject.GetComponent<RectTransform>();
		var canvasRectTransform = canvas.GetComponent<RectTransform>();

		var screenOffset = new Vector2(canvasRectTransform.sizeDelta.x / 2f, canvasRectTransform.sizeDelta.y / 2f);

		// Get the position on the canvas
		var viewportPosition = (Vector2)Camera.main.WorldToViewportPoint(position);
		var proportionalPosition = new Vector2(viewportPosition.x * canvasRectTransform.sizeDelta.x, viewportPosition.y * canvasRectTransform.sizeDelta.y);

		// Set the position and remove the screen offset
		popupRectTransform.localPosition = proportionalPosition - screenOffset;

		durationInverse = 1f / (Duration != 0f ? Duration : 1f);
		timer = 0f;

		StartCoroutine(FadeAndDestroy());
	}

	IEnumerator FadeAndDestroy() {
		var rectTransform = GetComponent<RectTransform>();
		var text = GetComponent<Text>();
		text.color = new Color(text.color.r, text.color.g, text.color.b, 1);

		while (text.color.a > 0.0f) {
			var alpha = Mathf.Lerp(1f, 0f, timer * durationInverse);
			timer += Time.deltaTime;

			text.color = new Color(text.color.r, text.color.g, text.color.b, alpha);

			rectTransform.Translate(new Vector3(0, .2F, 0));

			yield return null;
		}

		Destroy(gameObject);
	}
}
