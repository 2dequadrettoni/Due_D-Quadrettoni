
using UnityEngine;

using NodeList = System.Collections.Generic.List<Node>;


public partial class Player {

	////////////////////////////////////////////////////////////////////////
	/////////////////////////		PLAY MODE


	public	void	OnPlay() {

		bCanParseInput = false;

		// reset current position to spawn position
		transform.position = vSpawnPostion;

	}

	public void SetPath( NodeList _NodeList ) {

		// List sanity check
		if ( ( _NodeList == null ) || ( _NodeList.Count < 1 ) ) {
			Debug.Log( "Trying to set an invalid path to a player" );
			return;
		}

		// Copy list in execution one
		pNodeList = new NodeList( _NodeList );

	}


	public void Move() {

		// List sanity check
		if ( ( pNodeList == null ) || ( pNodeList.Count < 1 ) ) return;

		// Set flag for run the path
		bHasDestination = true;

		// Reset node index
		iNodeIdx = 0;

	}


	private void UpdateNavigation() {

		// If next node is reached
		if ( fNavInterpolant > 0.99999f ) {
			iNodeIdx++;							// Set new index for the next node
			fNavInterpolant = 0.0f;				// Reset interpolant
		}

		// If there is no list or doesnt contains at last one node or last node is reached
		if ( ( pNodeList == null ) || ( pNodeList.Count < 1 ) || ( iNodeIdx >= pNodeList.Count ) ) {
			bHasDestination = false;            // Restination is reached, make input avaibla again
			bIsMoving		= false;			// Set as not moving
			fNavInterpolant = 0.0f;				// Reset interpolant
			return;
		}


		// Pick current destination point vector
		Vector3 vDestination = pNodeList[ iNodeIdx ].worldPosition;

		// Increase interpolant ( with deltatime )
		fNavInterpolant += Time.deltaTime * fMoveSpeed;

		// Set position to interpolated vector
		transform.position = Vector3.Lerp( transform.position, vDestination, fNavInterpolant );

		// Set as moving
		bIsMoving = true;

	}

}