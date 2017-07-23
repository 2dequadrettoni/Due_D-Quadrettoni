using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public		enum			UsageType		{ NONE, INSTANT, ON_ACTION_END };

public partial class UsableObject : MonoBehaviour {

	public	Transform			pObject		= null;

	[SerializeField]
	private	UsageType			iUseType	= UsageType.NONE;
	public	UsageType	UseType {
		get { return iUseType; }
		set { iUseType = value; }
	}

	private	Animator			pAnimator	= null;

	private void Start() {
		
		if ( transform.childCount > 0 ) {
			pAnimator = transform.GetChild( 0 ).GetComponent<Animator>();
		}

	}

	public void OnReset() {

		pObject.SendMessage( "OnReset" );
	}

	public void OnUse( Player User ) {

		print( "SendMessage" );


		if ( transform.tag == "Key" ) {
			this.transform.GetComponent<Key>().OnUse( User );
			return;
		}


		if ( transform.tag == "Door" ) print( "Door used" );

		if ( pAnimator/* && pAnimator.HasState( 0, Animator.StringToHash( "OnUse" ) ) */ ) pAnimator.Play( "OnUse" );

		if ( pObject ) {
			pObject.SendMessage( "OnUse", User );
		}
		else {
//			transform.SendMessage( "OnUse", User );
		}

	}
}
