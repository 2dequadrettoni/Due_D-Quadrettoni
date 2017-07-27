using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class ChooseLevels : MonoBehaviour {
    public GameObject MainMenu;
    public GameObject LevelCanvas;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Space) && LevelCanvas.activeInHierarchy)
        {
            MainMenu.SetActive(true);
            LevelCanvas.SetActive(false);
        }

    }

    public void Loadlevel(int index)
    {

        SceneManager.LoadScene(index);
    }


}
