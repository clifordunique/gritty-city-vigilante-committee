using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IgnoreCollions : MonoBehaviour {

    private Collider2D _hurtBox;
    public bool test = true;
	// Use this for initialization
	void Start () {
        _hurtBox = GetComponent<BoxCollider2D>(); ;
	}
	
	// Update is called once per frame
	void Update () {


    }

    void OnTriggerEnter2D(Collider2D enemy)
    {

        if (enemy.gameObject.tag == "enemy")
        {
            Debug.Log("enemy collision");
            Physics2D.IgnoreCollision(enemy, _hurtBox, test);
        }
    }
}


