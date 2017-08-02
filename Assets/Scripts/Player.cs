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
	////////////////////////////////////////////////////////////////////////////////////

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
	private		string			sDirection				= "Down";
	private		bool			bFlipped				= false;


	//	INTERNAL VARS
	//////////////////////////////////////////////////////////////////////////////////////////////
	[SerializeField]
	private 	float			fMoveSpeed				= 3.0f;
	[SerializeField]
	private		float			fUseDistance			= 0.3f;
	private		Vector3			vSpawnPostion			= Vector3.zero;
	private		Vector3			vPlanPosition			= Vector3.zero;
	private		bool			bIsOK					= false;

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

	private		Sprite			pCursor					= null;


	// ACTIONS
	//////////////////////////////////////////////////////////////////////////////////////////////
	private		PlayerAction	pAction					= null;

	//////////////////////////////////////////////////////////////////////////////////////////////
	private		bool			bCanParseInput			= false;
	public		bool CanParseInput
	{
		get { return bCanParseInput;  }
		set { bCanParseInput = value;


		}
	}

	//////////////////////////////////////////////////////////////////////////////////////////////
	private		int				iID						= 0;
	public		int	ID
	{
		get { return iID; }
		set { iID = value; }
	}

	[HideInInspector]
	public		bool			IsInAnimationOverride	= false;


	//	UTILS
	public 		bool 			IsBusy() 				{ return pNavigation.bHasDestination;		}


	//	UNITY STUFF
	private		SpriteRenderer	pRenderer				= null;
	private		SpriteRenderer	pCursorRenderer			= null;
	private		Animator		pAnimator				= null;
	private		Transform		pPlanTile				= null;
	private		Transform		pMainStepTile			= null;

	private		Transform[]		vSteps					= null;

	private		Vector3			vNumberDefPosition		= Vector3.zero;


	////////////////////////////////////////////////////////////////////////
	////////////////////////////////////////////////////////////////////////
	////////////////			PLAYERn CLASS

	private void Start() {

		pRenderer			= transform.FindChild( "Sprite" ).GetComponent<SpriteRenderer>();
		pAnimator			= transform.FindChild( "Sprite" ).GetComponent<Animator>();
		pCursorRenderer		= transform.FindChild( "Cursor" ).GetComponent<SpriteRenderer>();
		
		pPlanTile			= transform.FindChild( "PlanTile" );
		pMainStepTile		= transform.FindChild( "StepTile" );

		if ( !GLOBALS.PathFinder ) {
			Debug.Log( "Pathfinder not found" );
			return;
		}

		if ( !GLOBALS.StageManager ) {
			Debug.Log( "StageManager not found" );
			return;
		}

		pNavigation.pNodeList	= new NodeList();

		vSpawnPostion.Set( transform.position.x, transform.position.y, transform.position.z );
		vPlanPosition.Set( transform.position.x, transform.position.y, transform.position.z );

		vSteps = new Transform[ StageManager.MAX_STAGES ];

		vNumberDefPosition = pMainStepTile.FindChild( "Number" ).localPosition;

		bIsOK = true;

	}

	public void SetCursor( bool val ) {

		pCursorRenderer.enabled = val;

	}
	

	private void Update() {

		if ( !bIsOK ) return;

		// hide icon
		pPlanTile.gameObject.SetActive( false );

		////////////////////////////////////////////////////////////////////////
		//		PLAN MODE
		//
		if ( bCanParseInput ) {
			this.ParseInput();

			if ( GLOBALS.StageManager.CurrentStage == StageManager.MAX_STAGES ) {
				bCanParseInput = false;
				SetCursor( false );
			}

		}

		////////////////////////////////////////////////////////////////////////
		//		PLAY MODE
		//
		if ( pNavigation.bHasDestination )
			this.UpdateNavigation();

    }

	public void PlayWinAnimation() {

		pAnimator.Play( "Win", 0, 0.0f );

	}

	public void Destroy() {
		Stop();
		pAnimator.Play( "Destroy", 0, 0.0f );

	}

}


#pragma warning restore CS0414 // Var assigned but never used
#pragma warning restore CS0162 // Unreachable code detected