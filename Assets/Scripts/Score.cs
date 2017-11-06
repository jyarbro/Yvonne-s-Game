using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour {
	public GameObject ScoreTextInstance;
	public GameObject HighScoreTextInstance;
	public GameObject HighScoreSummaryInstance;
	public GameObject OverlayCanvasInstance;

	public PopUpText ScoreUpTextType;
	public PopUpText ScoreDownTextType;

	int score;
	int highestScore;

	HighScoreSummary highScoreSummary;
	Text scoreText;
	Text highScoreText;
	Canvas overlayCanvas;

	#region Unity methods

	void Start() {
		scoreText = ScoreTextInstance.GetComponent<Text>();
		highScoreText = HighScoreTextInstance.GetComponent<Text>();
		highScoreSummary = HighScoreSummaryInstance.GetComponent<HighScoreSummary>();
		overlayCanvas = OverlayCanvasInstance.GetComponent<Canvas>();

		highestScore = highScoreSummary.GetHighScore();
	}

	#endregion

	public void StartPlaying() {
		score = 0;

		highestScore = highScoreSummary.GetHighScore();
		highScoreText.text = highestScore.ToString();
	}

	public void StopPlaying() {
		highScoreSummary.AddScore(score);
	}

	public void UpdateScore(int value, Vector3 location) {
		score += value;

		if (score < 0)
			score = 0;

		scoreText.text = score.ToString();

		if (score > highestScore)
			highScoreText.text = score.ToString();

		PointsPopUp(value, location);
	}

	void PointsPopUp(int value, Vector3 location) {
		PopUpText type;

		if (value >= 0)
			type = ScoreUpTextType;
		else
			type = ScoreDownTextType;

		var textBody = (value >= 0 ? "+" : "") + value;

		var instantiatedObject = Instantiate(type);
		instantiatedObject.transform.SetParent(OverlayCanvasInstance.transform, false);

		instantiatedObject.PopUp(textBody, location, overlayCanvas);
	}
}
