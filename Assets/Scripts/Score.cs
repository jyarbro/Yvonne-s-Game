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
	bool playing;

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

		StartPlaying();
	}

	#endregion

	public void StartPlaying() {
		if (playing)
			return;

		playing = true;
		score = 0;

		highScoreSummary.StartPlaying();
		highestScore = highScoreSummary.GetHighScore();

		scoreText.text = score.ToString();
		highScoreText.text = "High Score: " + highestScore.ToString();
	}

	public void StopPlaying() {
		if (!playing)
			return;

		playing = false;

		highScoreSummary.AddScore(score);
		highScoreSummary.StopPlaying();
	}

	public void UpdateScore(int value, Vector3 location) {
		score += value;

		if (score < 0)
			score = 0;

		scoreText.text = score.ToString();

		if (score > highestScore)
			highScoreText.text = "High Score: " + score.ToString();

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
