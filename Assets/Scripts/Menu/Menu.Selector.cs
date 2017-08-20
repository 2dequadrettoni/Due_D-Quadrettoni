using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;

public partial class Menu {
	
	// selector che scompare al caricamento
	IEnumerator Selector_To_Loading_To_Game( int scene_index ) {

		StartCoroutine( Fader.Hide( TRANSITION_TIME ) );
		while ( !Fader.FadeCompleted ) yield return null;

		// LOADING SCREEN
		ShowCanvas( Menu_Canvas.LOADING );

		StartCoroutine( Fader.Show( TRANSITION_TIME ) );
		yield return new WaitForSecondsRealtime(Random.Range(4, 7));

		StartCoroutine( Fader.Hide( 0.8f ) );
		while ( !Fader.FadeCompleted ) yield return null;

		SceneManager.LoadScene( scene_index );

	}


	// selector che scompare per il menu
	IEnumerator Selector_To_Menu () {

		StartCoroutine( Fader.Hide( TRANSITION_TIME ) );
		while ( !Fader.FadeCompleted ) yield return null;

		// SHOW MENU
		SceneManager.LoadScene(0);

	}

}
