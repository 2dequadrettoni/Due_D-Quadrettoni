
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

		if ( iCurrentStage >= vStages.Count ) {
			Debug.Log( "Maximum slot occupied" );
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

		// Prepare cursors for play sequence
		pUI.PrepareForPlay();

		// HAVE FUN
		bIsPlaying = true;

        pUI.ActivatePlayBtn();

    }

	public	void	Stop() {

		bIsPlaying = false;
		pPlayer1.Stop();
		pPlayer2.Stop();

		// invalidate all script function so nothing can happen after this call
		bIsOK = false;

	}


	private void Update() {

		// If manager had bad initialization
		if ( !bIsOK ) return;

		if (  iSelectedPlayer == 0 ) return;

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

		// if function reach this point Play mode is enabled and all stages are played

		/// PLAY PHASE
		if ( !bPlayingStage && ( iCurrentStage < vStages.Count ) ) {

			fUI_Interpolant	= 0.0f;

			bPlayingStage = true;

			Debug.Log( "stage " + iCurrentStage + "/" + ( vStages.Count - 1) );

		}

		if ( bPlayingStage ) {
			UpdateUISequence();
			ExecuteActions();
		}
		
	}

	private	void	UpdateUISequence() {

		if ( fUI_Interpolant < 1.0f ) {

			fUI_Interpolant += 2.0f * Time.deltaTime;
			
			pUI.PlaySequence( iCurrentStage, fUI_Interpolant );

		}

	}

	public	void AddActiveObject()		{ iActiveObjects++; }
	public	void RemoveActiveObject()	{ iActiveObjects = Mathf.Max( 0, iActiveObjects - 1 ); }

	// Check for active transitions or platform in scene and return true if found otherwise false
	private bool WorldAnimsPending() {
		
		// SWITCHERS - DOORS
		if ( iActiveObjects > 0 ) return true;

		// PLATFORMS
		foreach ( Platform p in vPlatforms ) {
			if ( p.IsActive ) {
				p.UpdatePosition();
				return true;
			}
		}

		return false;
	}

	private void ExecuteActions() {

		// Retrieve players action
		PlayerAction PA1 = vStages[ iCurrentStage ].GetAction( 1 );
		PlayerAction PA2 = vStages[ iCurrentStage ].GetAction( 2 );


		////////////////////////////////////////////////////////
		// PGs Actions
		{

			if ( PA1.IsValid() && PA1.GetType() == ActionType.USE ) {
				PA1.GetUsableObject().OnUse( pPlayer1 );
				PA1.Invalidate();
			}

			if ( PA2.IsValid() && PA2.GetType() == ActionType.USE ) {
				PA2.GetUsableObject().OnUse( pPlayer2 );
				PA2.Invalidate();
			}
		}


		////////////////////////////////////////////////////////
		// PGs Movements
		{
			
			{//	PLAYER 1

				if ( PA1.IsValid() && ( PA1.GetType() == ActionType.MOVE ) && !pPlayer1.IsBusy() ) {
					if ( !pPlayer1.FindPath( PA1.GetDestination() ) ) {
						pUI.ShowUnreachableMsg( "Player1" );
						bIsPlaying = bIsOK = false;
						pPlayer2.Stop();
						return;
					}

					pPlayer1.Move(); // Set as busy

					if ( PA1.GetUsableObject() != null ) {
						PA1.SetEndActionCallback( delegate { PA1.GetUsableObject().OnUse( pPlayer1 ); } );
					}
					PA1.Invalidate();
				}
			}

			{//	PLAYER 2

				if ( PA2.IsValid() && ( PA2.GetType() == ActionType.MOVE ) && !pPlayer2.IsBusy() ) {
					if ( !pPlayer2.FindPath( PA2.GetDestination() ) ) {
						pUI.ShowUnreachableMsg( "Player2" );
						bIsPlaying = bIsOK = false;
						pPlayer1.Stop();
						return;
					}

					pPlayer2.Move(); // Set as busy

					if ( PA2.GetUsableObject() != null ) {
						PA2.SetEndActionCallback( delegate { PA2.GetUsableObject().OnUse( pPlayer2 ); } );
					}
					PA2.Invalidate();
				}
			}
		}


		////////////////////////////////////////////////////////
		// if world transition or player movements ar not finished return, waitinig for them

		if ( WorldAnimsPending() || pPlayer1.IsBusy() || pPlayer2.IsBusy() ) return;


		// at end of all actions of all elements
		PA1.ExecuteCallBack();
		PA2.ExecuteCallBack();

		// wait for other world transitions
		if ( WorldAnimsPending() ) return;

		// wait for ui transition
		if ( fUI_Interpolant < 1.0f ) return;

		bPlayingStage = false;

		// Set for next stage
		iCurrentStage++;

	}

}

#pragma warning restore CS0414 // Var assigned but never used
#pragma warning restore CS0162 // Unreachable code detected