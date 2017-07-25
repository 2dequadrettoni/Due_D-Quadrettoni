using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSwitch : MonoBehaviour {

	private		bool			bPlayer1Arrived		= false;
	private		bool			bPlayer2Arrived		= false;
	[SerializeField]
	private		string			sNextScene			= "";

	private		bool			bShowWinMSg			= false;

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
			bShowWinMSg = true;
			if ( GLOBALS.StageManager.IsPlaying )
				GLOBALS.StageManager.Stop();

		}

	}



	Rect WindowRect = new Rect( Screen.width / 2f - 200, Screen.height / 2f - 50, 400, 100 );
	void OnGUI() {
		
		if ( bShowWinMSg ) {
			GUI.Window( 0, WindowRect, ShowGUI, "Completed" );
		}

	}


	void ShowGUI( int windowID ) {

		if ( SceneManager.sceneCount > ( SceneManager.GetActiveScene().buildIndex + 1 ) ) {

			if ( GUI.Button( new Rect( ( WindowRect.width / 6f ) - 50.0f, WindowRect.height / 1.5f, 100f, 20f ), "NEXT LEVEL" ) ) {
				SceneManager .LoadScene( SceneManager.GetActiveScene().buildIndex + 1 );
			}
		}
		else {

			if ( GUI.Button( new Rect( ( WindowRect.width / 2f ) + 50.0f, WindowRect.height / 1.5f, 100f, 20f ), "EXIT" ) ) {
 #if UNITY_EDITOR
				UnityEditor.EditorApplication.isPlaying = false;
#else
				Application.Quit();
#endif
			}

		}
        
    }







}
