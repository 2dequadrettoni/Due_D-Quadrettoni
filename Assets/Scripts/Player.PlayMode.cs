
using UnityEngine;

using NodeList = System.Collections.Generic.List<Node>;


public partial class Player {

	////////////////////////////////////////////////////////////////////////
	/////////////////////////		PLAY MODE


	public	void	OnPlay() {
		
		if ( !bIsOK ) return;

		foreach( Transform o in vSteps )
			if ( o != null )
			Destroy( o.gameObject );

		bCanParseInput = false;

		pAction = null;

		pAnimator.enabled = true;

		// reset current position to spawn position
		transform.position = vSpawnPostion;
		
	}

	public bool		FindPath( Vector3 Destination ) {

		pNavigation.pNodeList = null;

		return GLOBALS.PathFinder.FindPath( transform.position, Destination, out pNavigation.pNodeList );

	}


	public void		Move() {
		
		if ( !bIsOK ) return;

		// List sanity check
		if ( ( pNavigation.pNodeList == null ) || ( pNavigation.pNodeList.Count < 1 ) ) return;

		// Set flag for run the path
		pNavigation.bHasDestination = true;

		// Reset node index
		pNavigation.iNodeIdx = 0;

		pNavigation.vDestination = pNavigation.pNodeList[ 0 ].worldPosition;
		
	}

	public void		Stop() {

		if ( pNavigation.pNodeList != null ) {
			pNavigation.pNodeList.Clear();
			pNavigation.pNodeList		= null;
		}

		pNavigation.bHasDestination = false;
		pNavigation.bIsMoving		= false;

	}


	public	void	SetIdle() { pAnimator.Play( "Idle_" + sDirection ); }
	


	private void	UpdateNavigation() {

		// DIRECTION
		if ( pNavigation.iNodeIdx < pNavigation.pNodeList.Count - 1 )
		{	
			Node pCurrentNode	= pNavigation.pNodeList[ pNavigation.iNodeIdx ];
			Node pNextNode		= pNavigation.pNodeList[ pNavigation.iNodeIdx + 1 ];

			if ( pCurrentNode.gridX < pNextNode.gridX )
				sDirection = "Up";
			if ( pCurrentNode.gridX > pNextNode.gridX )
				sDirection = "Down";
			if ( pCurrentNode.gridX == pNextNode.gridX )
				sDirection = ( pCurrentNode.gridY < pNextNode.gridY ) ? "Down" : "Up";

			if ( pCurrentNode.gridY < pNextNode.gridY )
				pRenderer.flipX = bFlipped = true;
			if ( pCurrentNode.gridY > pNextNode.gridY )
				pRenderer.flipX = bFlipped = false;
			if ( pCurrentNode.gridY == pNextNode.gridY )
				pRenderer.flipX = bFlipped = ( pCurrentNode.gridX < pNextNode.gridX ) ? true : false;

		}


		// is arrived
		if ( Vector3.Distance( transform.position, pNavigation.vDestination ) < .2f ) {
			pNavigation.iNodeIdx++;

			// If there is no list or doesnt contains at last one node or last node is reached
			if ( pNavigation.iNodeIdx == pNavigation.pNodeList.Count ) {
				pNavigation.bHasDestination = false;            // Destination reached, make input avaible again
				pNavigation.bIsMoving		= false;			// Set as not moving
				transform.position			= pNavigation.vDestination;

				if ( !AnimationOverride )
					pAnimator.Play( "Idle_" + sDirection );
				return;
			}

			pNavigation.vDestination = pNavigation.pNodeList[ pNavigation.iNodeIdx ].worldPosition;
		}


		Vector3 vDirection = ( pNavigation.vDestination - transform.position ).normalized;
		transform.position += Time.deltaTime * ( fMoveSpeed * vDirection );
		pNavigation.bIsMoving = true;
		if ( !AnimationOverride )
			pAnimator.Play( "Walk_" + sDirection );
		
	}


	public void		Link( Platform pPlatform ) {

		bLinked = true;
		pLinkedObject = pPlatform;

	}

	public void		UnLink( Platform pPlatform ) {

		bLinked = false;
		pLinkedObject = null;

	}

	
	private	void	OnDrawGizmos() {

		if ( pNavigation.pNodeList != null ) {
			foreach (Node n in pNavigation.pNodeList) {
				Gizmos.color = Color.black;
				Gizmos.DrawCube(n.worldPosition, Vector3.one * (n.radius-.1f));
			}
		}

	}
	
}