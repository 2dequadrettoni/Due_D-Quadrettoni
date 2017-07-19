
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
		pNavigation.pNodeList = null;
		pNavigation.bHasDestination = false;

	}


	private void UpdateNavigation() {
		
		// If next node is reached
		if ( pNavigation.fNavInterpolant > 0.99999f ) {
			pNavigation.iNodeIdx++;							// Set new index for the next node
			pNavigation.fNavInterpolant = 0.0f;				// Reset interpolant
		}

		// If there is no list or doesnt contains at last one node or last node is reached
		if ( ( pNavigation.pNodeList == null ) || ( pNavigation.pNodeList.Count < 1 ) || ( pNavigation.iNodeIdx >= pNavigation.pNodeList.Count ) ) {
			pNavigation.bHasDestination = false;            // Restination is reached, make input avaibla again
			pNavigation.bIsMoving		= false;			// Set as not moving
			pNavigation.fNavInterpolant = 0.0f;				// Reset interpolant

//			pAnimator.Play( "Idle_" + sDirection );
			return;
		}

//		pAnimator.Play( "Walk_" + sDirection );

		// Pick current destination point vector
		Vector3 vDestination = pNavigation.pNodeList[ pNavigation.iNodeIdx ].worldPosition;

		// Increase interpolant ( with deltatime )
		pNavigation.fNavInterpolant += Time.deltaTime * fMoveSpeed;

		// Set position to interpolated vector
		transform.position = Vector3.Lerp( transform.position, vDestination, pNavigation.fNavInterpolant );

		// Set as moving
		pNavigation.bIsMoving = true;
		
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