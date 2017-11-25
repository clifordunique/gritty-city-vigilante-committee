using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class PlayerStatsAndStates : playerController
{

    //private Collider2D _hurtBox;
    private GameObject player;
    public bool test = true;
    
    private int HP;
    private int WP;
    private int wnum;
    public Text wpText;
    public Text hpText;
    public Text weaponSelcText;
    public Vector3 knockback;
    public float kbTime;
    // Use this for initialization
    void Start()
    {

        //_hurtBox = GetComponent<BoxCollider2D>();
        HP = 100;
        WP = 100;
        
        //hpText.text = "Count: " + HP.ToString();
        SetUItext();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        wnum = player.GetComponent<playerController>().wepNum;
        SetUItext();
    }

    void OnTriggerEnter2D(Collider2D tag)
    {

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
        }
        if (tag.gameObject.tag == "Hrecovery")
        {
            HP++;
            SetUItext();
            //do animation and associated sound
        }
        if(tag.gameObject.tag == "Wrecovery")
        {
            WP++;
            SetUItext();
            //do animation and associated sound.
        }

    }

    //controls firing and the rate of fire
    IEnumerator KnockBack()
    {
        kbTime = .15f;
        //Debug.Log(kbTime);
        //position needs to change after we figure out where he's shooting from
        
        player.GetComponent<playerController>().canMove = false;
        //add in detection from the wall as to not get stuck in or pushed further than;
        //if in the air // might take this out
        if (!player.GetComponent<playerController>().isGrounded)
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
        hpText.text = "HP: " + HP.ToString();
        wpText.text = "WP: " + WP.ToString();
        weaponSelcText.text = "Weapon: " + wnum.ToString();
        
    }
}


