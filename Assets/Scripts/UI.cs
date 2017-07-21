using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


interface IUI {

	void	UpdateIcon( int PlayerID, ActionType ActionType, int CurrentStage );


	void	ShowDeathMsg( string PlayerName );

}


public class UI : MonoBehaviour, IUI {

    // UI
    private     Transform		pTable              = null;

    private     GameObject[]	vIcons              = null;
    private     Image[,]		vPlayerIcons        = null;

	private		bool			bShowDeathMSg		= false;
	private		string			sPlayerName = "";

	private		StageManager	pStageManager		= null;

    // Use this for initialization
    void Start() {

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

		pStageManager = GameObject.Find( "GameManager" ).GetComponent<StageManager>();

    }

	public	void	UpdateIcon( int PlayerID, ActionType ActionType, int CurrentStage ) {

		Image pImage = vIcons[(int)ActionType].GetComponent<Image>();
		vPlayerIcons[ PlayerID-1, CurrentStage ].enabled = true;
		vPlayerIcons[ PlayerID-1, CurrentStage ].sprite = pImage.sprite;

	}

	public	void	ShowDeathMsg( string PlayerName ) {
		sPlayerName = PlayerName;
		bShowDeathMSg = true;
		Debug.LogError( sPlayerName + "is dead!!" );
		
		if ( pStageManager.IsPlaying ) {
			pStageManager.Stop();
		}
	}







	Rect WindowRect = new Rect( Screen.width / 2f, Screen.height / 2f, 400f, 100f );
	void OnGUI() {
		
		if ( bShowDeathMSg ) {
			GUI.Window( 0, WindowRect, ShowGUI, "Error" );
		}

	}

	void ShowGUI( int windowID ) {

		GUI.Label( new Rect( 10f, 40f, 400f, 80f ), sPlayerName );

		if ( GUI.Button( new Rect( ( WindowRect.width / 6f ) - 50.0f, WindowRect.height / 1.5f, 100f, 20f ), "RESTART" ) ) {
			bShowDeathMSg = false;
			SceneManager.LoadScene( SceneManager.GetActiveScene().buildIndex );
			return;
		}

		if ( GUI.Button( new Rect( ( WindowRect.width / 2f ) + 50.0f, WindowRect.height / 1.5f, 100f, 20f ), "EXIT" ) ) {
            bShowDeathMSg = false;

 #if UNITY_EDITOR
			UnityEditor.EditorApplication.isPlaying = false;
#else
			Application.Quit();
#endif
			return;

		}
        
    }

}
