using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


struct GO_Position {
	public GameObject	o;
	public Vector3		Position;
	private bool		bDone;
	public GO_Position( GameObject o, Vector3 Position ) {
		this.o = o;
		this.Position = Position;
		bDone = false;
	}
	public bool IsDone() { return bDone; }
	public void SetDone() { bDone = true; }
}



public class GameManager : MonoBehaviour {

	private		FinalTile		pFinalTile1						= null;
	private		FinalTile		pFinalTile2						= null;

	public static bool			InTutorialSequence				= false;
	public static int			TutorialStep					= 0;

	[Header("Tutorial Sprites")]
	public		Sprite			pTutorial_0_MoveSprite			= null;
	public		Sprite			pTutorial_1_SwitchSprite		= null;
	public		Sprite			pTutorial_2_NextStageSprite		= null;
	public		Sprite			pTutorial_3_PlaySprite			= null;
	public		Sprite			pTutorial_4_RestartSprite		= null;

	List<GO_Position> vObjects;

	private void Start() {
		
		pFinalTile1 = transform.GetChild( 0 ).GetComponent<FinalTile>();
		pFinalTile2 = transform.GetChild( 1 ).GetComponent<FinalTile>();

		pFinalTile1.iDesiredPlayerID = 1;
		pFinalTile2.iDesiredPlayerID = 2;

		int iSceneIndex = SceneManager.GetActiveScene().buildIndex;
		
		// Level Music
		AudioSource p = AudioManager.FindMusic( "Level" + iSceneIndex );
		if ( !p.isPlaying ) {

			AudioManager.StopAllMusics();

			p.loop= true;
			AudioManager.FadeInMusic( p, 5.0f );

		}

		GLOBALS.CurrentLevel = iSceneIndex;

		// Tutorials
		if ( iSceneIndex == 1 ) {
			InTutorialSequence			= true;
			UI.TutorialLvl				= true;
			Door.TutorialLvl			= true;
			FinalTile.TutorialLvl		= true;
			Switcher.TutorialLvl_Plane	= true;
			if ( TutorialStep < 4 )
				GLOBALS.TutorialOverride	= true;
			else {
				GLOBALS.TutorialOverride	= false;
			}
		}
		else {
			InTutorialSequence			= false;
			UI.TutorialLvl				= false;
			Door.TutorialLvl			= false;
			FinalTile.TutorialLvl		= false;
			Switcher.TutorialLvl_Plane	= false;
			GLOBALS.TutorialOverride	= false;
		}
		if ( iSceneIndex == 2 ) {
			Switcher.TutorialLvl		= true;
			Platform.TutorialLvl		= true;
		}
		else {
			Switcher.TutorialLvl		= false;
			Platform.TutorialLvl		= false;
		}
		if ( iSceneIndex == 6 ) {
			Key.TutorialLvl				= true;
		}
		else {
			Key.TutorialLvl				= false;
		}

		StartLevelComposition();

	}


	private	void	StartLevelComposition() {

		GameObject[] vKeys				= GameObject.FindGameObjectsWithTag("Key");
		GameObject[] vPlatforms			= GameObject.FindGameObjectsWithTag("Platform");
		GameObject[] vSwitchers			= GameObject.FindGameObjectsWithTag("Switcher");
		GameObject[] vSwitchers_Plane	= GameObject.FindGameObjectsWithTag("Plane_Switcher");
		GameObject[] vDoors				= GameObject.FindGameObjectsWithTag("Door");
		GameObject[] vTiles				= GameObject.FindGameObjectsWithTag("Tiles");
		GameObject[] vOthers			= GameObject.FindGameObjectsWithTag("Other");

		vObjects = new List<GO_Position>( vKeys.Length + vPlatforms.Length + vSwitchers.Length + vSwitchers_Plane.Length + vDoors.Length + vTiles.Length + vOthers.Length );

		{
			foreach( GameObject GO in vKeys				)	{ AddObj( GO ); }
			foreach( GameObject GO in vPlatforms		)	{ AddObj( GO ); }
			foreach( GameObject GO in vSwitchers		)	{ AddObj( GO ); }
			foreach( GameObject GO in vSwitchers_Plane	)	{ AddObj( GO ); }
			foreach( GameObject GO in vDoors			)	{ AddObj( GO ); }
			foreach( GameObject GO in vTiles			)	{ AddObj( GO ); }
			foreach( GameObject GO in vOthers			)	{ AddObj( GO ); }
		}

		StartCoroutine( LevelCompositionCoroutine() );

	}


