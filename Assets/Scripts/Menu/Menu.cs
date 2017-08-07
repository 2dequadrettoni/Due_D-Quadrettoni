using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public partial class Menu : MonoBehaviour {

	static	public	bool	bGameStarted	= false;

	public	GameObject		Menu_BlackScreen;
	private	Image			Menu_BlackScreenImage;

	public	GameObject		LevelSelection_BlackScreen;
	private	Image			LevelSelection_BlackScreenImage;

	public	GameObject		Loading_BlackScreen;
	private	Image			Loading_BlackScreenImage;

	public	GameObject		Cutscene_BlackScreen;
	private	Image			Cutscene_BlackScreenImage;
	private	Image			Cutscene_BigImage;

	public	GameObject		Logo_BlackScreen;
	private	Image			Logo_BlackScreenImage;

	public GameObject		MainMenuScreen;
	public GameObject		LevelSelectionScreen;
	public GameObject		LoadingScreen;
	public GameObject		CutsceneScreen;


	private	bool			bEnabled		= false;

	private int				iLevelToLoad	= 0;


	// Use this for initialization
	void Start () {

		AudioManager.Initialize();

		if ( !bGameStarted ) {
			MainMenuScreen.SetActive( false );
			LevelSelectionScreen.SetActive( false );
			LoadingScreen.SetActive( false );
			CutsceneScreen.SetActive( false );
			Logo_BlackScreen.SetActive( true );
		}
		else{
			MainMenuScreen.SetActive( true );
			LevelSelectionScreen.SetActive( false );
			LoadingScreen.SetActive( false );
			CutsceneScreen.SetActive( false );
			Logo_BlackScreen.SetActive( false );
		}

		Menu_BlackScreenImage						= Menu_BlackScreen.GetComponent<Image>();

		LevelSelection_BlackScreenImage				= LevelSelection_BlackScreen.GetComponent<Image>();
		Loading_BlackScreenImage					= Loading_BlackScreen.GetComponent<Image>();
		Cutscene_BlackScreenImage					= Cutscene_BlackScreen.GetComponent<Image>();
		Cutscene_BigImage							= CutsceneScreen.transform.GetChild( 0 ).GetComponent<Image>();
		Logo_BlackScreenImage						= Logo_BlackScreen.GetComponent<Image>();

		if ( vCutsceneSprites == null ) {

			vCutsceneSprites = new List<Sprite>();

			vCutsceneSprites.Add( Resources.Load<Sprite>("Cutscene/Starting/00") );
			vCutsceneSprites.Add( Resources.Load<Sprite>("Cutscene/Starting/01") );
			vCutsceneSprites.Add( Resources.Load<Sprite>("Cutscene/Starting/02") );
			vCutsceneSprites.Add( Resources.Load<Sprite>("Cutscene/Starting/03") );
			vCutsceneSprites.Add( Resources.Load<Sprite>("Cutscene/Starting/04") );
			vCutsceneSprites.Add( Resources.Load<Sprite>("Cutscene/Starting/05") );

		}



		if ( bGameStarted ) {

			
			AudioManager.PlayMusic("Menu_Theme");

			StartCoroutine( Menu_BlackImage_FadeOut() );

			return;
		}


		ShowLogo();

		bGameStarted = true;

	}



	public void NewGame()
	{	
		if ( !bEnabled ) return;

		Menu_BlackScreenImage.enabled = true;
		iLevelToLoad = 1;
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

#if UNITY_EDITOR
			UnityEditor.EditorApplication.isPlaying = false;
#else
			Application.Quit();
#endif
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

		MainMenuScreen.SetActive( false );
		LevelSelectionScreen.SetActive( false );
		LoadingScreen.SetActive( false );
		CutsceneScreen.SetActive( true );
		StartCutscene();

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
		Menu_BlackScreenImage.enabled = false;

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
	
}
