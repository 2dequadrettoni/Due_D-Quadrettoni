using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour {

	FinalTile F1 = null;
	FinalTile F2 = null;

	private void Start() {
		
		F1 = transform.GetChild( 0 ).GetComponent<FinalTile>();
		F2 = transform.GetChild( 1 ).GetComponent<FinalTile>();

		F1.iDesiredPlayerID = 1;
		F2.iDesiredPlayerID = 2;

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
