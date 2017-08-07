using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


#pragma warning disable CS0162 // Unreachable code detected
#pragma warning disable CS0414 // Var assigned but never used


public class UI : MonoBehaviour {

	[SerializeField]
	private		Sprite			AvatarPG1Enabled		= null;
	[SerializeField]
	private		Sprite			AvatarPG1Disabled		= null;

	[Space()][SerializeField]
	private		Sprite			AvatarPG2Enabled		= null;
	[SerializeField]
	private		Sprite			AvatarPG2Disabled		= null;


	 // Tutorials
	public	static	bool		TutorialLvl				= false;

	[Header("Cursor sprite")]
	[SerializeField]
	private		Sprite			pCursorSprite			= null;

	[Header("Tutorials sprites")]
	[SerializeField]
	private	Sprite				pTutorialSprite			= null;

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
	[Header("Button Selected Images")]
	public Sprite orangeNexTurn;
	public Sprite defaultNexTurn;
	public Sprite orangePause;
	public Sprite defaultPause;
	public Sprite orangeRestart;
	public Sprite defaultRestart;

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
    Transform buttonNextTurn, buttonPause, buttonRestart, buttonPlay;
    Animator nextStageAnimator;

    Sprite buttonNextTurnSprite;
	Sprite buttonPauseSprite;
	Sprite buttonRestartSprite;


	public GameObject FadeImage = null;


	Animator pBlackScreenAnimator;

    // Use this for initialization
    private void Start() {

		if ( FadeImage != null ) FadeImage.SetActive( true );

		Cursor.SetCursor( pCursorSprite.texture, Vector2.zero, CursorMode.Auto );

        // Canvas
        pCanvasObject = transform.GetChild( 0 );

        backGrid = pCanvasObject.Find("BackGrid").transform as RectTransform;
        originalScale = backGrid.localScale;
        backGrid.localScale = Vector3.zero;

        // Avatars
        AvatarPG1 = pCanvasObject.Find("AvatarPG1").GetComponent<Image>();
		AvatarPG2 = pCanvasObject.Find("AvatarPG2").GetComponent<Image>();


        // Actions Tables
		pTablePG1 = pCanvasObject.Find("TablePG1");
        pTablePG2 = pCanvasObject.Find("TablePG2");

        // Cursors
        pCursorPG1 = pTablePG1.Find("CursorPG1").transform as RectTransform;
		pCursorPG2 = pTablePG2.Find("CursorPG2").transform as RectTransform;

		// Actions Icons
		vIcons = new GameObject[8];
		Transform pIcons = pCanvasObject.Find("Icons");
		for ( int i = 0;  i < 8; i++ ) {
			vIcons[ i ] = pIcons.GetChild( i ).gameObject;

		}

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
        nextStageAnimator = pCanvasObject.Find("ButtonNextStage").GetComponent<Animator>();
		if ( nextStageAnimator == null ) print( "noooooooo" );
        

        //Button
        buttonNextTurn	= pCanvasObject.Find("ButtonNextStage");
        buttonPause		= pCanvasObject.Find("ButtonPause");
        buttonRestart	= pCanvasObject.Find("ButtonRestart");
		buttonPlay		= pCanvasObject.Find("ButtonPlay");

		{
			Button Button = pCanvasObject.Find("ButtonSelectP1").GetComponent<Button>();
			Button.onClick.RemoveAllListeners();
			Button.onClick.AddListener( () => GLOBALS.StageManager.SelectPlayer( 1 ) );
		}
		{
			Button Button = pCanvasObject.Find("ButtonSelectP2").GetComponent<Button>();
			Button.onClick.RemoveAllListeners();
			Button.onClick.AddListener( () => GLOBALS.StageManager.SelectPlayer( 2 ) );
		}

		{	// next
			Button Button = buttonNextTurn.GetComponent<Button>();
			Button.onClick.RemoveAllListeners();
			Button.onClick.AddListener( () => GLOBALS.StageManager.NextStage() );
		}
		{	// pause
			Button Button = buttonPause.GetComponent<Button>();
			Button.onClick.RemoveAllListeners();
			Button.onClick.AddListener( () => GLOBALS.UI.OnPause() );
		}
		{	// restart
			Button Button = buttonRestart.GetComponent<Button>();
			Button.onClick.RemoveAllListeners();
			Button.onClick.AddListener( () => GLOBALS.GameManager.RestartGame() );
		}
		{	// play
			Button Button = buttonPlay.GetComponent<Button>();
			Button.onClick.RemoveAllListeners();
			Button.onClick.AddListener( () => GLOBALS.StageManager.Play() );
		}



		//Button Next Stage sprite
        buttonNextTurnSprite = buttonNextTurn.GetComponent<Image>().sprite;
		buttonPauseSprite	= buttonPause.GetComponent<Image>().sprite;
		buttonRestartSprite = buttonRestart.GetComponent<Image>().sprite;
		
		// black screen
		pBlackScreenAnimator = pCanvasObject.Find( "Fade_image" ) .GetComponent<Animator>();

		pBlackScreenAnimator.Play( "Fade_Out" );

    }


