using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class triggerMan : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    void OnTriggerEnter(Collider other)
    {
        Debug.Log("hit");
        // ..and if the game object we intersect has the tag 'Pick Up' assigned to it..
        if (other.gameObject.tag == "mothaFuckin_shot")
        {
            Debug.Log("do it");
            // Make the other game object (the pick up) inactive, to make it disappear
            //other.gameObject.SetActive(false);
            Destroy(gameObject, .5f);
            // Add one to the score variable 'count'
            //count = count + 1;

            // Run the 'SetCountText()' function (see below)
            //SetCountText();
        }
    }
}

