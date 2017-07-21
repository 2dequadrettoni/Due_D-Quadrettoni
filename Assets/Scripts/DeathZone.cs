using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DeathZone : MonoBehaviour {

	[SerializeField]
	private		Platform		pPlatform		= null;

	private		UI				pUI				= null;
	private		StageManager	pStageManager	= null;

	private		bool			bTriggered		= false;

	private void Start() {
		
		pUI = GameObject.Find( "UI" ).GetComponent<UI>();

		pStageManager = GameObject.Find( "GameManager" ).GetComponent<StageManager>();

	}

	private void OnTriggerStay( Collider other ) {

		if ( !pPlatform || bTriggered ) return;

		if ( pStageManager.IsPlaying && other.tag == "Player" ) {

			if ( pPlatform.HasPlayerInside ) return;

			print( "Player is dead" );

			pUI.ShowDeathMsg( other.name );
			bTriggered = true;

		}

	}
}
