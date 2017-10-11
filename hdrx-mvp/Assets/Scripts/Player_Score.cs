using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player_Score : MonoBehaviour {

    private float timeLeft = 120;
    public int playerScore = 0;
    public GameObject timeLeftUI;
    public GameObject playerScoreUI;

	void Update () {
        timeLeft -= Time.deltaTime;
        timeLeftUI.gameObject.GetComponent<Text>().text = ("Time Left: " + (int)timeLeft);
        playerScoreUI.gameObject.GetComponent<Text>().text = ("Score: " + playerScore);

        if (timeLeft < 1)
        {
            SceneManager.LoadScene("Prototype_1");
        }
	}

    private void OnTriggerEnter2D(Collider2D trig)
    {
        if (trig.gameObject.name == "EndLevel") {
            CountScore();
        }
        if (trig.gameObject.name == "Coin")
        {
            playerScore += 10;
            Destroy(trig.gameObject);
        }
    }

    void CountScore()
    {
        playerScore = playerScore + (int)(timeLeft * 10);
    }
}
