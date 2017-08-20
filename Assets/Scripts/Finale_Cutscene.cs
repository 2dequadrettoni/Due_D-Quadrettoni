using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

#pragma warning disable CS0162 // Unreachable code detected
#pragma warning disable CS0414 // Var assigned but never use

public class Finale_Cutscene : MonoBehaviour {

	const		bool			bCutsceneDebug = false;

	const		float			WAIT_TIME = 3.0f;

	private		Sprite			pCurrentSprite	= null;

	private		int				iCurrentSpriteIndex = 0;

	private static		List<Sprite>	vCutsceneSprites = null;

	private		bool			bPlaying = false;

	private	Image				Cutscene_BigImage;

	public	GameObject			Canvas_Cutscene;


	// Use this for initialization
	void Start () {

		Cutscene_BigImage = Canvas_Cutscene.transform.GetChild( 0 ).GetComponent<Image>();

		if ( vCutsceneSprites == null ) {

			vCutsceneSprites = new List<Sprite>();

			vCutsceneSprites.Add( Resources.Load<Sprite>("Cutscene/Finale/00") );
			vCutsceneSprites.Add( Resources.Load<Sprite>("Cutscene/Finale/01") );
			vCutsceneSprites.Add( Resources.Load<Sprite>("Cutscene/Finale/02") );
			vCutsceneSprites.Add( Resources.Load<Sprite>("Cutscene/Finale/03") );
			vCutsceneSprites.Add( Resources.Load<Sprite>("Cutscene/Finale/04") );
			vCutsceneSprites.Add( Resources.Load<Sprite>("EndCredits") );

		}

		AudioManager.FadeInMusic( "Cutscene_Finale", 0.0f );

		Menu.bGameStarted = true;

		Cutscene_BigImage.sprite = pCurrentSprite = vCutsceneSprites[ iCurrentSpriteIndex ];


		// fa scomparire l'immagine nera
		StartCoroutine( Cutscene_BlackImage_FadeOut() );

	}
	
	private void EndCutsceneFrameCallback() {

		iCurrentSpriteIndex++;

		if ( iCurrentSpriteIndex < vCutsceneSprites.Count ) {

			Cutscene_BigImage.sprite = pCurrentSprite = vCutsceneSprites[ iCurrentSpriteIndex ];

			if ( bCutsceneDebug ) print( "EndCutsceneFrameCallback continue" );

			// fade black image out
			StartCoroutine( Cutscene_BlackImage_FadeOut() );

			return;
		}

		if ( bCutsceneDebug ) print( "EndCutsceneFrameCallback finished" );

		StopAllCoroutines();
		AudioManager.StopAllMusics();
		AudioManager.StopAllSounds();
		SceneManager.LoadScene( 0 );

	}


	IEnumerator ShowCutsceneFrame() {

		if ( bCutsceneDebug ) print( "ShowCutsceneFrame start" );


		bPlaying = true;

		float fCurrentiTime = 0.0f;

		while ( fCurrentiTime < WAIT_TIME + ( ( iCurrentSpriteIndex == vCutsceneSprites.Count - 1 ) ? 10 : 0 ) ) {

			fCurrentiTime += Time.unscaledDeltaTime;
			yield return null;

		}

		if ( bCutsceneDebug ) print( "ShowCutsceneFrame end" );

		bPlaying = false;

		StartCoroutine( Cutscene_BlackImage_FadeIn() );

	}



	IEnumerator Cutscene_BlackImage_FadeIn () {

		StartCoroutine( Fader.Hide( 2, () => EndCutsceneFrameCallback() ) );
		while ( !Fader.FadeCompleted ) yield return null;

	}





	IEnumerator Cutscene_BlackImage_FadeOut () {
		
		StartCoroutine( Fader.Hide( 2, () => StartCoroutine( ShowCutsceneFrame() ) ) );
		while ( !Fader.FadeCompleted ) yield return null;

	}

}


#pragma warning restore CS0414 // Var assigned but never used
#pragma warning restore CS0162 // Unreachable code detected