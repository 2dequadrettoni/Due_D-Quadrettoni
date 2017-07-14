
using System.Collections.Generic; 
using System.Collections;
using UnityEngine;

using NodeList = System.Collections.Generic.List<Node>;

public enum GAME_PHASE : byte {
	PLANNING, PLAYING
}

public static class GamePhase {
	
	private	static	GAME_PHASE	iActuaPahse = GAME_PHASE.PLANNING;


	public static void Switch() {
		
		if ( iActuaPahse == GAME_PHASE.PLAYING ) return;
		
		iActuaPahse = GAME_PHASE.PLAYING;
		
	}
	
	public static  GAME_PHASE GetCurrent() {
		return iActuaPahse;
	}
	
}


public class PhaseAction {

	public		enum			Action		{ WAIT, USE, MOVE };
	private		Action			iAction		= 0;

	// Action:USE
	private		GameObject		pObject		= null;

	// Action:MOVE
	private		NodeList		vNodeList	= null;

	// Create as User
	public		PhaseAction( GameObject _Object ) { 
		iAction	= Action.USE;
		pObject = _Object;
	}

	// Create as Mover
	public		PhaseAction( NodeList _PathList ) { 
		iAction	= Action.USE;
		vNodeList = _PathList;
	}

	// Create as waiter
	public		PhaseAction() { 
		iAction	= Action.WAIT;
	}

}

public delegate void	MaxActionsCallback();

public class GameManager : MonoBehaviour {

	const int iMaxActions = 10;

	
	private MaxActionsCallback	pCallback		= null;

	private	PhaseAction[]		vActions		= null;
	private	int					iActionsCount	= 0;

	private	bool				bIsOK			= false;

	private	Player				pPlayer			= null;

	private void	Start() {
		
		// if Player is not found then cannot execute play action
		GameObject PlayerObject = GameObject.Find( "Player" );
		if ( ( !PlayerObject ) || ( pPlayer = PlayerObject.GetComponent<Player>() ) ) return;

		bIsOK = true;

	}

	/// <summary>
	/// Set a callback called when last action is asigned
	/// </summary>
	/// <param name="_Callback"></param>
	public	void	SetMaxActionsCallback( MaxActionsCallback _Callback ) {

		pCallback = _Callback;

	}

	/// <summary>
	/// Add an action that will be played
	/// </summary>
	/// <param name="pAction"></param>
	public	void	AddAction( PhaseAction pAction ) {

		// If another action can be inserted into action array
		if ( iActionsCount < iMaxActions ) {

			// Add it
			vActions[ iActionsCount ] = pAction;
			iActionsCount++;

//			ResetWorld();

		}

		// Executed is max actions count is reached
		if ( iActionsCount == iMaxActions ) {

			// Call the callback then remove itself
			if ( pCallback != null ) {
				pCallback();
				pCallback = null;
			}

		}

	}

	/// <summary>
	/// Remove last action inserted
	/// </summary>
	public	void	RemLastAction() {

		// If there is not at last one return
		if ( iActionsCount < 1 ) return;

		// remove last and decrease actions count
		vActions[ vActions.Length - 1 ] = null;
		iActionsCount--;

	}

	/// <summary>
	/// Launch play sequence
	/// </summary>
	public	void	Play() {

		// If manager had bad initialization or actual action count is smaller that max action
		if ( !bIsOK || ( iActionsCount != iMaxActions ) ) return;

		if ( GamePhase.GetCurrent() == GAME_PHASE.PLANNING ) {

			GamePhase.Switch();
			pPlayer.CanParseInput = false;

		}

		while ( iActionsCount > 0 ) {

			StartCoroutine( CycleActions() );

		}

	}

	private	IEnumerator	CycleActions(){



		yield return 0;

	}

}