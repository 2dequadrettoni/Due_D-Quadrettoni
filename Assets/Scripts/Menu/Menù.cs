using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menù : MonoBehaviour {

    public   Image        black;
    public   Animator     anim;
/*
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
        if (black.color.a == 0)
        {
            black.enabled = false;
        }

	}
    */
   public void NewGame()
    {
    //    black.enabled = true;

        if (black.color.a == 1)
        {
            anim.SetBool("Fade_out", true);
            anim.SetBool("Fade_in", false);

        }
        else
        {
            anim.SetBool("Fade_in", true);
            anim.SetBool("Fade_out", false);

        }
        StartCoroutine(Fading());
    }

    public void ExitGame ()
    {
        Application.Quit();
    }

    
    

    IEnumerator Fading()
    {
        yield return new WaitUntil(() => black.color.a == 1);
        SceneManager.LoadScene(0);
        
    }
    

    
}
