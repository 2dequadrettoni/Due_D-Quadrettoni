using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Key : UsableObject {

	[Header("Value [ 1 - 255 ], zero is no valid Key ID")]
	[SerializeField][Range(1, 254 )]
	private		byte				KeyID						= 0;

	private		Animator			pAnimator					= null;
	private		SpriteRenderer		pSpriteRender				= null;

	// Use this for initialization
	void Start () {
		
		if ( KeyID == 0 ) {
			print( "Invalid key ID for key \"" + gameObject.name + "\"" );
			Destroy( gameObject );
			return;
		}

		pAnimator		= transform.GetChild( 0 ).GetComponent<Animator>();

		pSpriteRender	= transform.GetChild( 0 ).GetComponent<SpriteRenderer>();

	}

	private	void	Update() {

		//	 HIGHLIGHTING
		if ( GLOBALS.StageManager.IsPlaying ) {
			bIsHighLighted = false;
		}
		this.UpdateHighLighting();
		
	}

	new public void OnUse( Player User ) {

		print( "key picked" );
		User.ActuaKey = KeyID;
		Destroy( gameObject );

	}
}
