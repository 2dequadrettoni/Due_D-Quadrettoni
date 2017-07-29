﻿
using UnityEngine;

using NodeList = System.Collections.Generic.List<Node>;

//	using UnityEngine.EventSystems;

#pragma warning disable CS0162 // Unreachable code detected
#pragma warning disable CS0414 // Var assigned but never used

public partial class Player {

	const	bool	bPlanDebug		= true;

	private	Vector3	vPlanStageDestination =  Vector3.zero;


	private	void	SetStepTile( Vector3 vPosition ) {

		//////////////////////////////////////////////////////////////////////////////
		// INSTANTIATE A NEW STEP TILE
		Transform pStepTile = null;
		if ( vSteps[ pStageManager.CurrentStage ] == null ) {
			pStepTile = Instantiate( pMainStepTile, this.transform ) as Transform;;
			vSteps[ pStageManager.CurrentStage ] = pStepTile;

			//////////////////////////////////////////////////////////////////////////////
			// SHOW STEP SPRITE
			pStepTile.GetChild( 0 ).GetComponent<SpriteRenderer>().enabled = true;

			//////////////////////////////////////////////////////////////////////////////
			// SET RIGHT NUMBER
			SpriteRenderer	pSpriteRender = pStepTile.GetChild( 1 ).GetComponent<SpriteRenderer>();
			pSpriteRender.enabled = true;
			pSpriteRender.sprite = pStageManager.GetNumberSprite();

		}
		else {
			pStepTile = vSteps[ pStageManager.CurrentStage ];
		}

		pStepTile.position = vPosition;

	}


	////////////////////////////////////////////////////////////////////////
    /////////////////////////		PLAN MODE
	private void ParseInput() {

		//		if ( EventSystem.current.IsPointerOverGameObject() ) return;

		// Trace always mouse poited position
		RaycastHit pMouseHitted;
		bool pHittResult = Physics.Raycast( Camera.main.ScreenPointToRay( Input.mousePosition ), out pMouseHitted );

		UsableObject pUsableObject = pMouseHitted.collider.gameObject.GetComponent<UsableObject>();

		// Semi trasparent square sprite for current destination tile
		{
			string objTag = pMouseHitted.collider.tag;
			// make visible the sprite
			if ( objTag == "Tiles" ) {

				pPlanTile.gameObject.SetActive( true );

//				pPlanTile.localRotation = Quaternion.Euler( 0.0f, 87.92f, 45.6f );
				pPlanTile.position = pMouseHitted.collider.transform.position;

			}

		}
		
		// WAIT ACTION
		if ( Input.GetMouseButtonDown( 1 ) ) {

			pAction = new PlayerAction();
			pStageManager.SetAction( this.pAction, this.iID );
//			transform.position = vPlanPosition;
			if ( bPlanDebug ) Debug.Log( "Wait action set" );
			this.SetStepTile( Vector3.up * 10000 );

		}

		// MOVE - USE ACTION
		if ( Input.GetMouseButtonDown( 0 ) ) {

			pAction = null;

			if ( pHittResult ) {

				string objTag = pMouseHitted.collider.tag;
				
				if ( pUsableObject ) {

					// skip non usable objects
					if ( pUsableObject.UseType == UsageType.NONE ) return;

					// KEY
					// USABLE OBJECT
					if ( objTag == "Key" || objTag == "Door" || objTag == "Switcher" || objTag == "Plane_Switcher" ) {



				//////////////////////////////////////////////////////////////////////////////
				//		INSTANT USE
						if (  pUsableObject.UseType == UsageType.INSTANT ) {
							Vector3 vDestination = pMouseHitted.collider.transform.position;
							if ( Vector3.Distance( vPlanPosition, vDestination ) < fUseDistance*1.2f ) {
								this.SetStepTile( vPlanStageDestination = vDestination );
								pAction = new PlayerAction( pUsableObject );
								pStageManager.SetAction( this.pAction, this.iID );
								if ( bPlanDebug ) Debug.Log( "Usable object set" );
								return;
							};

						}


				
						if ( pUsableObject.UseType == UsageType.ON_ACTION_END ) {

							float fObjectDistance = Vector3.Distance( vPlanPosition, pMouseHitted.collider.transform.position );

							//////////////////////////////////////////////////////////////////////////////
							//		OBJECTS ON RANGE AND WITH USE AT STAGE END		
							if ( objTag == "Switcher" ) {

								if ( fObjectDistance > ( fUseDistance * 1.25f ) ) {

									// Calculate path
									pMouseHitted.collider.gameObject.layer = LayerMask.NameToLayer( "Default" );
									pPathFinder.UpdateGrid();
									NodeList vNodes = new NodeList();
									Vector3 vCoord  = pPathFinder.NodeFromWorldPoint( pMouseHitted.collider.gameObject.transform.position ).worldPosition;
									bool found = pPathFinder.FindPath( vPlanPosition, vCoord, out vNodes );
									pMouseHitted.collider.gameObject.layer = LayerMask.NameToLayer( "Unwalkable" );
									pPathFinder.UpdateGrid();

									if ( !found ) return;
									
									if ( ( vNodes.Count > 1 ) )
										vNodes.RemoveRange( vNodes.Count - 1, 1 );

									// if more than 1 node get last otherwise get the node coordinates
									Vector3 vDestinationNode = ( vNodes.Count > 1 ) ? ( vNodes[ vNodes.Count - 1 ].worldPosition ) : ( vNodes[ 0 ].worldPosition );
							//		transform.position = vDestination;

									pAction = new PlayerAction( vDestinationNode, pUsableObject );

								}
								else {
									
									pAction = new PlayerAction( pUsableObject );
								}
								
								Vector3 vDestination = pMouseHitted.collider.gameObject.transform.position;
								this.SetStepTile( vPlanStageDestination = vDestination );
								pStageManager.SetAction( this.pAction, this.iID );
								if ( bPlanDebug ) Debug.Log( "Movement to object set" );

								return;
							}

				//////////////////////////////////////////////////////////////////////////////
				//		OBJECTS WITH USE AT DESTINATION REACHED
							{
								Vector3 vDestination = pMouseHitted.collider.gameObject.transform.position;
								this.SetStepTile( vPlanStageDestination = vDestination );
								pAction = new PlayerAction( vDestination, pUsableObject );
								if ( bPlanDebug ) Debug.Log( "Movement to object set" );
							}
						}

					}

				}


				//////////////////////////////////////////////////////////////////////////////
				//		MOVEMENT ONLY
				if ( objTag == "Tiles" ) {
					Vector3 vDestination = pMouseHitted.collider.gameObject.transform.position;
					this.SetStepTile( vPlanStageDestination = vDestination );
					pAction = new PlayerAction( vDestination );
					if ( bPlanDebug ) Debug.Log( "Movement only set" );

				}

			}
			
			// Finally set action
			if ( pAction != null ) {
				pStageManager.SetAction( this.pAction, this.iID );
				// TODO: UpdateUI
			}
		}
	}


	public	void	OnNextStage() {

		vPlanPosition.Set( vPlanStageDestination.x, vPlanStageDestination.y, vPlanStageDestination.z );

	}

	public	void	OnPrevStage() {



	}

	public	void	OnClearStage() {



	}


}

#pragma warning restore CS0414 // Var assigned but never used
#pragma warning restore CS0162 // Unreachable code detected