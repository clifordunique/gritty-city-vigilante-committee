using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyBossHurtBox : MonoBehaviour {
	private int enemyHealth = 5; 

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.CompareTag("horzShot"))
		{
			Debug.Log ("Found player's shot"); 
			enemyHealth--; 
			if (enemyHealth <= 0) {
				Destroy(transform.parent.gameObject);
				GameObject enemySpawner = GameObject.FindWithTag ("enemy_spawner");
				enemySpawner.GetComponent<EnemySpawner>().enemyKilled ();
			}
		}
	}
}

