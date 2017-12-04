using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using Prime31;

public class PlayerStatsAndStates : playerController
{

    //private Collider2D _hurtBox;
    public CharacterController2D.CharacterCollisionState2D KBflags;
    private GameObject player;
    public bool test = true;
    public Slider hpBar;
    public Slider wpBar;
    public Slider superBar;
    public Image death_image;
    private float superMax;
    private float superAmount;
    private float HP;
    private float WP;
    private float maxHP;
    private float maxWP;
    private float HPrec;
    private float WPrec;
    private float superRec;
    private float HPdiff;
    private Transform saveTransform;
    private Vector3 Saveposition;
    public AudioSource DeathSong;
    public GameObject DeathmusicPlayer;
    // private float percent;
    private int wnum;
    public Text wpText;
   // public Text hpText;
    public Text weaponSelcText;
    public Vector3 knockback;
    public float kbTime;
   

    private bool _isStarting;
    private bool _isSavePosition;
    public bool _isDead;
    private bool _isKnockBack;


    private bool _enemyTrigger;
    // Use this for initialization
    void Awake()
    {
        death_image.enabled = false;
        _enemyTrigger = false;
        maxHP = 100;
        maxWP = 50;
        HPrec = 10;
        WPrec = 5;
        superRec = 1;

        superMax = 10;
        superAmount = 10;
        player = GameObject.FindGameObjectWithTag("Player");
        Saveposition = player.GetComponent<Transform>().position;
        //Debug.Log(Saveposition);
        StartCoroutine("StartUP");
        
    }

    
    // Update is called once per frame
    void Update()
    {
        HPdiff = maxHP - HP;
        wnum = player.GetComponent<playerController>().wepNum;
        WP = player.GetComponent<playerController>().ammoCount[wnum];
        superAmount = player.GetComponent<playerController>().ammoCount[3];
        SetAnimator();
        SetUItext();
        wpBar.value = wpPercent();
        superBar.value = superPercent();
        hpBar.value = healthPercent();

    }

    //checks if you're dead and stops knockback
    // this is called in KnockBack coroutine
    IEnumerator deathCheck()
    {
        StopCoroutine("KnockBack");
        _isKnockBack = false;
        _isDead = true;
        death_image.enabled = true;
        player.GetComponent<playerController>()._moveDirection = Vector3.zero;
        player.GetComponent<playerController>().canMove = false;
        //play assosiated sounds
        //Debug.Log("you are dead");
        DeathSong.Play();
        yield return new WaitForSeconds(5f);
        death_image.enabled = false;
        player.GetComponent<Transform>().position = Saveposition;
        _isDead = false;
        StartCoroutine("StartUP");
        
    }
    float superPercent()
    {
        return superAmount / superMax;
    }
    //returns health % for health bars
    float healthPercent()
    {
        return HP / maxHP;
    }


    //returns Health % for health bars
    float wpPercent()
    {
        return WP / maxWP;
    }

