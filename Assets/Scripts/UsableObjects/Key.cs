using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : UsableObject {

	[Header("Value [ 1 - 255 ], zero is no valid Key ID")]
	[SerializeField][Range(1, 254 )]
	private	byte	KeyID	= 0;

	// Use this for initialization
	void Start () {
		
		if ( KeyID == 0 ) {
			print( "Invalid key ID for key \"" + gameObject.name + "\"" );
			Destroy( gameObject );
		}

	}

	new public void OnUse( Player User ) {

		print( "key picked" );
		User.ActuaKey = KeyID;
		Destroy( gameObject );

	}
}
