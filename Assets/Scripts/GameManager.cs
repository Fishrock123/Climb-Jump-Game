using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

	public int targetFrameRate = 500;

	public string currentScene = "Level 1";

	// Use this for initialization
	void Start () {
		Application.targetFrameRate = targetFrameRate;
		QualitySettings.vSyncCount = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey("escape")) {
			Application.Quit();
		}
	}

	public void SetLevel (string sceneName) {
		SceneManager.UnloadSceneAsync(currentScene);

		currentScene = sceneName;

		SceneManager.LoadSceneAsync(sceneName);

		GameObject.Find("Player").GetComponent<PlayerBehavior>().Reset();
	}
}