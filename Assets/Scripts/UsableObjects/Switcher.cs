using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Switcher : UsableObject {

	public	Transform			pObject		= null;

	private	bool				bUsed		= false;
	public	bool Used {
		get { return bUsed; }
	}

	private	Animator			pAnimator	= null;



	private void Start() {
		
		pAnimator = transform.GetChild( 0 ).GetComponent<Animator>();

	}

	public	override	void	OnReset() {

		if ( bUsed ) {
			pObject.SendMessage( "OnReset" );
			if ( pAnimator && pAnimator.HasState( 0, Animator.StringToHash( "OnReset" ) ) ) {
				pAnimator.Play( "OnReset" );
				GLOBALS.StageManager.AddActiveObject();
			}
			else print( " Cannot reproduce OnReset animation" );
			bUsed = false;
			print( "switcher OnReset" );
		}

	}


	public override void OnUse( Player User ) {

		// SWITCHERS
		if ( pObject && ( transform.tag == "Switcher" || transform.tag == "Plane_Switcher" ) ) {

			// NOT USED
			if ( bUsed ) {
				OnReset();
			}
			// USED
			else {
				if ( pAnimator && pAnimator.HasState( 0, Animator.StringToHash( "OnUse" ) ) ) {
					pAnimator.Play( "OnUse" );
					GLOBALS.StageManager.AddActiveObject();
				}
				else print( " Cannot reproduce OnUse animation" );
				
				pObject.SendMessage( "OnUse", User );
				bUsed = true;
				print( "switcher OnUse" );
			}

		}
		
	}
}
