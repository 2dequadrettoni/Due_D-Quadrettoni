using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Door {

	//	 HIGHLIGHTING
	[Header("HighLighting")]
	[SerializeField]
	private		Sprite				pSpriteClosed				= null;
	[SerializeField]				// Default 
	private		Sprite				pSpriteClosedHighlighted	= null;
	[SerializeField]
	private		Sprite				pSpriteOpen					= null;
	[SerializeField]
	private		Sprite				pSpriteOpenHighlighted		= null;
	private		bool				bIsHighLighted				= false;
	public		bool IsHighLighted {
		get { return bIsHighLighted; }
		set { bIsHighLighted = value; }
	}

	private void UpdateHighLighting() {
		
		if ( bIsHighLighted ) {

			pAnimator.enabled = false;

			if ( Closed )
				pSpriteRender.sprite = pSpriteClosedHighlighted;
			else
				pSpriteRender.sprite = pSpriteOpenHighlighted;
		}
		else {

			pAnimator.enabled = true;

			if ( Closed )
				pSpriteRender.sprite = pSpriteClosed;
			else
				pSpriteRender.sprite = pSpriteOpen;
		}

	}

	private void OnMouseEnter() {
		
		bIsHighLighted = true;

		print( "asd" );

	}

	private void OnMouseExit() {

		bIsHighLighted = false;

	}

}
