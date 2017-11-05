﻿﻿using System.Collections; 
using System.Collections.Generic; 
using UnityEngine; 
using Prime31; 

public class EnemyPatrol : MonoBehaviour { 
	public float jumpTakeOffSpeed = 7; 
	public float maxSpeed = 7; 
	private SpriteRenderer spriteRenderer; 
	private Animator animator; 
	private bool facingRight = false; //Always tries to move right at the begining when set to true  
	public LayerMask enemyMask;  
	Rigidbody2D myBody;  
	Transform myTrans;  
	float myWidth; 
	public bool attacking;  
	// private DummyScript player; // TODO:: Replace with actual player script  

	public Collider[] attackHitboxes; // Assigned in Inspector with size of 4, to accomodate left/right arm, and left/right leg 

	//tells what our collision state is 
	public CharacterController2D.CharacterCollisionState2D flags; 
	public float walkSpeed = 6.0f; 
	public float jumpSpeed = 10f; 
	public float gravity = 20.0f; 
	public float doubleJumpSpeed = 16f; 
	public float wallJumpXAmount = .25f; 
	public float wallJumpYAmount = 1.5f; 
	public float wallRunAmount = 2f; 
	public float slopeSlideSpeed = 4f; 
	public float glideAmount = 2f; 
	public float glideTime = 2f; 
	public float crouchWalkSpeed = 3.0f; 
	public float powerJumpSpeed = 10.0f; 
	public float stompSpeed = 4.0f; 
	public LayerMask layerMask; 

	//player ability toggle 
	public bool canWallRun = true; 
	public bool canRunAfterWallJump = true; 
	public bool canDoubleJump = true; 
	public bool canWallJump = true; 
	public bool canGlide = true; 
	public bool canPowerJump = true; 
	public bool canStomp = true; 
	public bool canChangeLevel = true; 
	public bool canShoot = true; 

	//player states 
	public bool isGrounded; 
	public bool isJumping; 
	public bool isFacingRight; 
	public bool doubleJumped; 
	public bool wallJumped; 
	public bool iswallRunning; 
	public bool isSlopeSliding; 
	public bool isGliding; 
	public bool isCrouchWalking; 
	public bool isCrouched; 
	public bool isPowerJumping; 
	public bool isStomping; 
	public bool isChangingLevels; 
	//public bool isWallSliding 
	//public bool isDashing 
	//public bool isAirDashing 
	//public bool isShooting  //for animation 

	//some stuff to make the level disapear for the "gimmick" 
	//public bool isGone = false; 
	public BoxCollider2D[] boxTest; 
	public BoxCollider2D[] CityColliderBox; 
	public GameObject[] box;  
	public GameObject[] cityGameObject; 
	public SpriteRenderer[] boxy; 
	public SpriteRenderer[] citySprites; 
	//public BoxCollider2D 

	public GameObject shot; 
	public Transform shotSpawn; 

	public float fireRate = 10; 
	private float _nextFire; 

	//private variables 
	private Vector3 _moveDirection = Vector3.zero; 
	private CharacterController2D _CharacterController; 
	private bool _lastJumpedWasLeft; 
	private float _slopeAngle; 
	private Vector3 _slopeGradient = Vector3.zero; 
	private bool _startGlide; 
	private float _currentGlideTime; 
	private BoxCollider2D _boxCollider; 
	private Vector2 _originalBoxColliderSize; 
	private Vector3 _frontTopCorner; 
	private Vector3 _backTopCorner; 
	private Vector3 _frontBottomCorner; 
	private Vector3 _backBottomCorner; 

