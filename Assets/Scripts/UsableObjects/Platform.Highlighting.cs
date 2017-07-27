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

	private void UpdateHighLighting() {
		
		if ( bIsHighLighted ) {
			pSpriteRender.sprite = pSpriteHighlight;
		}
		else {
			pSpriteRender.sprite = pSpriteDefault;
		}

	}

}
