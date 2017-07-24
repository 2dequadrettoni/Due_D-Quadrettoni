﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Platform : MonoBehaviour {

	// LINK
	public		bool		HasPlayerInside		= false;
	public		bool		CanUnLink			= false;
	private		Player		pPlayer				= null;

	// MOVEMENT
	private		Vector3		vStartPosition		= Vector3.zero;
	private		Vector3		vEndPosition		= Vector3.zero;
	private		float		fInterpolant		= 0.0f;
	private		int			iDirection			= 1;
	public		float		fMoveSpeed			= 5.0f;

	public		bool		bActive				= false;


	// Use this for initialization
	void Start () {

		vStartPosition	= transform.GetChild( 0 ).position;
		vEndPosition	= transform.GetChild( 1 ).position;
		transform.position = vStartPosition;

	}
	

	// Update is called once per frame
	public void UpdatePosition() {

		if ( !bActive ) return;

		fInterpolant += ( Time.deltaTime * fMoveSpeed ) * iDirection;

		// PATROL POINT REACHED
		if ( ( fInterpolant < 0.0f ) || ( fInterpolant > 1.0f ) ) {
			bActive			= false;
			iDirection		*= -1;
			fInterpolant	= Mathf.Clamp01( fInterpolant );
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
		
		if ( !HasPlayerInside && other.tag == "Player" ) {

			Player pPlayer = other.GetComponent<Player>();
			if ( !pPlayer.Linked ) {
				pPlayer.Stop();
				pPlayer.Link( this );
				HasPlayerInside = true;
				this.pPlayer = pPlayer;
				print( "Player " + pPlayer.ID + " ENTER platform" );
			}
		}

	}

	private void OnTriggerExit( Collider other ) {
		
		if ( HasPlayerInside && other.tag == "Player" ) {

			Player pPlayer = ( other.name == "Player1" ) ? GLOBALS.Player1 : GLOBALS.Player2;
			if ( pPlayer.Linked ) {
				pPlayer.UnLink( this );
				HasPlayerInside = false;
				this.pPlayer = null;
				print( "Player " + pPlayer.ID + " EXIT platform" );
			}
		}

	}
}
