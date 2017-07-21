using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour {

	[Header("Value [ 1 - 255 ], zero is no valid Key ID")]
	public	byte	KeyID	= 0;

	// Use this for initialization
	void Start () {
		
		if ( KeyID == 0 ) {
			print( "Invalid key ID for key \"" + gameObject.name + "\"" );
			Destroy( gameObject );
		}

	}

	public void OnUse( Player User ) {

		User.ActuaKey = KeyID;
		Destroy( gameObject );

	}
}
