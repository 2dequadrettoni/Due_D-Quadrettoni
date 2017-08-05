
using System; // [Serializable]
using System.Collections.Generic;

using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    const int LEVEL_COUNT = 18;

    List< Transform> vButtons = new List<Transform>();

    private void Start()
    {

		// Get last saved level and if is invalid or the last set at zero
		int SavedLevel = SaveLoad.GetSavedlevel();
		if ( SavedLevel == -1 || SavedLevel >= LEVEL_COUNT ) {
			SaveLoad.SaveLevel( SavedLevel = 0 );
		}

		// Recupera i bottoni
        for (int i = 0; i < LEVEL_COUNT; i++)
        {

            vButtons.Add( transform.GetChild(i + 20 ) );
			vButtons[i].GetComponent<Button>().interactable = false;

        }

		// decide se il bottone è abilitato oppure no in base al fatto che il bottone corrente
		// si riferisca ad un numero di livello inferiore a quello raggiunto
        for (int i = 0; i < SavedLevel+1; i++)
        {
         
			vButtons[i].GetComponent<Button>().interactable = true;
			//	qui settare immagine del lucchetto aperto

        }

		// first level always avaiable
        vButtons[0].GetComponent<Button>().interactable = true;


    }


}