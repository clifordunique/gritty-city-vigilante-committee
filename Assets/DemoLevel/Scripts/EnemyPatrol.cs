using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrol : PhysicsObject {
	public float jumpTakeOffSpeed = 7;
	public float maxSpeed = 7;
	private float test = 23;
	private SpriteRenderer spriteRenderer;
	private Animator animator;
	private bool facingRight = true; 
	public LayerMask enemyMask; 
	Rigidbody2D myBody; 
	Transform myTrans; 
	float myWidth;


	public Collider[] attackHitboxes; // Assigned in Inspector with size of 4, to accomodate left/right arm, and left/right leg

	// Use this for initialization
	void Awake () {
		spriteRenderer = GetComponent<SpriteRenderer>();
		animator = GetComponent<Animator>();
		myTrans = this.transform;
		myBody = this.GetComponent<Rigidbody2D> (); 
		myWidth = GetComponent<SpriteRenderer>().bounds.extents.x;
	}

	private float aiHorizontal()
	{
		//Try to move to right 

		Vector2 lineCastPosRight = myTrans.position - myTrans.right * myWidth;
		Debug.DrawLine (lineCastPosRight, lineCastPosRight + Vector2.down);
		bool willBeGroundedRight = Physics2D.Linecast (lineCastPosRight, lineCastPosRight + Vector2.down, enemyMask);

		Vector2 lineCastPosLeft = myTrans.position + myTrans.right * myWidth;
		Debug.DrawLine (lineCastPosLeft, lineCastPosLeft + Vector2.down);
		bool willBeGroundedLeft = Physics2D.Linecast (lineCastPosLeft, lineCastPosLeft + Vector2.down, enemyMask);
		Debug.Log ("facing Right is" + facingRight); 


		if (facingRight == true) {
			Debug.Log ("entering right case");
			if (!willBeGroundedRight) {
				Debug.Log ("Will not be facing right");
				facingRight = false;//Face the left
				return 0f; 
			} else {
				return -0.1f;
			}
		} else 
		{
			Debug.Log ("entering left case");
			if (!willBeGroundedLeft) {
				Debug.Log ("Will not be facing left");
				facingRight = true;//Face the left
				return 0; 
			} else 
			{
				return 0.1f;
			}
		}
	}

	protected override void ComputeVelocity()
	{
		Vector2 move = Vector2.zero;
		move.x = aiHorizontal ();
//		//Vaguely using tutorial found https://www.youtube.com/watch?v=LPNSh9mwT4w
//		//Draws a line on the Left Side to check if the enemy will be grounded on the left
//		Vector2 lineCastPos = myTrans.position + myTrans.right * myWidth;
//		Debug.DrawLine (lineCastPos, lineCastPos + Vector2.down);
//		bool willBeGrounded = Physics2D.Linecast (lineCastPos, lineCastPos + Vector2.down, enemyMask);
//
//		Vector2 move = Vector2.zero;
//
//		float direc = 1; 
//		//Flips the enemy to face the other direction if it is going to arrive at an edge 
//		if (!willBeGrounded) {
//			Vector3 currRot = myTrans.eulerAngles; 
//			currRot.y += 180; 
//			myTrans.eulerAngles = currRot; 
//			direc = -1; 
//		}
//		move.x = 0.1f * direc; 

//		if (Input.GetButtonDown("Jump") && grounded )
//		{
//			//adds veloctiy along y access
//			velocity.y = jumpTakeOffSpeed;
//			//test = velocity.magnitude;
//			// Debug.Log("getting called when hit jump and grounded " );
//		}
//		//cancels jump when releasing button
//		else if (Input.GetButtonUp("Jump"))
//		{
//			// Debug.Log("cancelling jump");
//			if (velocity.y > 0)
//			{
//				velocity.y = velocity.y * 0.5f;
//
//			}
//		}

		//makes sprite flip back and forth depending on direction moving
		bool flipSprite = (spriteRenderer.flipX ? (move.x > 0.01f) : (move.x < -0.01f));

		if (flipSprite)
		{
			spriteRenderer.flipX = !spriteRenderer.flipX;
		}

		animator.SetBool("grounded", grounded);
		animator.SetFloat("velocityX", Mathf.Abs(velocity.x) / maxSpeed);
		targetVelocity = move * maxSpeed;

	}

	// Checks for keypress and calls launch attack with the corresponding attackHitBox 
	// Needs to be called in update
	// Following tutorial found at https://www.youtube.com/watch?v=mvVM1RB4HXk 
	protected override void CheckAttack()
	{
		//		if (spriteRenderer.flipX == false) {
		//			Debug.Log ("Facing right"); 
		//		} else {
		//			Debug.Log ("Facing left");
		//		}
		if (Input.GetKey (KeyCode.H)) 
		{
			if (spriteRenderer.flipX) //Checks the direction the sprite is facing and selects the correct arm side
			{
				Debug.Log ("left arm attack");
				LaunchAttack (attackHitboxes [0]); 
			} else 
			{
				Debug.Log ("right arm attack");
				LaunchAttack (attackHitboxes[1]);	
			}
		}

		if (Input.GetKey (KeyCode.G)) 
		{
			if (spriteRenderer.flipX) //Checks the direction the sprite is facing and selects the correct kick side
			{
				Debug.Log ("left kick attack");
				LaunchAttack (attackHitboxes[2]); 
			} else 
			{
				Debug.Log ("right kick attack");
				LaunchAttack (attackHitboxes[3]); 
			}

		}
	}

	// Checks if the attack hit box overlaps with a targethitbox and designates the damage amount
	// Following tutorial found at https://www.youtube.com/watch?v=mvVM1RB4HXk 
	private void LaunchAttack(Collider col)
	{
		//		Debug.Log ("Launching attack"); 
		Collider[] cols = Physics.OverlapBox(col.bounds.center,col.bounds.extents,col.transform.rotation, LayerMask.GetMask("Hitbox")); 
		foreach(Collider c in cols)
		{
			if (c.transform.parent.parent == transform) 
			{
				continue; 
			}
			float damage = 0; 
			//			Debug.Log (c.name);
			switch (c.name) 
			{
			case "Head": 
				damage = 30;
				Debug.Log ("Hit Head Damage = 30"); 
				break;
			case "Body":
				damage = 10; 
				Debug.Log ("Hit Body Damage = 10"); 
				break;
			default:
				Debug.Log ("Unable to identify the body part, check the switch statement"); 
				break;
			}

		}
	}
}
