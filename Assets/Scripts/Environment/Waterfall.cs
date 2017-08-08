﻿using UnityEngine;

public class Waterfall : MonoBehaviour {

	[SerializeField][Range(0.0f, 1.0f )]
	private	float	fVolume	 = 1.0f;

	AudioSource		pAudioSource = null;

	// Use this for initialization
	void Start () {

		pAudioSource =  AudioManager.Play( "Waterfall", true );

		if ( pAudioSource != null )
			pAudioSource.volume = fVolume;
		
	}

	private void OnDestroy() {
		
		if ( pAudioSource != null ) pAudioSource.Stop();

	}

}
