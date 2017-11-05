using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrol : PhysicsObject {
//	public float jumpTakeOffSpeed = 7;
//	public float maxSpeed = 7;
//	private float test = 23;
//	private SpriteRenderer spriteRenderer;
//	private Animator animator;
//	private bool facingRight = false; //Always tries to move right at the begining when set to true 
//	public LayerMask enemyMask; 
//	Rigidbody2D myBody; 
//	Transform myTrans; 
//	float myWidth;
//	public bool attacking; 
//	private DummyScript player; // TODO:: Replace with actual player script 
//
//	public Collider[] attackHitboxes; // Assigned in Inspector with size of 4, to accomodate left/right arm, and left/right leg
//
//	// Use this for initialization
//	void Awake () {
//		spriteRenderer = GetComponent<SpriteRenderer>();
//		animator = GetComponent<Animator>();
//		myTrans = this.transform;
//		myBody = this.GetComponent<Rigidbody2D> (); 
//		myWidth = GetComponent<SpriteRenderer>().bounds.extents.x;
//		attacking = false;
//		// player = (DummyScript)FindObjectOfType(typeof(DummyScript));// TODO:: Replace with actual player script
//	}
//
//	// Vaguely using tutorial found https://www.youtube.com/watch?v=LPNSh9mwT4w
//	// Takes the place of Input.GetAxis("Horizontal"), and returns a float < 1 and > -1 
//	private float aiHorizontal()
//	{
//		if (attacking == false) {
//			//Checks if left side of the enemy is grounded 
//			Vector2 lineCastPosLeft = myTrans.position - myTrans.right * myWidth;
//			// Debug.DrawLine (lineCastPosLeft, lineCastPosLeft + Vector2.down);
//			bool willBeGroundedLeft = Physics2D.Linecast (lineCastPosLeft, lineCastPosLeft + Vector2.down, enemyMask);
//
//			//Checks if left side of the enemy is grounded 
//			Vector2 lineCastPosRight = myTrans.position + myTrans.right * myWidth;
//			// Debug.DrawLine (lineCastPosRight, lineCastPosRight + Vector2.down);
//			bool willBeGroundedRight = Physics2D.Linecast (lineCastPosRight, lineCastPosRight + Vector2.down, enemyMask);
//
//			if (myTrans.position.x < player.myTrans.position.x) { //If the enemy is currently facing right continue moving right unless it will not be grounded
//				if (!willBeGroundedRight) {
//					// Debug.Log ("Will not be grounded on right, stopping ");
//					return 0f; 
//				} else {
//					return 0.1f;
//				}
//			} else { //If the enemy is currently facing left continue moving left unless it will not be grounded
//				if (!willBeGroundedLeft) {
//					// Debug.Log ("Will not be grounded on left, stopping");
//					return 0; 
//				} else {
//					return -0.1f;
//				}
//			}
//
//			//This script will make enemy just flip back and forth. 
//			//			if (facingRight == true) { //If the enemy is currently facing right continue moving right unless it will not be grounded
//			//				// Debug.Log ("entering right case");
//			//				if (!willBeGroundedRight) {
//			//					// Debug.Log ("Will not be grounded on right");
//			//					facingRight = false;//Face the left
//			//					return 0f; 
//			//				} else {
//			//					return 0.1f;
//			//				}
//			//			} else { //If the enemy is currently facing left continue moving left unless it will not be grounded
//			//				// Debug.Log ("entering left case");
//			//				if (!willBeGroundedLeft) {
//			//					// Debug.Log ("Will not be grounded on left");
//			//					facingRight = true;//Face the left
//			//					return 0; 
//			//				} else {
//			//					return -0.1f;
//			//				}
//			//			}
//		} else 
//		{
//			return 0;
//		}
//	}
//
//	protected override void ComputeVelocity()
//	{
//		Vector2 move = Vector2.zero;
//		move.x = aiHorizontal ();
//		//makes sprite flip back and forth depending on direction moving
//		bool flipSprite = (spriteRenderer.flipX ? (move.x > 0.01f) : (move.x < -0.01f));
//		if (flipSprite)
//		{
//			spriteRenderer.flipX = !spriteRenderer.flipX;
//		}
//		animator.SetBool("grounded", grounded);
//		animator.SetFloat("velocityX", Mathf.Abs(velocity.x) / maxSpeed);
//		targetVelocity = move * maxSpeed;
//	}
//		
//	private bool aiAttack()
//	{
//		
//		Debug.Log ("Running aiAttack");
//		foreach (Collider col in attackHitboxes) 
//		{
//			Collider[] cols = Physics.OverlapBox(col.bounds.center,col.bounds.extents,col.transform.rotation, LayerMask.GetMask("Hitbox"));
//			if (cols.Length > 0) 
//			{
//				return true;
//			}
//		}
//		Debug.Log ("Didn't find anything to attack");
//		return false; 
//	}
//
//	// Checks for keypress and calls launch attack with the corresponding attackHitBox 
//	// Needs to be called in update
//	// Following tutorial found at https://www.youtube.com/watch?v=mvVM1RB4HXk 
//	protected override void CheckAttack()
//	{
//		if (aiAttack()) 
//		{
//			attacking = true;
//			if (spriteRenderer.flipX) //Checks the direction the sprite is facing and selects the correct arm side
//			{
//				Debug.Log ("left enemy arm attack");
//				LaunchAttack (attackHitboxes [0]); 
//			} else 
//			{
//				Debug.Log ("right enemy arm attack");
//				LaunchAttack (attackHitboxes[1]);	
//			}
//		}
//
//		if (aiAttack()) 
//		{
//			if (spriteRenderer.flipX) //Checks the direction the sprite is facing and selects the correct kick side
//			{
//				Debug.Log ("left enemy kick attack");
//				LaunchAttack (attackHitboxes[2]); 
//			} else 
//			{
//				Debug.Log ("right enemy kick attack");
//				LaunchAttack (attackHitboxes[3]); 
//			}
//
//		}
//	}
//
//	// Checks if the attack hit box overlaps with a targethitbox and designates the damage amount
//	// Following tutorial found at https://www.youtube.com/watch?v=mvVM1RB4HXk 
//	private void LaunchAttack(Collider col)
//	{
//		animator.SetBool("attacking", attacking);
//		//		Debug.Log ("Launching attack"); 
//		Collider[] cols = Physics.OverlapBox(col.bounds.center,col.bounds.extents,col.transform.rotation, LayerMask.GetMask("Hitbox")); 
//		foreach(Collider c in cols)
//		{
//			if (c.transform.parent.parent == transform) 
//			{
//				continue; 
//			}
//			float damage = 0; 
//			//			Debug.Log (c.name);
//			switch (c.name) 
//			{
//			case "Head": 
//				damage = 30;
//				Debug.Log ("enemy Hit Head Damage = 30"); 
//				break;
//			case "Body":
//				damage = 10; 
//				Debug.Log ("enemy Hit Body Damage = 10"); 
//				break;
//			default:
//				Debug.Log ("enemy Unable to identify the body part, check the switch statement"); 
//				break;
//			}
//
//		}
//	}
}
