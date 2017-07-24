using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Openable : MonoBehaviour {

	[Header("Value [ 1 - 255 ], zero is no valid ID")]
	[SerializeField]
	private	byte	KeyID				= 0;
	private	bool	bUnlocked			= false;
	private	bool	bUsed				= false;

	public	enum	OpenableTypes {
		DOOR, TREASURE
	}

	[SerializeField]
	private	OpenableTypes	Type		= OpenableTypes.DOOR;

	private	Animator		pAnimator	= null;

	private void Start() {
		
		if ( transform.childCount > 0 ) {
			pAnimator = transform.GetChild( 0 ).GetComponent<Animator>();
		}

	}


	public void OnUse( Player User ) {

		if ( !bUnlocked && User.ActuaKey != KeyID ) {
			print( "You need right key" );
			return;
		}
		bUnlocked = true;

		if ( bUsed ) {
			bUsed = false;
			pAnimator.Play( "OnReset" );
		} else {
			bUsed = true;
			pAnimator.Play( "OnUse" );
		}

	}

}
