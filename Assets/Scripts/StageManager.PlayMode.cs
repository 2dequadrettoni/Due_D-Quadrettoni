
using System.Collections.Generic;
using System.Collections; // IEnumerator
using UnityEngine;

#pragma warning disable CS0162 // Unreachable code detected
#pragma warning disable CS0414 // Var assigned but never used

public partial class StageManager {

	const	bool	bPlayDebug		= false;


	//		STAGE ENABLE PLAY  MODE
	////////////////////////////////////////////////////////////////////////
	public	void	Play() {

		// If manager had bad initialization
		if ( !bIsOK || IsPlaying ) return;

		if ( vStages.Count < 1 ) {
			Debug.Log( "Stages have to be at last one" );
			return;
		}

		if ( GameManager.InTutorialSequence && GameManager.TutorialStep < 4 ) return;


		if ( iCurrentStage < MAX_STAGES ) {
			
			if ( !vStages[ iCurrentStage ].IsOK() ) {

				vStages[ iCurrentStage ].Default();		// Set empty action as wait actions

				if ( GLOBALS.UI ) {						// Update ui icons
					GLOBALS.UI.AddAction( 1, vStages[ iCurrentStage ].GetAction( 1 ).GetType(), iCurrentStage );
					GLOBALS.UI.AddAction( 2, vStages[ iCurrentStage ].GetAction( 2 ).GetType(), iCurrentStage );
					GLOBALS.UI.RemoveIconsGlow();
				}

			}
		}

		// hide cursor and reset default animation ( remove glow animation )
		GLOBALS.Player1.SetCursor( false );
		GLOBALS.Player2.SetCursor( false );

		// On Play callback
		GLOBALS.Player1.OnPlay();
		GLOBALS.Player2.OnPlay();

		// Set frist stage as current
		iCurrentStage = 0;

		// Animate play button
		GLOBALS.UI.ActivatePlayBtn();

		// Hide tutorial image while playing
		GLOBALS.TutorialSlot.sprite = null;

		AudioManager.Play( "Play_Button" );

		// HAVE FUN
		bIsPlaying = true;

    }

	public	void	Stop( bool AlsoPlayers = false) {

		bIsPlaying = false;
		if ( AlsoPlayers ) {
			GLOBALS.Player1.Stop();
			GLOBALS.Player2.Stop();
		}

		// invalidate all script function so nothing can happen after this call
		bIsOK = false;

	}


	private void Update() {

      
		// If manager had bad initialization
		if ( !bIsOK ) return;

		if (  iSelectedPlayer == 0 ) return;

		//////////////////////////////////////////////////////////////////////////
		// RESTARG LEVEL
		if ( Input.GetKeyDown( KeyCode.R ) ) {			// R
			GLOBALS.UI.GlowOrangeRestart();
		}
		if ( Input.GetKeyUp( KeyCode.R ) ) {		
			GLOBALS.UI.DefaultRestart();
			GLOBALS.GameManager.RestartGame();
		}

		if ( !bIsPlaying ) {

			if ( Input.GetKeyDown( KeyCode.Alpha1 ) ) {		// 1
				SelectPlayer( 1 );

			}
			if ( Input.GetKeyDown( KeyCode.Alpha2 ) ) {		// 2
				SelectPlayer( 2 );
			}

			//////////////////////////////////////////////////////////////////////////
			// PREV STAGE
//			if ( Input.GetKeyDown( KeyCode.Backspace ) ) {	// Backspace
//				PrevStage();
//			}
//			if ( Input.GetKeyUp( KeyCode.Backspace ) ) {		
//
//			}

			//////////////////////////////////////////////////////////////////////////
			// NEXT STAGE
			if ( Input.GetKeyDown( KeyCode.Space ) ) {		
				GLOBALS.UI.GlowOrangeNexTurn();
				NextStage();
			}

			if ( Input.GetKeyUp( KeyCode.Space ) ) {		
				GLOBALS.UI.GlowAnimationNextTurn( false );
				GLOBALS.UI.DefaultNextTurn();
			}

			//////////////////////////////////////////////////////////////////////////
			// PLAY
			if ( Input.GetKeyDown( KeyCode.Keypad0 ) || Input.GetKeyDown( KeyCode.Return ) || Input.GetKeyDown( KeyCode.KeypadEnter ) ) {
				Play();
			}

			return; 
		}

		// if function reach this point Play mode is enabled and all stages are played

		/// PLAY PHASE
		if ( !bPlayingStage && ( iCurrentStage < vStages.Count ) ) {

			fUI_Interpolant	= 0.0f;

			bPlayingStage = true;

			Debug.Log( "stage " + iCurrentStage + "/" + MAX_STAGES );

		}

		if ( bPlayingStage &&  ( iCurrentStage < vStages.Count ) ) {

			UpdateUISequence();
			ExecuteActions();
			return;
		}

		// Play tutorial sprite
		if ( GameManager.InTutorialSequence ) {
			if ( GameManager.TutorialStep == 4 ) {
				GLOBALS.GameManager.NextTutorial();
				GameManager.InTutorialSequence = false;
			}
		}
		
	}

	private	void	UpdateUISequence() {

		if ( fUI_Interpolant < 1.0f && iCurrentStage < MAX_STAGES ) {

			fUI_Interpolant += 3.0f * Time.deltaTime;
			
			GLOBALS.UI.PlaySequence( iCurrentStage, fUI_Interpolant );

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

		Player pPlayer1 = GLOBALS.Player1;
		Player pPlayer2 = GLOBALS.Player2;


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

				if ( PA1.IsValid() && ( PA1.GetType() == ActionType.MOVE || PA1.GetType() == ActionType.MOVE_USE ) && !pPlayer1.IsBusy() ) {
					if ( !pPlayer1.FindPath( PA1.GetDestination() ) || pPlayer1.Linked ) {
						AudioManager.Play( "PG_PathNotFound" );
						GLOBALS.UI.ShowUnreachableMsg( "Player1" );
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

				if ( PA2.IsValid() && ( PA2.GetType() == ActionType.MOVE || PA2.GetType() == ActionType.MOVE_USE ) && !pPlayer2.IsBusy() ) {
					if ( !pPlayer2.FindPath( PA2.GetDestination() ) || pPlayer2.Linked ) {
						AudioManager.Play( "PG_PathNotFound" );
						GLOBALS.UI.ShowUnreachableMsg( "Player2" );
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