	// Use this for initialization 
	void Start () 
	{ 
		//grabs the character attached to the script 
		_CharacterController = GetComponent<CharacterController2D>(); 
		_currentGlideTime = glideTime; 
		_boxCollider = GetComponent<BoxCollider2D>(); 
		_originalBoxColliderSize = _boxCollider.size; 
		box = GameObject.FindGameObjectsWithTag("level"); 
		cityGameObject = GameObject.FindGameObjectsWithTag("city"); 
		animator = GetComponent<Animator>(); 
		CityColliderBox = new BoxCollider2D[cityGameObject.Length]; 
		citySprites = new SpriteRenderer[cityGameObject.Length]; 

		boxy = new SpriteRenderer[box.Length]; 

		boxTest = new BoxCollider2D[box.Length]; 

		//stores data for "level" tagged data 
		for (var i = 0; i < box.Length; i += 1) 
		{ 
			boxTest[i] = box[i].GetComponent<BoxCollider2D>(); 
			boxy[i] = box[i].GetComponent<SpriteRenderer>(); 

		} 

		//stores all the data for "city" tagged items 
		for (var i = 0; i < cityGameObject.Length; i += 1) 
		{ 
			//Debug.Log(cityGameObject.Length); 

			CityColliderBox[i] = cityGameObject[i].GetComponent<BoxCollider2D>(); 
			if(cityGameObject[i].GetComponent<SpriteRenderer>() != null) 
			{ 
				citySprites[i] = cityGameObject[i].GetComponent<SpriteRenderer>(); 
			} 


		} 

		//makes it start with just objects with "level" tag 
		for (var i = 0; i < cityGameObject.Length; i += 1) 
		{ 

			citySprites[i].enabled = false; 

		} 
		for (var i = 0; i < CityColliderBox.Length; i++) 
		{ 

			if (CityColliderBox[i] != null) 
			{ 
				CityColliderBox[i].enabled = false; 
			} 
		} 

		isChangingLevels = false; 
		canShoot = true; 
	}//end of start 

