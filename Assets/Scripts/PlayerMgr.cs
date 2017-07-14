// Ref: http://wiki.unity3d.com/index.php/GridMove

using UnityEngine;

using NodeList = System.Collections.Generic.List<Node>;

#pragma warning disable CS0162 // Unreachable code detected
#pragma warning disable CS0414 // Var assigned but never used


public class PlayerMgr : MonoBehaviour {

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
	private		bool			bHasDestination			= false;	// 
	private		bool			bIsMoving				= false;	// Flag for global moving state
	private		float			fNavInterpolant			= 0.0f;
						
	private		int				iNodeIdx				= -1;		// Store actual index of path node list
	private		NodeList		pNodeList				= null;		// Is the node list for target position

	private		NodeList		pPreview				= null;


	// UTILS
	public 		bool 			IsMoving() 					{ return bIsMoving;		}


	// UNITY STUFF
	private		CapsuleCollider	pCollider				= null;
	private		int				iInstance				= -1;


	////////////////////////////////////////////////////////////////////////
	////////////////////////////////////////////////////////////////////////
	////////////////			PLAYERn CLASS

	private void Start() {
		
		pCollider	= GetComponent<CapsuleCollider>();

		pPathFinder = GameObject.Find( "PathFinder" ).GetComponent<Pathfinding>();

		pNodeList	= new NodeList();

		iInstance	= this.GetInstanceID();

//		GameManager p = new GameManager();
//		p.SetMaxActionsCallback( new MaxActionsCallback([ void( void ) ]) );

	}
	

	private void Update() {

		// If CAnnot parse user input skip
		if ( !bCanParseInput ) return;

		this.ParseInput();

		// if actually has destination but is not set to move
		if ( bHasDestination )
			this.UpdateNavigation();

    }


	////////////////////////////////////////////////////////////////////////
	////////////////////////////////////////////////////////////////////////
	////////////////////////////////////////////////////////////////////////
	////////////////////////////////////////////////////////////////////////
	/////////////////////////		PLAN MODE

	private void ParseInput() {
		
		// if has not a destination and mouse button is pressed
		if ( pPathFinder && !bHasDestination && Input.GetMouseButtonDown( 0 ) ) {

			// trace a ray from camera to game world
			// if ray hit something
			RaycastHit pMouseHitted;
			if ( Physics.Raycast( Camera.main.ScreenPointToRay( Input.mousePosition ), out pMouseHitted ) ) {

				// Search for a path to destination
				pNodeList.Clear();
				pPathFinder.UpdateGrid();
				if ( !pPathFinder.FindPath( transform.position, pMouseHitted.point, out pNodeList  ) ) return;
				
				// If path is found start moving
				PathPoints.vStart = pNodeList[0].worldPosition;
				PathPoints.vFinal = pNodeList[ pNodeList.Count - 1 ].worldPosition;
				bHasDestination = true;
				iNodeIdx = 0;

			}
		}

	}




	////////////////////////////////////////////////////////////////////////
	////////////////////////////////////////////////////////////////////////
	////////////////////////////////////////////////////////////////////////
	////////////////////////////////////////////////////////////////////////
	/////////////////////////		PLAY MODE



	private void UpdateNavigation() {

		// sanity check
		if ( pNodeList == null ) {
			bHasDestination = bIsMoving = false;
			PathPoints.Reset();
			return;
		}
		
		if ( fNavInterpolant > 0.99999f ) {
			iNodeIdx++;
			fNavInterpolant = 0.0f;
		}

		// If run out of nodes
		if ( iNodeIdx >= pNodeList.Count ) {
			bHasDestination = false;
			PathPoints.Reset();
			fNavInterpolant = 0.0f;
			bIsMoving = false;
			return;
		}


		// DESTINATION
		Vector3 vDestination = PathPoints.vCurrentDest = pNodeList[ iNodeIdx ].worldPosition;

		// Increase interpolant
		fNavInterpolant += Time.deltaTime * fMoveSpeed;

		// Set next lerped position
		transform.position = Vector3.Lerp( transform.position, vDestination, fNavInterpolant );

		bIsMoving = true;

	}

}

#pragma warning restore CS0414 // Var assigned but never used
#pragma warning restore CS0162 // Unreachable code detected