    //this deals with all triggers
    //from pickups to enemy collisions
    //save spots and spawn points
    void OnTriggerEnter2D(Collider2D tag)
    {
        if(tag.gameObject.tag == "instaDeath")
        {
            HP = 0;

            StartCoroutine("deathCheck");
        }
        //enemy hit boxes
        if (tag.gameObject.tag == "enemy")
        {
            StartCoroutine("KnockBack");

		}
        //pick up leo 
        //this can be expanded upon to
        //include more vigilantes
        if (tag.gameObject.tag == "leoPickup")
        {
            player.GetComponent<playerController>().hasLeo = true;
        }
        if (tag.gameObject.tag == "enemy shot")
        {
            
            StartCoroutine("KnockBack");

        }
        //save collider boxes
        if (tag.gameObject.tag == "save")
        {
            //saves to the center point of the tagged object
            Saveposition = tag.GetComponent<Transform>().position;
            //have some words pop up at the bottom left of screen 
            //to notify check point.
            //Debug.Log("Saved position" + Saveposition.ToString());
            StartCoroutine("SaveTimer");
        }
        //recover hp items
        if (tag.gameObject.tag == "Hrecovery")
        {
            if((maxHP - HP) <= HPrec)
            {
                HP = maxHP; 
            }
            else
            {
                HP += HPrec;
            }
            hpBar.value = healthPercent();
            //do animation and associated sound
        }

        //recover wp items
        if(tag.gameObject.tag == "Wrecovery")
        {
            addtowpMeter();
            //do animation and associated sound.
        }
        if (tag.gameObject.tag == "Superrecovery")
        {
            addtoSuperMeter();

        }
    }
    IEnumerator StartUP()
    {
          
        _isStarting = true;
        //assosiated start up animation and sound
        HP = maxHP;
        player.GetComponent<playerController>().ammoCount[0] = maxWP;
        player.GetComponent<playerController>().ammoCount[1] = maxWP;
        player.GetComponent<playerController>().ammoCount[2] = maxWP;
        player.GetComponent<playerController>().ammoCount[3] = superMax;
        SetUItext();
        hpBar.value = healthPercent();
        wpBar.value = wpPercent();
        superBar.value = superPercent();
        //set movement to zero
        //private Vector3 _moveDirection = Vector3.zero;
        player.GetComponent<playerController>()._moveDirection = Vector3.zero;
        yield return new WaitForSeconds(5f);
        //fancy intro screen
        _isStarting = false;
        player.GetComponent<playerController>().canMove = true;

    }
    void addtowpMeter()
    {
        if ((maxWP - player.GetComponent<playerController>().ammoCount[wnum]) <= WPrec)
        {
            player.GetComponent<playerController>().ammoCount[wnum] = maxWP;
        }
        else
        {
            player.GetComponent<playerController>().ammoCount[wnum] += WPrec;
        }
        wpBar.value = wpPercent();
    }

    void addtoSuperMeter()
    {
        //getting hit increases your super bar also.
        if ((superMax - superAmount) <= superRec)
        {
            player.GetComponent<playerController>().ammoCount[3] = superMax;
            player.GetComponent<playerController>().canSuperShot = true;
        }
        else
        {
            Debug.Log("hit wall get mad");
            player.GetComponent<playerController>().ammoCount[3] += superRec;
        }
        superBar.value = superPercent();
    }
    IEnumerator SaveTimer()
    {
        
        _isSavePosition = true;
         yield return new WaitForSeconds(1f);
        _isSavePosition = false;

    }
    //controls firing and the rate of fire
    IEnumerator KnockBack()
    {
        
        _isKnockBack = true;
        kbTime = .25f;
        //Debug.Log(kbTime);
        //position needs to change after we figure out where he's shooting from
        //player.GetComponent<playerController>().flags = _CharacterController.collisionState;
        KBflags = player.GetComponent<playerController>().flags;
        if (HPdiff <= 90)
        {
            HP -= 10;
            Debug.Log("more than 10 " + HP.ToString());
            player.GetComponent<playerController>().canMove = false;
            addtoSuperMeter();

        }
        else if ((HPdiff) > 10)
        {
            HP = 0;
            Debug.Log("less than 10 " + HP.ToString());
            StartCoroutine("deathCheck");
        }
        hpBar.value = healthPercent();

        //add in detection from the wall as to not get stuck in or pushed further than;
        if (!player.GetComponent<playerController>().isGrounded && (KBflags.left || KBflags.right) && KBflags.below )
        {
            knockback = new Vector3(-5, -2, 0);
        }
        else if(player.GetComponent<playerController>().isGrounded && !(KBflags.left || KBflags.right))
        {
            knockback = new Vector3(-5, 0, 0);
        }
        else //otherwise
        {
            knockback = new Vector3(0, 0, 0);

        }
        //player.animate hit animation
        //play sound of getting hit
        player.transform.Translate(knockback);
        //turn off hit hurt box for invcible frames.
        yield return new WaitForSeconds(kbTime);
        _isKnockBack = false;
        player.GetComponent<playerController>().canMove = true;
    }
    void SetUItext()
    {
        wpText.text = WP.ToString();
        weaponSelcText.text = "Weapon: " + wnum.ToString();
        
    }
    void SetAnimator()
    {
        player.GetComponent<playerController>().animator.SetBool("isStarting", _isStarting);
        player.GetComponent<playerController>().animator.SetBool("isSaving", _isSavePosition);
        player.GetComponent<playerController>().animator.SetBool("isKnockbacked", _isKnockBack);
        player.GetComponent<playerController>().animator.SetBool("isDead", _isDead);
        player.GetComponent<playerController>().animator.SetBool("canMove", player.GetComponent<playerController>().canMove);
    }
}


