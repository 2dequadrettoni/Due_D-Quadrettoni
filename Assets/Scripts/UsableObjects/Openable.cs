using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Openable : MonoBehaviour {

	[Header("Value [ 1 - 255 ], zero is no valid ID")]
	public	byte	KeyID	= 0;
	private	bool	bUnlocked	= false;
	private	bool	bUsed		= false;


	public void OnUse( Player User ) {

		if ( !bUnlocked && User.ActuaKey != KeyID ) {
			print( "You need right key" );
			return;
		}
		bUnlocked = true;

		if ( bUsed ) {

			// object reset
			GetComponent<BoxCollider>().enabled = true;
			GetComponent<MeshRenderer>().enabled = true;
			bUsed = false;

		} else {

			GetComponent<BoxCollider>().enabled = false;
			GetComponent<MeshRenderer>().enabled = false;
			bUsed = true;

		}

	}

}
