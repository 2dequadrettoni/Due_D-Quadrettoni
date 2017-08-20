using UnityEngine;



public static class GLOBALS {

	public	static	Logger			Logger				= null;

	public	static	int				CurrentLevel		= -1;

	public	static	UI				UI					= null;

	public	static	float			GameTime			= 0.0f;

	public	static	bool			IsPaused			= false;

	public	static	GameManager		GameManager			= null;

	public	static	StageManager	StageManager		= null;

	public	static	EventManager	EventManager		= null;

	public	static	Player			Player1				= null;

	public	static	Player			Player2				= null;

	public	static	Pathfinding		PathFinder			= null;

	public	static	SpriteRenderer	TutorialSlot		= null;

	public	static	bool			TutorialOverride	= false;

}



public class VARS : MonoBehaviour {

	private void Awake() {
		
		GLOBALS.Logger.Write( "Finding Scene essential game objects" );

		{//	UI
			GameObject o = GameObject.Find( "UI" );
			if ( o ) {
				GLOBALS.UI = o.GetComponent<UI>();
			}
			else GameManager.Exit( "UI not found, exiting.." );
		}

		{
			if ( Camera.main != null ) {
				Transform  pTutorialSlot = Camera.main.transform.Find( "TutorialSlot" );
				if ( pTutorialSlot != null )
					GLOBALS.TutorialSlot =  pTutorialSlot.GetComponent<SpriteRenderer>();
				else {
					GameManager.Exit( "Camera has not child 'TutorialSlot' " );
				}
			} else GameManager.Exit( "No camera tagged as Main Camera" );

		}



		

		// GameManager, StageManager and EventManager
		{
			GameObject GM = GameObject.Find( "GameManager" );
			if ( GM ) {
				GLOBALS.GameManager		= GM.GetComponent<GameManager>();
				GLOBALS.StageManager	= GM.GetComponent<StageManager>();
				GLOBALS.EventManager	= GM.GetComponent<EventManager>();
			}
			else GameManager.Exit( "GameManager not found, exiting.." );
		}

		{//	Player 1
			GameObject o = GameObject.Find( "Player1" );
			if ( o ) {
				GLOBALS.Player1 = o.GetComponent<Player>();
			}
			else GameManager.Exit( "Player1 not found, exiting.." );
		}

		{//	Player 2
			GameObject o = GameObject.Find( "Player2" );
			if ( o ) {
				GLOBALS.Player2 = o.GetComponent<Player>();
			}
			else GameManager.Exit( "Player2 not found, exiting.." );
		}

		{//	pathFinder
			GameObject o = GameObject.Find( "PathFinder" );
			if ( o ) {
				GLOBALS.PathFinder = o.GetComponent<Pathfinding>();
			}
			else GameManager.Exit( "PathFinder not found, exiting.." );
		}

		GLOBALS.Logger.Write( "All essentials found" );

	}

}