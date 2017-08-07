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

	public	GameObject			CutsceneScreen;

	public Image				Cutscene_BlackScreenImage;


	// Use this for initialization
	void Start () {

		Cutscene_BigImage = CutsceneScreen.transform.GetChild( 0 ).GetComponent<Image>();

		if ( vCutsceneSprites == null ) {

			vCutsceneSprites = new List<Sprite>();

			vCutsceneSprites.Add( Resources.Load<Sprite>("Cutscene/Finale/00") );
			vCutsceneSprites.Add( Resources.Load<Sprite>("Cutscene/Finale/01") );
			vCutsceneSprites.Add( Resources.Load<Sprite>("Cutscene/Finale/02") );
			vCutsceneSprites.Add( Resources.Load<Sprite>("Cutscene/Finale/03") );
			vCutsceneSprites.Add( Resources.Load<Sprite>("Cutscene/Finale/04") );

		}


		AudioManager.Initialize();
		AudioManager.PlayMusic("Cutscene_Finale");

		Cutscene_BigImage.sprite = pCurrentSprite = vCutsceneSprites[ iCurrentSpriteIndex ];

		Cutscene_BlackScreenImage.enabled = true;

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

		while ( fCurrentiTime < WAIT_TIME ) {

			fCurrentiTime += Time.unscaledDeltaTime;
			yield return null;

		}

		if ( bCutsceneDebug ) print( "ShowCutsceneFrame end" );

		bPlaying = false;

		StartCoroutine( Cutscene_BlackImage_FadeIn() );

	}



	IEnumerator Cutscene_BlackImage_FadeIn () {

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

		EndCutsceneFrameCallback();

	}





	IEnumerator Cutscene_BlackImage_FadeOut () {

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

		StartCoroutine( ShowCutsceneFrame() );

	}

}


#pragma warning restore CS0414 // Var assigned but never used
#pragma warning restore CS0162 // Unreachable code detected