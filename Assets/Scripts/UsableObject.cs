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

	private	bool				bUsed		= false;

	private	Animator			pAnimator	= null;

	private void Start() {
		
		if ( transform.childCount > 0 ) {
			pAnimator = transform.GetChild( 0 ).GetComponent<Animator>();
		}

	}

	public void OnReset() {

		bUsed = false;

	}

	public void OnUse( Player User ) {


		if ( transform.tag == "Door" || transform.tag == "Switcher" ) {
			GLOBALS.StageManager.AddActiveObject();
		}


		////////////////////////////////////////////////////////////////////////////
		// ALREADY USED
		if ( bUsed ) {

			if ( pObject )
				pObject.SendMessage( "OnReset" );

			if ( pAnimator && pAnimator.HasState( 0, Animator.StringToHash( "OnReset" ) ) ) pAnimator.Play( "OnReset", 0, 0 );

			bUsed = false;
			return;
		}

		////////////////////////////////////////////////////////////////////////////
		// USE

		bUsed = true;

		if ( transform.tag == "Key" ) {
			this.transform.GetComponent<Key>().OnUse( User );
			return;
		}

		if ( pAnimator && pAnimator.HasState( 0, Animator.StringToHash( "OnUse" ) ) ) pAnimator.Play( "OnUse" );

		if ( pObject ) {
			pObject.SendMessage( "OnUse", User );
		}

	}
}
