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

		AudioManager.StopAllMusics();
		AudioManager.FadeInMusic( "Cutscene_Start" );

		Cutscene_BigImage.sprite = pCurrentSprite = vCutsceneSprites[ iCurrentSpriteIndex ];

		Cutscene_BlackScreenImage.enabled = true;

		// fa scomparire l'immagine nera
		StartCoroutine( Cutscene_BlackImage_FadeOut( () => StartCoroutine( ShowCutsceneFrame() ) ) );

//		SceneManager.LoadScene( 0 );

	}


	private void EndCutsceneFrameCallback() {

		iCurrentSpriteIndex++;

		if ( iCurrentSpriteIndex < vCutsceneSprites.Count ) {

			Cutscene_BigImage.sprite = pCurrentSprite = vCutsceneSprites[ iCurrentSpriteIndex ];

			if ( bCutsceneDebug ) print( "EndCutsceneFrameCallback continue" );

			// fade black image out
			StartCoroutine( Cutscene_BlackImage_FadeOut( () => StartCoroutine( ShowCutsceneFrame() ) ) );

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

		StartCoroutine( Cutscene_BlackImage_FadeIn( () => EndCutsceneFrameCallback() ) );

	}



	IEnumerator Cutscene_BlackImage_FadeIn ( System.Action EndCallback ) {

		if ( bCutsceneDebug ) print( "Cutscene_BlackImage_FadeIn start" );

		yield return new WaitForEndOfFrame();

		Cutscene_BlackScreenImage.raycastTarget = true;
		Cutscene_BlackScreenImage.color = new Color(1, 1, 1, 0);

		while ( Cutscene_BlackScreenImage.color.a < 1 ) {

			float i = Cutscene_BlackScreenImage.color.a + ( Time.deltaTime * 3 );
			Cutscene_BlackScreenImage.color = new Color(1, 1, 1, i);
			yield return null;

		}

		Cutscene_BlackScreenImage.color = new Color(1, 1, 1, 1);

		if ( bCutsceneDebug ) print( "Cutscene_BlackImage_FadeIn processing end" );

		EndCallback();

	}





	IEnumerator Cutscene_BlackImage_FadeOut ( System.Action EndCallback ) {

		if ( bCutsceneDebug ) print( "Cutscene_BlackImage_FadeOut start" );

		yield return new WaitForEndOfFrame();

		Cutscene_BlackScreenImage.raycastTarget = true;
		Cutscene_BlackScreenImage.color = new Color(1, 1, 1, 1);

		while ( Cutscene_BlackScreenImage.color.a > 0 ) {
			float i = Cutscene_BlackScreenImage.color.a - ( Time.deltaTime * 3 );
			Cutscene_BlackScreenImage.color = new Color(1, 1, 1, i);
			yield return null;

		}

		if ( bCutsceneDebug ) print( "Cutscene_BlackImage_FadeOut processing end" );

		Cutscene_BlackScreenImage.color = new Color(1, 1, 1, 0);

		EndCallback();

	}





}


#pragma warning restore CS0414 // Var assigned but never used
#pragma warning restore CS0162 // Unreachable code detected