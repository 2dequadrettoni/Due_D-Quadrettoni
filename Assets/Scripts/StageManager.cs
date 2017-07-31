
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
	
	//////////////////////////////////////////////////////////////////////////////////////////////
	//////////////////////////////////////////////////////////////////////////////////////////////
	//					STAGES

	static		public int			MAX_STAGES		= 10;

	private		List<Stage>			vStages			= null;
	private		int					iTotalStages	= 1;
	public		int TotalStages
	{
		get { return iTotalStages;  }
	}

	private		int					iCurrentStage	= 0;
	public		int CurrentStage
	{
		get { return iCurrentStage;  }
	}



	//////////////////////////////////////////////////////////////////////////////////////////////
	//////////////////////////////////////////////////////////////////////////////////////////////
	//					PLAYERS
	private		int					iSelectedPlayer	= 0;
	public		int SelectedPlayer
	{
		get { return iSelectedPlayer;  }
		set { iSelectedPlayer = value; }
	}


	private		Player				pPlayer1		= null;
	private		Player				pPlayer2		= null;


	//////////////////////////////////////////////////////////////////////////////////////////////
	//////////////////////////////////////////////////////////////////////////////////////////////
	//					PLAN/PLAY MODE
	private		bool				bIsPlaying		= false;
	public		bool IsPlaying {
		get { return bIsPlaying; }
	}

	private		int					iActiveObjects	= 0;

	private		float				fUI_Interpolant	= 0.0f;
	private		bool				bPlayingStage	= false;


	private		bool				bIsOK			= false;

	////////////////////////////////////////////////////////////////////
	// PLATFORMS

	private		Platform[]			vPlatforms		= null;

	private		Sprite[]			vNumberSprites	= null;


	private		void	Start() {

		// Create Stage list, and first stage
		vStages = new List<Stage>();
		vStages.Add( new Stage() );

		// Get all platforms Scripts
		{
			GameObject[] vPlatformObjs = GameObject.FindGameObjectsWithTag( "Platform" );
			vPlatforms = new Platform[vPlatformObjs.Length];
			for ( int i = 0; i < vPlatformObjs.Length; i++ ) {

				GameObject o = vPlatformObjs [ i ];
				vPlatforms [ i ] = o.transform.GetChild(0).GetComponent<Platform>();
			}
		}

		// First stage set
		iCurrentStage = 0;

		GLOBALS.Player1.ID = 1;
		GLOBALS.Player2.ID = 2;

		vNumberSprites = Resources.LoadAll<Sprite>( "Numbers" );

		// Internal state
		bIsOK = true;

	}

	public	Sprite GetNumberSprite() {

		if ( iCurrentStage >= MAX_STAGES ) return null;

		return vNumberSprites[iCurrentStage];
	}

}
