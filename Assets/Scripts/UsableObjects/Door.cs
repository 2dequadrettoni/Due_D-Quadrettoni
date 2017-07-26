using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : UsableObject {

	[Header("Value [ 1 - 255 ], zero is no valid ID")]
	[SerializeField][Range(0, 254 )]
	private		byte				KeyID				= 1;
	[SerializeField]
	private		bool				bLocked				= false;
	public		bool Locked {
		get { return bLocked; }
	}

	private		bool				IsPlayingAnimation	= false;

	private		bool				bUsed				= false;
	public		bool Used {
		get { return bUsed; }
	}

	[Header("Switcher for this door")]
	[Header("Door must not be unlocked in order to parse switchers")]
	[SerializeField]
	private		Switcher[]			vSwitchers			= null;

	private		Animator			pAnimator			= null;

	private	void Start() {
		
		if ( transform.childCount > 0 ) {
			pAnimator = transform.GetChild( 0 ).GetComponent<Animator>();
		}

	}

	public	void	SetUsed( bool b ) {

		this.bUsed = b;
		IsPlayingAnimation = false;

	}

	private void Update() {
		
		if ( vSwitchers != null ) {

			if ( vSwitchers.Length == 0 ) return;

			foreach( Switcher o in vSwitchers ) {
				if ( !o.Used ) return;
			}
		}

		if ( !bUsed )
			this.OnUse( null );

	}

	public override	void	OnReset() {

		if ( IsPlayingAnimation ) return;

		print( "switcher OnReset" );

		if ( bUsed ) {

			if ( pAnimator ) {
				pAnimator.Play( "OnReset" );
				GLOBALS.StageManager.AddActiveObject();
				IsPlayingAnimation = true;
			//	bUsed = false;
			}
			else {
				print( " Cannot reproduce OnReset animation" );
			}
			
		}

	}


	public override	void OnUse( Player User ) {

		if ( IsPlayingAnimation ) return;

		print( "Door OnUse" );

		if ( bLocked && User && User.ActuaKey != KeyID ) {
			print( "You need right key" );
			return;
		}
		bLocked = false;

		if ( bUsed ) {
			OnReset();
		} else {
			
			if ( pAnimator ) {
				pAnimator.Play( "OnUse" );
				GLOBALS.StageManager.AddActiveObject();
				IsPlayingAnimation = true;
		//		bUsed = true;
			}
			else {
				print( " Cannot reproduce OnUse animation" );
			}
			
		}

	}

}
