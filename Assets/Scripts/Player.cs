﻿using UnityEngine;
using System.Collections.Generic;

using UnityEngine.EventSystems;

using NodeList = System.Collections.Generic.List<Node>;

#pragma warning disable CS0162 // Unreachable code detected
#pragma warning disable CS0414 // Var assigned but never used


public partial class Player: MonoBehaviour {

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
	private		Key			iActualKey					= null;
	public		Key ActuaKey {
		get { return iActualKey; }
		set { iActualKey = value; }
	}

	private		Sprite			pCursor					= null;


	// ACTIONS
	//////////////////////////////////////////////////////////////////////////////////////////////
//	private		PlayerAction	pAction					= null;

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

	private		Step[]			vSteps					= null;

	private		Vector3			vNumberDefPosition		= Vector3.zero;


	////////////////////////////////////////////////////////////////////////
	////////////////////////////////////////////////////////////////////////
	////////////////			PLAYERn CLASS

	private void Start() {

		pRenderer			= transform.Find( "Sprite" ).GetComponent<SpriteRenderer>();
		pAnimator			= transform.Find( "Sprite" ).GetComponent<Animator>();
		pCursorRenderer		= transform.Find( "Cursor" ).GetComponent<SpriteRenderer>();
		
		pPlanTile			= transform.Find( "PlanTile" );
		pMainStepTile		= transform.Find( "StepTile" );

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

		vSteps = new Step[ StageManager.MAX_STAGES ];

		vNumberDefPosition = pMainStepTile.Find( "Number" ).localPosition;

		bIsOK = true;

	}


	public	void	OnSpawnEnd() {
		// Set movement tutorial
		if ( GameManager.InTutorialSequence && GameManager.TutorialStep == 0 ) {
			GLOBALS.GameManager.NextTutorial();
		}
	}


	public	void	SetCursor( bool value ) {

		pCursorRenderer.enabled = value;

		pAnimator.Play( ( value ) ? "Selected" : "Idle_Up" );

	}
	

	private	void	Update() {

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
		GLOBALS.AudioManager.Play( "PG_Death" );

	}

}


#pragma warning restore CS0414 // Var assigned but never used
#pragma warning restore CS0162 // Unreachable code detected