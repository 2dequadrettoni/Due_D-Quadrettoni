using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


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

	private void Start() {
		
		pFinalTile1 = transform.GetChild( 0 ).GetComponent<FinalTile>();
		pFinalTile2 = transform.GetChild( 1 ).GetComponent<FinalTile>();

		pFinalTile1.iDesiredPlayerID = 1;
		pFinalTile2.iDesiredPlayerID = 2;

		int index = SceneManager.GetActiveScene().buildIndex;
		if ( index == 0 ) {
			InTutorialSequence			= true;
			UI.TutorialLvl				= true;
			Door.TutorialLvl			= true;
			FinalTile.TutorialLvl		= true;
			Switcher.TutorialLvl_Plane	= true;
		}
		else {
			InTutorialSequence			= false;
			UI.TutorialLvl				= false;
			Door.TutorialLvl			= false;
			FinalTile.TutorialLvl		= false;
			Switcher.TutorialLvl_Plane	= false;
		}
		if ( index == 1 ) {
			Switcher.TutorialLvl		= true;
			Platform.TutorialLvl		= true;
		}
		else {
			Switcher.TutorialLvl		= false;
			Platform.TutorialLvl		= false;
		}
		if ( index == 5 ) {
			Key.TutorialLvl				= true;
		}
		else {
			Key.TutorialLvl				= false;
		}

	}

	public	void	NextTutorial( bool bOverride ) {

		TutorialStep++;
		print( TutorialStep );
		switch( TutorialStep ) {

			case 1: {  GLOBALS.TutorialSlot.sprite = pTutorial_0_MoveSprite;		break; }
			case 2: {  GLOBALS.TutorialSlot.sprite = pTutorial_1_SwitchSprite;		break; }
			case 3: {  GLOBALS.TutorialSlot.sprite = pTutorial_2_NextStageSprite;	break; }
			case 4: {  GLOBALS.TutorialSlot.sprite = pTutorial_3_PlaySprite;		break; }
			case 5: {  GLOBALS.TutorialSlot.sprite = pTutorial_4_RestartSprite;		break; }
			default: { GLOBALS.TutorialSlot.sprite = null;							break; }
		}

		GLOBALS.TutorialOverride = bOverride;

	}



	private void Update() {


		if ( Input.GetKeyDown( KeyCode.Escape ) ) Application.Quit();


		if ( GLOBALS.StageManager.IsPlaying && pFinalTile1.IsInside && pFinalTile2.IsInside ) {

			SaveLoad.SaveLevel( GLOBALS.CurrentLevel );

			GLOBALS.UI.ShowLvlCompletedMsg();

//			GLOBALS.AudioManager.Play( "EndLevel" );

			GLOBALS.StageManager.Stop( false );

			GLOBALS.Player1.IsInAnimationOverride = true;
			GLOBALS.Player1.PlayWinAnimation();

			GLOBALS.Player2.IsInAnimationOverride = true;
			GLOBALS.Player2.PlayWinAnimation();

		}

	}



	public void RestartGame() {

//		GLOBALS.AudioManager.StopAll();

		SceneManager.LoadScene ( SceneManager.GetActiveScene().name );

	}



	public void	Exit() {
#if UNITY_EDITOR
			UnityEditor.EditorApplication.isPlaying = false;
#else
			Application.Quit();
#endif
	}


	
}
