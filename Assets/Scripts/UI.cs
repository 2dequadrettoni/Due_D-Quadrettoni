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
	private		Sprite			AvatarPG1Enabled		= null;
	[SerializeField]
	private		Sprite			AvatarPG1Disabled		= null;

	[Space()][SerializeField]
	private		Sprite			AvatarPG2Enabled		= null;
	[SerializeField]
	private		Sprite			AvatarPG2Disabled		= null;

    // Avatars
	private		Image			AvatarPG1				= null;
	private		Image			AvatarPG2				= null;

	// Actions Tables
    private		Transform		pTablePG1				= null;
	private		Transform		pTablePG2				= null;

	// Cursors
	private		RectTransform	pCursorPG1				= null;
	private		RectTransform	pCursorPG2				= null;

	private		Vector3			pCursorPG1SpawnPos		= Vector3.zero;
	private		Vector3			pCursorPG2SpawnPos		= Vector3.zero;

	// Actions Icons
	private		GameObject[]	vIcons					= null;

	// Actions Slots
	private		Image[,]		vActionsSlots			= null;

	// PopUp Windows
	private		bool			bShowDeathMSg			= false;
	private		bool			bShowUnreachableMsg		= false;
	private		bool			bShowLvlCompletedMsg	= false;
	private		string			sPlayerName = "";

	private		StageManager	pStageManager			= null;

    private RectTransform backGrid;
    private Vector3 originalScale;

    Transform pCanvasObject;


    // Use this for initialization
    private void Start() {

        // Canvas
        pCanvasObject = transform.GetChild( 0 );

        backGrid = pCanvasObject.GetChild(12).transform as RectTransform;
        originalScale = backGrid.localScale;
        backGrid.localScale = Vector3.zero;

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

		// Cursor spawn position
		pCursorPG1.localPosition = pCursorPG1SpawnPos = vActionsSlots[ 0, 0 ].rectTransform.localPosition + ( Vector3.up * vActionsSlots[ 0, 0 ].rectTransform.rect.height/2 );

		pCursorPG2.localPosition = pCursorPG2SpawnPos = vActionsSlots[ 1, 0 ].rectTransform.localPosition + ( Vector3.down * vActionsSlots[ 1, 0 ].rectTransform.rect.height/2 );


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

    public void ActivatePlayBtn()
    {
        pCanvasObject.GetChild(9).GetChild(1).GetComponent<Animator>().SetBool("isPlay", true);

    }


	public	void	UpdateAction( int PlayerID, ActionType ActionType, int CurrentStage ) {

		Image pImage = vIcons[ (int)ActionType ].GetComponent<Image>();
		vActionsSlots[ PlayerID-1, CurrentStage ].enabled = true;
		vActionsSlots[ PlayerID-1, CurrentStage ].sprite = pImage.sprite;

	}


	public	void	CursorsNextStep( int iStage ) {

        backGrid.localScale = new Vector3(originalScale.x * iStage, 1, 1);

        pCursorPG1.localPosition = new Vector3(
            vActionsSlots[0, iStage].rectTransform.localPosition.x,
			pCursorPG1.localPosition.y,
			pCursorPG1.localPosition.z
		);


        pCursorPG2.localPosition = new Vector3 (
			vActionsSlots[ 1, iStage ].rectTransform.localPosition.x,
			pCursorPG2.localPosition.y,
			pCursorPG2.localPosition.z
		);

	}


	public	void	PrepareForPlay() {
		pCursorPG1.localPosition = pCursorPG1SpawnPos;
		pCursorPG2.localPosition = pCursorPG2SpawnPos;
	}

	public	void	PlaySequence( int iStage, float fInterpolant ) {

        backGrid.localScale = new Vector3(originalScale.x * iStage, 1 , 1);

        pCursorPG1.localPosition = new Vector3 (
			Mathf.Lerp( pCursorPG1.localPosition.x, vActionsSlots[ 0, iStage ].rectTransform.localPosition.x, fInterpolant ),
			pCursorPG1.localPosition.y,
			pCursorPG1.localPosition.z
		);

		pCursorPG2.localPosition = new Vector3 (
			Mathf.Lerp( pCursorPG2.localPosition.x, vActionsSlots[ 1, iStage ].rectTransform.localPosition.x, fInterpolant ),
			pCursorPG2.localPosition.y,
			pCursorPG2.localPosition.z
		);

	}

	

	public	void	ShowLvlCompletedMsg() {

		bShowLvlCompletedMsg = true;
		if ( pStageManager.IsPlaying ) {
			pStageManager.Stop();
		}

	}

	
	public	void	ShowUnreachableMsg( string PlayerName ) {

		sPlayerName = PlayerName;
		bShowUnreachableMsg = true;
		if ( pStageManager.IsPlaying ) {
			pStageManager.Stop();
		}

	}

	public	void	ShowDeathMsg( string PlayerName ) {

		sPlayerName = PlayerName;
		bShowDeathMSg = true;
		if ( pStageManager.IsPlaying ) {
			pStageManager.Stop();
		}

	}



	////////////////////////////////////////////////////////////////////////////////////
	////////////////////////////////////////////////////////////////////////////////////
	////////////////////////////////////////////////////////////////////////////////////
	// // // //					WINDOWS

	// Default window rect
	static Rect	DefaultWindow		= new Rect( Screen.width / 2f - 200, Screen.height / 2f - 50, 400, 100 );
	
	Rect FailWindowRect				= new Rect( DefaultWindow );
	Rect LvlCompletedWindowRect		= new Rect( DefaultWindow );
	void OnGUI() {
		
		if ( bShowDeathMSg ) {
			GUI.Window( 0, FailWindowRect, ShowDeathGUI, "Player is dead!!" );
		}

		if ( bShowUnreachableMsg ) {
			GUI.Window( 0, FailWindowRect, ShowUnreachableGUI, sPlayerName + " is not able to reach his destination!!" );
		}

		if ( bShowLvlCompletedMsg ) {
			GUI.Window( 0, LvlCompletedWindowRect, ShowLvlCompletedGUI, "Level Completed" );
		}

	}

	// Death Window
	void ShowDeathGUI( int windowID ) {

		if ( GUI.Button( new Rect( ( FailWindowRect.width / 6f ) - 50.0f, FailWindowRect.height / 1.5f, 100f, 20f ), "RESTART" ) ) {
			bShowDeathMSg = false;
			SceneManager.LoadScene ( SceneManager.GetActiveScene().name );
			return;
		}

		if ( GUI.Button( new Rect( ( FailWindowRect.width / 2f ) + 50.0f, FailWindowRect.height / 1.5f, 100f, 20f ), "EXIT" ) ) {
            bShowDeathMSg = false;
 #if UNITY_EDITOR
			UnityEditor.EditorApplication.isPlaying = false;
#else
			Application.Quit();
#endif
		}
        
    }

	// Unreachable destination Window
	void ShowUnreachableGUI( int windowID ) {

		if ( GUI.Button( new Rect( ( FailWindowRect.width / 6f ) - 50.0f, FailWindowRect.height / 1.5f, 100f, 20f ), "RESTART" ) ) {
			bShowUnreachableMsg = false;
			SceneManager.LoadScene ( SceneManager.GetActiveScene().name );
			return;
		}

		if ( GUI.Button( new Rect( ( FailWindowRect.width / 2f ) + 50.0f, FailWindowRect.height / 1.5f, 100f, 20f ), "EXIT" ) ) {
            bShowUnreachableMsg = false;
 #if UNITY_EDITOR
			UnityEditor.EditorApplication.isPlaying = false;
#else
			Application.Quit();
#endif
		}
        
    }

	// LevelCompleted Window
	void ShowLvlCompletedGUI( int windowID ) {

		if ( SceneManager.sceneCount > ( SceneManager.GetActiveScene().buildIndex + 1 ) ) {

			if ( GUI.Button( new Rect( ( LvlCompletedWindowRect.width / 6f ) - 50.0f, LvlCompletedWindowRect.height / 1.5f, 100f, 20f ), "NEXT LEVEL" ) ) {
				SceneManager.LoadScene( SceneManager.GetActiveScene().buildIndex + 1 );
				bShowLvlCompletedMsg = false;
				return;
			}
		}

		if ( GUI.Button( new Rect( ( LvlCompletedWindowRect.width / 2f ) + 50.0f, LvlCompletedWindowRect.height / 1.5f, 100f, 20f ), "EXIT" ) ) {
			bShowLvlCompletedMsg = false;
#if UNITY_EDITOR
			UnityEditor.EditorApplication.isPlaying = false;
#else
			Application.Quit();
#endif
		}
        
    }


}