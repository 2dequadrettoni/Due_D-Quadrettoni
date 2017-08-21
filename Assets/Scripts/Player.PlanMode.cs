
using UnityEngine;

using NodeList = System.Collections.Generic.List<Node>;

//	using UnityEngine.EventSystems;

#pragma warning disable CS0162 // Unreachable code detected
#pragma warning disable CS0414 // Var assigned but never used

public partial class Player {

	private	RaycastHit	pMouseHitthedObject		= new RaycastHit();

	private	void	SetPlainTile( ) {

		string objTag = pMouseHitthedObject.collider.tag;
		// make visible the sprite
		if ( objTag == "Tiles" ) {

			pPlanTile.gameObject.SetActive( true );
			pPlanTile.position = pMouseHitthedObject.collider.transform.position;

		}

	}


	////////////////////////////////////////////////////////////////////////
    /////////////////////////		PLAN MODE
	private void ParseInput() {

		//		if ( EventSystem.current.IsPointerOverGameObject() ) return;

		if ( GLOBALS.IsPaused ) return;

		// Trace always mouse poited position
		bool pHitResult = Physics.Raycast( Camera.main.ScreenPointToRay( Input.mousePosition ), out pMouseHitthedObject );

		// Semi trasparent square sprite for current destination tile
		if ( pHitResult ) SetPlainTile();

		
		// WAIT ACTION
		if ( Input.GetMouseButtonDown( 1 ) ) {
			this.Action_Wait();
			return;
		}

		// GOTO - USE ACTION
		if ( Input.GetMouseButtonDown( 0 ) ) {

			if ( pHitResult ) {

				UsableObject pUsableObject = pMouseHitthedObject.collider.gameObject.GetComponent<UsableObject>();

				string objTag = pMouseHitthedObject.collider.tag;
				
				if ( pUsableObject ) {

					// skip non usable objects
					if ( pUsableObject.UseType == UsageType.NONE ) return;

//					if ( objTag == "Key" || objTag == "Door" || objTag == "Switcher" || objTag == "Plane_Switcher" ) {

						//////////////////////////////////////////////////////////////////////////////
						//		INSTANT USE
						if (  pUsableObject.UseType == UsageType.INSTANT )
							this.Action_UsableObject_Use_Instant( pUsableObject );


						//////////////////////////////////////////////////////////////////////////////
						//		ON ACTION END USE
						if ( pUsableObject.UseType == UsageType.ON_ACTION_END )
							this.Action_UsableObject_Use_OnActionEnd( pUsableObject );

//					}

				}

				//////////////////////////////////////////////////////////////////////////////
				//		MOVEMENT ONLY
				if ( objTag == "Tiles" ) {

					// player selection tutorial sprite
					if ( GameManager.InTutorialSequence ) {
						if ( GameManager.TutorialStep == 1 ) {
							GLOBALS.GameManager.NextTutorial();
						}
//						else return; // skip execution if is in tutorial mode
					}

					this.Action_MoveOnly();

				}

			}

		}

	}








	public	void	OnNextStage() {

		vPlanPosition.Set( vPlanStageDestination.x, vPlanStageDestination.y, vPlanStageDestination.z );

	}


	public	void	OnPrevStage() {

		StageManager pStageManager = GLOBALS.StageManager;

		if ( pStageManager.CurrentStage == 0 ) return;

		Transform pStep = vSteps[ pStageManager.CurrentStage - 1 ].pStepTransform;
		if ( pStep != null ) {
			Destroy( pStep.gameObject ); pStep = null;
		}
		vSteps[ pStageManager.CurrentStage - 1 ] = null;

		if ( pStageManager.CurrentStage == 1 ) {
			pStageManager.ClearStages();
		}
		else {

			PlayerAction pAction = pStageManager.GetStage( pStageManager.CurrentStage - 1 ).GetAction( ID );

			if ( pAction.GetType() == ActionType.MOVE ) {
				vPlanPosition = pAction.GetStartPoint();
			}

		}

	}

	public	void	OnClearStage() {

		if ( GLOBALS.StageManager.CurrentStage > 0 ) {



		}
	}


}

#pragma warning restore CS0414 // Var assigned but never used
#pragma warning restore CS0162 // Unreachable code detected