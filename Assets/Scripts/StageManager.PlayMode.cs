
using System.Collections.Generic;
using System.Collections; // IEnumerator
using UnityEngine;

#pragma warning disable CS0162 // Unreachable code detected
#pragma warning disable CS0414 // Var assigned but never used

public partial class StageManager {

	const	bool	bPlayDebug		= false;

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

		// Set frist stage as current
		iCurrentStage = 0;

		// HAVE FUN
		bIsPlaying = true;

	}

	public	void	Stop() {

		bIsPlaying = false;
		StopCoroutine( pCoroutine );
		pPlayer1.Stop();
		pPlayer2.Stop();

	}


	private void Update() {

		// If manager had bad initialization
		if ( !bIsOK ) return;

		if ( !bIsPlaying ) {
			if ( Input.GetKeyDown( KeyCode.Alpha1 ) ) {		// 1
				SelectPlayer( 1 );
			}
			if ( Input.GetKeyDown( KeyCode.Alpha2 ) ) {		// 2
				SelectPlayer( 2 );
			}
			if ( Input.GetKeyDown( KeyCode.Space ) ) {		// Spazio
				NextStage();
			}
			if ( Input.GetKeyDown( KeyCode.Keypad0 ) ) {	// 0 TN
				Play();
			}

			return; 
		}


		/// PLAY PHASE
		// If is not cycling
		if ( !bIsInCycle ) {

			// if there stage to process
			if ( iCurrentStage < vStages.Count ) {

				pCoroutine = ExecuteActions();

				// Execute actions in stage
				StartCoroutine( pCoroutine );

			}

		}
	}

	public	void AddActiveObject()		{ iActiveObjects++; }
	public	void RemoveActiveObject()	{ iActiveObjects = Mathf.Max( 0, iActiveObjects - 1 ); }

	public bool WorldAnimsPending() {

		// SWITCHERS
		if ( iActiveObjects > 0 ) return true;

		// PLATFORMS
		foreach ( Platform p in vPlatforms ) {
			if ( p.bActive ) {
				p.UpdatePosition();
				return true;
			}
		}

		return false;
	}

	private	IEnumerator	ExecuteActions() {
		
		// stop cycling to allow coroutine run
		bIsInCycle = true;

		if ( bPlayDebug ) Debug.Log( "coroutine start" );
		if ( bPlayDebug ) Debug.Log( "stage " + iCurrentStage + "/" + ( vStages.Count - 1) );

		// Retrieve players action
		PlayerAction PA1 = vStages[ iCurrentStage ].GetAction( 1 );
		PlayerAction PA2 = vStages[ iCurrentStage ].GetAction( 2 );

		if ( PA1.GetType() == ActionType.USE ) {
			PA1.GetUsableObject().OnUse( pPlayer1 );
		}

		if ( PA2.GetType() == ActionType.USE ) {
			PA2.GetUsableObject().OnUse( pPlayer2 );
		}

		////////////////////////////////////////////////////////
		{//	PLAYER 1

			if ( PA1.GetType() == ActionType.MOVE ) {
				if ( !pPlayer1.FindPath( PA1.GetDestination() ) ) {
					Debug.Log( "PG 1 Dice: pirla, non ci posso andare" );
					bIsPlaying = false;
					pPlayer2.Stop();
					yield break;
				}

				pPlayer1.Move();

				if ( PA1.GetUsableObject() != null ) {
					PA1.SetEndActionCallback( delegate { PA1.GetUsableObject().OnUse( pPlayer1 ); } );
				}

			}
		}

		////////////////////////////////////////////////////////
		{//	PLAYER 2

			if ( PA2.GetType() == ActionType.MOVE ) {
				if ( !pPlayer2.FindPath( PA2.GetDestination() ) ) {
					Debug.Log( "PG 2 Dice: pirla, non ci posso andare" );
					bIsPlaying = false;
					pPlayer1.Stop();
					yield break;
				}

				pPlayer2.Move();

				if ( PA2.GetUsableObject() != null ) {
					PA2.SetEndActionCallback( delegate { PA2.GetUsableObject().OnUse( pPlayer2 ); } );
				}

			}
		}


		// WHILE platforms or players are busy
		while ( WorldAnimsPending() || pPlayer1.IsBusy() || pPlayer2.IsBusy() ) {
			
			// Wait for next frame
			yield return null;
			
		}

		// at end of all actions of all elements
		PA1.ExecuteCallBack();
		PA2.ExecuteCallBack();

		// player are not busy, so stop cycle
		bIsInCycle = false;
		
		if ( bPlayDebug ) Debug.Log( "Coroutine end" );

		// if not victory condition is reached
		if ( !CompletedLevelCheck() )
			// Set for next stage
			iCurrentStage++;

		// exit coroutine
		yield return 0;

	}

}

#pragma warning restore CS0414 // Var assigned but never used
#pragma warning restore CS0162 // Unreachable code detected