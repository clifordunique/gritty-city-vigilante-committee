using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using Prime31;
using UnityEngine.Video;

public class playerController : MonoBehaviour {
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
    public float[] ammoCount;
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
    public bool canMove = true;
    public bool canCrouch = false;
    public bool canChangeWep = true;
    public bool canSuperShot = true;
    public bool hasLeo = false;
    private bool canMoveHorizontal = true;
    public Vector3 shotadjustment;
   
    //player states
    public bool isGrounded;
    private bool isJumping;
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
    public bool isSuper;

    //public bool isWallSliding
    //public bool isDashing
    //public bool isAirDashing
    public bool isShooting;  //for animation

    //some stuff to make the level disapear for the "gimmick"
    //public bool isGone = false;
    public GameObject[] box; 
    public GameObject[] cityGameObject;
    //public SpriteRenderer outworld_background;

    
    //public BoxCollider2D
    public Animator animator;

    public GameObject horizshot;
    public GameObject[] SuperMove;
    public GameObject[] vShot;
    public int wepNum;
    public Transform shotSpawn;
    public Transform superSpawn;
    public AudioSource YouAreNowDead;
    public Image super_image;
    //public GameObject DeathmusicPlayer;
    public float fireRate = .5f;
    
    //private variables
    public Vector3 _moveDirection = Vector3.zero;
    public CharacterController2D _CharacterController;
    private bool _lastJumpedWasLeft;
    private float _slopeAngle;
    private Vector3 _slopeGradient = Vector3.zero;
    private bool _startGlide;
    private float _currentGlideTime;
    private BoxCollider2D _boxCollider;
    private Vector2 _originalBoxColliderSize;
    private Vector3 _frontTopCorner;
    private Vector3 _backTopCorner;
    
    
	// Use this for initialization
	void Awake()
    {
        wepNum = 0;
        shotadjustment = new Vector3(0, -1, 0);
        //grabs the character attached to the script
        _CharacterController = GetComponent<CharacterController2D>();
        _currentGlideTime = glideTime;
        _boxCollider = GetComponent<BoxCollider2D>();
        _originalBoxColliderSize = _boxCollider.size;
        setupAwake();
        animator = GetComponent<Animator>();
        canShoot = true;
    }//end of start

