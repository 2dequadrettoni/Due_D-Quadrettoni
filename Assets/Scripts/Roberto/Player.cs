// Ref: http://wiki.unity3d.com/index.php/GridMove
using System.Collections.Generic; // IEnumerator
using System.Collections; // IEnumerator
using UnityEngine;
using UnityEngine.AI;

#pragma warning disable CS0162 // Unreachable code detected
#pragma warning disable CS0414 // Var assigned but never used

[RequireComponent( typeof(NavMeshAgent) )]
public class Player : MonoBehaviour {

	const	bool		bDebug = true;
			bool		dbg_bShowLine = false;
			Vector3		dbg_LineStart, dbg_LineEnd;

	[SerializeField]	private 	float			fMoveSpeed				= 3.0f;
						private		bool			bAllowDiagonals			= true;
						private		bool			bCorrectDiagonalSpeed	= true;

						private		enum			Directions : int		{ UP = 1, RIGHT = 1, DOWN = -1, LEFT = -1 };

						private		Vector3			vCurrentDestination		= Vector3.zero;


	//					AI
						private		NavMeshPath		pCurrentPath			= null;
						private		NavMeshAgent	pNavAgent				= null;
						private		bool			bHasDestination			= false;
						private		bool			bIsMoving				= false;
						private		int				iNodeIdx				= -1;


						private		List<Vector3>	vNodes;

						private		CapsuleCollider	pCollider				= null;


	private void Start() {
		
		pCollider		= GetComponent<CapsuleCollider>();

		pNavAgent		= this.GetComponent<NavMeshAgent>();
		pNavAgent.updateRotation = false;

		vNodes			= new List<Vector3>();

		pCurrentPath	= new NavMeshPath();

	}

	public void Update() {

		// if has not a destination
		if ( !bHasDestination ) {

			// if mouse button is pressed
			if ( Input.GetMouseButtonDown( 0 ) ) {

				// trace a ray from camera to game world
				Ray pRay = Camera.main.ScreenPointToRay( Input.mousePosition );
				RaycastHit pMouseHitted;

				// if ray hit something
				if ( Physics.Raycast( pRay, out pMouseHitted ) ) {

					Vector3 vMouseDestination = pMouseHitted.collider.gameObject.transform.position;
					Destroy( pMouseHitted.collider.gameObject );

					dbg_LineStart = transform.position;
					dbg_LineEnd = vMouseDestination;
					dbg_bShowLine = true;

					// try calculate path to collided object position
					pCurrentPath.ClearCorners();
					pNavAgent.ResetPath();
					bool bResult = pNavAgent.CalculatePath( vMouseDestination, pCurrentPath );

					// if path if not founded return
					if ( !bResult ) {
						return;
					}

//					pNavAgent.SetPath( pCurrentPath ); return;
					

					if ( bDebug ) Debug.Log( "path found, corners " + pCurrentPath.corners.Length );

					FindAllPlanesBetweenCorners( vMouseDestination );

					if ( vNodes.Count > 0 ) {
						if ( bDebug ) Debug.Log( "Destination set" );
						bHasDestination = true;
						iNodeIdx = -1;
					}

				}
			}
		}

		if ( dbg_bShowLine ) Debug.DrawLine( dbg_LineStart, dbg_LineEnd, Color.red );

		// if actually has destination but is not set to move
		if ( bHasDestination && !bIsMoving ) {

			iNodeIdx++;
			if ( iNodeIdx < vNodes.Count ) {
				StartCoroutine( Action_Move( vCurrentDestination = vNodes[ iNodeIdx ] ) );
				return;
			}

			dbg_bShowLine = false;
			if ( bDebug ) Debug.Log( "Reset state" );
			bHasDestination			= false;

		}

    }


