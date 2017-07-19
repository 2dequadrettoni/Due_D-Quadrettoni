
using System.Collections.Generic;
using System.Collections; // IEnumerator
using UnityEngine;

using NodeList = System.Collections.Generic.List<Node>;

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

		if ( !vStages[ iCurrentStage ].IsOK() ) {
			Debug.Log( "both player have to had proper action set" );
			return;
		}

		pPlayer1.OnPlay();
		pPlayer2.OnPlay();

		// HAVE FUN
		bIsPlaying = true;

		Time.timeScale = 0.1f;

	}


	private void Update() {

		// If manager had bad initialization
		if ( !bIsOK ) return;

		if ( !bIsPlaying ) {
			if ( Input.GetKeyDown( KeyCode.Alpha1 ) ) {
				SelectPlayer( 1 );
			}
			if ( Input.GetKeyDown( KeyCode.Alpha2 ) ) {
				SelectPlayer( 2 );
			}
			if ( Input.GetKeyDown( KeyCode.Space ) ) {
				NextStage();
			}
			if ( Input.GetKeyDown( KeyCode.Keypad0 ) ) {
				Play();
			}

			return; 
		}

		/// PLAY PHASE
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

				// Execute actions in stage
				StartCoroutine( ExecuteActions() );

			}

		}
	}

	private	IEnumerator	ExecuteActions() {
		
		// stop cycling to allow coroutine run
		bIsInCycle = true;

		Debug.Log( "coroutine start" );
		Debug.Log( "stage " + iCurrentStage + "/" + ( vStages.Count - 1) );

		// Retrieve players action
		PlayerAction PA1 = vStages[ iCurrentStage ].GetAction( 1 );
		PlayerAction PA2 = vStages[ iCurrentStage ].GetAction( 2 );

		////////////////////////////////////////////////////////
		{//	PLAYER 1
			if ( PA1.GetType() == ActionType.USE ) {
				PA1.GetUsableObject().OnUse( pPlayer1 );
			}

			if ( PA1.GetType() == ActionType.MOVE ) {
				if ( !pPlayer1.FindPath( PA1.GetDestination() ) ) {
					bIsPlaying = false;
					yield return 0;
				}

				if ( pPlayer1.transform == pPlayer2.transform ) {
					print( "stronzo" );
					bIsPlaying = false;
					yield return 0;
				}

				pPlayer1.Move();

				if ( PA1.GetUsableObject() != null ) {
					PA1.SetEndActionCallback( delegate { PA1.GetUsableObject().OnUse( pPlayer1 ); } );
				}

			}
		}

		////////////////////////////////////////////////////////
		{//	PLAYER 2
			if ( PA2.GetType() == ActionType.USE ) {
				PA2.GetUsableObject().OnUse( pPlayer2 );
			}

			if ( PA2.GetType() == ActionType.MOVE ) {
				if ( !pPlayer2.FindPath( PA2.GetDestination() ) ) {
					bIsPlaying = false;
					yield return 0;
				}

				if ( pPlayer1.transform == pPlayer2.transform ) {
					print( "stronzo2" );
					bIsPlaying = false;
					yield return 0;
				}

				pPlayer2.Move();

				if ( PA2.GetUsableObject() != null ) {
					PA2.SetEndActionCallback( delegate { PA2.GetUsableObject().OnUse( pPlayer2 ); } );
				}

			}
		}


		if ( PA2.GetDestination() == PA1.GetDestination() ) {

			print ( "MA PERCHEEEEEEEEE" );
			bIsPlaying = false;
			yield return 0;

		}


		// WHILE players are busy
		while ( pPlayer1.IsBusy() || pPlayer2.IsBusy() ) {

			// Wait for next frame
			yield return null;
		}

		PA1.ExecuteCallBack();
		PA2.ExecuteCallBack();

		PA1 = null;
		PA2 = null;

		// player are not busy, so stop cycle
		bIsInCycle = false;
		
		Debug.Log( "Coroutine end" );

		// Check victory condition
		if ( CompletedLevelCheck() ) {
			yield return 0;
		}

		// Set for next stage
		iCurrentStage++;

		//	stage completed
		yield return 0;

	}

}