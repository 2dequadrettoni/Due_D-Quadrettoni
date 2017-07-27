using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Door : UsableObject {


	//	 KEY AND LOCK STATE
	[Header("Key and loocked state")]
	[Header("Value [ 1 - 255 ], zero is no valid ID")]
	[SerializeField][Range(0, 254 )]
	private		byte				KeyID							= 1;
	[SerializeField]
	private		bool				bLocked							= false;
	public		bool Locked {
		get { return bLocked; }
	}


	// LINKED SWITCHERS
	[Header("Switcher for this door")]
	[Header("Door must not be unlocked in order to parse switchers")]
	[SerializeField]
	private		Switcher[]					vSwitchers				= null;

	//	 INTERNAL VARS
	private		bool						IsPlayingAnimation		= false;
	private		bool						bUsed					= false;
	public		bool Used {
		get { return bUsed; }
	}
	public		bool Closed {
		get { return !bUsed; }
	}
	private		Animator					pAnimator				= null;
	private		SpriteRenderer				pSpriteRender			= null;



	private		void	Start() {
		
		if ( transform.childCount > 0 ) {
			pAnimator = transform.GetChild( 0 ).GetComponent<Animator>();
		}

		pSpriteRender	= transform.GetChild( 0 ).GetComponent<SpriteRenderer>();

	}

	private		void	Update() {
		
		//	 HIGHLIGHTING
		if ( GLOBALS.StageManager.IsPlaying ) {
			bIsHighLighted = false;
		}
		this.UpdateHighLighting();

		if ( vSwitchers != null ) {

			if ( vSwitchers.Length == 0 ) return;

			foreach ( Switcher o in vSwitchers ) {
				if ( !o.Used ) return;
			}

		}

		if ( !bUsed )
			this.OnUse( null );

	}

	public	void	SetUsed( bool b ) {

		this.bUsed = b;
		IsPlayingAnimation = false;

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




// TODO: aree separate che gestiscono 2 condizioni di di vittoria, ognuna per pg