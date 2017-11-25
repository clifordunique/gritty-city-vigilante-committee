using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IgnoreCollions : playerController {

    private Collider2D _hurtBox;
    private GameObject player;
    public bool test = true;
    public int HP;
    public Vector3 knockback;
    public float kbTime;
	// Use this for initialization
	void Start () {
        
        _hurtBox = GetComponent<BoxCollider2D>();
        HP = 100;
        player = GameObject.FindGameObjectWithTag("Player");
	}
	
	// Update is called once per frame
	void Update () {


    }

    //player's hurt box.. can be modified based on tag;
    void OnTriggerEnter2D(Collider2D tag)
    {

        if (tag.gameObject.tag == "enemy")
        {
            Debug.Log("enemy collision");
            //Destroy(enemy.gameObject);
            HP--;
            // Debug.Log(HP);
            StartCoroutine("KnockBack");
            //Physics2D.IgnoreCollision(enemy, _hurtBox, test);
        }
    }

    //controls player getting knocked back
    IEnumerator KnockBack()
    {
        kbTime = 1f;
        // Debug.Log(kbTime);
        //canMove = false;
        //position needs to change after we figure out where he's shooting from
        //or how the character is shooting
        //add in negative for facing a certain way... easy peasy

        player.GetComponent <playerController> ().canMove = false;
        knockback = new Vector3(-10, 0, 0);
        player.transform.Translate(knockback);
        yield return new WaitForSeconds(kbTime);
        player.GetComponent<playerController>().canMove = true;
    }
}


