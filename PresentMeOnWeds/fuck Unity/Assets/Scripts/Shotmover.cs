using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotmover : playerController
{
    public float speed;
    private GameObject Player;
    private GameObject Shot;
    private Vector3 velo;
   
    private bool alive = false;


    void Start () {
        //connects to the player/object to find direction 
        // that they are facing.
        Player = GameObject.FindWithTag("Player");
        Shot = GameObject.FindWithTag("mothaFuckin_shot");
        Vector3 velo = Vector3.zero;
        speed = .5f;
	}
	
	
	void Update () {
        
        //checks the direction of player then gives the bullet a velocity
        if (isFacingRight && !alive)
         {
             //Debug.Log(isFacingRight);
             velo = new Vector3(-1f * speed, 0, 0);
             transform.Translate(velo);
             alive = true;

         }
         else if (!isFacingRight && !alive)
         {
             //Debug.Log(isFacingRight);
             velo = new Vector3(speed, 0, 0);
             transform.Translate(velo);

             alive = true;
         }
          else
         {
             //this ensures the bullets doesn't change direction 
             //after being fired.
             transform.Translate(velo);
             
         }
    }
    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
    // When this game object intersects a collider with 'is trigger' checked, 
    // store a reference to that collider in a variable named 'other'..
    void OnTriggerEnter2D(Collider2D other)
    {
        
        //Debug.Log("what");
        // ..and if the game object we intersect has the tag 'Pick Up' assigned to it..
        if (other.gameObject.CompareTag("killme") && gameObject.CompareTag("mothaFuckin_shot") == true)
        {
            //other = transform.parent.gameObject;
            //destroys enemy
            Destroy(other.transform.parent.gameObject);

            //destroys bullet
            Destroy(gameObject);
            
        }
    }
}
