using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Pause_Menu : MonoBehaviour {

    //public GameObject pause;
    public GameObject areYouSure;

    public void ResumeNightmare() {

       // pause.SetActive(false);
        areYouSure.SetActive(false);
        GLOBALS.UI.OnPause();
    }


	private void Update() {
		
		if ( Input.GetKeyDown( KeyCode.Escape ) ) No();

	}

	public void EndNightmare()
    {
       // pause.SetActive(false);
        areYouSure.SetActive(true);
        

    }

    public void No()
    {
        areYouSure.SetActive(false);
        //pause.SetActive(true);
    }

    public void Yes()
    {
		areYouSure.SetActive(false);
        Time.timeScale = GLOBALS.GameTime;
        GLOBALS.TutorialOverride = false;
        GameManager.InTutorialSequence = false;
        GameManager.TutorialStep = 0;
        SceneManager.LoadScene(0);
    }

}
