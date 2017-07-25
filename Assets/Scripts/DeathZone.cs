using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DeathZone : MonoBehaviour {

	[SerializeField]
	private		Platform[]		vPlatforms		= null;

	private		UI				pUI				= null;
	private		StageManager	pStageManager	= null;

	private		bool			bTriggered		= false;

	private void Start() {

		pUI = GLOBALS.UI;
		pStageManager = GLOBALS.StageManager;

	}

	private void OnTriggerStay( Collider other ) {

		if ( bTriggered ) return;

		if ( pStageManager && pStageManager.IsPlaying && other.tag == "Player" ) {

			foreach( Platform p in vPlatforms ) {
				if ( p && p.HasPlayerInside ) return;
			}

			print( "Player is dead" );

			if ( pUI ) pUI.ShowDeathMsg( other.name );
			bTriggered = true;

		}

	}
}