	private void FindAllPlanesBetweenCorners( Vector3 vDestination ) {

		vNodes.Clear();

		Vector3 f0ffset	= new Vector3( 0.0f, 0.01f, 0.0f );

		// no corners, could be a linear path
		if ( pCurrentPath.corners.Length < 1 ) {

			Vector3 vStartPoint	= transform.position - f0ffset;
			Vector3 vEndPoint	= vDestination - f0ffset;
			Vector3 vDirection	= ( vEndPoint - vStartPoint ).normalized;
			float fDistance		= Vector3.Distance( vStartPoint, vEndPoint );

			RaycastHit pRayCastinfo;
			while ( Physics.Raycast( vStartPoint, vDirection, out pRayCastinfo, fDistance ) ) {

				// next raycast origin point
				vStartPoint	= pRayCastinfo.collider.transform.position;

				if ( pRayCastinfo.collider.tag == "Plane" ) {

					vNodes.Add( vStartPoint );
					if ( bDebug ) Debug.Log( "node added, name: " + pRayCastinfo.collider.gameObject.name );

					pRayCastinfo.collider.gameObject.GetComponent<MeshRenderer>().enabled = false;

				}

			}

			vNodes.Add( vEndPoint );


			// no go on
			return;

		}

		// Print out every corners position
		if ( bDebug )  foreach( Vector3 vec in pCurrentPath.corners ) Debug.Log( "Corner  " + vec );

		////////////////////////////////////////////////////////////////////////////////////////////////
		////////////////////////////////////////////////////////////////////////////////////////////////
		//						FIRST STEP
		{

			Vector3 vStartPoint	= transform.position - f0ffset;
			Vector3 vEndPoint	= pCurrentPath.corners[ 0 ] - f0ffset;
			Vector3 vDirection	= ( vEndPoint - vStartPoint ).normalized;
			float fDistance		= Vector3.Distance( vStartPoint, vEndPoint );

			RaycastHit pRayCastinfo;
			while ( Physics.Raycast( vStartPoint, vDirection, out pRayCastinfo, fDistance ) ) {

//				if ( pRayCastinfo.collider.bounds.Contains( vEndPoint ) ) break;

				if ( pRayCastinfo.collider.tag == "Plane" ) {

					vStartPoint	= pRayCastinfo.collider.gameObject.transform.position;
					vNodes.Add( vStartPoint );
					if ( bDebug ) Debug.Log( "Plane added, name: " + pRayCastinfo.collider.gameObject.name );

					pRayCastinfo.collider.gameObject.GetComponent<MeshRenderer>().enabled = false;

				}

				fDistance		= Vector3.Distance( vStartPoint, vEndPoint );

			}

		}

		
		////////////////////////////////////////////////////////////////////////////////////////////////
		////////////////////////////////////////////////////////////////////////////////////////////////
		//						ALL UNTIL LAST CORNER

		
		// per ogni corner trovato
		for ( int i = 0; i < pCurrentPath.corners.Length - 1; i++ ) {

			if ( bDebug ) Debug.Log( "Corner " + i );

			Vector3 vStartPoint	= pCurrentPath.corners[ i ] - f0ffset;
			Vector3 vEndPoint	= pCurrentPath.corners[ i + 1 ] - f0ffset;
			Vector3 vDirection	= ( vEndPoint - vStartPoint ).normalized;
			float fDistance		= Vector3.Distance( vStartPoint, vEndPoint );
			
			RaycastHit pRayCastinfo;
			while ( Physics.Raycast( vStartPoint, vDirection, out pRayCastinfo, fDistance ) ) {

//				if ( pRayCastinfo.collider.bounds.Contains( vEndPoint ) ) break;

				// same vector
				if ( fDistance < 0.001f ) break;

				if ( pRayCastinfo.collider.tag == "Plane" ) {

					vStartPoint	= pRayCastinfo.collider.gameObject.transform.position;
					vNodes.Add( vStartPoint );
					if ( bDebug ) Debug.Log( "Plane added, name: " + pRayCastinfo.collider.gameObject.name );

					pRayCastinfo.collider.gameObject.GetComponent<MeshRenderer>().enabled = false;

				}

				fDistance		= Vector3.Distance( vStartPoint, vEndPoint );

			}
						
		}

		////////////////////////////////////////////////////////////////////////////////////////////////
		////////////////////////////////////////////////////////////////////////////////////////////////
		//						LAST STEP
		{

			Vector3 vStartPoint	= pCurrentPath.corners[ pCurrentPath.corners.Length - 1 ] - f0ffset;
			Vector3 vEndPoint	= vDestination - f0ffset;
			Vector3 vDirection	= ( vEndPoint - vStartPoint ).normalized;
			float fDistance		= Vector3.Distance( vStartPoint, vEndPoint );

			RaycastHit pRayCastinfo;
			while ( Physics.Raycast( vStartPoint, vDirection, out pRayCastinfo, fDistance ) ) {
				
				if ( pRayCastinfo.collider.bounds.Contains( vEndPoint ) ) break;

				// same vector
				if ( fDistance < 0.001f ) break;

				if ( pRayCastinfo.collider.tag == "Plane" ) {

					vStartPoint	= pRayCastinfo.collider.gameObject.transform.position;
					vNodes.Add( vStartPoint );
					if ( bDebug ) Debug.Log( "Plane added, name: " + pRayCastinfo.collider.gameObject.name );

					pRayCastinfo.collider.gameObject.GetComponent<MeshRenderer>().enabled = false;

				}

				fDistance		= Vector3.Distance( vStartPoint, vEndPoint );

			}

		}
		
	}

    public IEnumerator Action_Move( Vector3 vDestination ) {

		// Set as moving
		bIsMoving = true;
		
		// if Diagonal movement enabled, scale movement
		float fDiagonalFactor = 1.0f;
//		if( bAllowDiagonals && ( vDestination.x != 0.0f ) && ( vDestination.z != 0.0f ) ) {
//			fDiagonalFactor = 0.7071f;
//		}
		
		Debug.Log( "Coroutine Start" );

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

		Debug.Log( "Coroutine End" );

		// Now that movement is finished, set as not moving and return
		bIsMoving = false;
		yield return 0;
    }

}

#pragma warning restore CS0414 // Var assigned but never used
#pragma warning restore CS0162 // Unreachable code detected