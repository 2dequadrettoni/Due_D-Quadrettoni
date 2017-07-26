using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSwitch : MonoBehaviour {

	private		bool			bPlayer1Arrived		= false;
	private		bool			bPlayer2Arrived		= false;

	private void OnTriggerEnter( Collider other ) {
		
		// TODO: DISALBE PLAYER INPUT AND AUTO SET A WAIT ACTION UNTIL MAX STAGES IS REACHED OR P2 REACH EXITPOINT

		if ( !GLOBALS.StageManager.IsPlaying ) return;

		if ( other.name == "Player1" ) {
			bPlayer1Arrived = true;
		}

		if ( other.name == "Player2" ) {
			bPlayer2Arrived = true;
		}

		if ( bPlayer1Arrived && bPlayer2Arrived ) {
			
			GLOBALS.UI.ShowLvlCompletedMsg();

			if ( GLOBALS.StageManager.IsPlaying )
				GLOBALS.StageManager.Stop();

		}

	}

}
