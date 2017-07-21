using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSwitch : MonoBehaviour {

	private	bool	bPlayer1Arrived		= false;
	private	bool	bPlayer2Arrived		= false;
	[SerializeField]
	private	string	sNextScene			= "";

	private void OnTriggerEnter( Collider other ) {
		
		// TODO: DISALBE PLAYER INPUT AND AUTO SET A WAIT ACTION UNTIL MAX STAGES IS REACHED OR P2 REACH EXITPOINT

		if ( other.name == "Player1" ) {
			bPlayer1Arrived = true;
		}

		if ( other.name == "Player2" ) {
			bPlayer2Arrived = true;
		}

		if ( bPlayer1Arrived && bPlayer1Arrived ) {

			// Switch to next level
			SceneManager.LoadScene( SceneManager.GetActiveScene().buildIndex + 1 );
			

		}

	}


}
