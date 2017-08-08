using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AudioFader : MonoBehaviour {

	public void FadeOut ( AudioSource audioSource, float FadeTime) {
		StartCoroutine( FadeOutCoroutine( audioSource, FadeTime ) );
	}

	IEnumerator FadeOutCoroutine(AudioSource audioSource, float FadeTime) {

		float startVolume = audioSource.volume;
 
		while (audioSource.volume > 0)
		{
			audioSource.volume -= startVolume * Time.unscaledDeltaTime / FadeTime;
 
			yield return null;
		}
 
		audioSource.Stop();
		audioSource.volume = startVolume;

	}

	public void FadeIn ( AudioSource audioSource, float FadeTime) {
		StartCoroutine( FadeInCoroutine( audioSource, FadeTime ) );
	}
 
	IEnumerator FadeInCoroutine(AudioSource audioSource, float FadeTime) {
 
		audioSource.volume = 0;
		audioSource.Play();

		while (audioSource.volume < 1.0f)
		{
			audioSource.volume += Time.unscaledDeltaTime / FadeTime;
			yield return null;
		}
 
		audioSource.volume = 1f;

	}

}
