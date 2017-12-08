using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using Prime31;


public class EnemyBossHurtBox : MonoBehaviour {
	public Slider bossHpBar;
	private float HP;
	private float maxHP;
	private float HPrec;
	private float HPdiff;
	public GameObject player_hurt_box; 


	void Awake()
	{
		bossHpBar = GameObject.FindGameObjectWithTag("bossHpSlider").GetComponent<Slider>();
		bossHpBar.gameObject.SetActive(false);
		maxHP = 100;
		HP = 100; 
		HPrec = 10;
		bossHpBar.value = 100; 
		player_hurt_box = GameObject.FindGameObjectWithTag("player_hurt");

	}

    private void Update()
    {
		if (player_hurt_box.GetComponent<PlayerStatsAndStates> ()._isDead == true) 
		{
			Destroy(transform.parent.gameObject);
		}
		HPdiff = maxHP - HP;
        //should make a function to handle this but make sure 
        //that it's checking health every frame
        if (HP <= 0)
        {
			bossHpBar.gameObject.SetActive(false);
			GameObject camera = GameObject.FindGameObjectWithTag ("MainCamera"); 
			camera.GetComponent<cameraController> ().setBossDied (); 
			Destroy(transform.parent.gameObject);
        }

    }
    void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.CompareTag("horzShot"))
		{
			bossHpBar.gameObject.SetActive(true);
			Debug.Log ("Found player's shot"); 
			HP-=10; 
			bossHpBar.value = healthPercent();
			
		}
        if (other.gameObject.CompareTag("super_shot"))
        {
			bossHpBar.gameObject.SetActive(true);
			HP -= 15;
        }
	}



	float healthPercent()
	{
		return HP / maxHP;
	}
}

