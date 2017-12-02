using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHurtBox : MonoBehaviour {
	private int enemyHealth = 3; 

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.CompareTag("horzShot"))
		{
			Debug.Log ("Found player's shot"); 
			enemyHealth--; 
			if (enemyHealth <= 0) {
				Destroy(transform.parent.gameObject);
			}
		}
	}
}
