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
	private float direction = -1f;
	public float jumpVelocity = 20f;
	public float climbVelocity = 5f;
	public float jumpingVertVelocity = 5f;
	[SerializeField]
	private float jumpframes = 0;
	public float maxJumpframes = 10;
	public float jumpFramesMod = 2;

	[SerializeField]
	private bool lastFrameMove = false;
	[SerializeField]
	private float lastMoveDir = 0;
	[SerializeField]
	private int afterMoveFrames = 0;
	public int maxAfterMoveFrames = 2;
	public float moveCorrectionMod = 0.5f;

	[SerializeField]
	private bool jumping = false;
	private bool jumpWasDepressed = true;
	private bool didJump = false;

	private bool started = false;

	private Rigidbody2D body;

	void Start () {
		Reset();
	}

	public void Reset () {
		body = GetComponent<Rigidbody2D>();

		groundedFrames = 0;
		jumpframes = 0;
		lastFrameMove = false;
		lastMoveDir = 0;
		afterMoveFrames = 0;

		grounded = false;
		direction = -1;
		jumping = false;
		jumpWasDepressed = true;
		didJump = false;
		started = false;

		body.velocity = Vector2.zero;
		body.constraints = body.constraints & ~RigidbodyConstraints2D.FreezePositionY;

		transform.position = Vector3.zero;
	}
	
	void Update () {
		if (groundedFrames > 0 && !jumping && jumpWasDepressed && Input.GetButton("Jump")) {
			direction = -direction;
			jumping = true;
			didJump = true;

			jumpframes = maxJumpframes;
			
			jumpWasDepressed = false;
		}

		if (Input.GetButtonUp("Jump")) {
			jumpWasDepressed = true;
		}
	}

	void FixedUpdate () {
		if (Input.GetButton("Jump")) { 
			started = true;
		}

		if (groundedFrames > 0) {
			groundedFrames--;
		}
		if (afterMoveFrames > 0) {
			afterMoveFrames--;
		}
		if (jumpframes > 0) {
			jumpframes--;
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
		
		bool thisFrameMove = Mathf.Abs(Input.GetAxisRaw("Vertical")) > 0.001f;

		Vector2 velocity = Vector2.zero;

		if (started && (jumping || !grounded)) {
			float jumpFrameAdjust = 1f + ((jumpframes / maxJumpframes) * jumpFramesMod);
			velocity.x = (direction * jumpVelocity * jumpFrameAdjust) / (groundedFrames + 1);
		}
		velocity.y = (jumping || !grounded ? jumpingVertVelocity : climbVelocity) * Input.GetAxisRaw("Vertical");

		if (lastFrameMove && !thisFrameMove) {
			afterMoveFrames = maxAfterMoveFrames;
		}

		if (afterMoveFrames > 0 && !jumping && grounded) {
			Debug.LogFormat("Doing movement correction, frame: {0}, -dir: {1}", afterMoveFrames, -lastMoveDir);
			velocity.y = climbVelocity * -lastMoveDir * moveCorrectionMod;
		}

		if (started) body.velocity = velocity;

		lastFrameMove = thisFrameMove;
		if (thisFrameMove) lastMoveDir = Mathf.Sign(Input.GetAxisRaw("Vertical"));
	}
}
