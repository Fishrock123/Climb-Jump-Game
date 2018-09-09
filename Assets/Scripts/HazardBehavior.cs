using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HazardBehavior : MonoBehaviour {

	public GameObject player;

	void Start () {
		player = GameObject.Find("Player");
	}

	void OnTriggerEnter2D (Collider2D other) {
		if (other.gameObject == player) {
			player.GetComponent<PlayerBehavior>().Reset();
		}
	}
}
