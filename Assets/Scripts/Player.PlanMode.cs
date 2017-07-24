
using UnityEngine;

#pragma warning disable CS0162 // Unreachable code detected
#pragma warning disable CS0414 // Var assigned but never used

public partial class Player {

	const	bool	bPlanDebug		= false;

    ////////////////////////////////////////////////////////////////////////
    /////////////////////////		PLAN MODE
    
	private void ParseInput() {

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

			RaycastHit pMouseHitted;
			if ( Physics.Raycast( Camera.main.ScreenPointToRay( Input.mousePosition ), out pMouseHitted ) ) {

				string objTag = pMouseHitted.collider.tag;

				UsableObject pUsableObject = pMouseHitted.collider.gameObject.GetComponent<UsableObject>();
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
								// Usable object hitted
								pAction = new PlayerAction( pUsableObject );
								pStageManager.SetAction( this.pAction, this.iID );
								if ( bPlanDebug ) Debug.Log( "Usable object set" );
								return;
							};

						}


				//////////////////////////////////////////////////////////////////////////////
				//		OBJECTS WITH USE AT DESTINATION REACHED
						if ( pUsableObject.UseType == UsageType.ON_ACTION_END ) {
							transform.position = pPathFinder.NodeFromWorldPoint( pMouseHitted.point ).worldPosition;
							pAction = new PlayerAction( pMouseHitted.point, pUsableObject );
							if ( bPlanDebug ) Debug.Log( "Movement to object set" );
						}

					}
				}


				//////////////////////////////////////////////////////////////////////////////
				//		MOVEMENT ONLY
				if ( objTag == "Tiles" || objTag == "Platform" ) {
					transform.position = pPathFinder.NodeFromWorldPoint( pMouseHitted.point ).worldPosition;
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