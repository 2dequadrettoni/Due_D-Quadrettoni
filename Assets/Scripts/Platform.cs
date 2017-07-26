using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Platform : MonoBehaviour {


	//	 HIGHLIGHTING
	[SerializeField]
	private		Sprite				pSpriteDefault				= null;
	[SerializeField]
	private		Sprite				pSpriteHighlight			= null;
	private		bool				bIsHighLighted				= false;
	public		bool IsHighLighted {
		get { return bIsHighLighted; }
		set { bIsHighLighted = value; }
	}
	private		SpriteRenderer		pSpriteRender				= null;

	////////////////////////////////////////////////////////////////////////

	// LINK
	private		bool				bHasPlayerInside			= false;
	public		bool HasPlayerInside {
		get { return bHasPlayerInside; }

	}

	////////////////////////////////////////////////////////////////////////

	private		Player				pPlayer						= null;
	[Range(1,2)]
	public		int					iStartpoint					= 1;

	// MOVEMENT
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



	

	// Use this for initialization
	void Start () {

		if ( iStartpoint == 1 ) {
			vStartPosition	= transform.GetChild( 0 ).position;
			vEndPosition	= transform.GetChild( 1 ).position;
		}
		else {
			vStartPosition	= transform.GetChild( 1 ).position;
			vEndPosition	= transform.GetChild( 0 ).position;
		}
		transform .position = vStartPosition;

		pSpriteRender	= transform.GetChild( 2 ).GetComponent<SpriteRenderer>();

	}

	private void Update() {
		
		if ( bIsHighLighted ) {
			pSpriteRender.sprite = pSpriteHighlight;
		}
		else {
			pSpriteRender.sprite = pSpriteDefault;
		}

		bIsHighLighted = false;

	}


	// Update is called once per frame
	public void UpdatePosition() {

		if ( !bActive ) return;

		fInterpolant += ( Time.deltaTime * fMoveSpeed ) * iDirection;

		// PATROL POINT REACHED
		if ( ( fInterpolant <= 0.0f ) || ( fInterpolant >= 1.0f ) ) {
			bActive			= false;
			iDirection		*= -1;
			fInterpolant	= Mathf.Clamp01( fInterpolant );
			if ( pPlayer ) pPlayer.transform.position = ( iStartpoint == 1 ) ? vStartPosition : vEndPosition;
			GLOBALS.StageManager.RemoveActiveObject();
			return;
		}

		transform.position = Vector3.Lerp( vStartPosition, vEndPosition, fInterpolant );
		if ( pPlayer ) pPlayer.transform.position = transform.position;
	}


	public void	OnReset() {
		bActive = true;
	}

	public void	OnUse( Player User ) {
		bActive = true;
	}


	private void OnTriggerEnter( Collider other ) {
		
		if ( GLOBALS.StageManager.IsPlaying && !bHasPlayerInside && other.tag == "Player" ) {

			Player pPlayer = other.GetComponent<Player>();
			if ( !pPlayer.Linked ) {
				pPlayer.Stop();
				pPlayer.Link( this );
				bHasPlayerInside = true;
				this.pPlayer = pPlayer;
				pPlayer.transform.position = transform.position;
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
