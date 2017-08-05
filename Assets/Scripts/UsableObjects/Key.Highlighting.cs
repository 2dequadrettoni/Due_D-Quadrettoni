using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Key : UsableObject {
	
	
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


	private	void	UpdateHighLighting() {

		if ( bIsHighLighted ) {
			if ( pSpriteHighlight ) {
				pAnimator.enabled = false;
				pSpriteRender.sprite = pSpriteHighlight;
			}
		}
		else {
			if ( pSpriteDefault ) {
				pAnimator.enabled = true;
				pSpriteRender.sprite = pSpriteDefault;
			}
		}

	}

	private void OnMouseEnter() {
		
		bIsHighLighted = true;

		if ( pDoor != null ) pDoor.IsHighLighted = true;

		if ( TutorialLvl && GLOBALS.TutorialSlot != null && !GLOBALS.TutorialOverride ) GLOBALS.TutorialSlot.sprite = pTutorialSprite;

	}

	private void OnMouseExit() {
		
		bIsHighLighted = false;

		if ( pDoor != null ) pDoor.IsHighLighted = false;

		if ( TutorialLvl && GLOBALS.TutorialSlot != null && !GLOBALS.TutorialOverride ) GLOBALS.TutorialSlot.sprite = null;

	}


}
