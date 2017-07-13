// Ref: http://wiki.unity3d.com/index.php/GridMove
using System.Collections.Generic; // IEnumerator
using System.Collections; // IEnumerator
using UnityEngine;
using UnityEngine.AI;

#pragma warning disable CS0162 // Unreachable code detected
#pragma warning disable CS0414 // Var assigned but never used

public class Player : MonoBehaviour {

	const	bool		bDebug = true;

	[SerializeField]	private 	float			fMoveSpeed				= 3.0f;

						private		enum			Directions : int		{ UP = 1, RIGHT = 1, DOWN = -1, LEFT = -1 };

						private		Vector3			vStartPosition			= Vector3.zero;
						private		Vector3			vCurrentDestination		= Vector3.zero;


	//					AI
						private		bool			bHasDestination			= false;
						private		bool			bIsMoving				= false;

						private		CapsuleCollider	pCollider				= null;

						private		Pathfinding		pPathFinder				= null;
						private		int				iNodeIdx				= -1;
						private		List<Node>		pNodeList				= null;
	

	private void Start() {
		
		pCollider	= GetComponent<CapsuleCollider>();

		pPathFinder = GameObject.Find( "PathFinder" ).GetComponent<Pathfinding>();

		pNodeList = new List<Node>();
	}

	private void Update() {

		// if has not a destination
		if ( !bHasDestination ) {

			// if mouse button is pressed
			if ( Input.GetMouseButtonDown( 0 ) ) {

				// trace a ray from camera to game world
				Ray pRay = Camera.main.ScreenPointToRay( Input.mousePosition );
				RaycastHit pMouseHitted;

				// if ray hit something
				if ( Physics.Raycast( pRay, out pMouseHitted ) ) {

					pNodeList.Clear();
					if ( !pPathFinder.FindPath( transform.position, pMouseHitted.point, out pNodeList  ) ) return;

					bHasDestination = true;
					iNodeIdx = -1;

				}
			}
		}

		// if actually has destination but is not set to move
		if ( bHasDestination && !bIsMoving ) {

			iNodeIdx++;
			if ( ( pNodeList != null ) && ( iNodeIdx < pNodeList.Count ) ) {
				StartCoroutine( Action_Move( vCurrentDestination = pNodeList[ iNodeIdx ].worldPosition ) );
				return;
			}

			bHasDestination			= false;

		}

    }
	
	
    private IEnumerator Action_Move( Vector3 vDestination ) {

		// Set as moving
		bIsMoving = true;
		
		// if Diagonal movement enabled, scale movement
		float fDiagonalFactor = 1.0f;
		if( ( vDestination.x != 0.0f ) && ( vDestination.z != 0.0f ) ) {
			fDiagonalFactor = 0.7071f;
		}

		float fInterpolant = 0.0f;
		// While interpolant is not full
		while ( fInterpolant < 1.0f ) {

			// Increase interpolant
			fInterpolant += Time.deltaTime * fMoveSpeed * fDiagonalFactor;

			// Set next lerped position
			transform.position = Vector3.Lerp( transform.position, vDestination, Mathf.Clamp01( fInterpolant ) );
			transform.position = new Vector3( transform.position.x, 0.0f, transform.position.z );

			yield return null;

		}

		// Now that movement is finished, set as not moving and return
		bIsMoving = false;
		yield return 0;
    }

}

#pragma warning restore CS0414 // Var assigned but never used
#pragma warning restore CS0162 // Unreachable code detected