using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Menu {


	const		float			LOGO_WAIT_TIME = 3.0f;

	private static		AudioSource		pSource;


	public	void	ShowLogo() {

		AudioClip vSoundsClip = Resources.Load<AudioClip>( "Audio/Sounds/Incidente" );

		GameObject o = new GameObject( "bumbaudio" );

		pSource = o.AddComponent<AudioSource>();
		pSource.clip = vSoundsClip;
		pSource.volume = 1.0f;
		pSource.pitch = 1.0f;
		pSource.loop = false;

		pSource.Play();

		StartCoroutine( ShowLogoCoroutine() );

	}



	IEnumerator ShowLogoCoroutine() {

		// start loading sounds
		AudioManager.LoadResources();

		// LOGO FADE IN

		Logo_BlackScreenImage.raycastTarget = true;
		Logo_BlackScreenImage.color = new Color(1, 1, 1, 1);

		yield return new WaitForSecondsRealtime( 2 );

		while ( Logo_BlackScreenImage.color.a > 0 ) {
			float i = Logo_BlackScreenImage.color.a - ( Time.deltaTime * 2 );
			Logo_BlackScreenImage.color = new Color(1, 1, 1, i);
			yield return null;

		}

		Logo_BlackScreenImage.color = new Color(1, 1, 1, 0);


		// LOGO STAY DURING SOUND
		while( pSource.isPlaying ) {

			yield return null;
		}


		// LOGO FADE OUT

		while ( Logo_BlackScreenImage.color.a < 1 ) {
			float i = Logo_BlackScreenImage.color.a + ( Time.deltaTime * 2 );
			Logo_BlackScreenImage.color = new Color(1, 1, 1, i);
			yield return null;

		}

		Logo_BlackScreenImage.color = new Color(1, 1, 1, 1);

		MainMenuScreen.SetActive( true );
		LevelSelectionScreen.SetActive( false );
		LoadingScreen.SetActive( false );
		CutsceneScreen.SetActive( false );
		Logo_BlackScreen.SetActive( false );

		AudioManager.FadeInMusic( "Menu_Theme" );

		StartCoroutine( Menu_BlackImage_FadeOut() );

	}




}
