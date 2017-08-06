using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Key : UsableObject {

	public	static	bool			TutorialLvl					= false;
	[SerializeField]
	private	Sprite					pTutorialSprite				= null;


	private		Door				pDoor						= null;

	private		Animator			pAnimator					= null;
	private		SpriteRenderer		pSpriteRender				= null;

	// Use this for initialization
	void Start () {

		pAnimator		= transform.GetChild( 0 ).GetComponent<Animator>();

		pSpriteRender	= transform.GetChild( 0 ).GetComponent<SpriteRenderer>();

	}

	public	void	SetDoor( Door p ) {
		if ( p != null ) pDoor = p;
	}

	private	void	Update() {

		//	 HIGHLIGHTING
		if ( GLOBALS.StageManager.IsPlaying ) {
			bIsHighLighted = false;
		}
		this.UpdateHighLighting();
		
	}

	public override void OnUse( Player User ) {

		print( "key picked" );

		User.ActuaKey = this;

		// Hide from user view
		transform.position = Vector3.up * 10000.0f;

		AudioManager.Play( "Key_Pickup" );

	

	}
}
