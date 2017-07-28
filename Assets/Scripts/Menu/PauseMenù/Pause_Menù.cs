using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Pause_Menù : MonoBehaviour {

    public GameObject myCanvas;

	// Use this for initialization
	void Start () {
       
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ResumeNightmare() {

        myCanvas.SetActive(false);
    }

    public void EndNightmare()
    {
        myCanvas.SetActive(false);
        SceneManager.LoadScene(0);

    }

}
