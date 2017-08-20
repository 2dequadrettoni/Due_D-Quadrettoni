using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AudioFader : MonoBehaviour {

	public void FadeOut ( AudioSource audioSource, float FadeTime) {

		StartCoroutine( FadeOutCoroutine( audioSource, FadeTime ) );

	}

	public void FadeIn ( AudioSource audioSource, float FadeTime) {

		StartCoroutine( FadeInCoroutine( audioSource, FadeTime ) );

	}




	IEnumerator FadeOutCoroutine( AudioSource audioSource, float fFadeTime ) {

		float startVolume = audioSource.volume;

		float fFadingTimer = fFadeTime;

		while ( fFadingTimer > 0.0f ) {
			fFadingTimer -= Time.unscaledDeltaTime;
			audioSource.volume = startVolume * ( fFadingTimer / fFadeTime );
			yield return null;

		}

		audioSource.Stop();
		audioSource.volume = startVolume;

	}



	IEnumerator FadeInCoroutine( AudioSource audioSource, float fFadeTime ) {
 
		audioSource.volume = 0;
		audioSource.Play();

		float fFadingTimer = fFadeTime;

		while ( fFadingTimer > 0.0f ) {
			fFadingTimer -= Time.unscaledDeltaTime;
			audioSource.volume = 1.0f - ( fFadingTimer / fFadeTime );
			yield return null;

		} 
 
		audioSource.volume = 1.0f;

	}

}
