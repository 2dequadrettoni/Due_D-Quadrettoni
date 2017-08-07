using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;

public partial class Menu {
	
	// selector che scompare al caricamento
	IEnumerator Selector_BlackImage_FadeIn( int scene_index )
	{

		yield return new WaitForEndOfFrame();

		while (LevelSelection_BlackScreenImage.color.a < 1)
		{

			float i = LevelSelection_BlackScreenImage.color.a + (Time.deltaTime * 3);
			LevelSelection_BlackScreenImage.color = new Color(1, 1, 1, i);
			yield return null;

		}

		LevelSelection_BlackScreenImage.color = new Color(1, 1, 1, 1);

		MainMenuScreen.SetActive(false);
		LevelSelectionScreen.SetActive(false);
		LoadingScreen.SetActive(true);


		// LOADING SCREEN


		Loading_BlackScreenImage.enabled = true;
		Menu_BlackScreenImage.raycastTarget = false;

		yield return new WaitForEndOfFrame();

		while (Loading_BlackScreenImage.color.a > 0)
		{

			float i = Loading_BlackScreenImage.color.a - ( Time.deltaTime * 3 );
			Loading_BlackScreenImage.color = new Color(1, 1, 1, i);
			yield return null;

		}

		Loading_BlackScreenImage.color = new Color(1, 1, 1, 0);


		yield return new WaitForSecondsRealtime(Random.Range(4, 7));




		SceneManager.LoadScene(scene_index);

	}




	// selector che compare
	IEnumerator Selector_BlackImage_FadeOut () {


		yield return new WaitForEndOfFrame();

		Menu_BlackScreenImage.enabled = true;
		LevelSelection_BlackScreenImage.raycastTarget = true;

		Menu_BlackScreenImage.color = new Color(1, 1, 1, 0);

		while ( Menu_BlackScreenImage.color.a < 1 ) {
			
			float i = Menu_BlackScreenImage.color.a + ( Time.deltaTime * 3 );
			Menu_BlackScreenImage.color = new Color(1, 1, 1, i);
			yield return null;

		}

		Menu_BlackScreenImage.color = new Color(1, 1, 1, 1);

//		Menu_BlackScreenImage.enabled = false;
		LevelSelection_BlackScreenImage.raycastTarget = false;


		// SHOE SELECTOR


		LevelSelection_BlackScreenImage.enabled = true;
		LevelSelection_BlackScreenImage.raycastTarget = true;

		yield return new WaitForEndOfFrame();

		MainMenuScreen.SetActive(false);
		LevelSelectionScreen.SetActive(true);
		LoadingScreen.SetActive( false );

		LevelSelection_BlackScreenImage.color = new Color(1, 1, 1, 1);

		while ( LevelSelection_BlackScreenImage.color.a > 0 ) {
			
			float i = LevelSelection_BlackScreenImage.color.a - ( Time.deltaTime * 3 );
			LevelSelection_BlackScreenImage.color = new Color(1, 1, 1, i);
			yield return null;

		}

		LevelSelection_BlackScreenImage.color = new Color(1, 1, 1, 0);

		LevelSelection_BlackScreenImage.raycastTarget = false;
		LevelSelection_BlackScreenImage.enabled = false;

	}





	// selector che scompare per il menu
	IEnumerator Selector_BlackImage_FadeOutToMenu () {

		LevelSelection_BlackScreenImage.enabled = true;
		LevelSelection_BlackScreenImage.raycastTarget = true;

		yield return new WaitForEndOfFrame();

		while ( LevelSelection_BlackScreenImage.color.a < 1 ) {
			
			float i = LevelSelection_BlackScreenImage.color.a + ( Time.deltaTime * 3 );
			LevelSelection_BlackScreenImage.color = new Color(1, 1, 1, i);
			yield return null;

		}

		LevelSelection_BlackScreenImage.color = new Color(1, 1, 1, 1);



		// SHOW MENU
		SceneManager.LoadScene(0);

	}

}
