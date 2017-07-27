using UnityEngine;
using System.Collections.Generic;

using UnityEngine.EventSystems;

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
				const bool		bDebug 					= false;


	//	PATHFINDING
	private		Pathfinding		pPathFinder				= null;		// Pathfinding Script
	////////////////////////////////////////////////////////////////////////////////////
	public		Pathfinding	PathFinder {
		get{ return pPathFinder; }
	}

	public struct Navigation_t {
		public	bool			bHasDestination;
		public	bool			bIsMoving;				// Flag for global moving state
		public	int				iNodeIdx;				// Store actual index of path node list
		public	NodeList		pNodeList;				// Is the node list for target position
		public	Vector3			vDestination;
	};
	private		Navigation_t	pNavigation				= new Navigation_t();
	public		Navigation_t Navigation {
		get { return pNavigation; }
	}


	//	INTERNAL VARS
	//////////////////////////////////////////////////////////////////////////////////////////////
	[SerializeField]
	private 	float			fMoveSpeed				= 3.0f;
	[SerializeField]
	private		float			fUseDistance			= 0.3f;
	private		Vector3			vSpawnPostion			= Vector3.zero;
	private		Vector3			vPlanPosition			= Vector3.zero;
	private		bool			bIsOK					= false;
	// anims
	private		bool			bFlipped				= false;
	private		bool			bDirUP					= false;
	//////////////////////////////////////////////////////////////////////////////////////////////
	private		bool			bLinked					= false;
	public		bool	Linked {
		get { return bLinked; }
		set { bLinked = value; }
	}
	private		Platform		pLinkedObject			= null;
	public		Platform LinkedObject {
		get{ return pLinkedObject; }
	}


	// INVENTORY
	//////////////////////////////////////////////////////////////////////////////////////////////
	private		byte			iActualKey				= 0;
	public		byte ActuaKey {
		get { return iActualKey; }
		set { iActualKey = value; }
	}


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
	private		SpriteRenderer	pRenderer				= null;
	private		Animator		pAnimator				= null;
	[SerializeField]
	private		Sprite			pPlanSprite				= null;
	private		Sprite			pOriginalSprite			= null;
	[SerializeField]
	private		Transform		pCurrentDestSprite		= null;


	////////////////////////////////////////////////////////////////////////
	////////////////////////////////////////////////////////////////////////
	////////////////			PLAYERn CLASS

	private void Start() {

		pRenderer			= transform.GetChild( 0 ).GetComponent<SpriteRenderer>();
		pOriginalSprite		= pRenderer.sprite;
		if ( pPlanSprite )
			pRenderer.sprite	= pPlanSprite;

		pAnimator			= transform.GetChild( 0 ).GetComponent<Animator>();

		pPathFinder			= GLOBALS.PathFinder;

		pStageManager		= GLOBALS.StageManager;

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
		vPlanPosition.Set( transform.position.x, transform.position.y, transform.position.z );

//		pAnimator.Play( "Idle_Down" );

		bIsOK = true;

	}
	

	private void Update() {

		if ( !bIsOK ) return;

		// hide icon
		pCurrentDestSprite.localRotation = Quaternion.identity;

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

	public void Destroy() {

		pAnimator.Play( "Destroy", 0, 0.0f );

	}

}


#pragma warning restore CS0414 // Var assigned but never used
#pragma warning restore CS0162 // Unreachable code detected