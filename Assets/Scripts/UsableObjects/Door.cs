using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : UsableObject {

	[Header("Value [ 1 - 255 ], zero is no valid ID")]
	[SerializeField]
	private		byte				KeyID				= 0;
	[SerializeField]
	private		bool				bUnlocked			= false;
	private		bool				bUsed				= false;

	private		Animator			pAnimator			= null;

	private	void Start() {
		
		if ( transform.childCount > 0 ) {
			pAnimator = transform.GetChild( 0 ).GetComponent<Animator>();
		}

	}

	public override	void	OnReset() {

		if ( bUsed ) {
			if ( pAnimator ) pAnimator.Play( "OnReset" );
			else print( " Cannot reproduce OnReset animation" );
			GLOBALS.StageManager.AddActiveObject();
			print( "switcher OnReset" );
			bUsed = false;
		}

	}


	public override	void OnUse( Player User ) {

		print( "door used" );

		if ( !bUnlocked && User && User.ActuaKey != KeyID ) {
			print( "You need right key" );
			return;
		}
		bUnlocked = true;

		// NOT USED
		if ( bUsed ) {
			OnReset();
		} else {
			if ( pAnimator ) pAnimator.Play( "OnUse" );
			else print( " Cannot reproduce OnUse animation" );
			GLOBALS.StageManager.AddActiveObject();
			print( "Door OnUse" );
			bUsed = true;
		}

	}

}
