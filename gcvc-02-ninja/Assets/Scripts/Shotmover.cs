using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotmover : playerController
{
    public float speed;
    private GameObject Player;
    private Vector3 velo;
   
    private bool alive = false;


    void Start () {
        //connects to the player/object to find direction 
        // that they are facing.
        Player = GameObject.FindWithTag("Player");
        Vector3 velo = Vector3.zero;
        speed = 6;
	}
	
	
	void Update () {
        //velo = new Vector3(-1f * speed * Time.deltaTime, 0, 0);
        //transform.Translate(velo);
        //checks the direction of player then gives the bullet a velocity
        if (isFacingRight && !alive)
         {
             Debug.Log(isFacingRight);
             velo = new Vector3(-1f * speed * Time.deltaTime, 0, 0);
             transform.Translate(velo);
             alive = true;

         }
         else if (!isFacingRight && !alive)
         {
             Debug.Log(isFacingRight);
             velo = new Vector3(speed * Time.deltaTime, 0, 0);
             transform.Translate(velo);

             alive = true;
         }
          else
         {
             //this ensures the bullets don't change direction 
             //after being fired.
             transform.Translate(velo);
             //Debug.Log("continue");
         }
    }
}
