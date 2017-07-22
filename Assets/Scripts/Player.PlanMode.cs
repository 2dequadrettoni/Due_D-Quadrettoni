﻿
using UnityEngine;

using NodeList = System.Collections.Generic.List<Node>;


public partial class Player {

    ////////////////////////////////////////////////////////////////////////
    /////////////////////////		PLAN MODE
    
	private void ParseInput() {

		if ( Input.GetMouseButtonDown( 1 ) ) {

			pAction = new PlayerAction();
			pStageManager.SetAction( this.pAction, this.iID );
			transform.position = vPlanPosition;
			Debug .Log( "Wait action set" );

		}

		if ( Input.GetMouseButtonDown( 0 ) ) {

			pAction = null;

			RaycastHit pMouseHitted;
			if ( Physics.Raycast( Camera.main.ScreenPointToRay( Input.mousePosition ), out pMouseHitted ) ) {
				
				if ( pMouseHitted.collider.gameObject.tag != "Tiles" ) return;

				UsableObject pUsableObject = pMouseHitted.collider.gameObject.GetComponent<UsableObject>();
				if ( pUsableObject ) {

				//////////////////////////////////////////////////////////////////////////////
				//				OBJECTS INSTANT
					if ( pUsableObject.UseType == UsageType.INSTANT ) {

						if ( Vector3.Distance( vPlanPosition, pMouseHitted.point ) < fUseDistance ) {
							// Usable object hitted
							pAction = new PlayerAction( pUsableObject );
							Debug.Log( "Usable object set" );
						}

					}

				//////////////////////////////////////////////////////////////////////////////
				//				OBJECTS WITH USE AT DESTIANTION REACHED
					if ( pUsableObject.UseType == UsageType.ON_ACTION_END ) {
						transform.position = pPathFinder.NodeFromWorldPoint( pMouseHitted.point ).worldPosition;

				//		transform.position = pMouseHitted.point;
						pAction = new PlayerAction( pMouseHitted.point, pUsableObject );
						Debug.Log( "Movement to object set" );
					}
				}

				//////////////////////////////////////////////////////////////////////////////
				//				MOVEMENT ONLY
				if ( !pUsableObject ) {
					transform.position = pPathFinder.NodeFromWorldPoint( pMouseHitted.point ).worldPosition;
				//	transform.position = pMouseHitted.point;
					pAction = new PlayerAction( transform.position );
					Debug.Log( "Movement only set" );

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