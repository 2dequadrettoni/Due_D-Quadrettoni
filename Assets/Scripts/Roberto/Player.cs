// Ref: http://wiki.unity3d.com/index.php/GridMove
using System.Collections.Generic; // IEnumerator
using System.Collections; // IEnumerator
using UnityEngine;
using UnityEngine.AI;

#pragma warning disable CS0162 // Unreachable code detected
#pragma warning disable CS0414 // Var assigned but never used

[RequireComponent( typeof(NavMeshAgent) )]
public class Player : MonoBehaviour {

	const bool bDebug = true;

	[SerializeField]	private 	float			fMoveSpeed				= 3.0f;
						private		bool			bAllowDiagonals			= true;
						private		bool			bCorrectDiagonalSpeed	= true;

						private		enum			Directions : int		{ UP = 1, RIGHT = 1, DOWN = -1, LEFT = -1 };

						private		Vector2			vPosition				= Vector2.zero;


	//					AI
						private		NavMeshPath		pCurrentPath			= null;
						private		NavMeshAgent	pNavAgent				= null;
						private		bool			bHasDestination			= false;
						private		bool			bIsMoving				= false;
						private		int				iNodeIdx				= -1;


						private		List<Vector3>	vPlanes;

						private		CapsuleCollider	pCollider				= null;


	private void Start() {
		
		pCollider		= GetComponent<CapsuleCollider>();

		pNavAgent		= this.GetComponent<NavMeshAgent>();
		pNavAgent.updateRotation = false;

		vPlanes			= new List<Vector3>();

		pCurrentPath	= new NavMeshPath();

	}

	public void Update() {

		// if has not a destination
		if ( !bHasDestination ) {

			// if mouse button is pressed
			if ( Input.GetMouseButtonDown( 0 ) ) {

				if ( bDebug ) Debug.Log( "Mouse press" );

				// trace a ray from camera to game world
				Ray pRay = Camera.main.ScreenPointToRay( Input.mousePosition );
				RaycastHit pMouseHitted;

				// if ray hit something
				if ( Physics.Raycast( pRay, out pMouseHitted ) ) {

					if ( bDebug ) Debug.Log( "Mouse hit" );

					Vector3 vMouseCollision = pMouseHitted.collider.gameObject.transform.position;

					// try calculate path to collided object position
					pCurrentPath.ClearCorners();
					bool bResult = pNavAgent.CalculatePath( vMouseCollision, pCurrentPath );

					// if path if not founded return
					if ( !bResult ) {
						return;
					}

					transform.position = pCurrentPath.corners[ 0 ];

					if ( bDebug ) Debug.Log( "path found, length " + pCurrentPath.corners.Length );


					vPlanes.Clear();
					if ( bDebug )  foreach( Vector3 vec in pCurrentPath.corners )
						Debug.Log( "Corner  " + vec );


					for ( int i = 0; i < pCurrentPath.corners.Length - 1; i++ ) {

						if ( bDebug ) Debug.Log( "Corner " + i );

						Vector3 f0ffset = new Vector3( 0.0f, 0.01f, 0.0f );

						Vector3 vStartPoint	= pCurrentPath.corners[ i ] - f0ffset;
						Vector3 vEndPoint	= pCurrentPath.corners[ i + 1 ] - f0ffset;
						Vector3 vDirection	= ( vStartPoint - vEndPoint ).normalized;
						float fDistance		= Vector3.Distance( vStartPoint, vEndPoint );
						
						RaycastHit [] vHits = Physics.RaycastAll( vStartPoint, vDirection, fDistance);

						foreach( RaycastHit pHit in vHits ) {
							if ( pHit.collider.tag == "Plane" ) {
								vPlanes.Add( pHit.collider.gameObject.transform.position );
								if ( bDebug ) Debug.Log( "Plane added " );
							}
						}
						
					}
					
					iNodeIdx = -1;

					if ( vPlanes.Count > 0 ) {
						if ( bDebug ) Debug.Log( "Destination set" );
						bHasDestination = true;
					}

				}
			}
		}

		// if actually has destination but is not set to move
		if ( bHasDestination && !bIsMoving ) {

			iNodeIdx++;
			if ( iNodeIdx < pCurrentPath.corners.Length ) {
				bIsMoving = true;
				StartCoroutine( Action_Move( pCurrentPath.corners[ iNodeIdx ] ) );
				return;
			}

			if ( bDebug ) Debug.Log( "Reset state" );
			bHasDestination			= false;

		}
		

    }

    public IEnumerator Action_Move( Vector3 vDestination ) {

		// Set as moving
		
		float fInterpolant = 0.0f;
		
		// if Diagonal movement enabled, scale movement
		float fDiagonalFactor = 1.0f;
//		if( bAllowDiagonals && ( vDestination.x != 0.0f ) && ( vDestination.z != 0.0f ) ) {
//			fDiagonalFactor = 0.7071f;
//		}
		
		Debug.Log( "Coroutine Start" );

		// While interpolant is not full
		while ( fInterpolant < 1.0f ) {

			// Increase interpolant
			fInterpolant += Time.deltaTime * fMoveSpeed * fDiagonalFactor;

			// Set next lerped position
			transform.position = Vector3.Lerp( transform.position, vDestination, Mathf.Clamp01( fInterpolant ) );

			yield return null;

		}

		Debug.Log( "Coroutine End" );

		transform.position = vDestination;

		// Now that movement is finished, set as not moving and return
		bIsMoving = false;
		yield return 0;
    }

}

#pragma warning restore CS0414 // Var assigned but never used
#pragma warning restore CS0162 // Unreachable code detected