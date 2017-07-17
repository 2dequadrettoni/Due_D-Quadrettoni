
using System.Collections; // IEnumerator
using UnityEngine;

public partial class StageManager {

	////////////////////////////////////////////////////////////////////////
	/////////////////////////		PLAY MODE

	public	void	Play() {

		// If manager had bad initialization
		if ( !bIsOK ) return;

		if ( vStages.Count < 1 ) {
			Debug.Log( "Stages have to be at last one" );
			return;
		}

		PlayerAction PA1 = vStages[ iCurrentStage ].GetAction( 1 );
		PlayerAction PA2 = vStages[ iCurrentStage ].GetAction( 2 );

		if ( ( PA1 == null ) || ( PA2 == null ) ) {
			Debug.Log( "both player have to had proper action set" );
			return;
		}

		pPlayer1.ClearPath();
		pPlayer2.ClearPath();

		pPlayer1.OnPlay();
		pPlayer2.OnPlay();

		// HAVE FUN
		bIsPlaying = true;

	}


	private void Update() {

		// If manager had bad initialization
		if ( !bIsOK ) return;

		if ( bIsPlaying && Input.anyKeyDown ) {
			Debug.Log( "ANSIA, EH???" );
			return;
		}

		if ( Input.GetKeyDown( KeyCode.Alpha1 ) ) {
			SelectPlayer( 1 );
		}
		if ( Input.GetKeyDown( KeyCode.Alpha2 ) ) {
			SelectPlayer( 2 );
		}
		if ( Input.GetKeyDown( KeyCode.Escape ) ) {
			
			if ( iSelectedPlayer == 1 )
				pPlayer1.ClearPath();
			else
				pPlayer2.ClearPath();

			Debug.Log( "Path cleard for player " + iSelectedPlayer );
		}

		if ( Input.GetKeyDown( KeyCode.Return ) ) {
			SetAction();
		}

		if ( Input.GetKeyDown( KeyCode.Space ) ) {
			NextStage();
		}

		if ( Input.GetKeyDown( KeyCode.Keypad0 ) ) {
			Play();
		}


		if ( bIsPlaying ) {
			if ( GamePhase.GetCurrent() == GAME_PHASE.PLANNING ) {

				// We ar going to have fun so:
				// Set frist stage as current
				iCurrentStage = 0;

				// To Playing phase
				GamePhase.Switch();

				// Prey!!

			}

			// If is not cycling
			if ( !bIsInCycle ) {

				// if there stage to process
				if ( iCurrentStage < vStages.Count ) {

					Debug.Log( "coroutine start" );

					// Execute actions in stage
					StartCoroutine( ExecuteActions() );

				}

			}

		}
	}

	private	IEnumerator	ExecuteActions() {
		
		// stop cycling to allow coroutine run
		bIsInCycle = true;

		Debug.Log( "coroutine start" );
		Debug.Log( "stage " + ( iCurrentStage + 1 ) + "/" + ( vStages.Count - 1 ) );

		// Retrieve players action
		PlayerAction PA1 = vStages[ iCurrentStage ].GetAction( 1 );
		PlayerAction PA2 = vStages[ iCurrentStage ].GetAction( 2 );

		////////////////////////////////////////////////////////
		{//	PLAYER 1
			if ( PA1.GetType() == ActionType.USE ) {
				PA1.GetUsableObject().OnUse( pPlayer1 );
			}

			if ( PA1.GetType() == ActionType.MOVE ) {
				pPlayer1.SetPath( PA1.GetPath() );
				pPlayer1.Move();
			}
		}

		////////////////////////////////////////////////////////
		{//	PLAYER 2
			if ( PA2.GetType() == ActionType.USE ) {
				PA2.GetUsableObject().OnUse( pPlayer2 );
			}

			if ( PA2.GetType() == ActionType.MOVE ) {
				pPlayer2 .SetPath( PA2.GetPath() );
				pPlayer2.Move();
			}
		}

		// WHILE players are busy
		while ( pPlayer1.IsBusy() || pPlayer2.IsBusy() ) {

			// Wait for next frame
			yield return null;
		 }

		// player are not busy, so stop cycle
		bIsInCycle = false;
		
		// Checl vistory condition
		if ( CompletedLevelCheck() ) {
			yield return 0;
		}

		Debug.Log( "Coroutine end" );

		// Set for next stage
		iCurrentStage++;

		//	stage completed
		yield return 0;

	}

}