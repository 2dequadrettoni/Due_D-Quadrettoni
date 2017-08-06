
using UnityEngine;

#pragma warning disable CS0162 // Unreachable code detected
#pragma warning disable CS0414 // Var assigned but never used

public partial class StageManager {

	const	bool	bPlanDebug		= false;


	public	Stage	GetCurrentStage() {
		return vStages[ iCurrentStage ];
	}

	public	Stage	GetStage( int i ) {
		if ( ( i > -1 ) && ( i < MAX_STAGES ) ) return vStages[i];
		return null;
	}
	


	//		PLAYER SELECTION
	////////////////////////////////////////////////////////////////////////
	public	void	SelectPlayer( int PlayerID ) {

		if ( iSelectedPlayer == 0 ) return;

		if ( bIsPlaying ) return;

		// next stage tutorial sprite
		if ( GameManager.InTutorialSequence ) {
			if ( GameManager.TutorialStep == 2 ) {
				GLOBALS.GameManager.NextTutorial();
			}
	//		else return; // skip execution if is in tutorial mode
		}

		PlayerID = Mathf.Clamp( PlayerID, 1, 2 );

		// Switch to player 1
		if ( PlayerID == 1 ) {

			GLOBALS.Player1.SetCursor( GLOBALS.Player1.CanParseInput = true );
			GLOBALS.Player2.SetCursor( GLOBALS.Player2.CanParseInput = false );

		}
		// Switch to player 2
		else {

			GLOBALS.Player1.SetCursor( GLOBALS.Player1.CanParseInput = false );
			GLOBALS.Player2.SetCursor( GLOBALS.Player2.CanParseInput = true );

		}
		iSelectedPlayer = PlayerID;

		// Update avatars
		GLOBALS.UI.SelectPlayer( PlayerID );
		if ( bPlanDebug ) Debug.Log( "player " + PlayerID + " selected" );
	}


	//		ACTION SET
	////////////////////////////////////////////////////////////////////////
	public	void	SetAction( PlayerAction Action, int PlayerID ) {

		if ( !bIsOK ) return;

		if ( Action == null ) return;

		if ( vStages[ iCurrentStage ] == null ) vStages[ iCurrentStage ] = new Stage();

		vStages[ iCurrentStage ].SetAction( PlayerID, Action );
		if ( GLOBALS.UI )
			GLOBALS.UI.AddAction( PlayerID, Action.GetType(), iCurrentStage );

		if ( vStages[ iCurrentStage ].IsOK() ) {
			GLOBALS.UI.GlowAnimationNextTurn( true );
		}

		if ( bPlanDebug ) Debug.Log( "Action set for player " + iSelectedPlayer );

	}




	//		STAGE GOTO NEXT
	////////////////////////////////////////////////////////////////////////
	public	void	NextStage() {

		if ( !bIsOK ) return;

		if ( iCurrentStage == MAX_STAGES ) return;

		// Play tutorial sprite
		if ( GameManager.InTutorialSequence ) {
			if ( GameManager.TutorialStep == 3 ) {
				GLOBALS.GameManager.NextTutorial();
			}
		}
		
		if ( GameManager.InTutorialSequence && GameManager.TutorialStep < 3 ) return;

		GLOBALS.Player1.OnNextStage();
		GLOBALS.Player2.OnNextStage();

		if ( !vStages[ iCurrentStage ].IsOK() ) {

			vStages[ iCurrentStage ].Default();

			if ( GLOBALS.UI ) {
				GLOBALS.UI.AddAction( 1, vStages[ iCurrentStage ].GetAction( 1 ).GetType(), iCurrentStage );
				GLOBALS.UI.AddAction( 2, vStages[ iCurrentStage ].GetAction( 2 ).GetType(), iCurrentStage );
			}

		}
		
		if ( bPlanDebug ) Debug.Log( "UI_Next_Stage" );

//		GLOBALS.AudioManager.Play( "NextStage" );

		// Update cursors position
		GLOBALS.UI.CursorsStep( iCurrentStage + 1 );

		GLOBALS.UI.GlowAnimationNextTurn( false );

		// set next stage
		iCurrentStage++;
		vStages.Add( new Stage() );
		
	}

	
	//		STAGE GOTO PREVIOUS
	////////////////////////////////////////////////////////////////////////
	public	void	PrevStage() {

		if ( iCurrentStage < 1 ) return;

		GLOBALS.Player1.OnPrevStage();
		GLOBALS.Player2.OnPrevStage();

		GLOBALS.UI.RemoveLastActions();

		// set prev stage
		if ( iCurrentStage > 1 ) {
			vStages.RemoveAt( iCurrentStage );
		}
		else {
			ClearStages();
		}
		iCurrentStage--;

		// Update cursors position
		GLOBALS.UI.CursorsStep( iCurrentStage );

	}

	
	//		STAGE CLEAR
	////////////////////////////////////////////////////////////////////////
	public void		ClearStage() {

		if ( !bIsOK ) return;

		GLOBALS.Player1.OnClearStage();
		GLOBALS.Player2.OnClearStage();

		vStages[ iCurrentStage ].SetAction( 1, null );
		vStages[ iCurrentStage ].SetAction( 2, null );

		GLOBALS.UI.RemoveLastActions();

	}


	//		STAGE CLEAR ALL
	////////////////////////////////////////////////////////////////////////
	public	void	ClearStages() {

		// clear actual stages
		vStages.Clear();
		GLOBALS.UI.ResetActions();

		// create the first empty
		vStages.Add( new Stage() );

	}

}

#pragma warning restore CS0414 // Var assigned but never used
#pragma warning restore CS0162 // Unreachable code detected