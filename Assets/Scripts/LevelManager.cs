
using System; // [Serializable]
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    const int LEVEL_COUNT = 18;

    Transform[] figli;

    bool[] vLevels;

    private void Start()
    {
        vLevels = new bool[LEVEL_COUNT];

        figli = new Transform[LEVEL_COUNT];

        for (int i = 0; i < vLevels.Length; i++)
        {

            figli[i] = transform.GetChild(i + 20);

        }

        for (int i = 0; i < SaveLoad.GetSavedlevel(); i++)
        {

            vLevels[i] = true;

        }

        if (vLevels != null && vLevels.Length > 0)
        {
            for (int i = 0; i < vLevels.Length; i++)
            {

                if (vLevels[i])
                {
                    figli[i].GetComponent<Button>().interactable = true;
                }
                else
                {
                    figli[i].GetComponent<Button>().interactable = false;
                    //	qui settare immagine del lucchetto
                }

            }

            vLevels[0] = true;
            figli[0].GetComponent<Button>().interactable = true;
        }


    }


}