	// Update is called once per frame 
	void Update() { 

		//shoooting mechanics 
		//needs to change to be one bullet at a time 
		//probably by coroutine. and in bool isShooting 
		if (canShoot) 
		{ 
			if (Input.GetButton("Fire3")) 
			{ 
				//  
				//Debug.Log("pew pew"); 
				//_nextFire = Time.deltaTime + fireRate; 

				Instantiate(shot, shotSpawn.position, shotSpawn.rotation); 
			} 
		} 


		//makes things disapear or change up the level 
		//so far this make it where you can just make one 
		//sprite disapear. can be exapanded to the whole level 
		// isChangingLevels is there to tell the animator to do it's thing 
		//probably put code in to save position of character or not... would 
		//be cool to have a free fall on change. 
		if (canChangeLevel) 
		{ 
			//changes to the alternate level 
			//should program to return state with the same button 
			// put in a cooroutine to do this 
			if (Input.GetButton("Fire1") && !isChangingLevels) 
			{ 
				for (int i = 0; i < box.Length ; i++) 
				{ 
					boxTest[i].enabled = false; 
					boxy[i].enabled = false; 
				} 
				for ( var i = 0; i < cityGameObject.Length; i += 1) 
				{ 

					citySprites[i].enabled = true; 

				} 
				for (var i = 0; i < CityColliderBox.Length; i++) 
				{ 

					if (CityColliderBox[i] != null) 
					{ 
						CityColliderBox[i].enabled = true; 
					} 

				} 
				isChangingLevels = true; 

			} 

			//this changes it back to original level 
			else if (Input.GetButton("Fire2") && isChangingLevels) 
			{ 
				for (var i = 0; i < cityGameObject.Length; i += 1) 
				{ 

					citySprites[i].enabled = false; 

				} 
				for (var i = 0; i < CityColliderBox.Length; i++) 
				{ 
					if (CityColliderBox[i] != null) 
					{ 
						CityColliderBox[i].enabled = false; 
					} 
				} 

				for (var i = 0; i < box.Length; i++) 
				{ 

					boxTest[i].enabled = true; 
					boxy[i].enabled = true; 

				} 
				isChangingLevels = false; 
			} 
		} 
		if (!wallJumped) 
		{ 
			_moveDirection.x = aiHorizontal(); 
			_moveDirection.x *= walkSpeed; 
		} 

		//next few lines detect the slope of the ground and determines if they will slide 
		//down the hill or not 
		RaycastHit2D hit = Physics2D.Raycast(transform.position, -Vector3.up, 2f, layerMask); 

		if (hit) 
		{ 
			_slopeAngle = Vector2.Angle(hit.normal, Vector2.up); 
			_slopeGradient = hit.normal; 

			if(_slopeAngle > _CharacterController.slopeLimit) 
			{ 
				isSlopeSliding = true; 

			} 
			else 
			{ 
				isSlopeSliding = false; 
			} 
		} 
		//handles jumping 
		//player on the ground 
		if (isGrounded) 
		{ 
			_moveDirection.y = 0; 
			isJumping = false; 
			doubleJumped = false; 
			isStomping = false; 
			_currentGlideTime = glideTime; 
			//testing disapearing objects 

			if (_moveDirection.x < 0) 
			{ 
				//rotates player 

				transform.eulerAngles = new Vector3(0, 180, 0); 
				isFacingRight = false; 
			} 
			else if(_moveDirection.x > 0) 
			{ 
				transform.eulerAngles = new Vector3(0, 0, 0); 
				isFacingRight = true; 
			} 

			if (isSlopeSliding) 
			{ 
				_moveDirection = new Vector3(_slopeGradient.x * slopeSlideSpeed, -_slopeGradient.y * slopeSlideSpeed, 0f); 
			} 
			if (Input.GetButtonDown("Jump")) 
			{ 
				if(canPowerJump && isCrouched) 
				{ 
					_moveDirection.y = jumpSpeed + powerJumpSpeed; 
					//isPowerJumping = true; 
					StartCoroutine("PowerJumpTimer");     
					//mebbe add in a timer to wait a bit. 
				} 
				else 
				{ 
					_moveDirection.y = jumpSpeed; 
					isJumping = true; 
				} 


				//puts it here to be able to wall run 
				iswallRunning = true; 
			} 
		} 
		//player is in the air 
		else 
		{ 
			if (Input.GetButtonUp("Jump")) 
			{ 
				if(_moveDirection.y > 0) 
				{ 
					_moveDirection.y = _moveDirection.y * .5f; 
				} 
			} 
			//sets up double jump 
			if (Input.GetButtonDown("Jump")) 
			{//make sure you ahve the ability to do so 
				if (canDoubleJump) 
				{ 
					//make sure you ahven't already 
					if (!doubleJumped) 
					{ 
						_moveDirection.y = doubleJumpSpeed; 
						doubleJumped = true; 
					} 
				} 
			} 
		} 

		//gravity calculations 
		//glide will create it's own gravity 
		//to create the effect 
		//makes it to taht you can glide immediately 
		if (canGlide && Input.GetAxis("Vertical") > 0.5f && _CharacterController.velocity.y < 0.2f) 
		{ 
			if (_currentGlideTime > 0)//makes glide not be infinite 
			{ 


				isGliding = true; 
				if (_startGlide) 
				{ 
					_moveDirection.y = 0; 
					_startGlide = false; 
				} 
				_moveDirection.y -= glideAmount * Time.deltaTime; 
				_currentGlideTime -= glideAmount - Time.deltaTime; 
			} 
			else 
			{ 
				isGliding = false; 

				_moveDirection.y -= gravity * Time.deltaTime; 
			} 
		} 
		else if (canStomp && isCrouched && !isPowerJumping) 
		{ 
			_moveDirection.y -= gravity * Time.deltaTime + stompSpeed; 
			isStomping = true; 
		} 
		else 
		{ //when stop gliding 
			isGliding = false; 
			_startGlide = true; 

			_moveDirection.y -= gravity * Time.deltaTime; 
		} 

		_CharacterController.move(_moveDirection * Time.deltaTime); 

		//checks what is being collided with our character 
		flags = _CharacterController.collisionState; 

		isGrounded = flags.below; 

		//crouched and crouch f 
		_frontTopCorner = new Vector3(transform.position.x + _boxCollider.size.x / 2, transform.position.y + _boxCollider.size.y / 2, 0); 
		_backTopCorner = new Vector3(transform.position.x - _boxCollider.size.x / 2, transform.position.y + _boxCollider.size.y / 2, 0); 
		RaycastHit2D hitFrontCeiling = Physics2D.Raycast(_frontTopCorner, Vector2.up, 2f, layerMask); 
		RaycastHit2D hitBackCeiling = Physics2D.Raycast(_backTopCorner, Vector2.up, 2f, layerMask); 

		if (Input.GetAxis("Vertical") < 0 && _moveDirection.x == 0) 
		{ 
			if(!isCrouched && !isCrouchWalking) 
			{ 
				_boxCollider.size = new Vector2(_boxCollider.size.x, _originalBoxColliderSize.y / 2); 
				transform.position = new Vector3(transform.position.x, transform.position.y - (_originalBoxColliderSize.y / 4), 0); 
				_CharacterController.recalculateDistanceBetweenRays(); 
			} 
			isCrouched = true; 
			isCrouchWalking = false; 

		} 
		else if(Input.GetAxis("Vertical") < 0 && ( _moveDirection.x > 0 || _moveDirection.x < 0)) 
		{ 
			if (!isCrouched && !isCrouchWalking) 
			{ 
				_boxCollider.size = new Vector2(_boxCollider.size.x, _originalBoxColliderSize.y / 2); 
				transform.position = new Vector3(transform.position.x, transform.position.y - (_originalBoxColliderSize.y / 4), 0); 
				_CharacterController.recalculateDistanceBetweenRays(); 
			} 
			isCrouched = false; 
			isCrouchWalking = true; 
		} 
		else 
		{ 
			if(!hitFrontCeiling.collider && !hitBackCeiling && (isCrouched || isCrouchWalking)) 
			{ 

				_boxCollider.size = new Vector2(_boxCollider.size.x, _originalBoxColliderSize.y); 
				transform.position = new Vector3(transform.position.x, transform.position.y + (_originalBoxColliderSize.y / 4), 0); 
				_CharacterController.recalculateDistanceBetweenRays(); 
				isCrouched = false; 
				isCrouchWalking = false; 
			} 
		} 
		//check above us so we don't go through a ceiling 
		if (flags.above) 
		{ 
			//normal gravity calculation 
			_moveDirection.y -= gravity * Time.deltaTime; 
		} 
		//wall jumping code 
		//checks collsion left and right 
		if (flags.left || flags.right) 
		{ 

			if (canWallRun) 
			{ 
				if (Input.GetAxis("Vertical") > 0 && iswallRunning) 
				{ 
					_moveDirection.y = jumpSpeed / wallRunAmount; 
					StartCoroutine(WallRunTimer()); 
				} 
			} 
			//checks if can wall jump 
			if (canWallJump) 
			{ 
				if (Input.GetButtonDown("Jump") && !wallJumped && !isGrounded) 
				{ 
					if(_moveDirection.x < 0) 
					{ 
						//moving right 
						_moveDirection.x = jumpSpeed * wallJumpXAmount; 
						_moveDirection.y = jumpSpeed * wallJumpYAmount; 
						transform.eulerAngles = new Vector3(0, 180, 0); 
						_lastJumpedWasLeft = false; 
					} 
					else if(_moveDirection.x > 0) 
					{ 
						//moving left 
						_moveDirection.x = -jumpSpeed * wallJumpXAmount; 
						_moveDirection.y = jumpSpeed * wallJumpYAmount; 
						transform.eulerAngles = new Vector3(0, 0, 0); 
						_lastJumpedWasLeft = true; 
					} 
					StartCoroutine(WallJumpTimer()); 
				} 
			} 
			else 
			{ 
				if (canRunAfterWallJump) 
				{ 
					StopCoroutine(WallRunTimer()); 
					iswallRunning = true; 
				} 
			} 
		} 
		//animator states 
		animator.SetBool("isJumping", isJumping); 
		animator.SetBool("isGrounded", isGrounded); 
		//animator.SetBool("isFacingRight", isFacingRight); 
		animator.SetFloat("movementX", _moveDirection.x); 
		animator.SetFloat("movementY", _moveDirection.y); 


		//public bool isGrounded; 
		//public bool isJumping; 
		//public bool isFacingRight; 
		//public bool doubleJumped; 
		//public bool wallJumped; 
		//public bool iswallRunning; 
		//public bool isSlopeSliding; 
		//public bool isGliding; 
		//public bool isCrouchWalking; 
		//public bool isCrouched; 
		//public bool isPowerJumping; 
		//public bool isStomping; 
		//public bool isChangingLevels; 

	}//end of update 


