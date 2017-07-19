
using UnityEngine;

public partial class StageManager {

	////////////////////////////////////////////////////////////////////////
	/////////////////////////		PLAN MODE


	public	void	SelectPlayer( int PlayerID ) {

		PlayerID = Mathf.Clamp( PlayerID, 1, 2 );

		// Switch to player 1
		if ( PlayerID == 1 ) {
			pPlayer1.CanParseInput = true;
			pPlayer2.CanParseInput = false;
		}
		// Switch to player 2
		else {
			pPlayer1.CanParseInput = false;
			pPlayer2.CanParseInput = true;
		}
		iSelectedPlayer = PlayerID;
		Debug.Log( "player " + PlayerID + " selected" );
	}


	public Stage	GetCurrentStage() {
		return vStages[ iCurrentStage ];
	}


	public	void	SetAction( PlayerAction Action, int PlayerID ) {

		if ( !bIsOK ) return;

		if ( Action == null ) return;

		vStages[ iCurrentStage ].SetAction( PlayerID, Action );

		pUI.UpdateIcon( PlayerID, Action.GetType(), iCurrentStage );

		Debug.Log( "Action set for player " + iSelectedPlayer );

	}


	// Go to next stage
	public	void	NextStage() {

		if ( !bIsOK ) return;

		pPlayer1.OnNextStage();
		pPlayer2.OnNextStage();

		// If actions are not assigned warn player
		if ( !vStages[ iCurrentStage ].IsOK() ) {
			Debug.Log( "both player have to had proper action set" );
			return;
		}
		
		Debug.Log( "Next stage" );

		// set next stage
		iCurrentStage++;
		vStages.Add( new Stage() );
		
	}

	// Go to next stage
	public	void	PrevStage() {

		if ( iCurrentStage < 1 ) return;
		
		pPlayer1.OnPrevStage();
		pPlayer2.OnPrevStage();

		// set prev stage
		vStages.RemoveAt( iCurrentStage );
		iCurrentStage--;
		

	}

	// Clear actions in current stage
	public void		ClearStage() {

		if ( !bIsOK ) return;

		pPlayer1.OnClearStage();
		pPlayer2.OnClearStage();

		vStages[ iCurrentStage ].SetAction( 1, null );
		vStages[ iCurrentStage ].SetAction( 2, null );

	}

}