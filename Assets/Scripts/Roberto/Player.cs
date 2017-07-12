// Ref: http://wiki.unity3d.com/index.php/GridMove
using System.Collections.Generic; // IEnumerator
using System.Collections; // IEnumerator
using UnityEngine;
using UnityEngine.AI;

[RequireComponent( typeof(NavMeshAgent) )]
public class Player : MonoBehaviour {

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


	private void Start() {
		
		pNavAgent	= this.GetComponent<NavMeshAgent>();
		vPlanes		= new List<Vector3>();

	}

	public void Update() {

		// if has not a destination
		if ( !bHasDestination ) {

			// if mouse button is pressed
			if ( Input.GetMouseButtonDown( 0 ) ) {

				Debug.Log( "Mouse press" );

				// trace a ray from camera to game world
				Ray pRay = Camera.main.ScreenPointToRay( Input.mousePosition );
				RaycastHit pMouseHitted;

				// if ray hit something
				if ( Physics.Raycast( pRay, out pMouseHitted ) ) {

					Debug.Log( "Mouse hit" );

					Vector3 vMouseCollision = pMouseHitted.collider.gameObject.transform.position;

					// try calculate path to collided object position
					pCurrentPath = new NavMeshPath();
					bool bResult = pNavAgent.CalculatePath( vMouseCollision, pCurrentPath );

					// if path if not founded return
					if ( !bResult ) {
						pCurrentPath = null;
						return;
					}

					Debug.Log( "path found, length " + pCurrentPath.corners.Length );


					vPlanes.Clear();
					foreach( Vector3 vec in pCurrentPath.corners ) {
						Debug.Log( "Corner  " + vec );
					}

					for ( int i = 0; i < pCurrentPath.corners.Length - 1; i++ ) {

						Vector3 vStartPoint = Vector3.zero;
						if ( i == 0 ) {
							vStartPoint = transform.position;
						}
						else {
							vStartPoint = pCurrentPath.corners[ i ];
						}

						Debug.Log( "Corner " + i );
						
						foreach( Vector3 vec in pCurrentPath.corners ) {
							vPlanes.Add( vec );
						}
						/*

						Vector3 vDirection = ( vStartPoint - pCurrentPath.corners[i + 1] ).normalized;
						RaycastHit[] vHits = Physics.BoxCastAll(
									vStartPoint,
									new Vector3( 0.01f, 300.0f, Vector3.Distance( vStartPoint, pCurrentPath.corners[i + 1] ) ),
									vDirection,
									Quaternion.LookRotation( vDirection )
								);

						Debug.Log( "Hits " + vHits.Length );

						foreach( RaycastHit pHit in vHits ) {
							if ( pHit.collider.tag == "Plane" )
								vPlanes.Add( pHit.collider.gameObject.transform.position );
						}
						*/
					}
					
					iNodeIdx = -1;

					if ( vPlanes.Count > 0 ) {
						Debug.Log( "Destination set" );
						bHasDestination = true;
					}

				}
			}
		}

		// if actually has destination but is not set to move
		if ( !bIsMoving && bHasDestination ) {

			iNodeIdx++;
			StartCoroutine( Action_Move( this.transform, /*pCurrentPath.corners[ iNodeIdx ]*/ vPlanes[iNodeIdx]) );

		}
		

    }

    public IEnumerator Action_Move( Transform transform, Vector3 vDestination ) {

		// Set as moving
		bIsMoving = true;
		float fInterpolant = 0.0f;

		// if Diagonal movement enabled, scale movement
		float fDiagonalFactor = 1.0f;
		if( bAllowDiagonals && ( vDestination.x != 0.0f ) && ( vDestination.z != 0.0f ) ) {
			fDiagonalFactor = 0.7071f;
		}

		// While interpolant is not full
		while ( fInterpolant < 1.0f ) {

			// Increase interpolant
			fInterpolant += Time.deltaTime * fMoveSpeed * fDiagonalFactor;

			// Set next lerped position
			transform.position = Vector3.Lerp( transform.position, vDestination, fInterpolant );

			yield return null;

		}

		// Now that movement is finished, set as not moving and return
		bIsMoving = false;
		if ( iNodeIdx >= pCurrentPath.corners.Length ) bHasDestination = false;
		yield return 0;
    }

}
