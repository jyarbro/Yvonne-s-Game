using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.UI;

public class HighScoreController : MonoBehaviour {
	List<int> highScores;

	public void AddScore(int score) {
		if (score <= 0)
			return;

		if (highScores == null)
			Load();

		highScores.Add(score);

		highScores.Sort();
		highScores.Reverse();

		if (highScores.Count > 10)
			highScores = highScores.GetRange(0, 10);

		UpdateText();

		PlayerPrefs.SetString("HighScores", JsonUtility.ToJson(highScores));
		Save();
	}

	public int GetHighScore() {
		if (highScores == null)
			Load();

		return highScores.FirstOrDefault();
	}

	void UpdateText() {
		var text = gameObject.GetComponent<Text>();

		text.text = "Game Over\n\nHigh Scores:";

		for (var i = 0; i < highScores.Count; i++) {
			var score = highScores[i];

			text.text += "\n" + (i + 1) + ") " + score;
		}
	}

	void Save() {
		var bf = new BinaryFormatter();

		using (var file = File.Open(Application.persistentDataPath + "/HighScores.dat", FileMode.OpenOrCreate)) {
			bf.Serialize(file, highScores);
		}
	}

	void Load() {
		if (highScores != null)
			return;

		if (File.Exists(Application.persistentDataPath + "/HighScores.dat")) {
			var bf = new BinaryFormatter();

			using (var file = File.Open(Application.persistentDataPath + "/HighScores.dat", FileMode.Open)) {
				highScores = (List<int>)bf.Deserialize(file);
			}
		}
		else 
			highScores = new List<int>();

		PlayerPrefs.SetString("HighScores", JsonUtility.ToJson(highScores));
	}
}
