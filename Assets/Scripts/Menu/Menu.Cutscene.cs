using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

using UnityEngine.SceneManagement;

#pragma warning disable CS0162 // Unreachable code detected
#pragma warning disable CS0414 // Var assigned but never used


public partial class Menu {
	
	const		bool			bCutsceneDebug = false;

	const		float			WAIT_TIME = 3.0f;

	private		Sprite			pCurrentSprite	= null;

	private		int				iCurrentSpriteIndex = 0;

	private static		List<Sprite>	vCutsceneSprites = null;

	private		bool			bPlaying = false;


	private	void	StartCutscene() {

		AudioManager.StopAllMusics( false );
		AudioManager.FadeInMusic( "Cutscene_Start", 6 );

		Cutscene_BigImage.sprite = pCurrentSprite = vCutsceneSprites[ iCurrentSpriteIndex ];

		StartCoroutine( Cutscene_Show( () => StartCoroutine( ShowCutsceneFrame() ) ) );

	}


	private void EndCutsceneFrameCallback() {

		iCurrentSpriteIndex++;

		if ( iCurrentSpriteIndex < vCutsceneSprites.Count ) {

			Cutscene_BigImage.sprite = pCurrentSprite = vCutsceneSprites[ iCurrentSpriteIndex ];

			if ( bCutsceneDebug ) print( "EndCutsceneFrameCallback continue" );

			StartCoroutine( Cutscene_Show( () => StartCoroutine( ShowCutsceneFrame() ) ) );

			return;
		}

		if ( bCutsceneDebug ) print( "EndCutsceneFrameCallback finished" );

		StopAllCoroutines();
		SceneManager.LoadScene( iLevelToLoad );

	}


	IEnumerator ShowCutsceneFrame() {

		if ( bCutsceneDebug ) print( "ShowCutsceneFrame start" );

		bPlaying = true;

		float fCurrentiTime = 0.0f;

		while ( fCurrentiTime < WAIT_TIME ) {

			fCurrentiTime += Time.unscaledDeltaTime;
			yield return null;

		}

		if ( bCutsceneDebug ) print( "ShowCutsceneFrame end" );

		bPlaying = false;

		StartCoroutine( Cutscene_Hide( () => EndCutsceneFrameCallback() ) );

	}



	IEnumerator Cutscene_Hide ( System.Action EndCallback ) {

		StartCoroutine( Fader.Hide( 2, EndCallback ) );
		while ( !Fader.FadeCompleted ) yield return null;

	}





	IEnumerator Cutscene_Show ( System.Action EndCallback ) {

		StartCoroutine( Fader.Show( 2, EndCallback ) );
		while ( !Fader.FadeCompleted ) yield return null;

	}


}


#pragma warning restore CS0414 // Var assigned but never used
#pragma warning restore CS0162 // Unreachable code detected