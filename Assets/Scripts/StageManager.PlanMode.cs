
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

			pPlayer1.SetCursor( pPlayer1.CanParseInput = true );
			pPlayer2.SetCursor( pPlayer2.CanParseInput = false );

		}
		// Switch to player 2
		else {

			pPlayer1.SetCursor( pPlayer1.CanParseInput = false );
			pPlayer2.SetCursor( pPlayer2.CanParseInput = true );

		}
		iSelectedPlayer = PlayerID;

		// Update avatars
		pUI.SelectPlayer( PlayerID );
		if ( bPlanDebug ) Debug.Log( "player " + PlayerID + " selected" );
	}



	public	Stage	GetCurrentStage() {
		return vStages[ iCurrentStage ];
	}



	public	Stage	GetStage( int i ) {
		if ( i < MAX_STAGES ) return vStages[i];
		return null;
	}



	public	void	ClearStages() {

		vStages.Clear();
		pUI.ResetActions();

	}


	public	void	SetAction( PlayerAction Action, int PlayerID ) {

		if ( !bIsOK ) return;

		if ( Action == null ) return;

		vStages[ iCurrentStage ].SetAction( PlayerID, Action );
		if ( pUI )
			pUI.AddAction( PlayerID, Action.GetType(), iCurrentStage );

		if ( bPlanDebug ) Debug.Log( "Action set for player " + iSelectedPlayer );

	}


	// Go to next stage
	public	void	NextStage() {

		if ( !bIsOK ) return;

		if ( iCurrentStage == MAX_STAGES ) return;

		pPlayer1.OnNextStage();
		pPlayer2.OnNextStage();

		if ( !vStages[ iCurrentStage ].IsOK() ) {

			vStages[ iCurrentStage ].Default();

			if ( pUI ) {
				pUI.AddAction( 1, vStages[ iCurrentStage ].GetAction( 1 ).GetType(), iCurrentStage );
				pUI.AddAction( 2, vStages[ iCurrentStage ].GetAction( 2 ).GetType(), iCurrentStage );
			}

		}
		
		if ( bPlanDebug ) Debug.Log( "Next stage" );

		// Update cursors position
		pUI.CursorsStep( iCurrentStage + 1 );

		// set next stage
		iCurrentStage++;
		vStages.Add( new Stage() );
		
	}

	// Go to next stage
	public	void	PrevStage() {

		if ( iCurrentStage < 1 ) return;
		
		pPlayer1.OnPrevStage();
		pPlayer2.OnPrevStage();

		pUI.RemoveLastActions();

		// set prev stage
		vStages.RemoveAt( iCurrentStage );
		iCurrentStage--;

		// Update cursors position
		pUI.CursorsStep( iCurrentStage );
		

	}

	// Clear actions in current stage
	public void		ClearStage() {

		if ( !bIsOK ) return;

		pPlayer1.OnClearStage();
		pPlayer2.OnClearStage();

		vStages[ iCurrentStage ].SetAction( 1, null );
		vStages[ iCurrentStage ].SetAction( 2, null );

		UI.RemoveLastActions();

	}

}

#pragma warning restore CS0414 // Var assigned but never used
#pragma warning restore CS0162 // Unreachable code detected