using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {
	public Text HighScoreText;
	public Text ScoreText;
	public Text WaveText;
	public PopUpText ScoreUpText;
	public PopUpText ScoreDownText;
	public GameObject Canvas;
	public GameObject HighScoreController;
	public HealthBar HealthBar;

	public float StartDelay;
	public float SpawnDelay;
	public float WaveDelay;
	public int StartHealth;
	public int Wave;
	public bool Playing;

	public List<PointEntity> PointEntities;

	int health;
	int score;

	CursorLockMode wantedMode;

	#region Unity Methods

	void Start() {
		Cursor.lockState = CursorLockMode.Locked;

		NewGame();
	}

	void Update() {
		if (Input.GetKey(KeyCode.Escape)) {
#if UNITY_EDITOR
			UnityEditor.EditorApplication.isPlaying = false;
#else
			Application.Quit();
#endif
		}

		if (!Playing) {
			if (Input.anyKey || Input.GetMouseButtonDown(0))
				NewGame();
		}
	}

	#endregion

	public void UpdateScore(int value) {
		score += value;

		if (score < 0)
			score = 0;

		UpdateScoreBoard();
	}

	public void UpdateHealth(int value) {
		health += value;

		if (health > StartHealth)
			health = StartHealth;

		HealthBar.UpdateDisplay(health);
		UpdateScoreBoard();

		if (health <= 0)
			EndGame();
	}

	public void PointsPopUp(int points, Vector3 location) {
		PopUpText text;

		if (points >= 0)
			text = ScoreUpText;
		else
			text = ScoreDownText;

		var textBody = (points >= 0 ? "+" : "") + points;

		var instantiatedObject = Instantiate(text);
		instantiatedObject.transform.SetParent(Canvas.transform, false);

		instantiatedObject.PopUp(textBody, location);
	}
	
	IEnumerator SpawnWaves() {
		yield return new WaitForSeconds(StartDelay);

		while (Playing) {
			for (var i = 0; i < Wave; i++) {
				SpawnEntity();
				yield return new WaitForSeconds(SpawnDelay);
			}

			Wave = Mathf.CeilToInt(Wave * 1.25F);
			UpdateScoreBoard();

			yield return new WaitForSeconds(WaveDelay);
		}
	}

	void SpawnEntity() {
		var type = PointEntities[0];

		foreach (var pointEntity in PointEntities) {
			if (Random.value < pointEntity.Frequency)
				type = pointEntity;
		}

		var x = -4 + (8 * Random.value);

		var position = new Vector3(x, 12, 0);
		var rotation = Quaternion.identity;

		Instantiate(type, position, rotation);
	}

	void UpdateScoreBoard() {
		var highScoreController = HighScoreController.GetComponent<HighScoreController>();
		HighScoreText.text = "High Score: " + highScoreController.GetHighScore();
		ScoreText.text = score.ToString();
		WaveText.text = "Wave Size: " + Wave;
	}

	void NewGame() {
		Playing = true;
		HighScoreController.SetActive(false);

		health = StartHealth;
		score = 0;
		Wave = 1;

		UpdateScoreBoard();
		HealthBar.UpdateDisplay(health);

		StartCoroutine(SpawnWaves());
	}

	void EndGame() {
		var highScoreController = HighScoreController.GetComponent<HighScoreController>();

		highScoreController.AddScore(score);
		Playing = false;

		HighScoreController.SetActive(true);
	}
}
