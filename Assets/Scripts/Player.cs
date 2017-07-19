using UnityEngine;
using System.Collections.Generic;

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

	////////////////////////////////////////////////////////////////////////
	//		PLAY MODE

	// Called on play
	void OnPlay();

	// Play move sequence
	void Move();

}


public partial class Player: MonoBehaviour, IPlayer {

	//	DEBUG
				const bool		bDebug 					= true;
	[SerializeField]
	private		float			fUseDistance			= 1.0f;

	//	PATHFINDING
	private		Pathfinding		pPathFinder				= null;		// Pathfinding Script
	////////////////////////////////////////////////////////////////////////////////////
	public		Pathfinding	PathFinder {
		get{ return pPathFinder; }
	}

	struct Navigation {
		public	bool			bHasDestination;
		public	bool			bIsMoving;				// Flag for global moving state
		public	float			fNavInterpolant;
		public	int				iNodeIdx;				// Store actual index of path node list
		public	NodeList		pNodeList;				// Is the node list for target position
	};
	private		Navigation		pNavigation				= new Navigation();	

	//	INTERNAL VARS
	//////////////////////////////////////////////////////////////////////////////////////////////
	[SerializeField]
	private 	float			fMoveSpeed				= 3.0f;
	private		Vector3			vSpawnPostion			= Vector3.zero;
	private		bool			bIsOK					= false;
	// anims
	private		bool			bFlipped				= false;
	private		bool			bDirUP					= false;


	// ACTIONS
	//////////////////////////////////////////////////////////////////////////////////////////////
	private		StageManager	pStageManager			= null;
	private		PlayerAction	pAction					= null;

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
	public 		bool 			IsBusy() 				{ return pNavigation.bHasDestination;		}


	//	UNITY STUFF
	private		CapsuleCollider	pCollider				= null;
	private		SpriteRenderer	pRenderer				= null;
	private		Animator		pAnimator				= null;
	[SerializeField]
	private		Sprite			pPlanSprite				= null;
	private		Sprite			pOriginalSprite			= null;


	////////////////////////////////////////////////////////////////////////
	////////////////////////////////////////////////////////////////////////
	////////////////			PLAYERn CLASS

	private void Start() {
		
		pCollider			= GetComponent<CapsuleCollider>();

		pRenderer			= transform.GetChild( 0 ).GetComponent<SpriteRenderer>();
		pOriginalSprite		= pRenderer.sprite;
		if ( pPlanSprite )
			pRenderer.sprite	= pPlanSprite;

		pAnimator			= transform.GetChild( 0 ).GetComponent<Animator>();
		pAnimator.enabled	= false;

		pPathFinder			= GameObject.Find( "PathFinder" ).GetComponent<Pathfinding>();
		pStageManager		= GameObject.Find( "GameManager" ).GetComponent<StageManager>();

		if ( !pPathFinder ) {
			Debug.Log( "Pathfinder not found" );
			return;
		}

		if ( !pStageManager ) {
			Debug.Log( "StageManager not found" );
			return;
		}

		pNavigation.pNodeList	= new NodeList();

		vSpawnPostion.Set( transform.position.x, transform.position.y, transform.position.z );

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
		if ( pNavigation.bHasDestination )
			this.UpdateNavigation();

    }

}

#pragma warning restore CS0414 // Var assigned but never used
#pragma warning restore CS0162 // Unreachable code detected