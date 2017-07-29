using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Door : UsableObject {


	//	 KEY AND LOCK STATE
	[Header("Key and loocked state")]
	[Header("Value [ 1 - 255 ], zero is no valid ID")]
	[SerializeField][Range(0, 255)]
	private		byte						KeyID					= 0;
	[SerializeField]
	private		bool						bLocked					= false;
	public		bool Locked {
		get { return bLocked; }
	}

	//	 INTERNAL VARS
	[SerializeField]
	private		bool						bClosed					= false;
	public		bool Closed {
		get { return bClosed; }
	}
	private		bool						bStartClosed			= false;
	private		Animator					pAnimator				= null;
	private		SpriteRenderer				pSpriteRender			= null;

	// LINKED SWITCHERS
	[Header("Switcher for this door")]
	[Header("Door must not be unlocked in order to parse switchers")]
	[SerializeField]
	private		Switcher[]					vSwitchers				= null;

	// Used for highlighting
	private		List<Switcher>				vUsers					= new List<Switcher>();


	private		void	Start() {
		
		if ( transform.childCount > 0 ) {
			pAnimator = transform.GetChild( 0 ).GetComponent<Animator>();

			if ( !pAnimator ) {
				GLOBALS.UI.ShowMessage( "Invalid Door", "A door object has not sprite with animator component", GLOBALS.GameManager.Exit );
				return;
			}

			}
		else {
			GLOBALS.UI.ShowMessage( "Invalid Door", "A door object has not sprite as child", GLOBALS.GameManager.Exit );
			Destroy( gameObject );
		}

		pSpriteRender	= transform.GetChild( 0 ).GetComponent<SpriteRenderer>();


		// If door need a key, make sure that no switcher can use it
		if ( KeyID == 0 ) {
			vUsers = new List<Switcher>();
			foreach( Switcher o in vSwitchers )
				vUsers.Add( o );
		}
		else {
			vSwitchers = null;
		}

		GLOBALS.EventManager.AddReceiver( this );

		if ( bClosed )
			pAnimator.Play( "Close", 0, 0.9f );
		else
			pAnimator.Play( "Open",  0, 0.9f );

		bStartClosed = bClosed;

	}



	private		void	Update() {
		
		//	 HIGHLIGHTING
		if ( GLOBALS.StageManager.IsPlaying ) {
			bIsHighLighted = false;
		}
		this.UpdateHighLighting();

	}



	public		void	AddUser( Switcher o ) {
		if ( ( vUsers != null ) && o ) vUsers.Add( o );
	}



	private		void	VerifySwitchers() {

		bool bSameState = vSwitchers[0].Used;

		for ( int i = 1; i < vSwitchers.Length; i++ ) {

			if ( vSwitchers[i].Used != bSameState ) {
/*				if ( bStartClosed )
					if ( !bClosed ) this.Close();
				else
					if ( bClosed )  this.Open();
*/				return;
			}
		}


		if ( bClosed )  this.Open(); else this.Close();
		
	}



	// called when a switcher in the world is used
	public		void	OnEvent( Switcher o ) {

		if ( !o || ( vSwitchers == null ) || ( vSwitchers.Length == 0 ) ) return;

		VerifySwitchers();

	}



	public		void	SetClosed( bool b ) {

		this.bClosed = b;

	}



	public override	void	OnReset() {	Close(); }

	private	void			Close() {

		if ( bClosed ) return;

		print( "DOOR CLOSING" );

		pAnimator.Play( "Close" );
		GLOBALS.StageManager.AddActiveObject();

	}


	public	override void	OnUse() {  if ( bClosed ) Open(); else Close(); }
	public	override void	OnUse( Player User ) { this.TryOpen(  User ); }

	private	void			TryOpen( Player User ) {


		if ( User && User.ActuaKey > 0 &&		// Usar have to have a valid key
			bLocked && KeyID > 0 &&				// Door must be blocked and have a valid kei ID set
			User.ActuaKey == KeyID )			// User KeyuId and Door KeyID myust be equal
		{

			bLocked = false;
			this.Open();
			return;
		}



		print( "You need right key" );

	}

	public void				Open() {

		if ( !bClosed ) return;

		print( "DOOR OPENING" );

		pAnimator.Play( "Open" );
		GLOBALS.StageManager.AddActiveObject();

	}


}