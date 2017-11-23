using System.Collections;
using System.Collections.Generic;
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
    public GameObject owB;
    public SpriteRenderer outworld_background;
    public VideoPlayer[] videos;
    public GameObject[] videoPlayers;
    public AudioSource[] music;
    public GameObject[] musicPlayers;
    //public BoxCollider2D
    public Animator animator;

    public GameObject shot;
    public Transform shotSpawn;
    
    public float fireRate = .5f;
    
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
    
    
	// Use this for initialization
	void Start ()
    {
        //grabs the character attached to the script
        _CharacterController = GetComponent<CharacterController2D>();
        _currentGlideTime = glideTime;
        _boxCollider = GetComponent<BoxCollider2D>();
        _originalBoxColliderSize = _boxCollider.size;
        box = GameObject.FindGameObjectsWithTag("outworld");
        cityGameObject = GameObject.FindGameObjectsWithTag("regular");
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
        for (int i = 0; i < box.Length; i++)
        {
            boxTest[i].enabled = false;
            boxy[i].enabled = false;
            //GameObject.FindGameObjectsWithTag("outworld_background").enabled = true;
        }
        owB = GameObject.FindGameObjectWithTag("outworld_background");
        outworld_background = owB.GetComponent<SpriteRenderer>();
        outworld_background.enabled = false;

        videoPlayers = GameObject.FindGameObjectsWithTag("video");
        videos = new VideoPlayer[videoPlayers.Length];

        videos[0] = videoPlayers[0].GetComponent<VideoPlayer>();
        videos[1] = videoPlayers[1].GetComponent<VideoPlayer>();
        videos[0].playOnAwake = true;
        videos[1].playOnAwake = false;

        musicPlayers = GameObject.FindGameObjectsWithTag("music");
        music = new AudioSource[musicPlayers.Length];
        music[0] = musicPlayers[0].GetComponent<AudioSource>();
        music[1] = musicPlayers[1].GetComponent<AudioSource>();
        music[0].playOnAwake = false;
        music[1].playOnAwake = true;
        isChangingLevels = true;
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
                Debug.Log("pew pew");

                StartCoroutine("Fire");
                
                
                //_nextFire = Time.time + fireRate;
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
                videos[0].Stop();
                music[0].Stop();
                music[1].Play();
                outworld_background.enabled = false;
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
                outworld_background.enabled = true;
                videos[0].Play();
                music[1].Stop();
                music[0].time = 20f;
                music[0].Play();
                StartCoroutine("WallyWowPlay");
                               
                isChangingLevels = false;
            }
        }
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
    IEnumerator WallyWowPlay()
    {
        videos[1].Play();
        //outworld_background.enabled = true;
        yield return new WaitForSeconds(6f);
        outworld_background.enabled = false;
        videos[1].Stop();
        
    }
    //controls firing and the rate of fire
    IEnumerator Fire()
    {
        Debug.Log(fireRate);
        canShoot = false;
        //position needs to change after we figure out where he's shooting from
        //or how the character is shooting
        //add in negative for facing a certain way... easy peasy
        Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
        yield return new WaitForSeconds(fireRate);
        canShoot = true;
    }
}
