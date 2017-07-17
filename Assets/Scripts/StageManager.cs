
using System.Collections.Generic; // List
using UnityEngine;

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
	void	SetAction();

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
	private		bool				bIsInCycle		= false;

	private		bool				bIsOK			= false;

	private		void	Start() {
		
		// if Player is not found then cannot execute play action
		GameObject _P1 = GameObject.Find( "Player1" );
		if ( ( !_P1 ) || !( pPlayer1 = _P1.GetComponent<Player>() ) ) return;

		GameObject _P2 = GameObject.Find( "Player2" );
		if ( ( !_P2 ) || !( pPlayer2 = _P2.GetComponent<Player>() ) ) return;

		// Create Stage list, and first stage
		vStages = new List<Stage>();
		vStages.Add( new Stage() );

		// Fill up with empty stage
//		for( int i = 0; i < iTotalStages; i++ ) vStages.Add (new Stage() );

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

		Debug.Log( "Level completed" );

		return false;

	}

}
