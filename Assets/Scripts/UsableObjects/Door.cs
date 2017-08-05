using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#pragma warning disable CS0162 // Unreachable code detected

public partial class Door : UsableObject {

	const	bool					bDoorDebug					= false;

	public	static	bool			TutorialLvl					= false;
	[SerializeField]
	private	Sprite					pTutorialSprite				= null;

	////////////////////////////////////////////////////////////////////////

	//	 KEY AND LOCK STATE
	[Header("Key and loocked state")]
	[SerializeField]
	private		Key					pKey						= null;
	[SerializeField]
	private		bool				bLocked						= false;
	public		bool Locked {
		get { return bLocked; }
	}

	////////////////////////////////////////////////////////////////////////

	//	 INTERNAL VARS
	[SerializeField]
	private		bool				bClosed						= false;
	public		bool Closed {
		get { return bClosed; }
	}

	private		Animator			pAnimator					= null;
	private		SpriteRenderer		pSpriteRender				= null;

	////////////////////////////////////////////////////////////////////////

	// LINKED SWITCHERS
	[Header("Switcher for this door")]
	[Header("Door must not be unlocked in order to parse switchers")]
	[SerializeField]
	private		Switcher[]			vSwitchers					= null;

	////////////////////////////////////////////////////////////////////////

	// Used for highlighting
	private		List<Switcher>		vUsers						= new List<Switcher>();




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

		transform.FindChild( "Direction" ).GetComponent<SpriteRenderer>().enabled = false;

		// If door need a key, make sure that no switcher can use it
		if ( pKey == null ) {
			if ( vSwitchers != null && vSwitchers.Length > 0 ) {
				vUsers = new List<Switcher>();
				foreach( Switcher o in vSwitchers )
					vUsers.Add( o );
			}
		}
		else {
			pKey.SetDoor( this );

			vSwitchers = null;
		}

		GLOBALS.EventManager.AddReceiver( this );

		if ( bClosed )
			pAnimator.Play( "Close", 0, 0.9f );
		else
			pAnimator.Play( "Open",  0, 0.9f );

//		bStartClosed = bClosed;

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

		bool bSameState = vUsers[0].Used;

		for ( int i = 1; i < vUsers.Count; i++ ) {

			if ( vUsers[i].Used != bSameState ) {
				return;
			}
		}

		if ( bClosed )  this.Open(); else this.Close();
		
	}



	private		bool	ContainsSwitcher( Switcher p ) {
		foreach ( Switcher o in vSwitchers ) {

			if ( !o ) continue;
			if ( o.GetInstanceID() == p.GetInstanceID() ) return true;

		}
		return false;
	}



	// called when a switcher in the world is used
	public		void	OnEvent( Switcher o ) {

		if ( ( vSwitchers == null ) || ( vSwitchers.Length == 0 ) || !ContainsSwitcher( o ) ) return;

		VerifySwitchers();

	}



	public		void	SetClosed( bool b ) {

		this.bClosed = b;

	}



	public override	void	OnReset() {	Close(); }

	private	void			Close() {

		if ( bClosed ) return;

		if ( bDoorDebug ) print( "DOOR CLOSING" );

//		GLOBALS.AudioManager.Play( "Door_Closing" );

		pAnimator.Play( "Close" );
		GLOBALS.StageManager.AddActiveObject();

	}


	public	override void	OnUse() {  if ( bClosed ) Open(); else Close(); }
	public	override void	OnUse( Player User ) { this.TryOpen(  User ); }

	private	void			TryOpen( Player User ) {


		if ( User && ( User.ActuaKey != null ) &&						// Usar have to have a valid key
			bLocked && ( pKey != null ) &&								// Door must be blocked and have a valid key set
			User.ActuaKey.GetInstanceID() == pKey.GetInstanceID() )		// User Key and Door Key must be equal
		{
			User.ActuaKey = null;
			bLocked = false;
			this.Open();

//			GLOBALS.AudioManager.Play( "Key_Use" );

			return;
		}



		if ( bDoorDebug ) print( "You need right key" );

	}

	public void				Open() {

		if ( !bClosed ) return;

		if ( bDoorDebug ) print( "DOOR OPENING" );

//		GLOBALS.AudioManager.Play( "Door_Opening" );

		pAnimator.Play( "Open" );
		GLOBALS.StageManager.AddActiveObject();

	}


}

#pragma warning restore CS0162 // Unreachable code detected