	//wall jump time 
	IEnumerator WallJumpTimer() 
	{ 
		wallJumped = true; 
		yield return new WaitForSeconds(0.5f); 
		wallJumped = false; 
	} 

	//wall run time 
	IEnumerator WallRunTimer() 
	{ 
		iswallRunning = true; 
		yield return new WaitForSeconds(.5f); 
		iswallRunning = false; 
	} 

	//powerjump timer 
	IEnumerator PowerJumpTimer() 
	{ 
		isPowerJumping = true; 
		yield return new WaitForSeconds(1f); 
		isPowerJumping = false; 

	} 


	//****************************************************************************************AI STUFF******************************************************************************************** 
	void Awake () { 
		myTrans = this.transform; 
		myWidth = GetComponent<SpriteRenderer>().bounds.extents.x; 
		attacking = false; 
	} 

	// Vaguely using tutorial found https://www.youtube.com/watch?v=LPNSh9mwT4w 
	// Takes the place of Input.GetAxis("Horizontal"), and returns a float < 1 and > -1  
	private float aiHorizontal() 
	{ 
		if (attacking == false) { 
			//      //Checks if left side of the enemy is grounded  
			//      Vector2 lineCastPosLeft = myTrans.position - myTrans.right * myWidth; 
			//      Debug.DrawLine (lineCastPosLeft, lineCastPosLeft + Vector2.down); 
			//      bool willBeGroundedLeft = Physics2D.Linecast (lineCastPosLeft, lineCastPosLeft + Vector2.down, enemyMask); 
			// 
			// 
			//      //Checks if left side of the enemy is grounded  
			//      Vector2 lineCastPosRight = myTrans.position + myTrans.right * myWidth; 
			//      Debug.DrawLine (lineCastPosRight, lineCastPosRight + Vector2.down); 
			//      bool willBeGroundedRight = Physics2D.Linecast (lineCastPosRight, lineCastPosRight + Vector2.down, enemyMask); 

			_frontBottomCorner = new Vector3(transform.position.x + _boxCollider.size.x / 2, transform.position.y - _boxCollider.size.y / 2, 0); 
			_backBottomCorner = new Vector3(transform.position.x - _boxCollider.size.x / 2, transform.position.y - _boxCollider.size.y / 2, 0); 
			RaycastHit2D hitFrontGround = Physics2D.Raycast(_frontBottomCorner, Vector2.down, 2f, layerMask); 
			RaycastHit2D hitBackGround = Physics2D.Raycast(_backBottomCorner, Vector2.down, 2f, layerMask); 
			Debug.DrawRay (_frontBottomCorner, Vector2.down);  
			Debug.DrawRay (_backBottomCorner, Vector2.down);  

			//      if (myTrans.position.x < player.myTrans.position.x) { //If the enemy is currently facing right continue moving right unless it will not be grounded 
			//        if (!willBeGroundedRight) { 
			//          // Debug.Log ("Will not be grounded on right, stopping "); 
			//          return 0f;  
			//        } else { 
			//          return 0.1f; 
			//        } 
			//      } else { //If the enemy is currently facing left continue moving left unless it will not be grounded 
			//        if (!willBeGroundedLeft) { 
			//          // Debug.Log ("Will not be grounded on left, stopping"); 
			//          return 0;  
			//        } else { 
			//          return -0.1f; 
			//        } 
			//      } 
			//      if (hitBackGround.collider && hitFrontGround.collider) { 
			//        isFacingRight = true; 
			//        return 1f; 
			//      } 
			//      if (hitBackGround.collider && !hitFrontGround.collider && isFacingRight) { 
			//         
			//        Debug.Log ("Front hit" + hitFrontGround.collider);  
			//        Debug.Log ("Back hit" + hitBackGround.collider);  
			//        Debug.Log ("Will nnnnnnnnnnnnnnnnnnnnnnnnot be grounded on left"); 
			//        isFacingRight = false;//Face the left 
			//        StartCoroutine("faceRightTime"); 
			//                  return -1f;  
			// 
			//      } 
			//      /*if (hitFrontGround.collider && !hitBackGround.collider && !isFacingRight) { 
			//        Debug.Log ("mother fucker"); 
			//        Debug.Log ("Will nnnnnnnnnnnnnnnnnnnnnnnnot be grounded on right"); 
			//        //facingRight = true;//Face the left 
			//        return 1f;  
			// 
			//      }*/ 


			Debug.Log ("Left " + hitBackGround.collider + " Right " + hitFrontGround.collider);  

			if (facingRight == true) { //If the enemy is currently facing right continue moving right unless it will not be grounded 
				Debug.Log ("entering right case"); 
				if(hitFrontGround.collider == null) 
				{ 
					Debug.Log ("Will nnnnnnnnnnnnnnnnnnnnnnnnot be grounded on right"); 
					facingRight = false;//Face the left 
					return 0f;  
				} else { 
					Debug.Log ("Will  be grounded on right"); 
					return 1f; 
				} 
			} else { //If the enemy is currently facing left continue moving left unless it will not be grounded 
				Debug.Log ("entering left case"); 
				if(hitBackGround.collider == null) 
				{ 
					Debug.Log ("Will nnnnnnnnnnnnnnnnnot be grounded on left"); 
					facingRight = true;//Face the left 
					return 0f;  
				} else { 
					Debug.Log ("Will  be grounded on left"); 

					return -1f; 
				} 
			} 
		} else  
		{ 
			return 0; 
		} 
	} 

