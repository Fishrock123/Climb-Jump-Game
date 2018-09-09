using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OOBBehavior : MonoBehaviour {

	public GameObject player;

	void OnTriggerExit2D (Collider2D other) {
		if (other.gameObject == player) {
			player.GetComponent<PlayerBehavior>().Reset();
		}
	}
}
