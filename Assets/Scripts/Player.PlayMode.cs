
using UnityEngine;

using NodeList = System.Collections.Generic.List<Node>;


public partial class Player {

	////////////////////////////////////////////////////////////////////////
	/////////////////////////		PLAY MODE


	public	void	OnPlay() {
		
		if ( !bIsOK ) return;

		bCanParseInput = false;

		pAction = null;

		// restore default
		pRenderer.sprite = pOriginalSprite;

		pAnimator.enabled = true;

		// reset current position to spawn position
		transform.position = vSpawnPostion;
		
	}

	public bool	FindPath( Vector3 Destination ) {

		pNavigation.pNodeList = null;

		return pPathFinder.FindPath( transform.position, Destination, out pNavigation.pNodeList );

	}


	public void Move() {
		
		if ( !bIsOK ) return;

		// List sanity check
		if ( ( pNavigation.pNodeList == null ) || ( pNavigation.pNodeList.Count < 1 ) ) return;

		// Set flag for run the path
		pNavigation.bHasDestination = true;

		// Reset node index
		pNavigation.iNodeIdx = 0;
		
	}

	public void	Stop() {

		pNavigation.pNodeList.Clear();
		pNavigation.pNodeList		= null;
		pNavigation.bHasDestination = false;
		pNavigation.bIsMoving		= false;

	}


	private void UpdateNavigation() {

		// always start from zero
		// Pick current destination point vector
		Vector3 vDestination = pNavigation.pNodeList[ pNavigation.iNodeIdx ].worldPosition;

		// is arrived
		if ( Vector3.Distance( transform.position, vDestination ) < .2f ) {
			pNavigation.iNodeIdx++;
		}

		
		// If there is no list or doesnt contains at last one node or last node is reached
		if ( pNavigation.iNodeIdx == pNavigation.pNodeList.Count ) {
			pNavigation.bHasDestination = false;            // Destination reached, make input avaible again
			pNavigation.bIsMoving		= false;			// Set as not moving
			transform.position			= vDestination;

//			pAnimator.Play( "Idle_" + sDirection );
			return;
		}

		Vector3 vDirection = ( vDestination - transform.position ).normalized;

		transform.position += Time.deltaTime * ( fMoveSpeed * vDirection );

		pNavigation.bIsMoving = true;

//		pAnimator.Play( "Walk_" + sDirection );

		
	}


	public void Link( Platform pPlatform ) {

		bLinked = true;
		pLinkedObject = pPlatform;

	}

	public void UnLink( Platform pPlatform ) {

		bLinked = false;
		pLinkedObject = null;

	}

	
	void OnDrawGizmos() {

		if (pNavigation.pNodeList != null) {
			foreach (Node n in pNavigation.pNodeList) {
				Gizmos.color = Color.black;
				Gizmos.DrawCube(n.worldPosition, Vector3.one * (n.radius-.1f));
			}
		}

	}
	
}