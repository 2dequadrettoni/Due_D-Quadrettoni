using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour {

	private	enum	MenuScreenChilds { BACKGROUND, LOGO, BUTTON_NEWGAME, BUTTON_LEVELSELECTION, BUTTON_QUIT, BLACKSCREEN }

//	private	enum	LevelSelectionScreenChilds {  }


	public	GameObject		Menu_BlackScreen;
    private	Image			Menu_BlackScreenImage;
    private	Animator		Menu_BlackScreenImage_Animator;

	public	GameObject		LevelSelection_BlackScreen;
	private	Image			LevelSelection_BlackScreenImage;
    private	Animator		LevelSelection_BlackScreenImage_Animator;

    public GameObject		MainMenuScreen;
    public GameObject		LevelSelectionScreen;
	public GameObject		LoadingScreen;


    // Use this for initialization
    void Start () {

		MainMenuScreen.SetActive( true );
        LevelSelectionScreen.SetActive( false );
		LoadingScreen.SetActive( false );

		Menu_BlackScreenImage			= Menu_BlackScreen.GetComponent<Image>();
		Menu_BlackScreenImage_Animator	= Menu_BlackScreen.GetComponent<Animator>();

		LevelSelection_BlackScreenImage	= LevelSelection_BlackScreen.GetComponent<Image>();
		LevelSelection_BlackScreenImage_Animator = LevelSelection_BlackScreen.GetComponent<Animator>();


		AudioManager.Initialize();
		AudioManager.PlayMusic("Menu_Theme");

		Menu_BlackScreenImage.gameObject.gameObject.SetActive( true );
		Menu_BlackScreenImage.enabled = true;
		Menu_BlackScreenImage_Animator.enabled = true;
		Menu_BlackScreenImage_Animator.Play( "Fade_Out", 0, 0.0f );

		

	}



    public void NewGame()
    {

		Menu_BlackScreenImage.enabled = true;
		Menu_BlackScreenImage_Animator.enabled = true;
		Menu_BlackScreenImage_Animator.Play( "Fade_In", 0, 0.0f );

        StartCoroutine( Fade_Out() );
    }



    public void ExitGame ()
    {
        Application.Quit();
    }




    IEnumerator Fade_Out()
    {
        yield return new WaitUntil( () => Menu_BlackScreenImage.color.a == 1 );

		MainMenuScreen.SetActive( false );
        LevelSelectionScreen.SetActive( false );
		LoadingScreen.SetActive( true );

		yield return new WaitForSecondsRealtime( 3.0f );

		MainMenuScreen.SetActive( true );
        LevelSelectionScreen.SetActive( false );
		LoadingScreen.SetActive( false );

        SceneManager.LoadScene(1);
        
		yield return null;
    }
    

    public void SelectNightmare()
    {
        MainMenuScreen.SetActive(false);
        LevelSelectionScreen.SetActive(true);
		LoadingScreen.SetActive( false );

    }
    
}
