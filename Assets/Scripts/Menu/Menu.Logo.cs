using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Menu {


	const float			LOGO_WAIT_TIME = 3.0f;

	private static		AudioSource		pLogoSource;	


	public	void	ShowLogo() {

		AudioClip vSoundsClip = Resources.Load<AudioClip>( "Audio/Sounds/Incidente" );

		GameObject o = new GameObject( "dunpaudio" );

		pLogoSource = o.AddComponent<AudioSource>();
		pLogoSource.clip = vSoundsClip;
		pLogoSource.volume = 1.0f;
		pLogoSource.pitch = 1.0f;
		pLogoSource.loop = false;

		GLOBALS.Logger = new Logger();

		Fader.Initialize();
		Fader.Show();
		AudioManager.Initialize();

		StartCoroutine( ShowLogoCoroutine() );

	}



	IEnumerator ShowLogoCoroutine() {

		yield return new WaitForSecondsRealtime( 1.0f );

		// start loading sounds
		StartCoroutine( AudioManager.LoadResources() );

		pLogoSource.Play();
		yield return new WaitForSecondsRealtime( 6.5f );
		Logo_Animator.Play( "Animate" );
		
		// LOGO STAY DURING SOUND
		while( pLogoSource.isPlaying ) yield return null;

		Destroy( pLogoSource.gameObject );

		// LOGO FADE OUT
		StartCoroutine( Fader.Hide( TRANSITION_TIME ) );
		while ( !Fader.FadeCompleted ) yield return null;

		// WAIT FOR AUDIO RESOURCES LOAD COMPLETE
		while ( AudioManager.Loaded == false ) yield return null;

		// FINALLY GO TO MENU CANVAS
		ShowCanvas( Menu_Canvas.MAINMENU );
		AudioManager.FadeInMusic( "Menu_Theme" );

		// SHOW IT, ENABLE IT AT END
		StartCoroutine( Fader.Show( TRANSITION_TIME, () => bMenuEnabled = true ) );
		while ( !Fader.FadeCompleted ) yield return null;

	}




}
