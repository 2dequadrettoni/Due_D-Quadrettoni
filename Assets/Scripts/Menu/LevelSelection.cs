using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;


public class LevelSelection : MonoBehaviour {

    public	GameObject		MainMenuScreen;
    public	GameObject		LevelSelectionScreen;
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Space) && LevelSelectionScreen.activeInHierarchy)
        {
            MainMenuScreen.SetActive(true);
            LevelSelectionScreen.SetActive(false);
        }

    }



    public void Loadlevel(int index)
    {
        SceneManager.LoadScene(index);
    }




}
