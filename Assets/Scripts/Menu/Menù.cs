using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menù : MonoBehaviour {

    public   Image        black;
    public   Animator     anim;
    public GameObject MainMenu;
    public GameObject LevelCanvas;


    // Use this for initialization
    void Start () {

		AudioManager.Initialize();
		AudioManager.PlayMusic("Menu_Theme");
		
	}

    public void NewGame()
    {

        anim.SetBool("Fade_out", false);
        anim.SetBool("Fade_in", true);
		anim.speed = 2;

        StartCoroutine(Fade_Out());
    }

    public void ExitGame ()
    {
        Application.Quit();
    }

    
    

    IEnumerator Fade_Out()
    {
        yield return new WaitUntil(() => black.color.a == 1);
        SceneManager.LoadScene(1);
        
    }
    
    public void SelectNightmare()
    {
        MainMenu.SetActive(false);
        LevelCanvas.SetActive(true);

    }
    
}
