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
			if ( o.tag == "Door" ) {

				Door pDoor = o.GetComponent<UsableObject>() as Door;
				pDoor.IsHighLighted = val;
				continue;

			}

			// PLATFORMS
			if ( o.tag == "Platform" ) {

				Platform pPlatform = o.GetChild(0).GetComponent<Platform>() as Platform;
				pPlatform.IsHighLighted = val;

			}
		}

	}


	private void OnMouseEnter() {
		
		if ( GLOBALS.StageManager.IsPlaying ) return;

		this.SetObjectsAsHighlighted( bIsHighLighted = true );

		print( ( TutorialLvl || TutorialLvl_Plane ) );
		print( GLOBALS.TutorialSlot != null );
		print( GLOBALS.TutorialOverride == false );

		if ( ( TutorialLvl || TutorialLvl_Plane ) && GLOBALS.TutorialSlot != null && !GLOBALS.TutorialOverride ) GLOBALS.TutorialSlot.sprite = pTutorialSprite;

	}


	private void OnMouseExit() {

		if ( GLOBALS.StageManager.IsPlaying ) return;

		this.SetObjectsAsHighlighted( bIsHighLighted = false );

		if ( ( TutorialLvl || TutorialLvl_Plane ) && GLOBALS.TutorialSlot != null && !GLOBALS.TutorialOverride ) GLOBALS.TutorialSlot.sprite = null;
	}

}
