﻿using System.Collections;
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


	private void OnMouseEnter() {
		
		bIsHighLighted = true;

		pAnimator.enabled = false;
		pSpriteRender.sprite = pSpriteHighlight;

	}

	private void OnMouseExit() {
		
		bIsHighLighted = false;

		pAnimator.enabled = true;
		pSpriteRender.sprite = pSpriteDefault;

	}


}
