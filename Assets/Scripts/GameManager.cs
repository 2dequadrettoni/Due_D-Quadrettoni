using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour {

	private		FinalTile	F1				= null;
	private		FinalTile	F2				= null;


	private void Start() {
		
		F1 = transform.GetChild( 0 ).GetComponent<FinalTile>();
		F2 = transform.GetChild( 1 ).GetComponent<FinalTile>();

		F1.iDesiredPlayerID = 1;
		F2.iDesiredPlayerID = 2;

		int index = SceneManager.GetActiveScene().buildIndex;
		if ( index == 0 ) {
			UI.TutorialLvl				= true;
			Player.TutorialSequence		= true;
			Door.TutorialLvl			= true;
			FinalTile.TutorialLvl		= true;
			Switcher.TutorialLvl_Plane	= true;
		}
		else {
			UI.TutorialLvl				= false;
			Player.TutorialSequence		= false;
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



	private void Update() {


		if ( Input.GetKeyDown( KeyCode.Escape ) ) Application.Quit();


		if ( GLOBALS.StageManager.IsPlaying && F1.IsInside && F2.IsInside ) {

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
