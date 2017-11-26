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
    public Image death_image;
    private float HP;
    private float WP;
    private float maxHP;
    private float maxWP;
    private float HPrec;
    private float WPrec;
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
        SetUItext();
       
    }

    //checks if you're dead and stops knockback
    // this is called in KnockBack coroutine
    IEnumerator deathCheck()
    {
        StopCoroutine("KnockBack");
        death_image.enabled = true;
        player.GetComponent<playerController>().canMove = false;
        //play death animation and assosiated sounds
        //Debug.Log("you are dead");
        DeathSong.Play();
        
        
        yield return new WaitForSeconds(5f);
        death_image.enabled = false;
        player.GetComponent<Transform>().position = Saveposition;
        StartCoroutine("StartUP");
        
        

        
    }
    //returns health % for health bars
    float healthPercent()
    {
        return HP / maxHP;
    }

<<<<<<< HEAD
    //returns Health % for health bars
    float wpPercent()
    {
        return WP / maxWP;
    }
    void OnTriggerEnter2D(Collider2D tag)
    {
        //enemy hit boxes
        if (tag.gameObject.tag == "enemy")
        {
            StartCoroutine("KnockBack");
=======
		if (tag.gameObject.tag == "enemy")
		{
			HP--;
			SetUItext();
			// StartCoroutine("KnockBack");
		}
        if (tag.gameObject.tag == "enemy shot")
        {
            HP--;
            SetUItext();
            // StartCoroutine("KnockBack");
>>>>>>> 2b1ec3fd6344a69046716e2b245dc9dbffc13198
        }
        //save collider boxes
        if (tag.gameObject.tag == "save")
        {
            //saves to the center point of the tagged object
            Saveposition = tag.GetComponent<Transform>().position;
            Debug.Log("Saved position" + Saveposition.ToString());
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
            if ((maxWP - WP) <= WPrec)
            {
                WP = maxWP;
            }
            else
            {
                WP += WPrec;
            }
            
            SetUItext();
            wpBar.value = wpPercent();
            //do animation and associated sound.
        }

    }
    IEnumerator StartUP()
    {
        //assosiated start up animation and sound
        HP = maxHP;
        WP = maxWP;
        SetUItext();
        hpBar.value = healthPercent();
        wpBar.value = wpPercent();

        //set movement to zero
        //private Vector3 _moveDirection = Vector3.zero;
        player.GetComponent<playerController>()._moveDirection = Vector3.zero;
        yield return new WaitForSeconds(5f);
        //fancy intro screen
        player.GetComponent<playerController>().canMove = true;
    }

    //controls firing and the rate of fire
    IEnumerator KnockBack()
    {
        
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

        }
        else if ((HPdiff) > 10)
        {
            HP = 0;
            Debug.Log("less than 10 " + HP.ToString());
            StartCoroutine("deathCheck");
        }
        hpBar.value = healthPercent();

        //add in detection from the wall as to not get stuck in or pushed further than;
        //if in the air // might take this out
        if (!player.GetComponent<playerController>().isGrounded && (KBflags.left || KBflags.right) && KBflags.below )
        {
            knockback = new Vector3(-5, -2, 0);
        }
        else //otherwise
        {
            knockback = new Vector3(-5, 0, 0);

        }
        //player.animate hit animation
        //play sound of getting hit
        player.transform.Translate(knockback);
        //turn off hit hurt box for invcible frames.
        yield return new WaitForSeconds(kbTime);
        player.GetComponent<playerController>().canMove = true;
    }
    void SetUItext()
    {
        wpText.text = WP.ToString();
        weaponSelcText.text = "Weapon: " + wnum.ToString();
        
    }
}