	private void AddObj( GameObject o ) {

		GO_Position p = new GO_Position( o, o.transform.position );
		vObjects.Add( p );

		o.transform.position = new Vector3( Random.Range( -100, 100 ), Random.Range( -100, 100 ), Random.Range( -100, 100 ) );

	}

	IEnumerator LevelCompositionCoroutine() {

		int iCurrentObjectLeft = vObjects.Count;

		GLOBALS.Player1.pSpriteRenderer.enabled = false;
		GLOBALS.Player2.pSpriteRenderer.enabled = false;

		while ( iCurrentObjectLeft > 0 ) {
			foreach( GO_Position p in vObjects	) {

				if ( p.IsDone() ) continue;

				if ( Vector3.Distance( p.o.transform.position, p.Position ) > 0.001f ) {

					p.o.transform.position = Vector3.LerpUnclamped( p.o.transform.position, p.Position, Time.unscaledDeltaTime * 3.0f );
				}
				else {
					p.o.transform.position = p.Position;
					p.SetDone();
					iCurrentObjectLeft--;
				}

			}

			yield return null;
		}

		GLOBALS.Player1.pSpriteRenderer.enabled = true;
		GLOBALS.Player2.pSpriteRenderer.enabled = true;

		GLOBALS.Player1.Spawn();
		GLOBALS.Player2.Spawn();

	}

	public	void	NextTutorial() {

		TutorialStep++;
		switch( TutorialStep ) {

			case 1: {  GLOBALS.TutorialSlot.sprite = pTutorial_0_MoveSprite;		break; }
			case 2: {  GLOBALS.TutorialSlot.sprite = pTutorial_1_SwitchSprite;		break; }
			case 3: {  GLOBALS.TutorialSlot.sprite = pTutorial_2_NextStageSprite;	break; }
			case 4: {  GLOBALS.TutorialSlot.sprite = pTutorial_3_PlaySprite;		break; }
			case 5: {  GLOBALS.TutorialSlot.sprite = pTutorial_4_RestartSprite;		break; }
			default: { GLOBALS.TutorialSlot.sprite = null;							break; }
		}

	}



	private void Update() {


		if ( Input.GetKeyDown( KeyCode.Escape ) ) GLOBALS.UI.OnPause();


		if ( GLOBALS.StageManager.IsPlaying && pFinalTile1.IsInside && pFinalTile2.IsInside ) {

			SaveLoad.SaveLevel( GLOBALS.CurrentLevel );

			AudioManager.Play( "Level_End" );

			GLOBALS.StageManager.Stop( false );

			GLOBALS.Player1.IsInAnimationOverride = true;
			GLOBALS.Player1.PlayWinAnimation();

			GLOBALS.Player2.IsInAnimationOverride = true;
			GLOBALS.Player2.PlayWinAnimation();

			GLOBALS.UI.BlackScreenFadeIn();

		}

	}



	public void RestartGame() {

		if ( GameManager.InTutorialSequence && GameManager.TutorialStep < 5 ) return;

		AudioManager.StopAllSounds();
//		AudioManager.StopAllMusics();

		SceneManager.LoadScene ( SceneManager.GetActiveScene().name );

	}




	public void	Exit() {
#if UNITY_EDITOR
			UnityEditor.EditorApplication.isPlaying = false;
#else
			Application.Quit();
#endif
	}


	private void OnDestroy() {
		
		AudioManager.StopAllSounds();
		AudioManager.StopAllMusics();

	}


}
