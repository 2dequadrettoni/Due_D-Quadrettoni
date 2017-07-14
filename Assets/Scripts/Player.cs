// Ref: http://wiki.unity3d.com/index.php/GridMove

	#define PATHFINDING

using System.Collections.Generic; // IEnumerator
using System.Collections; // IEnumerator
using UnityEngine;

using NodeList = System.Collections.Generic.List<Node>;

#pragma warning disable CS0162 // Unreachable code detected
#pragma warning disable CS0414 // Var assigned but never used

public class Player : MonoBehaviour {

				const bool		bDebug 					= true;

	[SerializeField]
	private 	float			fMoveSpeed				= 3.0f;
	
	// DIRECTION
	public		enum		DIRECTION					{ NONE, UP, RIGHT, DOWN, LEFT };
	private		DIRECTION	iDirection					= DIRECTION.NONE;
	private 	void		SetDir( DIRECTION ii ) 		{ iDirection = ii; }
	private 	bool		HasDir( DIRECTION ii )		{ return ( (iDirection&ii) == ii ); }
	private		void		AddDir( DIRECTION ii )		{ if ( !HasDir( ii ) ) iDirection &= ii; }
	private 	void		RemDir( DIRECTION ii )		{ if (  HasDir( ii ) ) iDirection |= ii; }
	private		bool		IsDir(  DIRECTION ii )		{ return ( iDirection == ii ); }
	public		DIRECTION 	GetDir() 					{ DIRECTION ii = iDirection; return ii; }

	
	// flag for input parsing
	private		bool		bCanParseInput				= true;
	public	bool CanParseInput
	{
		get { return bCanParseInput;  }
		set { bCanParseInput = value; }
	}



	//					PATHFINDING
	private struct Pathpoints_t {
		public	Vector3	vStart;
		public	Vector3	vCurrentDest;
		public	Vector3	vFinal;
		public void Reset() { vStart = vCurrentDest = vFinal = Vector3.zero; }
	}

	private		Pathpoints_t	PathPoints;

	private		Pathfinding		pPathFinder				= null;		// Pathfinding Script
	private		bool			bHasDestination			= false;	// Flag for coroutines run
	private		bool			bIsMoving				= false;	// Flag for global moving state
						
	private		int				iNodeIdx				= -1;		// Store actual index of path node list
	private		NodeList		pNodeList				= null;		// Is the node list for target position
	private		IEnumerator 	pMoveCoroutine			= null;     // Store the Enumerator reference to Move Coroutine


	// UTILS
	public 		bool 			IsMoving() 					{ return bIsMoving;		}


	// UNITY STUFF
	private		CapsuleCollider	pCollider				= null;


	////////////////////////////////////////////////////////////////////////
	////////////////////////////////////////////////////////////////////////
	////////////////			PLAYER CLASS

	private void Start() {
		
		pCollider	= GetComponent<CapsuleCollider>();

		pPathFinder = GameObject.Find( "PathFinder" ).GetComponent<Pathfinding>();

		pNodeList	= new NodeList();

//		GameManager p = new GameManager();
//		p.SetMaxActionsCallback( new MaxActionsCallback([ void( void ) ]) );

	}
	

	private void Update() {

		// If CAnnot parse user input skip
		if ( !bCanParseInput ) return;

		this.ParseInput();

		// if actually has destination but is not set to move
		if ( bHasDestination && !bIsMoving )
			this.UpdateNavigation();

    }
	

	private void ParseInput() {
		
		// if has not a destination and mouse button is pressed
		if ( pPathFinder && !bHasDestination && Input.GetMouseButtonDown( 0 ) ) {

			// trace a ray from camera to game world
			// if ray hit something
			RaycastHit pMouseHitted;
			if ( Physics.Raycast( Camera.main.ScreenPointToRay( Input.mousePosition ), out pMouseHitted ) ) {

				// Search for a path to destination
				pNodeList.Clear();
				if ( !pPathFinder.FindPath( transform.position, pMouseHitted.point, out pNodeList  ) ) return;
				
				// If path is found start moving
				PathPoints.vStart = pNodeList[0].worldPosition;
				PathPoints.vFinal = pNodeList[ pNodeList.Count - 1 ].worldPosition;
				bHasDestination = true;
				iNodeIdx = -1;
			}
		}
	}
	

	private void UpdateNavigation() {

		// sanity check
		if ( pNodeList == null ) {
			bHasDestination = bIsMoving = false;
			PathPoints.Reset();
			return;
		}
			
		iNodeIdx++;
		if (iNodeIdx < pNodeList.Count ) {
				
			pMoveCoroutine = Action_Move( PathPoints.vCurrentDest = pNodeList[ iNodeIdx ].worldPosition );
			StartCoroutine( pMoveCoroutine );
			return;
		}

		// After path node is reached reset input receiver
		bHasDestination = false;
		PathPoints.Reset();

	}
	
	
	private void Action_Stop() {
		
		if ( bHasDestination ) {
			StopCoroutine( pMoveCoroutine );
			bHasDestination = false;
			PathPoints.Reset();
		}
		
	}


	private IEnumerator Action_Move( Vector3 vDestination ) {

		float fInterpolant		= 0.0f;		// Will store interpolation vale [ 0.0f, 1.0f ]
		float fDiagonalFactor	= 1.0f;		// Used to avoid diagonal speed bug

		// Set as moving
		bIsMoving = true;
		
		// if diagonal movement, scale movement
		if( ( vDestination.x != 0.0f ) && ( vDestination.z != 0.0f ) ) {
			fDiagonalFactor = 0.7071f;
		}

		{//	UPDATE DIRECTION
			if ( ( transform.position.x - vDestination.x ) < 0.001f ) {
				RemDir( DIRECTION.LEFT ); RemDir( DIRECTION.RIGHT );
			}
			else AddDir( ( transform.position.x < vDestination.x ) ? DIRECTION.LEFT : DIRECTION.RIGHT );

			if ( ( transform.position.z - vDestination.z ) < 0.001f ) {
				RemDir( DIRECTION.UP ); RemDir( DIRECTION.DOWN );
			}
			else AddDir( ( transform.position.z < vDestination.z ) ? DIRECTION.DOWN : DIRECTION.UP );
		}

		
		{//	COROUTINE CORE
			// While interpolant is not full
			while ( fInterpolant < 1.0f ) {

				// Increase interpolant
				fInterpolant += Time.deltaTime * fMoveSpeed * fDiagonalFactor;

				// Set next lerped position
				transform.position = Vector3.Lerp( transform.position, vDestination, fInterpolant );

				yield return null;

			}
		}

		// Now that movement is finished, set as not moving and return
		bIsMoving = false;
		yield return 0; // yield break in cycles
    }
	
	

}

#pragma warning restore CS0414 // Var assigned but never used
#pragma warning restore CS0162 // Unreachable code detected