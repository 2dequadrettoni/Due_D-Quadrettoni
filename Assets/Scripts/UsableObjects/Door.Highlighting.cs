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

	private	void	SetUsersAsHighlighted( bool val ) {

		if ( vUsers.Count == 0 ) return;

		foreach( Switcher o in vUsers ) {

			if ( o == null ) continue;

			o.IsHighLighted = val;

		}

	}


	void	OnMouseEnter()  {

		if ( GLOBALS.StageManager.IsPlaying ) return;

		SetUsersAsHighlighted( bIsHighLighted = true );

	}

	void	OnMouseExit() {

		if ( GLOBALS.StageManager.IsPlaying ) return;

		SetUsersAsHighlighted( bIsHighLighted = false );

	}
}
