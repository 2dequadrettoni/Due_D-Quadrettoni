using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class UI : MonoBehaviour {

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

    // UI selected image
    public Sprite orangeNexTurn, defaultNexTurn, orangePause, defaultPause, orangeRestart, defaultRestart;

	// PopUp Windows
	////////////////////////////////////////////////////////////////////
	////				ERROR MSG
	public		delegate void	MessageCallback();
	private		bool			bShowMessageMsg			= false;
	struct _Msg {
		public string Title;
		public string Text;
		public MessageCallback Func;
	};
	_Msg Message;

	////////////////////////////////////////////////////////////////////
	////				OTHER MSGs
	private		bool			bShowDeathMsg			= false;
	private		bool			bShowUnreachableMsg		= false;
	private		bool			bShowLvlCompletedMsg	= false;
	private		string			sPlayerName = "";

	private		StageManager	pStageManager			= null;

    private RectTransform backGrid;
    private Vector3 originalScale;

    Transform pCanvasObject;

	[HideInInspector]
    public bool isPause = false;
	GameObject pausePrefab;
    Transform buttonNextTurn, buttonPause, buttonRestart;
    Animator nextStageAnimator;
    Sprite buttonNextTurnSprite;

    // Use this for initialization
    private void Start() {

        // Canvas
        pCanvasObject = transform.GetChild( 0 );

        backGrid = pCanvasObject.GetChild(13).transform as RectTransform;
        originalScale = backGrid.localScale;
        backGrid.localScale = Vector3.zero;

        // Avatars
        AvatarPG1 = pCanvasObject.GetChild( 1 ).GetComponent<Image>();
		AvatarPG2 = pCanvasObject.GetChild( 2 ).GetComponent<Image>();


        // Actions Tables
		pTablePG1 = pCanvasObject.GetChild( 3 );
        pTablePG2 = pCanvasObject.GetChild( 4 );

        // Cursors
        pCursorPG1 = pTablePG1.GetChild( 10 ).transform as RectTransform;
		pCursorPG2 = pTablePG2.GetChild( 10 ).transform as RectTransform;

		// Actions Icons
		vIcons = new GameObject[3];
		vIcons[ (int) ActionType.MOVE ] = pCanvasObject.GetChild( 5 ).gameObject;
		vIcons[ (int) ActionType.USE  ] = pCanvasObject.GetChild( 6 ).gameObject;
		vIcons[ (int) ActionType.WAIT ] = pCanvasObject.GetChild( 7 ).gameObject;


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

		pCursorPG1SpawnPos = pCursorPG1.localPosition;
		pCursorPG2SpawnPos = pCursorPG2.localPosition;


        // prefab Pause Menu
        pausePrefab = GameObject.Find("Pause_Menu");
        pausePrefab.SetActive(false);

        //Button Next Stage Animator 
        nextStageAnimator = pCanvasObject.FindChild("ButtonNextStage").GetComponent<Animator>();
		if ( nextStageAnimator == null ) print( "noooooooo" );
        

        //Button
        buttonNextTurn = pCanvasObject.GetChild(11);
        buttonPause = pCanvasObject.GetChild(14);
        buttonRestart = pCanvasObject.FindChild( "ButtonRestart" );

		//Button Next Stage sprite
        buttonNextTurnSprite = buttonRestart.GetComponent<Image>().sprite;
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

    public	void	ActivatePlayBtn()
    {
        pCanvasObject.GetChild(10).GetComponent<Animator>().SetBool("isPlay", true);
        pCanvasObject.GetChild(10).GetComponent<Button>().interactable = false;
    }


	public	void	AddAction( int PlayerID, ActionType ActionType, int CurrentStage ) {

		Image pImage = vIcons[ (int)ActionType ].GetComponent<Image>();
		vActionsSlots[ PlayerID-1, CurrentStage ].enabled = true;
		vActionsSlots[ PlayerID-1, CurrentStage ].sprite = pImage.sprite;

	}

	public	void	RemoveLastActions() {

		vActionsSlots[ 0, GLOBALS.StageManager.CurrentStage ].enabled = false;
		vActionsSlots[ 1, GLOBALS.StageManager.CurrentStage ].enabled = false;

	}

	public	void	ResetActions() {

		foreach( Image p in vActionsSlots ) {
			p.enabled = false;
		}

	}


	public	void	CursorsStep( int iStage ) {

		if ( ( iStage + 1 ) > StageManager.MAX_STAGES ) {
			backGrid.localScale = new Vector3(originalScale.x * iStage, 1, 1);
			return;
		}

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

        backGrid.localScale = new Vector3(originalScale.x * iStage, 1, 1);

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
			pStageManager.Stop( true );
	}

	}

	public	void	ShowDeathMsg( string PlayerName ) {

		sPlayerName = PlayerName;
		bShowDeathMsg = true;
		if ( pStageManager.IsPlaying ) {
			pStageManager.Stop( true );
		}

	}

	public	void	ShowMessage( string Title, string Text, MessageCallback Func = null ) {

		bShowMessageMsg = true;
		Message.Title = Title;
		Message.Text = Text;
		Message.Func = Func;

	}



	////////////////////////////////////////////////////////////////////////////////////
	////////////////////////////////////////////////////////////////////////////////////
	////////////////////////////////////////////////////////////////////////////////////
	// // // //					WINDOWS

	// Default window rect
	static Rect	DefaultWindow		= new Rect( Screen.width / 2f - 200, Screen.height / 2f - 50, 400, 100 );

	// Death Window
	void Show_Message_GUI( int windowID ) {

		GUI.Label( new Rect( ( DefaultWindow.width / 6f ) - 50.0f, DefaultWindow.height / 1.5f, 100f, 20f ), Message.Text );

		if ( GUI.Button( new Rect( ( DefaultWindow.width / 6f ) - 50.0f, DefaultWindow.height / 0.5f, 100f, 20f ), "OK" ) ) {
			bShowMessageMsg = false;
			if ( Message.Func != null ) Message.Func();
		}

	}

	// Death Window or Unreachable destination Window
	void Show_RestartExit_GUI( int windowID ) {

		if ( GUI.Button( new Rect( ( DefaultWindow.width / 6f ) - 50.0f, DefaultWindow.height / 1.5f, 100f, 20f ), "RESTART" ) ) {
			SceneManager.LoadScene ( SceneManager.GetActiveScene().name );
			return;
		}

		if ( GUI.Button( new Rect( ( DefaultWindow.width / 2f ) + 50.0f, DefaultWindow.height / 1.5f, 100f, 20f ), "EXIT" ) ) {
			GLOBALS.GameManager.Exit();
		}
        
    }

	// LevelCompleted Window
	void Show_LvlCompleted_GUI( int windowID ) {

		if ( SceneManager.sceneCount > ( SceneManager.GetActiveScene().buildIndex + 1 ) ) {

			if ( GUI.Button( new Rect( ( DefaultWindow.width / 6f ) - 50.0f, DefaultWindow.height / 1.5f, 100f, 20f ), "NEXT LEVEL" ) ) {
				SceneManager.LoadScene( SceneManager.GetActiveScene().buildIndex + 1 );
				bShowLvlCompletedMsg = false;
				return;
			}
		}

		if ( GUI.Button( new Rect( ( DefaultWindow.width / 2f ) + 50.0f, DefaultWindow.height / 1.5f, 100f, 20f ), "EXIT" ) ) {
			GLOBALS.GameManager.Exit();
		}
        
    }

	void OnGUI() {
		
		if ( bShowMessageMsg ) {
			GUI.Window( 0, DefaultWindow, Show_Message_GUI, Message.Title );
		}

		if ( bShowDeathMsg ) {
			GUI.Window( 0, DefaultWindow, Show_RestartExit_GUI, "Player is dead!!" );
		}

		if ( bShowUnreachableMsg ) {
			GUI.Window( 0, DefaultWindow, Show_RestartExit_GUI, sPlayerName + " is not able to reach his destination!!" );
		}

		if ( bShowLvlCompletedMsg ) {
			GUI.Window( 0, DefaultWindow, Show_LvlCompleted_GUI, "Level Completed" );
		}

	}

    //Set time scale 0 or at gameTime, able and disable pausePrefab menu 
    public void OnPause() 
    {

		if (isPause)
		{
            isPause = false;
			GLOBALS.IsPaused = false;
            pausePrefab.SetActive(false);
            Debug.Log("resume");
            Time.timeScale = GLOBALS.GameTime;
		}

        else  // if (!isPause)
		{
            isPause = true;
            pausePrefab.SetActive(true);
			GLOBALS.GameTime = Time.timeScale;
			GLOBALS.IsPaused = true;
			Time.timeScale = 0;
		}

	}

    //Able and Disable the animation button nex turn glow
    public void GlowAnimationNextTurn( bool Active )
    {

		nextStageAnimator.SetBool(("isTurnComplete"), Active);
/*        if (Active)
        {
            nextStageAnimator.SetBool(("isTurnComplete"), true);
        }
        else
        {
            nextStageAnimator.SetBool(("isTurnComplete"), false);
        }
		*/
    }

    public void GlowOrangeNexTurn()
    {
        buttonNextTurnSprite = orangeNexTurn;
    }

    public void GlowOrangePause()
    {
        buttonNextTurnSprite = orangePause;
    }

    public void GlowOrangeRestart()
    {
        buttonNextTurnSprite = orangeRestart;
    }

    public void DefaultNextTurn()
    {
        buttonNextTurnSprite = defaultNexTurn;
    }

    public void DefaultPause()
    {
        buttonNextTurnSprite = defaultPause;
    }

    public void DefaultRestart()
    {
        buttonNextTurnSprite = defaultRestart;
    }

}