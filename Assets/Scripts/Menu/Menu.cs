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

	public	GameObject		Loading_BlackScreen;
	private	Image			Loading_BlackScreenImage;
	private	Animator		Loading_BlackScreenImage_Animator;

	public GameObject		MainMenuScreen;
	public GameObject		LevelSelectionScreen;
	public GameObject		LoadingScreen;

	private	bool			bEnabled		= false;


	// Use this for initialization
	void Start () {

		MainMenuScreen.SetActive( true );
		LevelSelectionScreen.SetActive( false );
		LoadingScreen.SetActive( false );

		Menu_BlackScreenImage						= Menu_BlackScreen.GetComponent<Image>();

		LevelSelection_BlackScreenImage				= LevelSelection_BlackScreen.GetComponent<Image>();
		LevelSelection_BlackScreenImage_Animator	= LevelSelection_BlackScreen.GetComponent<Animator>();

		Loading_BlackScreenImage					= Loading_BlackScreen.GetComponent<Image>();
		Loading_BlackScreenImage_Animator			= Loading_BlackScreen.GetComponent<Animator>();


		AudioManager.Initialize();
		AudioManager.PlayMusic("Menu_Theme");

		StartCoroutine( Menu_BlackImage_FadeOut() );

	}



	public void NewGame()
	{	
		if ( !bEnabled ) return;
		print( "enabled" );
		Menu_BlackScreenImage.enabled = true;
		StartCoroutine( Menu_BlackImage_FadeIn() );

	}



	public void SelectNightmare()
	{

		if ( !bEnabled ) return;

		StartCoroutine( Selector_BlackImage_FadeOut() );

	}



	public void ExitGame ()
	{
		if ( !bEnabled ) return;
		Application.Quit();
	}


	public void Back()
	{
		if (!bEnabled) return;

		StartCoroutine(Selector_BlackImage_FadeOutToMenu());

	}



	public void Loadlevel(int index)
	{
		LevelSelection_BlackScreenImage.enabled = true;
		StartCoroutine(Selector_BlackImage_FadeIn(index));
	}







	void OnFadeInCompleted() {

		SceneManager.LoadScene( 1 );

	}

	void	OnFadeOutCompleted() {
		bEnabled = true;
		Menu_BlackScreenImage.raycastTarget = false;
		Menu_BlackScreenImage.enabled = false;

	}











	// menu che scompare al new game
	IEnumerator Menu_BlackImage_FadeIn () {

		Menu_BlackScreenImage.raycastTarget = true;

		yield return new WaitForEndOfFrame();

		while ( Menu_BlackScreenImage.color.a < 1 ) {

			float i = Menu_BlackScreenImage.color.a + ( Time.deltaTime * 3 );
			Menu_BlackScreenImage.color = new Color(1, 1, 1, i);
			yield return null;

		}

		Menu_BlackScreenImage.color = new Color(1, 1, 1, 1);

		MainMenuScreen.SetActive( false );
		LevelSelectionScreen.SetActive( false );
		LoadingScreen.SetActive( true );




		// LOADING SCREEN




		Loading_BlackScreenImage.enabled = true;
		Menu_BlackScreenImage.raycastTarget = false;

		yield return new WaitForEndOfFrame();

		while (Loading_BlackScreenImage.color.a > 0)
		{

			float i = Loading_BlackScreenImage.color.a - ( Time.deltaTime * 3 );
			Loading_BlackScreenImage.color = new Color(1, 1, 1, i);
			yield return null;

		}

		Loading_BlackScreenImage.color = new Color(1, 1, 1, 0);


		yield return new WaitForSecondsRealtime(Random.Range(4, 7));

		OnFadeInCompleted();

	}






	// menu che compare
	IEnumerator Menu_BlackImage_FadeOut () {

		yield return new WaitForEndOfFrame();

		while ( Menu_BlackScreenImage.color.a > 0 ) {
			
			float i = Menu_BlackScreenImage.color.a - ( Time.deltaTime * 3 );
			Menu_BlackScreenImage.color = new Color(1, 1, 1, i);
			yield return null;

		}

		Menu_BlackScreenImage.color = new Color(1, 1, 1, 0);

		OnFadeOutCompleted();

	}










	// selector che scompare al caricamento
	IEnumerator Selector_BlackImage_FadeIn( int scene_index )
	{

		yield return new WaitForEndOfFrame();

		while (LevelSelection_BlackScreenImage.color.a < 1)
		{

			float i = LevelSelection_BlackScreenImage.color.a + (Time.deltaTime * 3);
			LevelSelection_BlackScreenImage.color = new Color(1, 1, 1, i);
			yield return null;

		}

		LevelSelection_BlackScreenImage.color = new Color(1, 1, 1, 1);

		MainMenuScreen.SetActive(false);
		LevelSelectionScreen.SetActive(false);
		LoadingScreen.SetActive(true);


		// LOADING SCREEN


		Loading_BlackScreenImage.enabled = true;
		Menu_BlackScreenImage.raycastTarget = false;

		yield return new WaitForEndOfFrame();

		while (Loading_BlackScreenImage.color.a > 0)
		{

			float i = Loading_BlackScreenImage.color.a - ( Time.deltaTime * 3 );
			Loading_BlackScreenImage.color = new Color(1, 1, 1, i);
			yield return null;

		}

		Loading_BlackScreenImage.color = new Color(1, 1, 1, 0);


		yield return new WaitForSecondsRealtime(Random.Range(4, 7));




		SceneManager.LoadScene(scene_index);

	}




	// selector che compare
	IEnumerator Selector_BlackImage_FadeOut () {


		yield return new WaitForEndOfFrame();

		Menu_BlackScreenImage.enabled = true;
		LevelSelection_BlackScreenImage.raycastTarget = true;

		Menu_BlackScreenImage.color = new Color(1, 1, 1, 0);

		while ( Menu_BlackScreenImage.color.a < 1 ) {
			
			float i = Menu_BlackScreenImage.color.a + ( Time.deltaTime * 3 );
			Menu_BlackScreenImage.color = new Color(1, 1, 1, i);
			yield return null;

		}

		Menu_BlackScreenImage.color = new Color(1, 1, 1, 1);

		Menu_BlackScreenImage.enabled = false;
		LevelSelection_BlackScreenImage.raycastTarget = false;


		// SHOE SELECTOR


		LevelSelection_BlackScreenImage.enabled = true;
		LevelSelection_BlackScreenImage.raycastTarget = true;

		yield return new WaitForEndOfFrame();

		MainMenuScreen.SetActive(false);
		LevelSelectionScreen.SetActive(true);
		LoadingScreen.SetActive( false );

		LevelSelection_BlackScreenImage.color = new Color(1, 1, 1, 1);

		while ( LevelSelection_BlackScreenImage.color.a > 0 ) {
			
			float i = LevelSelection_BlackScreenImage.color.a - ( Time.deltaTime * 3 );
			LevelSelection_BlackScreenImage.color = new Color(1, 1, 1, i);
			yield return null;

		}

		LevelSelection_BlackScreenImage.color = new Color(1, 1, 1, 0);

		LevelSelection_BlackScreenImage.raycastTarget = false;
		LevelSelection_BlackScreenImage.enabled = false;

	}





	// selector che scompare per il menu
	IEnumerator Selector_BlackImage_FadeOutToMenu () {

		LevelSelection_BlackScreenImage.enabled = true;
		LevelSelection_BlackScreenImage.raycastTarget = true;

		yield return new WaitForEndOfFrame();

		while ( LevelSelection_BlackScreenImage.color.a < 1 ) {
			
			float i = LevelSelection_BlackScreenImage.color.a + ( Time.deltaTime * 3 );
			LevelSelection_BlackScreenImage.color = new Color(1, 1, 1, i);
			yield return null;

		}

		LevelSelection_BlackScreenImage.color = new Color(1, 1, 1, 1);



		// SHOW MENU
		SceneManager.LoadScene(0);

	}
	
}
