using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#pragma warning disable CS0162 // Unreachable code detected

public partial class Platform : MonoBehaviour {

	const	bool					bPlatformDebug				= false;

	public	static	bool			TutorialLvl					= false;
	[SerializeField]
	private	Sprite					pTutorialSprite				= null;

	////////////////////////////////////////////////////////////////////////

	// LINK
	private		bool				bHasPlayerInside			= false;
	public		bool HasPlayerInside {
		get { return bHasPlayerInside; }

	}
	private		Player				pPlayer						= null;

	////////////////////////////////////////////////////////////////////////

	// MOVEMENT
	[Header("Movement")][Range(1,2)]
	public		int					iStartpoint					= 1;
	private		Vector3				vStartPosition				= Vector3.zero;
	private		Vector3				vEndPosition				= Vector3.zero;
	private		float				fInterpolant				= 0.0f;
	private		int					iDirection					= 1;
	[SerializeField]
	private		float				fMoveSpeed					= 5.0f;

	private		bool				bActive						= false;
	public		bool IsActive {
		get { return bActive; }

	}

	private		SpriteRenderer		pSpriteRender				= null;


	private		List<Switcher>		vSwitchers					= new List<Switcher>();

	////////////////////////////////////////////////////////////////////////

	// PLATFORM
	private		Transform			pPlatform					= null;

	////////////////////////////////////////////////////////////////////////

	// DOCK 1
	private		Transform			pDock1						= null;
	private		Transform			pPoint1						= null;
	private		Platform_Dock		pPlatform_Dock1				= null;

	////////////////////////////////////////////////////////////////////////

	// DOCK 2
	private		Transform			pDock2						= null;
	private		Transform			pPoint2						= null;
	private		Platform_Dock		pPlatform_Dock2				= null;

	private		
	

	// Use this for initialization
	void Start () {

		// PLATFORM
		pPlatform	= transform.parent.transform.GetChild( 0 );

		// DOCK 1
		pDock1		= transform.parent.transform.GetChild( 1 );
		pPoint1		= pDock1.GetChild( 0 );
		pPlatform_Dock1 = pDock1.GetComponent<Platform_Dock>();

		// DOCK 1
		pDock2		= transform.parent.transform.GetChild( 2 );
		pPoint2		= pDock2.GetChild( 0 );
		pPlatform_Dock2 = pDock2.GetComponent<Platform_Dock>();


		if ( iStartpoint == 1 ) {
			vStartPosition	= pPoint1.position;
			vEndPosition	= pPoint2.position;
			pPlatform_Dock1.CurrentPlatformDock = true;
			pPlatform_Dock1.CurrentPlatformDock = false;
		}
		else {
			vStartPosition	= pPoint2.position;
			vEndPosition	= pPoint1.position;
			pPlatform_Dock1.CurrentPlatformDock = false;
			pPlatform_Dock1.CurrentPlatformDock = true;
		}
		transform.position	= vStartPosition;

		pSpriteRender		= pPlatform.GetChild(0).GetComponent<SpriteRenderer>();

		pPoint1.GetComponent<MeshRenderer>().enabled = false;
		pPoint2.GetComponent<MeshRenderer>().enabled = false;

		GLOBALS.EventManager.AddReceiver( this );

	}

	public	void	AddUser( Switcher o ) {
		if ( o ) vSwitchers.Add( o );
	}

	public	void	OnEvent( Switcher o ) {

		if ( !o ) return;

	}

	private	void	 Update() {

		//	 HIGHLIGHTING
		if ( GLOBALS.StageManager.IsPlaying ) {
			bIsHighLighted = false;
		}
		this.UpdateHighLighting();
		/*

		if ( !bActive && GLOBALS.StageManager.IsPlaying && !bHasPlayerInside ) {

			Player pPlayer = null;

			if ( pPlatform_Dock1.PlayerOn && ( pPlayer = pPlatform_Dock1.CurrentPlayer ) ||
				( pPlatform_Dock2.PlayerOn && ( pPlayer = pPlatform_Dock2.CurrentPlayer ) ) ) {

				if ( !pPlayer.Linked && !pPlayer.IsBusy() ) {
					pPlayer.Stop();
					pPlayer.SetIdle();
					pPlayer.Link( this );
					bHasPlayerInside = true;
					pPlayer.transform.position = transform.position;
					this.pPlayer = pPlayer;
					print( "Player " + pPlayer.ID + " ENTER platform" );
				}
			}

		}
		*/

	}


	// Update is called once per frame
	public void UpdatePosition() {

		if ( !bActive ) return;

		fInterpolant += ( Time.deltaTime * fMoveSpeed ) * iDirection;

		// OTHER POINT REACHED
		if ( ( fInterpolant <= 0.0f ) || ( fInterpolant >= 1.0f ) ) {
			bActive			= false;
			iDirection		*= -1;
			fInterpolant	= Mathf.Clamp01( fInterpolant );
			GLOBALS.StageManager.RemoveActiveObject();
			transform.position = Vector3.Lerp( vStartPosition, vEndPosition, fInterpolant );

			if ( ( (int)fInterpolant + 1 ) == iStartpoint ) {
				pPlatform_Dock1.CurrentPlatformDock = true;
				pPlatform_Dock2.CurrentPlatformDock = false;
			}
			else {
				pPlatform_Dock1.CurrentPlatformDock = false;
				pPlatform_Dock2.CurrentPlatformDock = true;
			}


			if ( bHasPlayerInside ) {

				pPlayer.transform.position = transform.position;

				if ( pPlayer.FindPath ( ( ( (int)fInterpolant + 1 ) == iStartpoint ) ?  pDock1.position : pDock2.position ) ) {
					pPlayer.Move();
				}
			}

			return;
		}

		transform.position = Vector3.Lerp( vStartPosition, vEndPosition, fInterpolant );
		if ( pPlayer ) pPlayer.transform.position = transform.position;
	}


	public void	OnReset() {
		AudioManager.Play( "Platform_OnUse" );
		bActive = true;
	}

	public void	OnUse() {
		AudioManager.Play( "Platform_OnUse" );
		bActive = true;
	}
	

	private void OnTriggerStay( Collider other ) {
		
		if ( pPlayer != null && pPlayer.IsBusy() && pPlayer.Linked ) {
			pPlayer.UnLink( this );
			bHasPlayerInside = false;
			return;
		}

		if ( !bActive && GLOBALS.StageManager.IsPlaying && !bHasPlayerInside && other.tag == "Player" ) {

			if ( !pPlatform_Dock1.PlayerOn && !pPlatform_Dock2.PlayerOn ) return;

			Player pPlayer = other.GetComponent<Player>();
			if ( !pPlayer.Linked && !pPlayer.IsBusy() ) {
				pPlayer.Stop();
				pPlayer.SetIdle();
				pPlayer.Link( this );
				bHasPlayerInside = true;
				pPlayer.transform.position = transform.position;
				this.pPlayer = pPlayer;
				print( "Player " + pPlayer.ID + " ENTER platform" );
			}
		}

	}
	
	private void OnTriggerExit( Collider other ) {
		
		if ( GLOBALS.StageManager.IsPlaying && bHasPlayerInside && other.tag == "Player" ) {

			Player pPlayer = ( other.name == "Player1" ) ? GLOBALS.Player1 : GLOBALS.Player2;
			if ( pPlayer.Linked ) {
				pPlayer.UnLink( this );
				bHasPlayerInside = false;
				this.pPlayer = null;
				print( "Player " + pPlayer.ID + " EXIT platform" );
			}
		}

	}
	
}

#pragma warning restore CS0162 // Unreachable code detected