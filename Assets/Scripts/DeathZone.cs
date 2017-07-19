using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DeathZone : MonoBehaviour {

	private		Platform pPlatform = null;

	private void OnTriggerStay( Collider other ) {
		
		if ( !pPlatform ) return;

		if ( other.tag == "Player" ) {
			if ( pPlatform.HasPlayerInside ) return;

			print( "Player is dead" );

		}

	}

}
