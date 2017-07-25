using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


interface IUI {

	void	SelectPlayer( int ID );

	void	UpdateAction( int PlayerID, ActionType ActionType, int CurrentStage );

	void	ShowDeathMsg( string PlayerName );

}


public class UI : MonoBehaviour, IUI {

	[SerializeField]
	private		Sprite			AvatarPG1Enabled	= null;
	[SerializeField]
	private		Sprite			AvatarPG1Disabled	= null;

	[Space()][SerializeField]
	private		Sprite			AvatarPG2Enabled	= null;
	[SerializeField]
	private		Sprite			AvatarPG2Disabled	= null;

    // Avatars
	private		Image			AvatarPG1			= null;
	private		Image			AvatarPG2			= null;

	// Actions Tables
    private		Transform		pTablePG1			= null;
	private		Transform		pTablePG2			= null;

	// Cursors
	private		RectTransform	pCursorPG1			= null;
	private		RectTransform	pCursorPG2			= null;

	private		Vector3			pCursorPG1SpawnPos	= Vector3.zero;
	private		Vector3			pCursorPG2SpawnPos	= Vector3.zero;

	// Actions Icons
	private		GameObject[]	vIcons              = null;

	// Actions Slots
	private		Image[,]		vActionsSlots        = null;

	private		bool			bShowDeathMSg		= false;
	private		string			sPlayerName = "";

	private		StageManager	pStageManager		= null;

    // Use this for initialization
    void Start() {

        // Canvas
		Transform pCanvasObject = transform.GetChild( 0 );


		// Avatars
		AvatarPG1 = pCanvasObject.GetChild( 0 ).GetComponent<Image>();
		AvatarPG2 = pCanvasObject.GetChild( 1 ).GetComponent<Image>();


        // Actions Tables
		pTablePG1 = pCanvasObject.GetChild( 2 );
		pTablePG2 = pCanvasObject.GetChild( 3 );

		// Cursors
		pCursorPG1 = pTablePG1.GetChild( 10 ).transform as RectTransform;
		pCursorPG2 = pTablePG2.GetChild( 10 ).transform as RectTransform;




		// Actions Icons
		vIcons = new GameObject[3];
		vIcons[ (int) ActionType.MOVE ] = pCanvasObject.GetChild( 4 ).gameObject;
		vIcons[ (int) ActionType.USE  ] = pCanvasObject.GetChild( 5 ).gameObject;
		vIcons[ (int) ActionType.WAIT ] = pCanvasObject.GetChild( 6 ).gameObject;


		// Actions Slots
		// Player 1
		vActionsSlots = new Image[ 2, 10 ];
        for (int i = 0; i < 10; i++) {
			Image pImage = pTablePG1.transform.GetChild(i).GetComponent<Image>();
			pImage.enabled = false;
			vActionsSlots[0, i] = pImage;
		}
		// Player 2
		for (int i = 0; i < 10; i++) {
			Image pImage = pTablePG2.transform.GetChild(i).GetComponent<Image>();
			pImage.enabled = false;
			vActionsSlots[1, i] = pImage;
		}

		pStageManager = GLOBALS.StageManager;


//		vActionsSlots[ 0, 0 ].rectTransform.rect.height
//		vActionsSlots[ 0, 0 ].rectTransform.localPosition

		
		pCursorPG1.localPosition = pCursorPG1SpawnPos = vActionsSlots[ 0, 0 ].rectTransform.localPosition + ( Vector3.up * vActionsSlots[ 0, 0 ].rectTransform.rect.height/2 );

		pCursorPG2.localPosition = pCursorPG2SpawnPos = vActionsSlots[ 1, 0 ].rectTransform.localPosition + ( Vector3.up * vActionsSlots[ 1, 0 ].rectTransform.rect.height/2 );


//
//		vActionsSlots[ 1, 0 ].rectTransform.rect.height

//		pCursorPG1SpawnPos = pCursorPG1.localPosition;
//		pCursorPG2SpawnPos = pCursorPG2.localPosition;


    }


	public	void	SelectPlayer( int ID ) {

		// Update avatars
		if ( ID == 1 ) {
			AvatarPG1.sprite = AvatarPG1Enabled;
			AvatarPG2.sprite = AvatarPG2Disabled;
		}
		else {
			AvatarPG1.sprite = AvatarPG1Disabled;
			AvatarPG2.sprite = AvatarPG2Enabled;
		}

	}


	public	void	UpdateAction( int PlayerID, ActionType ActionType, int CurrentStage ) {

		Image pImage = vIcons[ (int)ActionType ].GetComponent<Image>();
		vActionsSlots[ PlayerID-1, CurrentStage ].enabled = true;
		vActionsSlots[ PlayerID-1, CurrentStage ].sprite = pImage.sprite;

	}


	public	void	CursorsNextStep( int iStage ) {

		pCursorPG1.localPosition = new Vector3 (
			vActionsSlots[ 0, iStage ].rectTransform.anchoredPosition.x,
			pCursorPG1.localPosition.y,
			pCursorPG1.localPosition.z
		);

		pCursorPG2.localPosition = new Vector3 (
			vActionsSlots[ 1, iStage ].rectTransform.anchoredPosition.x,
			pCursorPG2.localPosition.y,
			pCursorPG2.localPosition.z
		);


//		pCursorPG1.localPosition += Vector3.right * 26.0f;
//		pCursorPG2.localPosition += Vector3.left  * 26.0f;
	}


	public	void	PrepareForPlay() {
		pCursorPG1.localPosition = pCursorPG1SpawnPos;
		pCursorPG2.localPosition = pCursorPG2SpawnPos;
	}

	public	void	PlaySequence( int iStage, float fInterpolant ) {

		pCursorPG1.localPosition = new Vector3 (
			Mathf.Lerp( pCursorPG1.localPosition.x, vActionsSlots[ 0, iStage ].rectTransform.anchoredPosition.x, fInterpolant ),
			pCursorPG1.localPosition.y,
			pCursorPG1.localPosition.z
		);

		pCursorPG2.localPosition = new Vector3 (
			Mathf.Lerp( pCursorPG2.localPosition.x, vActionsSlots[ 1, iStage ].rectTransform.anchoredPosition.x, fInterpolant ),
			pCursorPG2.localPosition.y,
			pCursorPG2.localPosition.z
		);

	}

	




	public	void	ShowDeathMsg( string PlayerName ) {
		sPlayerName = PlayerName;
		bShowDeathMSg = true;
		Debug.LogError( sPlayerName + "is dead!!" );
		
		if ( pStageManager.IsPlaying ) {
			pStageManager.Stop();
		}
	}







	Rect WindowRect = new Rect( Screen.width / 2f-200f, Screen.height / 2f-200f, 400f, 100f );
	void OnGUI() {
		
		if ( bShowDeathMSg ) {
			GUI.Window( 0, WindowRect, ShowGUI, "Player is dead!!" );
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
