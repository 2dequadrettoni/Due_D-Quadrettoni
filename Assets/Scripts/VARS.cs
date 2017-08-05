using UnityEngine;



public static class GLOBALS {

	public	static	int				CurrentLevel		= -1;

	public	static	UI				UI					= null;

	public	static	float			GameTime			= 0.0f;

	public	static	bool			IsPaused			= false;

	public	static	GameManager		GameManager			= null;

	public	static	StageManager	StageManager		= null;

	public	static	EventManager	EventManager		= null;

	public	static	AudioManager	AudioManager		= null;

	public	static	Player			Player1				= null;

	public	static	Player			Player2				= null;

	public	static	Pathfinding		PathFinder			= null;

	public	static	SpriteRenderer	TutorialSlot		= null;

	public	static	bool			TutorialOverride	= false;

}



public class VARS : MonoBehaviour {

	private void Awake() {

		{//	UI
			GameObject o = GameObject.Find( "UI" );
			if ( o ) {
				GLOBALS.UI = o.GetComponent<UI>();
			}
		}

		if ( Camera.main != null ) GLOBALS.TutorialSlot = Camera.main.transform.FindChild( "TutorialSlot" ).GetComponent<SpriteRenderer>();

		// GameManager, StageManager and EventManager
		{
			GameObject GM = GameObject.Find( "GameManager" );
			if ( GM ) {
				GLOBALS.GameManager		= GM.GetComponent<GameManager>();
				GLOBALS.StageManager	= GM.GetComponent<StageManager>();
				GLOBALS.EventManager	= GM.GetComponent<EventManager>();
			}
			else GLOBALS.UI.ShowMessage( "Error", "Cannot find GameManager object", delegate { Application.Quit(); } );

			GameObject AM = GameObject.Find( "AudioManager" );
			if ( AM ) {
				GLOBALS.AudioManager = AM.GetComponent<AudioManager>();
			}
		}

		{//	Player 1
			GameObject o = GameObject.Find( "Player1" );
			if ( o ) {
				GLOBALS.Player1 = o.GetComponent<Player>();
			}
		}

		{//	Player 2
			GameObject o = GameObject.Find( "Player2" );
			if ( o ) {
				GLOBALS.Player2 = o.GetComponent<Player>();
			}
		}

		{//	pathFinder
			GameObject o = GameObject.Find( "PathFinder" );
			if ( o ) {
				GLOBALS.PathFinder = o.GetComponent<Pathfinding>();
			}
		}

	}

}