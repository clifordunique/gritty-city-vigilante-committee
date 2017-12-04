using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyBossHurtBox : MonoBehaviour {
	private int enemyHealth = 2;
    private void Update()
    {
        //should make a function to handle this but make sure 
        //that it's checking health every frame
        if (enemyHealth <= 0)
        {
			GameObject camera = GameObject.FindGameObjectWithTag ("MainCamera"); 
			camera.GetComponent<cameraController> ().setBossDied (); 
			Destroy(transform.parent.gameObject);
//            GameObject enemySpawner = GameObject.FindWithTag("enemy_spawner");
//            enemySpawner.GetComponent<EnemySpawner>().enemyKilled();
        }
    }
    void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.CompareTag("horzShot"))
		{
        
			Debug.Log ("Found player's shot"); 
			enemyHealth--; 
			
		}
        if (other.gameObject.CompareTag("super_shot"))
        {
            enemyHealth -= 5;
        }
	}
}

