using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
public class AudioFader : MonoBehaviour {


	public void FadeIn( AudioSource p ) {

		StartCoroutine( FadeInCoroutine( p ) );
	}


	public static IEnumerator FadeInCoroutine( AudioSource p ) {

		float startVolume = p.volume;

		while ( p.volume < 1.0f ) {
			p.volume += startVolume * Time.unscaledDeltaTime / 2;
			yield return null;
		}

		p.volume = 1.0f;
	}

	public void FadeOut( AudioSource p ) {

		StartCoroutine( FadeOutCoroutine( p ) );
	}


	IEnumerator FadeOutCoroutine( AudioSource p ) {
		while ( p.volume > 0.0f ) {
			p.volume -= Time.unscaledDeltaTime / 10;
			yield return null;
		}
		p.volume = 0.0f;
	}

}
*/

public static class AudioFadeScript {

	public static IEnumerator FadeOut(AudioSource audioSource, float FadeTime) {
		float startVolume = audioSource.volume;
 
		while (audioSource.volume > 0)
		{
			audioSource.volume -= startVolume * Time.deltaTime / FadeTime;
 
			yield return null;
		}
 
		audioSource.Stop();
		audioSource.volume = startVolume;
	}
 
	public static IEnumerator FadeIn(AudioSource audioSource, float FadeTime) {
		float startVolume = 0.2f;
 
		audioSource.volume = 0;
		audioSource.Play();
 
		while (audioSource.volume < 1.0f)
		{
			audioSource.volume += startVolume * Time.deltaTime / FadeTime;
 
			yield return null;
		}
 
		audioSource.volume = 1f;
	}

}
 