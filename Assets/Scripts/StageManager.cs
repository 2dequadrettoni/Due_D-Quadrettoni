
using System.Collections;
using System.Collections.Generic; // List
using UnityEngine;
using UnityEngine.SceneManagement;

////////////////////////////////////////////////////////////////////////
//   GESTISCE LE AZIONI DI ENTRAMBI I GIOCATORI


interface IStageManager {

	/// Property	SelectedPlayer : Int

	////////////////////////////////////////////////////////////////////////
	//		PLAN MODE

	// Select player with that ID: 1 or 2 ( clmped value )
	void	SelectPlayer( int PlayerID );

	// Return Current stage
	Stage	GetCurrentStage();

	// Set the action for the current player in current stage
	void	SetAction( PlayerAction Action, int PlayerID );

	// Clear actions in current stage
	void	ClearStage();

	// Go to next stage
	void	NextStage();

	// Go to prev stage
	void	PrevStage();


	////////////////////////////////////////////////////////////////////////
	//		PLAY MODE

	// Play all actions
	void	Play();
}




public partial class StageManager : MonoBehaviour, IStageManager {


	private		List<Stage>			vStages			= null;
	private		int					iTotalStages	= 1;
	public		int TotalStages
	{
		get { return iTotalStages;  }
	}

	//////////////////////////////////////////////////////////////////////////////////////////////
	private		int					iCurrentStage	= 0;
	public		int StageCount
	{
		get { return iCurrentStage;  }
	}

	//////////////////////////////////////////////////////////////////////////////////////////////
	private		int					iSelectedPlayer	= 1;
	public		int SelectedPlayer
	{
		get { return iSelectedPlayer;  }
		set { SelectPlayer( value ); }
	}


	private		Player				pPlayer1		= null;
	private		Player				pPlayer2		= null;

	private		bool				bIsPlaying		= false;
	public		bool IsPlaying {
		get { return bIsPlaying; }
	}

	private		int					iActiveObjects	= 0;


	private		IEnumerator			pCoroutine		= null;

	private		bool				bIsInCycle		= false;

	private		bool				bIsOK			= false;

	private		UI					pUI				= null;
	public		UI	UI {
		get { return pUI; }
	}

	////////////////////////////////////////////////////////////////////
	// PLATFORMS

	Platform[] vPlatforms = null;


	private		void	Start() {

		// if Player is not found then cannot execute play action


		
		if ( !( pPlayer1 = GLOBALS.Player1 ) ) return;
		if ( !( pPlayer2 = GLOBALS.Player2 ) ) return;

		pUI = GLOBALS.UI;

		// Create Stage list, and first stage
		vStages = new List<Stage>();
		vStages.Add( new Stage() );

		// Get all platforms Scripts
		GameObject[] vPlatformObjs = GameObject.FindGameObjectsWithTag( "Platform" );
		vPlatforms = new Platform[vPlatformObjs.Length];
		for ( int i = 0; i < vPlatformObjs.Length; i++ ) {

			GameObject o = vPlatformObjs [ i ];
			o.transform.GetChild( 0 ).GetComponent<MeshRenderer>().enabled = false;
			o.transform.GetChild( 1 ).GetComponent<MeshRenderer>().enabled = false;
			vPlatforms [ i ] = o.GetComponent<Platform>();
		}

		Time.timeScale = 0.2f;

		// First stage set
		iCurrentStage = 0;

		pPlayer1.ID = 1;
		pPlayer2.ID = 2;

		// Player 1 start
		SelectPlayer( 1 );

		// Internal state
		bIsOK = true;

	}

	private		bool	CompletedLevelCheck() {

//		Debug.Log( "CompletedLevelCheck" );

		return false;

	}

}
