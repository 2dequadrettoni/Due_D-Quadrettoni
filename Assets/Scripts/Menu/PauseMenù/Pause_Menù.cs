using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Pause_Menù : MonoBehaviour {

    public GameObject pause;
    public GameObject areYouSure;


    // Use this for initialization
    void Start () {
       
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ResumeNightmare() {

        pause.SetActive(false);
        areYouSure.SetActive(false);
    }

    public void EndNightmare()
    {
        pause.SetActive(false);
        areYouSure.SetActive(true);
        

    }

    public void No()
    {
        areYouSure.SetActive(false);
        pause.SetActive(true);
    }

    public void Yes()
    {
        SceneManager.LoadScene(0);
    }

}
