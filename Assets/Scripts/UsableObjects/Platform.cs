using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#pragma warning disable CS0162 // Unreachable code detected

public partial class Platform : MonoBehaviour {

	const	bool	bPlatformDebug		= false;

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


	// PLATFORM
	private		Transform			pPlatform					= null;

	// DOCK 1
	private		Transform			pDock1						= null;
	private		Transform			pPoint1						= null;

	// DOCK 2
	private		Transform			pDock2						= null;
	private		Transform			pPoint2						= null;
	

	// Use this for initialization
	void Start () {

		// PLATFORM
		pPlatform	= transform.parent.transform.GetChild( 0 );

		// DOCK 1
		pDock1		= transform.parent.transform.GetChild( 1 );
		pPoint1		= pDock1.GetChild( 0 );

		// DOCK 1
		pDock2		= transform.parent.transform.GetChild( 2 );
		pPoint2		= pDock2.GetChild( 0 );


		if ( iStartpoint == 1 ) {
			vStartPosition	= pPoint1.position;
			vEndPosition	= pPoint2.position;
		}
		else {
			vStartPosition	= pPoint2.position;
			vEndPosition	= pPoint1.position;
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
			if ( pPlayer ) pPlayer.transform.position = transform.position;
			if ( pPlayer && pPlayer.FindPath ( ( ( (int)fInterpolant + 1 ) == iStartpoint ) ?  pDock1.position : pDock2.position ) )
				pPlayer.Move();

			return;
		}

		transform.position = Vector3.Lerp( vStartPosition, vEndPosition, fInterpolant );
		if ( pPlayer ) pPlayer.transform.position = transform.position;
	}


	public void	OnReset() {
		bActive = true;
	}

	public void	OnUse() {
		bActive = true;
	}


	private void OnTriggerStay( Collider other ) {
		
		if ( !bActive && GLOBALS.StageManager.IsPlaying && !bHasPlayerInside && other.tag == "Player" ) {

			Player pPlayer = other.GetComponent<Player>();
			if ( !pPlayer.Linked ) {
				print( "asdasdasdsa" );
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