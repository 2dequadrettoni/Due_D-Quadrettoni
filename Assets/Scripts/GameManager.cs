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

	}

	private void Update() {

		if ( Input.GetKeyDown( KeyCode.Escape ) ) Application.Quit();


		if ( F1.IsInside && F2.IsInside ) {

			GLOBALS.UI.ShowLvlCompletedMsg();

			if ( GLOBALS.StageManager.IsPlaying )
				GLOBALS.StageManager.Stop( false );

			GLOBALS.Player1.PlayWinAnimation();
			GLOBALS.Player2.PlayWinAnimation();

		}

	}

	public void RestartGame() {

		SceneManager.LoadScene ( SceneManager.GetActiveScene().name );

	}


	
}
