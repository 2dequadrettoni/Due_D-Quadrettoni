using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public partial class Platform {


	//	 HIGHLIGHTING
	[Header("HighLighting")]
	[SerializeField]
	private		Sprite				pSpriteDefault				= null;
	[SerializeField]
	private		Sprite				pSpriteHighlight			= null;
	private		bool				bIsHighLighted				= false;
	public		bool IsHighLighted {
		get { return bIsHighLighted; }
		set { bIsHighLighted = value; }
	}


	private void	UpdateHighLighting() {
		
		if ( bIsHighLighted ) {
			pSpriteRender.sprite = pSpriteHighlight;
		}
		else {
			pSpriteRender.sprite = pSpriteDefault;
		}

	}


	private	void	SetUsersAsHighlighted( bool val ) {

		if ( vSwitchers.Count == 0 ) return;

		foreach( Switcher o in vSwitchers ) {

			if ( o == null ) continue;

			Switcher p	= o.GetComponent<Switcher>();
			if ( p ) p.IsHighLighted = val;

		}

	}



	private void OnMouseEnter() {

		if ( GLOBALS.StageManager.IsPlaying ) return;
		
		SetUsersAsHighlighted( bIsHighLighted = true );

	}


	private void OnMouseExit() {

		if ( GLOBALS.StageManager.IsPlaying ) return;
		
		SetUsersAsHighlighted( bIsHighLighted = false );

	}

}