	private bool aiAttack() 
	{ 

		// Debug.Log ("Running aiAttack"); 
		foreach (Collider col in attackHitboxes)  
		{ 
			Collider[] cols = Physics.OverlapBox(col.bounds.center,col.bounds.extents,col.transform.rotation, LayerMask.GetMask("Hitbox")); 
			if (cols.Length > 0)  
			{ 
				return true; 
			} 
		} 
		// Debug.Log ("Didn't find anything to attack"); 
		return false;  
	} 

	// Checks for keypress and calls launch attack with the corresponding attackHitBox  
	// Needs to be called in update 
	// Following tutorial found at https://www.youtube.com/watch?v=mvVM1RB4HXk  
	protected void CheckAttack() 
	{ 
		//    if (aiAttack())  
		//    { 
		//      attacking = true; 
		//      if (spriteRenderer.flipX) //Checks the direction the sprite is facing and selects the correct arm side 
		//      { 
		//        // Debug.Log ("left enemy arm attack"); 
		//        LaunchAttack (attackHitboxes [0]);  
		//      } else  
		//      { 
		//        // Debug.Log ("right enemy arm attack"); 
		//        LaunchAttack (attackHitboxes[1]);   
		//      } 
		//    } 
		// 
		//    if (aiAttack())  
		//    { 
		//      if (spriteRenderer.flipX) //Checks the direction the sprite is facing and selects the correct kick side 
		//      { 
		//        // Debug.Log ("left enemy kick attack"); 
		//        LaunchAttack (attackHitboxes[2]);  
		//      } else  
		//      { 
		//        // Debug.Log ("right enemy kick attack"); 
		//        LaunchAttack (attackHitboxes[3]);  
		//      } 
		// 
		//    } 
	} 