    // Update is called once per frame
    void Update() {
        SetAnimation();
        if (canMove)
        {
           
            if (canShoot)
            {
                //horizontal shot
                if (Input.GetButton("Fire3"))
                {
                    StartCoroutine("Fire");
                }
                //vigilante help shot
                if (Input.GetButton("Fire5"))
                {
                    StartCoroutine("vFire");
                }
                //supershot
                if (canSuperShot && Input.GetButton("Fire6"))
                {
                    
                    StartCoroutine("preSuper");
                    
                }
                //change vigilante
                if (canChangeWep)
                {
                    if (Input.GetButton("RightBump"))
                    {
                        StartCoroutine("wepSelRight");
                    }
                }

                
            }

            //changes to the alternate level
            if (canChangeLevel)
            {   
                if (Input.GetButton("Fire1"))
                {
                    StartCoroutine("SwitchGimmick");
                }
            }

            //wall jump mechanic
            if (!wallJumped)
            {
                _moveDirection.x = Input.GetAxis("Horizontal");
                _moveDirection.x *= walkSpeed;
            }

            //next few lines detect the slope of the ground and determines if they will slide
            //down the hill or not
            RaycastHit2D hit = Physics2D.Raycast(transform.position, -Vector3.up, 2f, layerMask);
            if (hit)
            {
                _slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
                _slopeGradient = hit.normal;

                if (_slopeAngle > _CharacterController.slopeLimit)
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
                else if (_moveDirection.x > 0)
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
                    if (canPowerJump && Input.GetAxis("Vertical") < 0)
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
                
                if (_moveDirection.x < 0)
                {
                    //rotates player

                    transform.eulerAngles = new Vector3(0, 180, 0);
                    isFacingRight = false;
                }
                else if (_moveDirection.x > 0)
                {
                    transform.eulerAngles = new Vector3(0, 0, 0);
                    isFacingRight = true;
                }
                if (Input.GetButtonUp("Jump"))
                {
                    if (_moveDirection.y > 0)
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
            else if (canStomp && Input.GetAxis("Vertical") < 0 && !isPowerJumping)
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

            if (canMoveHorizontal)
            {
                _CharacterController.move(_moveDirection * Time.deltaTime);
            }
            

            //checks what is being collided with our character
            flags = _CharacterController.collisionState;

            isGrounded = flags.below;
            if (canCrouch)
            {     //crouched and crouch f
                _frontTopCorner = new Vector3(transform.position.x + _boxCollider.size.x / 2, transform.position.y + _boxCollider.size.y / 2, 0);
            _backTopCorner = new Vector3(transform.position.x - _boxCollider.size.x / 2, transform.position.y + _boxCollider.size.y / 2, 0);
            RaycastHit2D hitFrontCeiling = Physics2D.Raycast(_frontTopCorner, Vector2.up, 2f, layerMask);
            RaycastHit2D hitBackCeiling = Physics2D.Raycast(_backTopCorner, Vector2.up, 2f, layerMask);
            
                if (Input.GetAxis("Vertical") < 0 && _moveDirection.x == 0)
                {
                    if (!isCrouched && !isCrouchWalking)
                    {
                        _boxCollider.size = new Vector2(_boxCollider.size.x, _originalBoxColliderSize.y / 2);
                        transform.position = new Vector3(transform.position.x, transform.position.y - (_originalBoxColliderSize.y / 4), 0);
                        _CharacterController.recalculateDistanceBetweenRays();
                    }
                    isCrouched = true;
                    isCrouchWalking = false;

                }
                else if (Input.GetAxis("Vertical") < 0 && (_moveDirection.x > 0 || _moveDirection.x < 0))
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
                    if (!hitFrontCeiling.collider && !hitBackCeiling && (isCrouched || isCrouchWalking))
                    {

                        _boxCollider.size = new Vector2(_boxCollider.size.x, _originalBoxColliderSize.y);
                        transform.position = new Vector3(transform.position.x, transform.position.y + (_originalBoxColliderSize.y / 4), 0);
                        _CharacterController.recalculateDistanceBetweenRays();
                        isCrouched = false;
                        isCrouchWalking = false;
                    }
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
                        if (_moveDirection.x < 0)
                        {
                            //moving right
                            _moveDirection.x = jumpSpeed * wallJumpXAmount;
                            _moveDirection.y = jumpSpeed * wallJumpYAmount;
                            transform.eulerAngles = new Vector3(0, 180, 0);
                            _lastJumpedWasLeft = false;
                        }
                        else if (_moveDirection.x > 0)
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
            
        }
}//end of update
    void SetAnimation()
    {
        //animator states
        animator.SetBool("isJumping", isJumping);
        animator.SetBool("isGrounded", isGrounded);
        animator.SetBool("canMove", canMove);
        animator.SetBool("isShooting", isShooting);
        //animator.SetBool("isFacingRight", isFacingRight);
        animator.SetFloat("movementX", _moveDirection.x);
        animator.SetFloat("movementY", _moveDirection.y);
        animator.SetBool("isSuper", isSuper);

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
    }
    //makes it start with just objects with "level" tag
    void setupAwake()
    {
        box = GameObject.FindGameObjectsWithTag("outworld");
        cityGameObject = GameObject.FindGameObjectsWithTag("regular");
        SuperMove = GameObject.FindGameObjectsWithTag("super_shot");
        super_image.enabled = false;
        for(int i = 0; i < SuperMove.Length; i++)
        {
            SuperMove[i].SetActive(false);
        }
        

        //all red boxes disabled
        for (int i = 0; i < box.Length; i++)
        {
            box[i].SetActive(false);
        }
        
        isChangingLevels = true;
    }
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

    //turns red and blue platforms on or off
    IEnumerator SwitchGimmick()
    {
        //changes to red platforms
        if (!isChangingLevels)
        {
            for (int i = 0; i < box.Length; i++)
            {
                box[i].SetActive(false);
            }
            for (var i = 0; i < cityGameObject.Length; i++)
            {
                cityGameObject[i].SetActive(true);
            }
            //stop anything else playing (music wise)
            //play music here for this level or mebbe don't need it
            yield return new WaitForSeconds(.35f);
            isChangingLevels = true;
            
        }

        //changes to blue
        else if (isChangingLevels)
        {
            for (var i = 0; i < cityGameObject.Length; i++)
            {
                cityGameObject[i].SetActive(false);
            }
            for (var i = 0; i < box.Length; i++)
            {
                box[i].SetActive(true);
            }
            //stop music (optional)
            //play new mucsic (optional)
            yield return new WaitForSeconds(.35f);
            isChangingLevels = false;
        }
               
    }
    //controls firing and the rate of fire
    IEnumerator Fire()
    {
        canShootCheck();


        Instantiate(horizshot, shotSpawn.position + shotadjustment, shotSpawn.rotation);
        yield return new WaitForSeconds(fireRate);
        isShooting = false;
        canMoveHorizontal = true;
        canShoot = true;
    }

    //vigilante shot
    IEnumerator vFire()
    {
        //isShooting = false;
        
        //wepNum0 = hulk
        if (wepNum == 0 && (ammoCount[wepNum] > 0))
        {
            canShootCheck();
            Instantiate(vShot[wepNum], shotSpawn.position, shotSpawn.rotation);
            ammoCount[wepNum] -= 5;
        }
        //wepNum1 = wolverine
        if (wepNum == 1 && (ammoCount[wepNum] > 0))
        {
            if (isFacingRight)
            {
                shotadjustment = new Vector3(6, 3.5f, 0);

            }
            else
            {
                shotadjustment = new Vector3(-6, 3.5f, 0);
            }
            

            canShootCheck();
            Instantiate(vShot[wepNum], shotSpawn.position + shotadjustment, shotSpawn.rotation);
            ammoCount[wepNum] -= 1;
        }
        //wepNum2 = leo
        if (wepNum == 2 && (ammoCount[wepNum] > 0) && hasLeo)
        {
            canShootCheck();
            Instantiate(vShot[wepNum], shotSpawn.position, shotSpawn.rotation);
            ammoCount[wepNum] -= 2;
        }

        yield return new WaitForSeconds(fireRate);
        isShooting = false;
        canMoveHorizontal = true;
        canShoot = true;
    }
    void canShootCheck()
    {
        if (isGrounded)
        {
            canMoveHorizontal = false;
        }
        canShoot = false;
        isShooting = true;
    }
    //animation before super shot
    IEnumerator preSuper()
    {
        canMove = false;
        canShoot = false;
        isSuper = true;
        super_image.enabled = true;
        transform.position += new Vector3(0, 15, 0);
        YouAreNowDead.Play();
        yield return new WaitForSeconds(4f);
        StartCoroutine("SuperShot");
    }
    //super shot
    IEnumerator SuperShot()
    {
        super_image.enabled = false;
        for (int i = 0; i < SuperMove.Length; i++)
        {
            SuperMove[i].SetActive(true);
        }
        yield return new WaitForSeconds(2f);
        for (int i = 0; i < SuperMove.Length; i++)
        {
            SuperMove[i].SetActive(false);
        }
        ammoCount[3] = 0;
        canSuperShot = false;
        isSuper = false;
        canShoot = true;
        canMove = true;
       
    }

    //weapon select
    IEnumerator wepSelRight()
    {
        canChangeWep = false;
        if (wepNum == 2)
        {
            wepNum = 0;
        }
        else if (wepNum == 1 && !hasLeo)
        {
            wepNum = 0;
        }
        else
        {
            wepNum++;
        }
        yield return new WaitForSeconds(.25f);
        canChangeWep = true;
    }

}
