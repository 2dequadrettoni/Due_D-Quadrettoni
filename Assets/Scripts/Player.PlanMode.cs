
using UnityEngine;

using NodeList = System.Collections.Generic.List<Node>;

//	using UnityEngine.EventSystems;

#pragma warning disable CS0162 // Unreachable code detected
#pragma warning disable CS0414 // Var assigned but never used

public partial class Player {

	const	bool	bPlanDebug		= false;

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

		//	if ( objTag == "Key" || objTag == "Switcher" || objTag == "Plane_Switcher" || objTag == "Tiles" || objTag == "Platform" ) {

				// make visible the sprite
				if ( objTag == "Tiles" ) {
					pCurrentDestSprite.localRotation = Quaternion.Euler( 90.0f, 0.0f, -90.0f );
					pCurrentDestSprite.position = pMouseHitted.collider.transform.position;
				}

		//	}

		}
		
		// WAIT ACTION
		if ( Input.GetMouseButtonDown( 1 ) ) {

			pAction = new PlayerAction();
			pStageManager.SetAction( this.pAction, this.iID );
			transform.position = vPlanPosition;
			if ( bPlanDebug ) Debug.Log( "Wait action set" );

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
				//		OBJECTS INSTANT
						if (  pUsableObject.UseType == UsageType.INSTANT ) {
							if ( Vector3.Distance( vPlanPosition, pMouseHitted.collider.transform.position ) < fUseDistance ) {
								transform.position = vPlanPosition;
								// Usable object hitted
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

								if ( fObjectDistance > (fUseDistance * 1.25f ) ) {

									NodeList vNodes = new NodeList();
									bool found = pPathFinder.FindPath( vPlanPosition, pMouseHitted.collider.transform.position, out vNodes );

									if ( !found ) return;

									if ( ( vNodes.Count > 1 ) )
										vNodes.RemoveRange( vNodes.Count - 1, 1 );

									// if more than 1 node get last otherwise get the node coordinates
									Vector3 vDestination = ( vNodes.Count > 1 ) ? ( vNodes[ vNodes.Count - 1 ].worldPosition ) : ( vNodes[ 0 ].worldPosition );

									transform.position = vDestination;
									pAction = new PlayerAction( vDestination, pUsableObject );

								}
								else {
//									transform.position = vPlanPosition;
									pAction = new PlayerAction( pUsableObject );
								}
								
								pStageManager.SetAction( this.pAction, this.iID );
								if ( bPlanDebug ) Debug.Log( "Movement to object set" );

								return;
							}

				//////////////////////////////////////////////////////////////////////////////
				//		OBJECTS WITH USE AT DESTINATION REACHED
							{

								transform.position = pMouseHitted.transform.position;
								pAction = new PlayerAction( pMouseHitted.point, pUsableObject );
								if ( bPlanDebug ) Debug.Log( "Movement to object set" );
							}
						}

					}

				}


				//////////////////////////////////////////////////////////////////////////////
				//		MOVEMENT ONLY
				if ( objTag == "Tiles" || objTag == "Platform" ) {
					transform.position = pMouseHitted.collider.gameObject.transform.position;
				//	transform.position = pCurrentDestSprite.position;
					pAction = new PlayerAction( transform.position );
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

		vPlanPosition.Set( transform.position.x, transform.position.y, transform.position.z );

	}

	public	void	OnPrevStage() {



	}

	public	void	OnClearStage() {



	}


}

#pragma warning restore CS0414 // Var assigned but never used
#pragma warning restore CS0162 // Unreachable code detected