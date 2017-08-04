using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Switcher : UsableObject {
	

	////////////////////////////////////////////////////////////////////////

	[SerializeField]
	private		Transform[]			vObjects					= null;

	////////////////////////////////////////////////////////////////////////
	private		bool				bUsed						= false;
	public	bool Used {
		get { return bUsed; }
	}

	private		Animator			pAnimator					= null;

	private		SpriteRenderer		pSpriteRender				= null;



	private void Start() {
		
		if ( transform.childCount > 0 ) {
			pAnimator = transform.GetChild( 0 ).GetComponent<Animator>();

			if ( !pAnimator ) {
				GLOBALS.UI.ShowMessage( "Invalid switcher", "A switcher object has not sprite with animator component", GLOBALS.GameManager.Exit );
				return;
			}

			}
		else {
			GLOBALS.UI.ShowMessage( "Invalid switcher", "A switcher object has not sprite as child", GLOBALS.GameManager.Exit );
			Destroy( gameObject );
		}

		pSpriteRender	= transform.GetChild( 0 ).GetComponent<SpriteRenderer>();
		
	}



	private void Update() {
		
		//	 HIGHLIGHTING
		if ( GLOBALS.StageManager.IsPlaying ) {
			bIsHighLighted = false;
		}
		this.UpdateHighLighting();

	}


	// CALLED AT ANIMATION END
	public	void	SetUsed( bool value ) {

		this.bUsed = value;

		if ( vObjects == null || vObjects.Length == 0 )
			GLOBALS.EventManager.SentEvent( this );

		// USABLE OBJECTS ( DOORS, PLATFORMS )
		if ( ( vObjects != null ) && ( vObjects.Length > 0 ) ) {
			foreach( Transform o in vObjects ) {
				if ( o ) o.gameObject.BroadcastMessage( value ? "OnUse" : "OnReset" );
			}
		}
	}




	public	override	void	OnReset() {

		print( "SWITCHER OnReset" );

		if ( pAnimator ) {
			pAnimator.Play( "OnReset" );
			GLOBALS.StageManager.AddActiveObject();
			return;
		}

	}




	public override void OnUse( Player User ) {

		GLOBALS.AudioManager.Play( ( tag == "Switcher" ) ? "Switcher_OnUse" : "Switcher_Plane_OnUse" );

		if ( bUsed ) {
			OnReset();
			return;
		}
		

		print( "SWITCHER OnUse" );
		// Sanity check
		if ( pAnimator ) {
			pAnimator.Play( "OnUse" );
			GLOBALS.StageManager.AddActiveObject();
			return;
		}
		
	}

}
