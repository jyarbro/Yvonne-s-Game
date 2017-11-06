using UnityEngine;

public class Game : MonoBehaviour {
	public GameObject CollectablesInstance;
	public GameObject PlayerInstance;

	bool playing;

	Collectables collectables;
	Player player;

	#region Unity Methods

	void Start() {
		collectables = CollectablesInstance.GetComponent<Collectables>();
		player = PlayerInstance.GetComponent<Player>();

		StartPlaying();
	}

	void Update() {
		if (Input.GetKey(KeyCode.Escape)) {
#if UNITY_EDITOR
			UnityEditor.EditorApplication.isPlaying = false;
#else
			Application.Quit();
#endif
		}

		if (!playing) {
			if (Input.anyKey || Input.GetMouseButtonDown(0))
				StartPlaying();
		}

		if (!player.IsAlive())
			StopPlaying();
	}

	#endregion

	void StartPlaying() {
		playing = true;

#if !UNITY_EDITOR
		Cursor.lockState = CursorLockMode.Locked;
#endif

		player.StartPlaying();
		collectables.StartPlaying();
	}

	void StopPlaying() {
		playing = false;
		Cursor.lockState = CursorLockMode.None;

		player.StopPlaying();
		collectables.StopPlaying();
	}
}
