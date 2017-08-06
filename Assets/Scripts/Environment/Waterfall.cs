using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waterfall : MonoBehaviour {

	private	bool	bIsOK = false;
	[SerializeField][Range(0.0f, 1.0f )]
	private	float	fVolume	 = 1.0f;

	AudioSource		pAudioSource = null;

	// Use this for initialization
	void Start () {

		if ( GLOBALS.AudioManager == null ) return;

		pAudioSource =  GLOBALS.AudioManager.Play( "Waterfall", true );

		if ( pAudioSource )
			pAudioSource.volume = fVolume;
		
	}

	private void OnDestroy() {
		
		if ( pAudioSource != null ) pAudioSource.Stop();

	}

}
