using UnityEngine;



public static class GLOBALS {

	public	static	bool			IsPaused		= false;

	public	static	GameManager		GameManager		= null;

	public	static	StageManager	StageManager	= null;

	public	static	Player			Player1			= null;

	public	static	Player			Player2			= null;

	public	static	Pathfinding		PathFinder		= null;

	public	static	UI				UI				= null;

}



public class VARS : MonoBehaviour {

	private void Awake() {

		{//	GameManager
			GameObject o = GameObject.Find( "GameManager" );
			if ( o ) {
				GLOBALS.GameManager = o.GetComponent<GameManager>();
			}
		}

		{//	StageMananger
			GameObject o = GameObject.Find( "GameManager" );
			if ( o ) {
				GLOBALS.StageManager = o.GetComponent<StageManager>();
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

		{//	UI
			GameObject o = GameObject.Find( "UI" );
			if ( o ) {
				GLOBALS.UI = o.GetComponent<UI>();
			}
		}

	}

}