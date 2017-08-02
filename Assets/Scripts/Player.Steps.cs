
using UnityEngine;

#pragma warning disable CS0162 // Unreachable code detected
#pragma warning disable CS0414 // Var assigned but never used

public partial class Player {


	// try restore previous step cicle per vedere se uno step coincide e rende visibile il numero

	// used when returning to previous action
	private	void	TryRestorePrevSteps( Node vThisStepNode ) {

		if ( vSteps.Length == 0 ) return;

		Player		pOtherPlayer		= ( ID == 1 ) ? GLOBALS.Player2 : GLOBALS.Player1;

		Transform	pOtherPlayerStep	= null;
		bool		IsShared			= pOtherPlayer.HasThisStep( vThisStepNode, ref pOtherPlayerStep );

		for( int i = vSteps.Length; i > -1; i-- ) {

			Transform p = vSteps[ i ];

			Node pNode;
			if ( p &&  ( ( pNode = GLOBALS.PathFinder.NodeFromWorldPoint( p.position ) ) != null ) ) {
				if ( vThisStepNode == pNode && IsShared ) {

			//		pPreviousStep.FindChild( "Full_Tile" ).GetComponent<SpriteRenderer>().enabled = false;
			//		pPreviousStep.FindChild( "Half_Tile" ).GetComponent<SpriteRenderer>().enabled = false;
			//		pPreviousStep.FindChild( "Number" ).GetComponent<SpriteRenderer>().enabled = false;

				}
			}

		}

	}

	// Check if currently other player has this step
	private	bool	HasThisStep( Node vStepNode, ref Transform pOtherStep ) {
		

		for( int i = vSteps.Length - 1; i > -1; i-- ) {
			Transform p = vSteps[ i ];

			if ( !p ) continue;

			Node pNode = GLOBALS.PathFinder.NodeFromWorldPoint( p.position );
			if ( pNode == vStepNode && p.gameObject.activeSelf ) {
				pOtherStep = p;
				
				return true;
			}
		}

		pOtherStep = null;
		return false;

	}


	private	void	CheckSharedSteps() {
		/*
		Node		pNode				= GLOBALS.PathFinder.NodeFromWorldPoint( p.position );

		Player		pOtherPlayer		= ( ID == 1 ) ? GLOBALS.Player2 : GLOBALS.Player1;

		Transform	pOtherPlayerStep	= null;
		bool		IsShared			= pOtherPlayer.HasThisStep( pNode, ref pOtherPlayerStep );


	*/






		foreach( Transform p in vSteps ) {

			if ( !p ) continue;

			Node		pNode				= GLOBALS.PathFinder.NodeFromWorldPoint( p.position );
			Player		pOtherPlayer		= ( ID == 1 ) ? GLOBALS.Player2 : GLOBALS.Player1;

			Transform	pOtherPlayerStep	= null;
			bool		IsShared			= pOtherPlayer.HasThisStep( pNode, ref pOtherPlayerStep );

			pOtherPlayerStep.FindChild( "Number" ).localPosition = ( IsShared ) ? pOtherPlayer.vNumberDefPosition : Vector3.zero;

		}

	}


	private	void	SetStepTile( Vector3 vPosition ) {

		Node		pNode				= GLOBALS.PathFinder.NodeFromWorldPoint( vPosition );

		Player		pOtherPlayer		= ( ID == 1 ) ? GLOBALS.Player2 : GLOBALS.Player1;

		Transform	pOtherPlayerStep	= null;
		bool		IsShared			= pOtherPlayer.HasThisStep( pNode, ref pOtherPlayerStep );

		Transform	pPreviousStep		= null;
		bool		IsAlreadyUsed		= this.HasThisStep( pNode, ref pPreviousStep );

		Transform	pCurrentStep		= vSteps[ GLOBALS.StageManager.CurrentStage ];
		

		//////////////////////////////////////////////////////////////////////////////
		//  HIDE ALL PREVIOUS STEPS ON THIS POSITION
		while( IsAlreadyUsed ) {

			pPreviousStep.FindChild( "Full_Tile" ).GetComponent<SpriteRenderer>().enabled = false;
			pPreviousStep.FindChild( "Half_Tile" ).GetComponent<SpriteRenderer>().enabled = false;
			pPreviousStep.FindChild( "Number"    ).GetComponent<SpriteRenderer>().enabled = false;

			IsAlreadyUsed = this.HasThisStep( pNode, ref pPreviousStep );

		}


		//////////////////////////////////////////////////////////////////////////////
		// CREATE ONE IF NOT EXISTS
		if ( pCurrentStep == null )
			pCurrentStep = Instantiate( pMainStepTile, this.transform ) as Transform;


		//////////////////////////////////////////////////////////////////////////////
		// CHOOSE WHICH SPRITE RENDER

		pCurrentStep.FindChild( "Full_Tile" ).GetComponent<SpriteRenderer>().enabled = ( IsShared ) ? false : true;
		pCurrentStep.FindChild( "Half_Tile" ).GetComponent<SpriteRenderer>().enabled = ( IsShared ) ? true : false;


		//////////////////////////////////////////////////////////////////////////////
		// SET RIGHT NUMBER POSITION AND VALUE
		Transform pNumber = pCurrentStep.FindChild( "Number" );
		pNumber.localPosition = ( IsShared ) ? vNumberDefPosition : Vector3.zero;

		SpriteRenderer	pSpriteRender = pNumber.GetComponent<SpriteRenderer>();
		pSpriteRender.enabled = true;
		pSpriteRender.sprite = GLOBALS.StageManager.GetNumberSprite();

		pCurrentStep.position = vPosition;

		vSteps[ GLOBALS.StageManager.CurrentStage ] = pCurrentStep;
		
	}


}

#pragma warning restore CS0414 // Var assigned but never used
#pragma warning restore CS0162 // Unreachable code detected