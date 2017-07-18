
using UnityEngine;

using NodeList = System.Collections.Generic.List<Node>;


public partial class Player {

	////////////////////////////////////////////////////////////////////////
	/////////////////////////		PLAY MODE


	public	void	OnPlay() {
		
		if ( !bIsOK ) return;

		bCanParseInput = false;

		// restore default
		pRenderer.sprite = pOriginalSprite;

		pAnimator.enabled = true;

		// reset current position to spawn position
		transform.position = vSpawnPostion;
		
	}

	public NodeList	FindPath( Vector3 Destination ) {

		NodeList pPath = null;
		if ( pPathFinder.FindPath( transform.position, Destination, out pPath ) )
			return pPath;

		return null;

	}

	public void SetPath( NodeList _NodeList ) {
		
		if ( !bIsOK ) return;

		// List sanity check
		if ( ( _NodeList == null ) || ( _NodeList.Count < 1 ) ) {
			Debug.Log( "Trying to set an invalid path to a player" );
			return;
		}

		// Copy list in execution one
		pNavigation.pNodeList = new NodeList( _NodeList );
		
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


	private void UpdateNavigation() {
		

		// Pick current destination point vector
		Vector3 vDestination = pNavigation.pNodeList[ pNavigation.iNodeIdx ].worldPosition;

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
		if ( pNavigation.fNavInterpolant > 0.99999f ) {
			pNavigation.iNodeIdx++;							// Set new index for the next node
			pNavigation.fNavInterpolant = 0.0f;				// Reset interpolant
		}

		// If there is no list or doesnt contains at last one node or last node is reached
		if ( ( pNavigation.pNodeList == null ) || ( pNavigation.pNodeList.Count < 1 ) || ( pNavigation.iNodeIdx >= pNavigation.pNodeList.Count ) ) {
			pNavigation.bHasDestination = false;            // Restination is reached, make input avaibla again
			pNavigation.bIsMoving		= false;			// Set as not moving
			pNavigation.fNavInterpolant = 0.0f;				// Reset interpolant

			if ( pUsableObject )  { pUsableObject.OnUse( this ); pUsableObject = null; }

//			pAnimator.Play( "Idle_" + sDirection );
			return;
		}

//		pAnimator.Play( "Walk_" + sDirection );

		// Increase interpolant ( with deltatime )
		pNavigation.fNavInterpolant += Time.deltaTime * fMoveSpeed;

		// Set position to interpolated vector
		transform.position = Vector3.Lerp( transform.position, vDestination, pNavigation.fNavInterpolant );

		// Set as moving
		pNavigation.bIsMoving = true;

		
	}

}