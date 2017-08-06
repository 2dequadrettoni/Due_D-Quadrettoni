using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour {


	public	GameObject		Menu_BlackScreen;
    private	Image			Menu_BlackScreenImage;
    private	Animator		Menu_BlackScreenImage_Animator;

	public	GameObject		LevelSelection_BlackScreen;
	private	Image			LevelSelection_BlackScreenImage;
    private	Animator		LevelSelection_BlackScreenImage_Animator;

    public GameObject		MainMenuScreen;
    public GameObject		LevelSelectionScreen;
	public GameObject		LoadingScreen;

	private	bool			bEnabled		= false;


    // Use this for initialization
    void Start () {

		MainMenuScreen.SetActive( true );
        LevelSelectionScreen.SetActive( false );
		LoadingScreen.SetActive( false );

		Menu_BlackScreenImage			= Menu_BlackScreen.GetComponent<Image>();

		LevelSelection_BlackScreenImage	= LevelSelection_BlackScreen.GetComponent<Image>();
		LevelSelection_BlackScreenImage_Animator = LevelSelection_BlackScreen.GetComponent<Animator>();

		AudioManager.Initialize();
		AudioManager.PlayMusic("Menu_Theme");

		StartCoroutine( BlackImage_FadeOut() );

	}



	public void NewGame()
    {	
		if ( !bEnabled ) return;
		print( "enabled" );
		Menu_BlackScreenImage.enabled = true;
        StartCoroutine( BlackImage_FadeIn() );

    }



	public void SelectNightmare()
    {

		if ( !bEnabled ) return;

		print("enabled");
		MainMenuScreen.SetActive(false);
        LevelSelectionScreen.SetActive(true);
		LoadingScreen.SetActive( false );

    }



    public void ExitGame ()
    {
		if ( !bEnabled ) return;
		Application.Quit();
    }




	void	OnFadeInCompleted() {

		SceneManager.LoadScene( 1 );

	}

	IEnumerator BlackImage_FadeIn () {

		Menu_BlackScreenImage.raycastTarget = true;

		yield return new WaitForEndOfFrame();

		yield return new WaitForSecondsRealtime( 1.5f );

		while ( Menu_BlackScreenImage.color.a < 1 ) {

			float i = Menu_BlackScreenImage.color.a + ( Time.deltaTime );
			Menu_BlackScreenImage.color = new Color(1, 1, 1, i);
			yield return null;

		}

		Menu_BlackScreenImage.color = new Color(1, 1, 1, 1);

		MainMenuScreen.SetActive( false );
		LevelSelectionScreen.SetActive( false );
		LoadingScreen.SetActive( true );

		yield return new WaitForSecondsRealtime( Random.Range( 7, 9 ) );

		OnFadeInCompleted();

	}

	void	OnFadeOutCompleted() {
		bEnabled = true;
		Menu_BlackScreenImage.raycastTarget = false;
		Menu_BlackScreenImage.enabled = false;

	}


	IEnumerator BlackImage_FadeOut () {

		yield return new WaitForEndOfFrame();


		yield return new WaitForSecondsRealtime( 1.7f );


		while ( Menu_BlackScreenImage.color.a > 0 ) {
			
			float i = Menu_BlackScreenImage.color.a - ( Time.deltaTime * 3 );
			Menu_BlackScreenImage.color = new Color(1, 1, 1, i);
			yield return null;

		}

		Menu_BlackScreenImage.color = new Color(1, 1, 1, 0);

		OnFadeOutCompleted();

	}
    
}
