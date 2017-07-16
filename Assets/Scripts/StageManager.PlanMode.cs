
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


	public	void	SetAction() {

		if ( !bIsOK ) return;

		Player pPlayer = ( iSelectedPlayer == 1 ) ? pPlayer1 : pPlayer2;

		if ( pPlayer.Action == null ) return;

		vStages[ iCurrentStage ].SetAction( iSelectedPlayer, pPlayer.Action );

		Debug.Log( "Action set for player " + iSelectedPlayer );

		pPlayer.Action = null;

	}

	// Clear actions in current stage
	public void		ClearStage() {

		if ( !bIsOK ) return;

		pPlayer1.Action = pPlayer2.Action = null;

		vStages[ iCurrentStage ].SetAction( 1, null );
		vStages[ iCurrentStage ].SetAction( 2, null );

	}


	// Go to next stage
	public	void	NextStage() {

		if ( !bIsOK ) return;

		// If actions are not assigned warn player
		if ( !vStages[ iCurrentStage ].IsOK() ) {
			Debug.Log( "both player have to had proper action set" );
			return;
		}

		Debug.Log( "Next stage" );

		PlayerAction P1Action = vStages[ iCurrentStage ].GetAction(1);
		PlayerAction P2Action = vStages[ iCurrentStage ].GetAction(2);

		// If in current stage player moves
		if ( P1Action.GetType() == ActionType.MOVE  ) {
			// set prev position to actual posititon
			pPlayer1.PrevPostion = pPlayer1.transform.position;
			pPlayer1.ClearPath();
		}
		// same for player 2
		if ( P2Action.GetType() == ActionType.MOVE  ) {
			pPlayer2.PrevPostion = pPlayer2.transform.position;
			pPlayer2.ClearPath();
		}

		// set next stage
		iCurrentStage++;
		vStages.Add( new Stage() );
		
	}

	// Go to next stage
	public	void	PrevStage() {

		if ( iCurrentStage < 1 ) return;

		// Get previous actions
		PlayerAction P1Action = vStages[ iCurrentStage - 1 ].GetAction(1);
		PlayerAction P2Action = vStages[ iCurrentStage - 1 ].GetAction(2);

		// for player 1 a path was set
		if ( P1Action.GetType() == ActionType.MOVE  ) {
			// restore actual position on prev last node position
			pPlayer1.transform.position = P1Action.GetFinalPosition().worldPosition;
			// restore actual start position on prev start node position
			pPlayer1.PrevPostion = P1Action.GetPath()[0].worldPosition;
			// Set prev nodelist as pcurent path
			pPlayer1.SetPath( P1Action.GetPath() );
			// Show preview
			pPlayer1.ShowPreview();
		}
		// same for player 2
		if ( P2Action.GetType() == ActionType.MOVE  ) {
			pPlayer2.transform.position = P2Action.GetFinalPosition().worldPosition;
			pPlayer2.PrevPostion = P2Action.GetPath()[0].worldPosition;
			pPlayer2.SetPath( P2Action.GetPath() );
			pPlayer2.ShowPreview();
		}
		
		// invaldate current stage actions
		vStages[ iCurrentStage ].SetAction( 1, null );
		vStages[ iCurrentStage ].SetAction( 2, null );

		// set prev stage
		vStages.RemoveAt( iCurrentStage );
		iCurrentStage--;
		

	}

}