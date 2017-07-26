using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Switcher : UsableObject {


	//	 HIGHLIGHTING
	[SerializeField]
	private		Sprite				pSpriteDisabled				= null;
	[SerializeField]				// Default 
	private		Sprite				pSpriteDisabledHighlighted	= null;
	[SerializeField]
	private		Sprite				pSpriteEnabled				= null;
	[SerializeField]
	private		Sprite				pSpriteEnabledHighlighted	= null;

	private		bool				bIsHighLighted				= false;
	public		bool IsHighLighted {
		get { return bIsHighLighted; }
		set { bIsHighLighted = value; }
	}
	private		SpriteRenderer		pSpriteRender				= null;

	////////////////////////////////////////////////////////////////////////

	[SerializeField]
	private		Transform[]			vObjects					= null;

	private		bool				IsPlayingAnimation			= false;

	////////////////////////////////////////////////////////////////////////

	private		bool				bUsed						= false;
	public	bool Used {
		get { return bUsed; }
	}

	private		Animator			pAnimator					= null;



	private void Start() {
		
		pAnimator		= transform.GetChild( 0 ).GetComponent<Animator>();
		

		pSpriteRender	= transform.GetChild( 0 ).GetComponent<SpriteRenderer>();
		

	}

	private	void	HighlightObjects() {

		if ( vObjects.Length == 0 ) return;

		foreach( Transform o in vObjects ) {

			if ( o == null ) continue;

			// DOORS
			UsableObject p	= o.GetComponent<UsableObject>();
			if ( p ) {
				if ( p is Door ) {

					Door pDoor = p as Door;
//					pDoor.IsHighLighted = true;

					continue;
				}
			}

			// PLATFORMS
			Platform p2		= o.GetComponent<Platform>();
			if ( p2 ) {
				if ( p2 is Platform ) {

					Platform pPlatform = p2 as Platform;
					pPlatform.IsHighLighted = true;

				}
			}
		}

	}

	private void Update() {
		
		if ( bIsHighLighted ) {
			pAnimator.enabled = false;
			if ( bUsed )
				pSpriteRender.sprite = pSpriteEnabledHighlighted;
			else
				pSpriteRender.sprite = pSpriteDisabledHighlighted;

			HighlightObjects();
		}
		else {
			pAnimator.enabled = true;
			if ( bUsed )
				pSpriteRender.sprite = pSpriteEnabled;
			else
				pSpriteRender.sprite = pSpriteDisabled;
		}

		bIsHighLighted = false;

	}

	public	void	SetUsed( bool b ) {
		this.bUsed = b;
	}

	public	override	void	OnReset() {

//		if ( IsPlayingAnimation ) return;

		print( "switcher OnReset" );

		if ( ( vObjects.Length > 0 ) && bUsed ) {
			foreach( Transform o in vObjects ) o.SendMessage( "OnReset" );
		}

		if ( pAnimator ) {
			pAnimator.Play( "OnReset" );
			GLOBALS.StageManager.AddActiveObject();
//			IsPlayingAnimation = true;
		}
		else {
			print( " Cannot reproduce OnReset animation" );
		}

	}


	public override void OnUse( Player User ) {

//		if ( IsPlayingAnimation ) return;

		print( "switcher OnUse" );

		// SWITCHERS
		if ( ( vObjects.Length > 0 ) ) {
			foreach( Transform o in vObjects ) o.SendMessage( "OnUse", User );
		}

		if ( bUsed ) {
			OnReset();
		}
		else {
			if ( pAnimator ) {
				pAnimator.Play( "OnUse" );
				GLOBALS.StageManager.AddActiveObject();
//				IsPlayingAnimation = true;
			}
			else {
				print( " Cannot reproduce OnUse animation" );
			}
		}
		
		
	}
}
