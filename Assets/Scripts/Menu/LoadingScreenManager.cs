using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingScreenManager : MonoBehaviour {
	
	public GameObject loadingScreenObj;
	AsyncOperation async;



	public void LoadScreen(int i){
	
		StartCoroutine (LoadingScreen(i));
	}

	IEnumerator LoadingScreen(int index){

		loadingScreenObj.SetActive (true);
		async = SceneManager.LoadSceneAsync(index);
		async.allowSceneActivation = false;

		while (async.isDone == false) {

			if (async.progress == 0.9f) {
				async.allowSceneActivation = true;
			}

			yield return null;	
		}

	}
}

