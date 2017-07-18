
using UnityEngine;

using NodeList = System.Collections.Generic.List<Node>;


public partial class Player {

	////////////////////////////////////////////////////////////////////////
	/////////////////////////		PLAN MODE

	private void ParseInput() {
		
		// if has not a destination and mouse button is pressed
		if ( pPathFinder && !bHasDestination && Input.GetMouseButtonDown( 0 ) ) {

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

	public void ShowPreview() {

		if ( pNodeList.Count < 1 ) return;

		// Destroy if already exists
		if ( pPathPreviewContainer )
			Destroy( pPathPreviewContainer );

		// Create new one
		pPathPreviewContainer = new GameObject( "NodeListPreviewContainer" );

		// for each node of path
		foreach ( Node pNode in pNodeList ) {

			// create a cube on the node
			GameObject p = GameObject.CreatePrimitive( PrimitiveType.Cube );
			p.transform.localScale = new Vector3( p.transform.localScale.x, 0.001f, p.transform.localScale.z ) * pNode.radius;
			p.transform.position = pNode.worldPosition;
			p.transform.SetParent( pPathPreviewContainer.transform );

		}

		// set new position on last node of path
		transform.position = pNodeList[ pNodeList.Count - 1 ].worldPosition;

	}

	// Clear current path
	public void ClearPath() {
		
		if ( !bIsOK ) return;

		// reret current position to previous position
		transform.position = vPrevPostion;

		// clear current node list
		pNodeList.Clear();

		// Destroy if already exists
		Destroy( pPathPreviewContainer );

	}

	private NodeList GetPreviewPath() {
		
		if ( !bIsOK ) return null;

		// Sanity list check
		if ( pNodeList.Count == 0 ) return null;

		// Return a copy of current preview path
		return new NodeList( pNodeList );
	}

}