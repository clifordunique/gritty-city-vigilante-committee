using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerPlatformerController : PhysicsObject {
    public float jumpTakeOffSpeed = 7;
    public float maxSpeed = 7;
    private float test = 23;
    protected SpriteRenderer spriteRenderer;
    private Animator animator;
	private bool facingRight;
    

    public GameObject shot;
    public Transform shotSpawn;
    public float fireRate;

    private float nextFire;

    public Collider[] attackHitboxes; // Assigned in Inspector with size of 4, to accomodate left/right arm, and left/right leg

	// Use this for initialization
	void Awake () {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
	}
 
    protected override void ComputeVelocity()
    {
        Vector2 move = Vector2.zero;
        move.x = Input.GetAxis("Horizontal");

        if (Input.GetButtonDown("Jump") && grounded )
        {
            //adds veloctiy along y access
            velocity.y = jumpTakeOffSpeed;
            //test = velocity.magnitude;
            Debug.Log("getting called when hit jump and grounded " );
        }
    	//cancels jump when releasing button
        else if (Input.GetButtonUp("Jump"))
        {
            Debug.Log("cancelling jump");
            if (velocity.y > 0)
            {
                velocity.y = velocity.y * 0.5f;
                
            }
        }
        //this fire a shot
        else if (Input.GetButton("Fire1") && Time.time > nextFire)
        {
            //limits how often you can shoot
            nextFire = Time.time + fireRate;
            Debug.Log("hashtag fired a shot son");
            Debug.Log(spriteRenderer.flipX);
            //Instantiate(object, player poistion, player rotation)
            Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
          //  GetComponent<AudioSource>().Play();
        }

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
