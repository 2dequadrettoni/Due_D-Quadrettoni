using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public partial class Menu : MonoBehaviour {

	public GameObject		CanvasLogo				= null;
	public GameObject		CanvasMainMenu			= null;
	public GameObject		CanvasSelector			= null;
	public GameObject		CanvasLoading			= null;
	public GameObject		CanvasCutscene			= null;


	enum Menu_Canvas{ LOGO, MAINMENU, SELECTOR, LOADING, CUTSCENE }
	private	Menu_Canvas iCurrentCanvas				= 0;

	const	float			TRANSITION_TIME			= 1.0f;

	static	public	bool	bGameStarted			= false;

	private	Image			Cutscene_BigImage		= null;

	private	Animator		Logo_Animator			= null;

	private	bool			bMenuEnabled			= false;

	private int				iLevelToLoad			= 0;


	// Use this for initialization
	void Start () {

		Cutscene_BigImage	= CanvasCutscene.transform.GetChild( 0 ).GetComponent<Image>();
		Logo_Animator		= CanvasLogo.transform.GetChild( 1 ).GetComponent<Animator>();


		if ( vCutsceneSprites == null ) {

			vCutsceneSprites = new List<Sprite>();

			vCutsceneSprites.Add( Resources.Load<Sprite>("Cutscene/Starting/00") );
			vCutsceneSprites.Add( Resources.Load<Sprite>("Cutscene/Starting/01") );
			vCutsceneSprites.Add( Resources.Load<Sprite>("Cutscene/Starting/02") );
			vCutsceneSprites.Add( Resources.Load<Sprite>("Cutscene/Starting/03") );
			vCutsceneSprites.Add( Resources.Load<Sprite>("Cutscene/Starting/04") );
			vCutsceneSprites.Add( Resources.Load<Sprite>("Cutscene/Starting/05") );

		}

		if ( !bGameStarted ) {
			ShowCanvas( Menu_Canvas.LOGO );
			ShowLogo();
			bGameStarted = true;
			bMenuEnabled = false;
			return;
		}
		else{
			ShowCanvas( Menu_Canvas.MAINMENU );
		}

		StartCoroutine( Fader.Show( 3, () => bMenuEnabled = true ) );

	}

	private void Update() {
		
		if ( Input.GetKeyDown( KeyCode.Escape ) && iCurrentCanvas == Menu_Canvas.LOGO ) {

			if ( AudioManager.Loaded && bMenuEnabled == false ) {

				print( "showing menu" );

				Fader.Hide();

				StopAllCoroutines();

				pLogoSource.Stop();
				Destroy( pLogoSource.gameObject );

				ShowCanvas( Menu_Canvas.MAINMENU );
				AudioManager.FadeInMusic( "Menu_Theme" );

				// SHOW MENU, ENABLE IT AT END
				StartCoroutine( Fader.Show( TRANSITION_TIME, () => bMenuEnabled = true ) );

			}

		}

	}

	private	void	ShowCanvas( Menu_Canvas i ) {
		
		iCurrentCanvas = i;
		CanvasLogo.SetActive(				( i == Menu_Canvas.LOGO		) ? true : false );
		CanvasMainMenu.SetActive(			( i == Menu_Canvas.MAINMENU ) ? true : false );
		CanvasSelector.SetActive(			( i == Menu_Canvas.SELECTOR ) ? true : false );
		CanvasLoading.SetActive(			( i == Menu_Canvas.LOADING  ) ? true : false );
		CanvasCutscene.SetActive(			( i == Menu_Canvas.CUTSCENE ) ? true : false );

	}

	public void NewGame() {

		if ( !bMenuEnabled ) return;
		iLevelToLoad = 1;
		StartCoroutine( Menu_To_Cutscene() );

	}



	public void SelectNightmare() {

		if ( !bMenuEnabled ) return;
		StartCoroutine( Menu_To_Selector() );

	}



	public void ExitGame () {

		if ( !bMenuEnabled ) return;
#if UNITY_EDITOR
			UnityEditor.EditorApplication.isPlaying = false;
#else
			Application.Quit();
#endif
	
	}


	public void Back() {

		if (!bMenuEnabled) return;
		StartCoroutine(Selector_To_Menu());

	}



	public void Loadlevel(int index) {

		StartCoroutine( Selector_To_Loading_To_Game( index ) );

	}




	// menu che scompare al new game
	IEnumerator Menu_To_Cutscene () {

		StartCoroutine( Fader.Hide( TRANSITION_TIME ) );
		while ( !Fader.FadeCompleted ) yield return null;

		// LOADING SCREEN
		ShowCanvas( Menu_Canvas.LOADING );

		StartCoroutine( Fader.Show( TRANSITION_TIME ) );
		yield return new WaitForSecondsRealtime( Random.Range( 4, 7 ) );

		StartCoroutine( Fader.Hide( 0.8f ) );
		while ( !Fader.FadeCompleted ) yield return null;

		ShowCanvas( Menu_Canvas.CUTSCENE );
		StartCutscene();

	}

	IEnumerator Menu_To_Selector () {


		StartCoroutine( Fader.Hide( TRANSITION_TIME ) );
		while ( !Fader.FadeCompleted ) yield return null;

		// SHOW SELECTOR
		ShowCanvas( Menu_Canvas.SELECTOR );

		StartCoroutine( Fader.Show( TRANSITION_TIME ) );
		while ( !Fader.FadeCompleted ) yield return null;

	}
	
}
