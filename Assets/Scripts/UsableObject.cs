using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public		enum			UsageType		{ NONE, INSTANT, ON_ACTION_END };

public class UsableObject : MonoBehaviour {

	public	Transform			pObject  = null;

	[SerializeField]
	private	UsageType			iUseType = UsageType.NONE;
	public	UsageType	UseType {
		get { return iUseType; }
		set { iUseType = value; }
	}

	public void OnReset() {

		pObject.SendMessage( "OnReset" );
	}

	public void OnUse( Player User ) {

		print( "SendMessage" );

		if ( pObject ) {
			pObject.SendMessage( "OnUse", User );
		}
		else {
//			transform.SendMessage( "OnUse", User );
		}

	}
}
