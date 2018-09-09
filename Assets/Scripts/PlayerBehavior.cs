using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehavior : MonoBehaviour {

	public BoxCollider2D groundedDetector;

	public ContactFilter2D detectorContactFilter;

	[SerializeField]
	private int groundedFrames = 0;
	public int maxGroundedFrames = 3;
	[SerializeField]
	private bool grounded = false;

	[SerializeField]
	private int direction = -1;
	public float jumpVelocity = 20f;
	public float climbVelocity = 5f;
	public float jumpingVertVelocity = 5f;

	[SerializeField]
	private bool jumping = false;
	private bool didJump = false;

	private Rigidbody2D body;

	void Start () {
		body = GetComponent<Rigidbody2D>();
	}

	public void Reset () {
		groundedFrames = 0;
		grounded = false;
		direction = -1;
		jumping = false;
		didJump = false;

		transform.position = Vector3.zero;
	}
	
	void Update () {
		if (groundedFrames > 0 && !jumping && Input.GetButton("Jump")) {
			direction = -direction;
			jumping = true;
			didJump = true;
		}
	}

	void FixedUpdate () {
		if (groundedFrames > 0) {
			groundedFrames--;
		}

		if (!didJump && Physics2D.BoxCast(
				transform.position, 
				groundedDetector.size, 
				0f, 
				Vector2.zero, 
				Mathf.Infinity, 
				detectorContactFilter.layerMask)) {
			groundedFrames = maxGroundedFrames;
			jumping = false;
			grounded = true;
		} else {
			didJump = false;
			grounded = false;
		}

		Vector2 velocity = Vector2.zero;

		velocity.x = jumping || !grounded ? direction * jumpVelocity : 0f;

		velocity.y = (jumping || !grounded ? jumpingVertVelocity : climbVelocity) * Input.GetAxis("Vertical");

		body.velocity = velocity;
	}
}
