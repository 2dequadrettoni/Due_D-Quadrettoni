using UnityEngine;

using NodeList = System.Collections.Generic.List<Node>;

#pragma warning disable CS0162 // Unreachable code detected
#pragma warning disable CS0414 // Var assigned but never used

interface IPlayer {
	
	/// Property	Action : PlayerAction
	/// Property	PrevPostion : Vector3
	/// Property	CanParseInput : bool
	/// Property	ID : int

	////////////////////////////////////////////////////////////////////////
	//		PLAY MODE

	// Check if player is busy( now only moving )
	bool IsBusy();

	// Clear the current Path
	void ClearPath();

	////////////////////////////////////////////////////////////////////////
	//		PLAY MODE

	// Called on play
	void OnPlay();

	// Set a new path
	void SetPath( NodeList _NodeList );

	// Play move sequence
	void Move();

}


public partial class Player: MonoBehaviour, IPlayer {

	//	DEBUG
				const bool		bDebug 					= true;
				const float		fUseDistance			= 5.0f;

	//	DIRECTION
	public		enum			DIRECTION				{ NONE, UP, RIGHT, DOWN, LEFT };
	private		DIRECTION		iDirection				= DIRECTION.NONE;
	private 	void			SetDir( DIRECTION ii ) 	{ iDirection = ii; }
	private 	bool			HasDir( DIRECTION ii )	{ return ( (iDirection&ii) == ii ); }
	private		void			AddDir( DIRECTION ii )	{ if ( !HasDir( ii ) ) iDirection &= ii; }
	private 	void			RemDir( DIRECTION ii )	{ if (  HasDir( ii ) ) iDirection |= ii; }
	private		bool			IsDir(  DIRECTION ii )	{ return ( iDirection == ii ); }
	public		DIRECTION 		GetDir() 				{ DIRECTION ii = iDirection; return ii; }


	//	PATHFINDING
	private		Pathfinding		pPathFinder				= null;		// Pathfinding Script
	////////////////////////////////////////////////////////////////////////////////////
	public		Pathfinding	PathFinder {
		get{ return pPathFinder; }
	}



	private		bool			bHasDestination			= false;	// 
	private		bool			bIsMoving				= false;	// Flag for global moving state
	private		float			fNavInterpolant			= 0.0f;
	private		int				iNodeIdx				= -1;		// Store actual index of path node list
	private		NodeList		pNodeList				= null;		// Is the node list for target position

	private		GameObject		pPathPreviewContainer	= null;

	//	INTERNAL VARS
	[SerializeField]
	private 	float			fMoveSpeed				= 3.0f;
	private		Vector3			vSpawnPostion			= Vector3.zero;
	private		bool			bIsOK					= false;
	// anims
	private		bool			bFlipped				= false;
	private		bool			bDirUP					= false;


	// ACTIONS
	//////////////////////////////////////////////////////////////////////////////////////////////
	private		PlayerAction	pAction					= null;
	public		PlayerAction Action
	{
		get { return pAction;  }
		set { pAction = value; }
	}

	private		StageManager	pStageManager			= null;
	private		UsableObject	pUsableObject			= null;
	public		UsableObject UsableObject
	{
		get { return pUsableObject;  }
		set { pUsableObject = value; }
	}

	//////////////////////////////////////////////////////////////////////////////////////////////
	private		Vector3			vPrevPostion			= Vector3.zero;
	public		Vector3 PrevPostion
	{
		get { return vPrevPostion;  }
		set { vPrevPostion = value; }
	}
	//////////////////////////////////////////////////////////////////////////////////////////////
	private		bool			bCanParseInput			= false;
	public		bool CanParseInput
	{
		get { return bCanParseInput;  }
		set { bCanParseInput = value; }
	}

	//////////////////////////////////////////////////////////////////////////////////////////////
	private		int				iID						= 0;
	public		int	ID
	{
		get { return iID; }
		set { iID = value; }
	}


	//	UTILS
	public 		bool 			IsBusy() 				{ return bHasDestination;		}


	//	UNITY STUFF
	private		CapsuleCollider	pCollider				= null;
	private		SpriteRenderer	pRenderer				= null;
	private		Animator		pAnimator				= null;


	////////////////////////////////////////////////////////////////////////
	////////////////////////////////////////////////////////////////////////
	////////////////			PLAYERn CLASS

	private void Start() {
		
		pCollider			= GetComponent<CapsuleCollider>();

		pRenderer			= transform.GetChild( 0 ).GetComponent<SpriteRenderer>();

		pAnimator			= transform.GetChild( 0 ) .GetComponent<Animator>();

		pPathFinder			= GameObject.Find( "PathFinder" ).GetComponent<Pathfinding>();

		pStageManager		= GameObject.Find( "GameManager" ).GetComponent<StageManager>();

		if ( !pPathFinder ) {
			Debug.Log( "Pathfinder not found" );
			return;
		}

		pNodeList			= new NodeList();

		vPrevPostion = vSpawnPostion = transform.position;

//		GameManager p = new GameManager();
//		p.SetMaxActionsCallback( new MaxActionsCallback([ void( void ) ]) );

//		pAnimator.Play( "Idle_Down" );

		bIsOK = true;

	}
	

	private void Update() {

		if ( !bIsOK ) return;

		////////////////////////////////////////////////////////////////////////
		//		PLAN MODE
		//
		if ( bCanParseInput )
			this.ParseInput();

		////////////////////////////////////////////////////////////////////////
		//		PLAY MODE
		//
		if ( bHasDestination )
			this.UpdateNavigation();


		if ( HasDir( DIRECTION.UP ) )	bDirUP = true;	else bDirUP = false;
		if ( HasDir( DIRECTION.LEFT ) ) bFlipped = true; 	else bFlipped = false;

    }

}

#pragma warning restore CS0414 // Var assigned but never used
#pragma warning restore CS0162 // Unreachable code detected