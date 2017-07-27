using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Switcher {

	//	 HIGHLIGHTING
	[Header("HighLighting")]
	[SerializeField]
	private		Sprite				pSpriteDisabled				= null;
	[SerializeField]				// Default 
	private		Sprite				pSpriteDisabledHighlighted	= null;
	[SerializeField]
	private		Sprite				pSpriteEnabled				= null;
	[SerializeField]
	private		Sprite				pSpriteEnabledHighlighted	= null;

	private		bool				bIsHighLighted				= false;
	public		bool IsHighLighted {
		get { return bIsHighLighted; }
		set { bIsHighLighted = value; }
	}

	private void UpdateHighLighting() {

		if ( bIsHighLighted ) {

			pAnimator.enabled = false;

			if ( bUsed )
				pSpriteRender.sprite = pSpriteEnabledHighlighted;
			else
				pSpriteRender.sprite = pSpriteDisabledHighlighted;
		}
		else {

			pAnimator.enabled = true;

			if ( bUsed )
				pSpriteRender.sprite = pSpriteEnabled;
			else
				pSpriteRender.sprite = pSpriteDisabled;
		}


	}


	private	void	SetObjectsAsHighlighted( bool val ) {

		if ( vObjects.Length == 0 ) return;

		foreach( Transform o in vObjects ) {

			if ( o == null ) continue;

			// DOORS
			UsableObject p	= o.GetComponent<UsableObject>();
			if ( p ) {
				if ( p is Door ) {

					Door pDoor = p as Door;
					pDoor.IsHighLighted = val;
					continue;
				}
			}

			// PLATFORMS
			Platform p2 = o.GetComponent<Platform>();
			if ( p2 ) {
				if ( p2 is Platform ) {

					Platform pPlatform = p2 as Platform;
					pPlatform.IsHighLighted = val;

				}
			}
		}

	}


	private void OnMouseEnter() {
		
		if ( GLOBALS.StageManager.IsPlaying ) return;

		bIsHighLighted = true;

		this.SetObjectsAsHighlighted( true );

	}


	private void OnMouseExit() {

		if ( GLOBALS.StageManager.IsPlaying ) return;
		
		bIsHighLighted = false;

		this.SetObjectsAsHighlighted( false );
	}

}