	// Checks if the attack hit box overlaps with a targethitbox and designates the damage amount 
	// Following tutorial found at https://www.youtube.com/watch?v=mvVM1RB4HXk  
	private void LaunchAttack(Collider col) 
	{ 
		animator.SetBool("attacking", attacking); 
		//    Debug.Log ("Launching attack");  
		Collider[] cols = Physics.OverlapBox(col.bounds.center,col.bounds.extents,col.transform.rotation, LayerMask.GetMask("Hitbox"));  
		foreach(Collider c in cols) 
		{ 
			if (c.transform.parent.parent == transform)  
			{ 
				continue;  
			} 
			float damage = 0;  
			//      Debug.Log (c.name); 
			switch (c.name)  
			{ 
			case "Head":  
				damage = 30; 
				Debug.Log ("enemy Hit Head Damage = 30");  
				break; 
			case "Body": 
				damage = 10;  
				Debug.Log ("enemy Hit Body Damage = 10");  
				break; 
			default: 
				Debug.Log ("enemy Unable to identify the body part, check the switch statement");  
				break; 
			} 

		} 
	} 
	//facing Right Timer 
	IEnumerator faceRightTime() 
	{ 
		//isFacingRight = true; 
		yield return new WaitForSeconds (5f); 
		isFacingRight = false; 
	} 


} 
File contents are unchanged.
38 file changes in working directory
View changes
commit:4c1e87
WIP on master: Auto stash before merge of "master" and "develop"