
using UnityEngine;

using NodeList = System.Collections.Generic.List<Node>;


public partial class Player {

	////////////////////////////////////////////////////////////////////////
	/////////////////////////		PLAY MODE


	public	void	OnPlay() {

		if ( !bIsOK ) return;

		bCanParseInput = false;

		// reset current position to spawn position
		transform.position = vSpawnPostion;

	}

	public void SetPath( NodeList _NodeList ) {

		if ( !bIsOK ) return;

		// List sanity check
		if ( ( _NodeList == null ) || ( _NodeList.Count < 1 ) ) {
			Debug.Log( "Trying to set an invalid path to a player" );
			return;
		}

		// Copy list in execution one
		pNodeList = new NodeList( _NodeList );

	}


	public void Move() {

		if ( !bIsOK ) return;

		// List sanity check
		if ( ( pNodeList == null ) || ( pNodeList.Count < 1 ) ) return;

		// Set flag for run the path
		bHasDestination = true;

		// Reset node index
		iNodeIdx = 0;

	}


	private void UpdateNavigation() {


		// Pick current destination point vector
		Vector3 vDestination = pNodeList[ iNodeIdx ].worldPosition;

		// Set direction



//		if ( transform.position.x < vDestination.x ) { AddDir( DIRECTION.LEFT ); RemDir( DIRECTION.RIGHT ); } else { RemDir( DIRECTION.LEFT ); AddDir( DIRECTION.RIGHT ); }
//		if ( transform.position.z < vDestination.z ) { AddDir( DIRECTION.DOWN ); RemDir( DIRECTION.UP );    } else { RemDir( DIRECTION.DOWN ); AddDir( DIRECTION.UP );    }

/*		// If is under destination
		string sDirection;
		if ( transform.position.z < vDestination.z )
			sDirection = "Up";
		else // else
			sDirection = "Down";


		// 
		if ( transform.position.x < vDestination.x )
			pRenderer.flipX = false;
		else
			pRenderer.flipX = true;

	*/

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

			if ( pUsableObject )  { pUsableObject.OnUse( this ); pUsableObject = null; }

//			pAnimator.Play( "Idle_" + sDirection );
			return;
		}

//		pAnimator.Play( "Walk_" + sDirection );

		// Increase interpolant ( with deltatime )
		fNavInterpolant += Time.deltaTime * fMoveSpeed;

		// Set position to interpolated vector
		transform.position = Vector3.Lerp( transform.position, vDestination, fNavInterpolant );

		// Set as moving
		bIsMoving = true;

	}

}