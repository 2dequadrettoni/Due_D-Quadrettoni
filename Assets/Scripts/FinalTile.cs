using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalTile : MonoBehaviour {

	[HideInInspector]
	public	int	iDesiredPlayerID = 0;

	[HideInInspector]
	public	bool	IsInside = false;

	private void OnTriggerEnter( Collider other ) {

		if ( IsInside ) return;

		if ( GLOBALS.StageManager && GLOBALS.StageManager.IsPlaying && other.tag == "Player" ) {

			Player pPlayer = ( other.name == "Player1" ) ? GLOBALS.Player1 : GLOBALS.Player2;

			if ( pPlayer.ID == iDesiredPlayerID ) IsInside = true;


		}
	}

	private void OnTriggerExit( Collider other ) {
		
		IsInside = false;

	}


}
