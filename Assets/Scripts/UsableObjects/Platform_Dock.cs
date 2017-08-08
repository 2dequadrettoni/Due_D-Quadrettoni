using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#pragma warning disable CS0162 // Unreachable code detected

public class Platform_Dock : MonoBehaviour {


	public	bool	PlayerOn = false;


	private void OnTriggerStay( Collider other ) {
		
		if ( GLOBALS.StageManager.IsPlaying && other.tag == "Player" )
			PlayerOn = true;

	}

	private void OnTriggerExit( Collider other ) {

		if ( GLOBALS.StageManager.IsPlaying && other.tag == "Player" )
			PlayerOn = false;
		
	}


}

#pragma warning restore CS0162 // Unreachable code detected