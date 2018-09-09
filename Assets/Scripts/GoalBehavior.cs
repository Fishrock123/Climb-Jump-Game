using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalBehavior : MonoBehaviour {

	public string nextLevelName;

	void OnTriggerEnter2D (Collider2D other) {
		if (other.gameObject.CompareTag("Player")) {
			GameObject.Find("Game Manager").GetComponent<GameManager>().SetLevel(nextLevelName);
		}
	}
}
