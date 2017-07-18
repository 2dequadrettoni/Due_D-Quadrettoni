
using UnityEngine;

using NodeList = System.Collections.Generic.List<Node>;


public partial class Player {

    ////////////////////////////////////////////////////////////////////////
    /////////////////////////		PLAN MODE
    
	private void ParseInput() {

		if ( Input.GetMouseButtonDown( 0 ) ) {

			pAction = null;

			RaycastHit pMouseHitted;
			if ( Physics.Raycast( Camera.main.ScreenPointToRay( Input.mousePosition ), out pMouseHitted ) ) {
				
				UsableObject pUsableObject = pMouseHitted.collider.gameObject.GetComponent<UsableObject>();
				if ( pUsableObject ) {

				//////////////////////////////////////////////////////////////////////////////
				//				OBJECTS INSTANT
					if ( pUsableObject.GetUseType() == UsageType.INSTANT ) {

						if ( Vector3.Distance( transform.position, pMouseHitted.transform.position ) < fUseDistance ) {
							// Usable object hitted
							pAction = new PlayerAction( pUsableObject );
							Debug.Log( "Usable object set" );
						}

					}

				//////////////////////////////////////////////////////////////////////////////
				//				OBJECTS WITH USE AT DESTIANTION REACHED
					if ( pUsableObject.GetUseType() == UsageType.ON_ACTION_END ) {
						transform.position = pMouseHitted.point;
						pAction = new PlayerAction( transform.position, pUsableObject );
						Debug.Log( "Movement to object set" );
					}
				}

				//////////////////////////////////////////////////////////////////////////////
				//				MOVEMENT ONLY
				if ( !pUsableObject ) {
					transform.position = pMouseHitted.point;
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

		

	}

	public	void	OnPrevStage() {



	}

	public	void	OnClearStage() {



	}


    /*
	private void ParseInput() {
		
		// if has not a destination and mouse button is pressed
		if ( Input.GetMouseButtonDown( 0 ) ) {

			// trace a ray from camera to game world
			// if ray hit something
			RaycastHit pMouseHitted;
			if ( Physics.Raycast( Camera.main.ScreenPointToRay( Input.mousePosition ), out pMouseHitted ) ) {


				UsableObject	pEndTurnUsableObject = null;


				// Usable object check
				UsableObject _UsableObject = null;
				if ( _UsableObject = pMouseHitted.collider.gameObject.GetComponent<UsableObject>() ) {

					// is usable object and in use radius
					if ( Vector3.Distance( transform.position, pMouseHitted.transform.position ) < fUseDistance ) {
						// Set as action to use it
						Debug.Log( "Usable action set" );
						if ( _UsableObject.Type == UsableObject.Usabletypes.ON_USE ) {
							_UsableObject.OnUse( this );
							pAction = new PlayerAction( _UsableObject );
							pStageManager.SetAction();
							pUsableObject = _UsableObject;
							return;
						}
					}
					pUsableObject = pEndTurnUsableObject = _UsableObject;
					
				}

				if ( _UsableObject == null && pUsableObject != null ) {
					pUsableObject.OnReset();
					pUsableObject = null;
				}


				////////////////////////////////////////////////////////////
				//	Prepare for the new path
				pNodeList.Clear();

				////////////////////////////////////////////////////////////
				// Search for a path to destination
				pPathFinder.UpdateGrid();
				if ( !pPathFinder.FindPath( vPrevPostion, pMouseHitted.point, out pNodeList  ) ) return;

				////////////////////////////////////////////////////////////
				// finally show the preview
				this.ShowPreview();
				
				// preset as current action
				if ( pEndTurnUsableObject )
					pAction = new PlayerAction( pNodeList, pEndTurnUsableObject );
				else
					pAction = new PlayerAction( pNodeList );

				pStageManager.SetAction();					
			}
		}
	}
    */


}