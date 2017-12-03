using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotmover : playerController
{
    public float speed;
    public Animator Shot_animator;
    private GameObject player;
    public Vector3 velo;
   // private Collider2D other;
    private bool alive = false;
    private bool hasHit = false;
    private float hit = 0;



    void Start () {
        //connects to the player/object to find direction 
        // that they are facing.
        player = GameObject.FindWithTag("Player");
        Vector3 velo = Vector3.zero;
        speed = .5f;
        Shot_animator = GetComponent<Animator>();
        //Debug.Log(gameObject);
    }
	
	
	void Update ()
    {

             //horizontal
            if (isFacingRight && !alive && !hasHit)
            {
                //if(gameObject = "horiz_shot")
                velo = new Vector3(-1f * speed, 0, 0);
                transform.Translate(velo);
                alive = true;
            Debug.Log("1hasHit " + hasHit);
                

            }
            if (!isFacingRight && !alive && !hasHit)
            {
                //Debug.Log(isFacingRight);
                //velo = new Vector3(0, speed, 0);
                velo = new Vector3(speed, 0, 0);
                transform.Translate(velo);

                alive = true;
                Debug.Log("2hasHit " + hasHit);
            }
           
            //this ensures the bullets doesn't change direction 
            //after being fired.
            if (hasHit && !alive)
            {
            hit++;
            Debug.Log(hit);
            StartCoroutine("deathAnimation");
            Debug.Log("3hasHit " + hasHit);

            }
        else
        {
            transform.Translate(velo);
        }
        SetAnimator();
        
    }
    void OnBecameInvisible()
    {
        //also can put another function and make timed destoryer
        //like make another coroutine to destroy it a few seconds after being 
        //initialized
        Destroy(gameObject);
    }
    // When this game object intersects a collider with 'is trigger' checked, 
    // store a reference to that collider in a variable named 'other'..
    void OnTriggerEnter2D(Collider2D other)
    {
        
        //Debug.Log("what");
        // ..and if the game object we intersect has the tag 'Pick Up' assigned to it..
        if (other.gameObject.CompareTag("killme"))
        {
            // Handled in the EnemyHurtBox Now
            // alive = false;
            // destorys or change to hitpoint --
            // hasHit = true;
            // alive = false;
            // Destroy(other.transform.parent.gameObject);
            //yield return new WaitForSeconds(3f);
            
            //destroys bullet
            //Destroy(gameObject);


        }
    }

    //death animation for the bullet no need for enemy
    IEnumerator deathAnimation()
    {
        //bullet is nolonger alive
        
        Debug.Log("play end bullet animation");
        //destorys or change to hitpoint --
        //velo = Vector3.zero;
        //transform.Translate(velo);
        //Destroy(other.transform.parent.gameObject);
        yield return new WaitForSeconds(1f);
        //destroys bullet
        Destroy(gameObject);

    }
    void SetAnimator()
    {
        animator.SetBool("isAlive", alive);
        animator.SetBool("hasHit", hasHit);
        animator.SetFloat("goingStraight", hit);
    }
}
