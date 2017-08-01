﻿
using UnityEngine;

#pragma warning disable CS0162 // Unreachable code detected
#pragma warning disable CS0414 // Var assigned but never used

public partial class StageManager {

	const	bool	bPlanDebug		= false;

	////////////////////////////////////////////////////////////////////////
	/////////////////////////		PLAN MODE


	public	void	SelectPlayer( int PlayerID ) {

		if ( iSelectedPlayer == 0 ) return;

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



	public	Stage	GetCurrentStage() {
		return vStages[ iCurrentStage ];
	}



	public	Stage	GetStage( int i ) {
		if ( ( i > -1 ) && ( i < MAX_STAGES ) ) return vStages[i];
		return null;
	}



	public	void	ClearStages() {

		// clear actual stages
		vStages.Clear();
		GLOBALS.UI.ResetActions();

		// create the first empty
		vStages.Add( new Stage() );

	}


	public	void	SetAction( PlayerAction Action, int PlayerID ) {

		if ( !bIsOK ) return;

		if ( Action == null ) return;

		if ( vStages[ iCurrentStage ] == null ) vStages[ iCurrentStage ] = new Stage();

		vStages[ iCurrentStage ].SetAction( PlayerID, Action );
		if ( GLOBALS.UI )
			GLOBALS.UI.AddAction( PlayerID, Action.GetType(), iCurrentStage );

		if ( bPlanDebug ) Debug.Log( "Action set for player " + iSelectedPlayer );

	}


	// Go to next stage
	public	void	NextStage() {

		if ( !bIsOK ) return;

		if ( iCurrentStage == MAX_STAGES ) return;

		GLOBALS.Player1.OnNextStage();
		GLOBALS.Player2.OnNextStage();

		if ( !vStages[ iCurrentStage ].IsOK() ) {

			vStages[ iCurrentStage ].Default();

			if ( GLOBALS.UI ) {
				GLOBALS.UI.AddAction( 1, vStages[ iCurrentStage ].GetAction( 1 ).GetType(), iCurrentStage );
				GLOBALS.UI.AddAction( 2, vStages[ iCurrentStage ].GetAction( 2 ).GetType(), iCurrentStage );
			}

		}
		
		if ( bPlanDebug ) Debug.Log( "Next stage" );

		// Update cursors position
		GLOBALS.UI.CursorsStep( iCurrentStage + 1 );

		// set next stage
		iCurrentStage++;
		vStages.Add( new Stage() );
		
	}

	// Go to next stage
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

	// Clear actions in current stage
	public void		ClearStage() {

		if ( !bIsOK ) return;

		GLOBALS.Player1.OnClearStage();
		GLOBALS.Player2.OnClearStage();

		vStages[ iCurrentStage ].SetAction( 1, null );
		vStages[ iCurrentStage ].SetAction( 2, null );

		GLOBALS.UI.RemoveLastActions();

	}

}

#pragma warning restore CS0414 // Var assigned but never used
#pragma warning restore CS0162 // Unreachable code detected