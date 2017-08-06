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
		SceneManager.LoadScene(0);
    }

}
