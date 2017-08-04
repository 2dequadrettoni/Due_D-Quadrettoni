﻿
using System.Collections.Generic;
using UnityEngine;

using System; // [Serializable]

using UnityEngine.SceneManagement;
using UnityEngine.UI;

#pragma warning disable CS0162 // Unreachable code detected
#pragma warning disable CS0414 // Var assigned but never used

/*
public class LevelManager : MonoBehaviour {

	[Serializable]
	public class Level {
		public bool IsAvaiable = false;
		public Scene Scena;
	}


	Transform[] figli;

	public Level[] vLevels;

	private void Start() {

		figli = new Transform[vLevels.Length];

		for( int i = 0; i < vLevels.Length; i++ ) {

			figli[i] = transform.FindChild( "Button" + i );

		}

		for ( int i = 0; i < SaveLoad.GetSavedlevel(); i++ ) {

			vLevels[ i ].IsAvaiable = true;

		}

		if ( vLevels != null && vLevels.Length > 0 ) {
			for( int i = 0; i < vLevels.Length; i++ ) {

				if ( vLevels[ i ].IsAvaiable ) {
					figli[i].GetComponent<Button>().interactable = true;
				}
				else {
					figli[i].GetComponent<Button>().interactable = false;
					//	qui settare immagine del lucchetto
				}

			}

			vLevels[0].IsAvaiable = true;
		}


	}

}



	*/


























public  class	Step {

	public	SpriteRenderer	pFull_Tile_Renderer	= null;
	public	SpriteRenderer	pHalf_Tile_Renderer	= null;
	public	SpriteRenderer	pNumber_Renderer	= null;

	public	bool			IsShared			= false;
	public	bool			IsVisible			= true;
	public	Transform		pStepTransform		= null;
	public	Vector3			vPosition			= Vector3.zero;
	public	Node			pNode				= null;

	public	Step			pSharer				= null;
	

	public	Step( Transform StepTransform, Vector3 Position ) {

		this.pStepTransform		= StepTransform;
		this.vPosition			= this.pStepTransform.position = Position;

		pFull_Tile_Renderer		= StepTransform.FindChild( "Full_Tile" ).GetComponent<SpriteRenderer>();
		pHalf_Tile_Renderer		= StepTransform.FindChild( "Half_Tile" ).GetComponent<SpriteRenderer>();
		pNumber_Renderer		= StepTransform.FindChild( "Number"    ).GetComponent<SpriteRenderer>();
	}

	public	void	UpdateNode() {

		pNode = GLOBALS.PathFinder.NodeFromWorldPoint( vPosition );

	}

}



public partial class Player {



	// Check if currently other player has this step
	public	bool	Steps_HasThis( Node pStepNode, ref Step pReturnStep ) {

		for( int i = vSteps.Length - 1; i > -1; i-- ) {
			Step pStep = vSteps[ i ];

			if ( pStep == null ) continue;

			if (  pStep.pNode.IsEqual( pStepNode ) ) {

				pReturnStep = pStep;
				return true;
			}
		}

		pReturnStep = null;
		return false;

	}


	private	void	Spets_HidePreviousNumbers( Node pNode ) {

		for( int i = vSteps.Length - 1; i > -1; i-- ) {
			Step pStep = vSteps[ i ];

			if ( pStep == null ) continue;

			if ( pStep.pNode.IsEqual( pNode ) ) {
				
				pStep.IsVisible = pStep.pNumber_Renderer.enabled = false;

			}
		}

	}


	private	void	Steps_ResetLastNumberOn( Node pNode ) {

		for( int i = vSteps.Length; i > 0; i-- ) {
			Step pStep = vSteps[ i-1 ];

			if ( pStep == null ) continue;

			if ( pStep.pNode.IsEqual( pNode ) && ( pStep.IsVisible == false ) ) {
				pStep.IsVisible = pStep.pNumber_Renderer.enabled = true;
				return;
			}
		}

	}


	private	void	Steps_Update( Step pCurrentStep ) {

		Player		pOtherPlayer		= ( ID == 1 ) ? GLOBALS.Player2 : GLOBALS.Player1;

		Step		pOtherPlayerStep	= null;

		bool		IsShared			= pOtherPlayer.Steps_HasThis( pCurrentStep.pNode, ref pOtherPlayerStep );

		Spets_HidePreviousNumbers( pCurrentStep.pNode );

		if ( IsShared ) {

			pCurrentStep.pFull_Tile_Renderer.enabled = pOtherPlayerStep.pFull_Tile_Renderer.enabled = false;
			pCurrentStep.pHalf_Tile_Renderer.enabled = pOtherPlayerStep.pHalf_Tile_Renderer.enabled = true;

			pOtherPlayerStep.IsShared = pCurrentStep.IsShared = true;

			pCurrentStep.pSharer		= pOtherPlayerStep;
			pOtherPlayerStep.pSharer	= pCurrentStep;

		}
		else {
			if ( pCurrentStep.pSharer != null ) {

				pCurrentStep.pSharer.pFull_Tile_Renderer.enabled = true;
				pCurrentStep.pSharer.pHalf_Tile_Renderer.enabled = false;

				Steps_ResetLastNumberOn( pCurrentStep.pSharer.pNode );

				pCurrentStep.IsShared = pCurrentStep.IsShared = false;
				pCurrentStep.pSharer.pSharer = null;
				
			}

			pCurrentStep.pFull_Tile_Renderer.enabled = true;
			pCurrentStep.pHalf_Tile_Renderer.enabled = false;
		}

		pCurrentStep.pNumber_Renderer.enabled = true;
		pCurrentStep.pNumber_Renderer.sprite = GLOBALS.StageManager.GetNumberSprite();
		

	}

	

	private	void	Steps_Set( Vector3 vPosition ) {

		int			iCurrentStage		= GLOBALS.StageManager.CurrentStage;
		Step		pCurrentStep		= vSteps[ iCurrentStage ];

		//////////////////////////////////////////////////////////////////////////////
		// CREATE ONE IF NOT EXISTS
		if ( pCurrentStep == null ) {
			Transform pStepTransform = Instantiate( pMainStepTile, this.transform ) as Transform;
			pCurrentStep = new Step( pStepTransform, vPosition );
		}

		pCurrentStep.pStepTransform.position = pCurrentStep.vPosition = vPosition;
		pCurrentStep.UpdateNode();

		vSteps[ iCurrentStage ] = pCurrentStep;

		//////////////////////////////////////////////////////////////////////////////
		// CHOOSE WHICH SPRITE RENDER AND RIGHT NUMBER POSITION AND VALUE
		Steps_Update( pCurrentStep );
		
		vSteps[ iCurrentStage ] = pCurrentStep;
		
	}


}

#pragma warning restore CS0414 // Var assigned but never used
#pragma warning restore CS0162 // Unreachable code detected