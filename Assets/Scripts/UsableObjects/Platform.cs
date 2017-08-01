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



	

	// Use this for initialization
	void Start () {

		// PLATFORM
		Transform Platform	= transform.parent.transform.GetChild( 0 );

		// DOCK 1
		Transform Dock1		= transform.parent.transform.GetChild( 1 );
		Transform Point1	= Dock1.GetChild( 0 );

		// DOCK 1
		Transform Dock2		= transform.parent.transform.GetChild( 2 );
		Transform Point2	= Dock2.GetChild( 0 );


		if ( iStartpoint == 1 ) {
			vStartPosition	= Point1.position;
			vEndPosition	= Point2.position;
		}
		else {
			vStartPosition	= Point2.position;
			vEndPosition	= Point1.position;
		}
		transform.position	= vStartPosition;

		pSpriteRender		= Platform.GetChild(0).GetComponent<SpriteRenderer>();

		Point1.GetComponent<MeshRenderer>().enabled = false;
		Point2.GetComponent<MeshRenderer>().enabled = false;

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


	private void OnTriggerEnter( Collider other ) {
		
		if ( GLOBALS.StageManager.IsPlaying && !bHasPlayerInside && other.tag == "Player" ) {

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