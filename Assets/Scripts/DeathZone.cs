using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DeathZone : MonoBehaviour {

	private void OnTriggerEnter( Collider other ) {

		if ( GLOBALS.StageManager && GLOBALS.StageManager.IsPlaying && other.tag == "Player" ) {

			Player pPlayer = ( other.name == "Player1" ) ? GLOBALS.Player1 : GLOBALS.Player2;

			if ( pPlayer.Linked ) return;

			pPlayer.Destroy();
			if ( GLOBALS.UI ) GLOBALS.UI.ShowDeathMsg( other.name );

		}

	}

}