	public void		BlackScreenFadeIn() {

		pBlackScreenAnimator.Play( "Fade_In" );

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
		// reset cursor start position
		pCursorPG1.localPosition = pCursorPG1SpawnPos;
		pCursorPG2.localPosition = pCursorPG2SpawnPos;

		// start play button animation and disable its interop
		buttonPlay.GetComponent<Animator>().SetBool("isPlay", true);
        buttonPlay.GetComponent<Button>().interactable = false;

		// make sure next turn button animation stop
		this.GlowAnimationNextTurn( false );

    }


	public	void	AddAction( int PlayerID, ActionType ActionType, int CurrentStage ) {

		Image pImage = vIcons[ (int)ActionType + 4 ].GetComponent<Image>();
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


	public	void	RemoveIconsGlow() {

		//	PG 1
		{	PlayerAction PA1	= pStageManager.GetCurrentStage().GetAction( 1 );
			Image pImage		= vIcons[ (int)PA1.GetType() ].GetComponent<Image>();
			vActionsSlots[ 0, pStageManager.CurrentStage ].sprite = pImage.sprite;
		}

		//	PG 2
		{	PlayerAction PA2	= pStageManager.GetCurrentStage().GetAction( 2 );
			Image pImage		= vIcons[ (int)PA2.GetType() ].GetComponent<Image>();
			vActionsSlots[ 1, pStageManager.CurrentStage ].sprite = pImage.sprite;
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
		
		RemoveIconsGlow();

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

		if ( GUI.Button( new Rect( ( DefaultWindow.width / 6f ) - 50.0f, DefaultWindow.height / 1.5f, 100f, 20f ), "NEXT LEVEL" ) ) {

			SceneManager.LoadScene( SceneManager.GetActiveScene().buildIndex + 1 );
			bShowLvlCompletedMsg = false;
			return;
		}

		if ( GUI.Button( new Rect( ( DefaultWindow.width / 4f ) + 50.0f, DefaultWindow.height / 1.5f, 100f, 20f ), "MAIN MENU" ) ) {

			Time.timeScale = GLOBALS.GameTime;
			GLOBALS.TutorialOverride = false;
			GameManager.InTutorialSequence = false;
			GameManager.TutorialStep = 0;
			SceneManager.LoadScene(0);
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
			
			if ( SceneManager.GetActiveScene().buildIndex == ( SceneManager.sceneCountInBuildSettings - 1 ) )
				// final cutscene start
				SceneManager.LoadScene( "Finale"  );
			else
				// load next level
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
        buttonPauseSprite = orangePause;
    }

    public void GlowOrangeRestart()
    {
        buttonRestartSprite = orangeRestart;
    }

    public void DefaultNextTurn()
    {
        buttonNextTurnSprite = defaultNexTurn;
    }

    public void DefaultPause()
    {
        buttonPauseSprite = defaultPause;
    }

    public void DefaultRestart()
    {
        buttonRestartSprite = defaultRestart;
    }

}


#pragma warning restore CS0414 // Var assigned but never used
#pragma warning restore CS0162 // Unreachable code detected