using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManagement : MonoBehaviour {

	// Use this for initialization
	void Start () {
        PlayerPrefs.SetInt("HighScore", 10);
        PlayerPrefs.GetInt("HighScore");
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
