using UnityEngine;

using NodeList = System.Collections.Generic.List<Node>;


#pragma warning disable CS0162 // Unreachable code detected
#pragma warning disable CS0414 // Var assigned but never used


public partial class Player {

	const	bool		bPlanDebug				= false;

	private	Vector3		vPlanStageDestination	= Vector3.zero;

	// ACTION WAIT
	//////////////////////////////////////////////////////////////////////////////
	private	void	Action_Wait() {

		GLOBALS.StageManager.SetAction( new PlayerAction( vPlanPosition ), this.iID );

		if ( bPlanDebug ) Debug.Log( "Wait action set" );

		this.Steps_Set( Vector3.up * 10000 );	// hide step tile from user view

	}

	// ACTION USE INSTANTLY AN USABLE OBJECT
	//////////////////////////////////////////////////////////////////////////////
	private	void	Action_UsableObject_Use_Instant( UsableObject pUsableObject ) {

		if ( !pMouseHitthedObject.collider ) return;

		Vector3 vDestination =   pMouseHitthedObject.collider.transform.position;

		// Door has a proper point as destination
		if ( pUsableObject.tag == "Door" )
			vDestination = pMouseHitthedObject.collider.transform.FindChild( "Origin" ).position;


		if ( Vector3.Distance( vPlanPosition, vDestination ) < fUseDistance*1.2f ) {
			
			this.Steps_Set( vPlanStageDestination = vDestination );
			
			GLOBALS.StageManager.SetAction( new PlayerAction( vPlanPosition, pUsableObject ), this.iID );

			if ( bPlanDebug ) Debug.Log( "Usable object set" );
		};

	}


	private	void	Action_UsableObject_Use_OnActionEnd( UsableObject pUsableObject ) {

		if ( !pMouseHitthedObject.collider ) return;

		float fObjectDistance = Vector3.Distance( vPlanPosition, pMouseHitthedObject.collider.transform.position );

		//////////////////////////////////////////////////////////////////////////////
		//		OBJECTS ON RANGE AND WITH USE AT STAGE END		
		if ( pUsableObject.tag == "Switcher" ) {

			// if a switcher( not plane ) is set ON_ACTION_END, pg will move to reach it
			if ( fObjectDistance > ( fUseDistance * 1.25f ) ) {

				Pathfinding pPathFinder = GLOBALS.PathFinder;

				// Calculate path
				pMouseHitthedObject.collider.gameObject.layer = LayerMask.NameToLayer( "Default" );
				pPathFinder.UpdateGrid();			// remove this obstacle
				NodeList vNodes = new NodeList();
				Vector3 vCoord  = pPathFinder.NodeFromWorldPoint( pMouseHitthedObject.collider.gameObject.transform.position ).worldPosition;
				bool bPathFound = pPathFinder.FindPath( vPlanPosition, vCoord, out vNodes );
				pMouseHitthedObject.collider.gameObject.layer = LayerMask.NameToLayer( "Unwalkable" );
				pPathFinder.UpdateGrid();			// reset this obstacle

				// default is set to object position
				Vector3 vDestinationNode = pMouseHitthedObject.collider.gameObject.transform.position;

				if ( bPathFound ) {

					if ( vNodes.Count > 1  ) {
						// If path node list contains more than one node, remove last one
						vNodes.RemoveRange( vNodes.Count - 1, 1 );
						vDestinationNode = vNodes[ vNodes.Count - 1 ].worldPosition;
					}
					else
						vDestinationNode = vNodes[ 0 ].worldPosition;

				}

				GLOBALS.StageManager.SetAction( new PlayerAction( vPlanPosition, vDestinationNode, pUsableObject ), this.iID );

			}
			// this is a plane switcher
			else {
				GLOBALS.StageManager.SetAction( new PlayerAction( vPlanPosition, pUsableObject ), this.iID );

			}

			this.Steps_Set( vPlanStageDestination = pMouseHitthedObject.collider.gameObject.transform.position );

			if ( bPlanDebug ) Debug.Log( "Movement to object set" );

			return;
		}

		//////////////////////////////////////////////////////////////////////////////
		//		OBJECTS WITH USE AT DESTINATION REACHED
		{
			Vector3 vDestination = pMouseHitthedObject.collider.gameObject.transform.position;

			// Door has a proper point as destination
			if ( pUsableObject.tag == "Door" )
				vDestination = pMouseHitthedObject.transform.GetChild( 2 ).position;
								
			this.Steps_Set( vPlanStageDestination = vDestination );

			GLOBALS.StageManager.SetAction( new PlayerAction( vPlanPosition, vDestination, pUsableObject ), this.iID );

			if ( bPlanDebug ) Debug.Log( "Movement to object set" );
		}

	}

	private	void	Action_MoveOnly() {

		if ( !pMouseHitthedObject.collider ) return;

		Vector3 vDestination = pMouseHitthedObject.collider.gameObject.transform.position;
					
		this.Steps_Set( vPlanStageDestination = vDestination );

		GLOBALS.StageManager.SetAction( new PlayerAction( vPlanPosition, vDestination ), this.iID );

		if ( bPlanDebug ) Debug.Log( "Movement only set" );
	}




}


#pragma warning restore CS0414 // Var assigned but never used
#pragma warning restore CS0162 // Unreachable code detected