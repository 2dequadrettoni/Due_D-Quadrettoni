using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{

    // UI
    private     Transform		pTable              = null;

    private     GameObject[]	vIcons              = null;
    private     Image[,]		vPlayerIcons        = null;

    // Use this for initialization
    void Start()
    {

        // Canvas
		Transform pCanvasObject = transform.GetChild(0);

        // Table sprite
		pTable = pCanvasObject.GetChild(0);

		vIcons = new GameObject[3];
		for (int i = 1; i < 4; i++) {
		vIcons[i-1] = pCanvasObject.transform.GetChild(i).gameObject;
		}

		// Player action icons
		// Player 1
		vPlayerIcons = new Image[2, 10];
        for (int i = 0; i < 9; i++) {
			Image pImage = pTable.transform.GetChild(i).GetComponent<Image>();
			pImage.enabled = false;
			vPlayerIcons[0, i] = pImage;
		}
		// Player 2
		for (int i = 9; i < 18; i++)
		{
			Image pImage = pTable.transform.GetChild(i).GetComponent<Image>();
			pImage.enabled = false;
			vPlayerIcons[1, i - 9] = pImage;
		}

    }

	public	void	UpdateIcon( int PlayerID, ActionType ActionType, int CurrentStage ) {

		Debug.Log( "UpdateIcon " + PlayerID + ", " + (int)ActionType + ", " + CurrentStage );


		Image pImage = vIcons[(int)ActionType].GetComponent<Image>();
		vPlayerIcons[ PlayerID-1, CurrentStage ].enabled = true;
		vPlayerIcons[ PlayerID-1, CurrentStage ].sprite = pImage.sprite;

	}

}
