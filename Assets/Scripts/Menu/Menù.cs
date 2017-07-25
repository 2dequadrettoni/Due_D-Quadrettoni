using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menù : MonoBehaviour {

    public   Image        black;
    public   Animator     anim;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

   public void NewGame()
    {
        black.enabled = true;
        StartCoroutine(Fading());
    }

    public void ExitGame ()
    {
        Application.Quit();
    }
    

    IEnumerator Fading()
    {
        anim.SetBool("Fade", true);
        yield return new WaitUntil(() => black.color.a == 1);
        SceneManager.LoadScene(0);
        
    }
}
