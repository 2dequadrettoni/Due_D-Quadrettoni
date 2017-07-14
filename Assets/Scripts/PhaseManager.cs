
using System.Collections.Generic; 
using System.Collections;
using UnityEngine;

////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////



	//   GESTISCE LE AZIONI DI ENTRAMBI I GIOCATORI



public delegate void	MaxActionsCallback();

public class PhaseManager : MonoBehaviour {

	const int iMaxActions = 10;

	
	private MaxActionsCallback	pCallback		= null;

	private	PhaseAction[]		vActions		= null;
	private	int					iActionsCount	= 0;

	private	bool				bIsOK			= false;

//	private	PlayerMgr				pPlayer			= null;

	private void	Start() {
		
/*		// if Player is not found then cannot execute play action
		GameObject PlayerObject = GameObject.Find( "Player" );
		if ( ( !PlayerObject ) || ( pPlayer = PlayerObject.GetComponent<PlayerMgr1>() ) ) return;
**/
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
//			pPlayer.CanParseInput = false;

		}

		while ( iActionsCount > 0 ) {

			StartCoroutine( CycleActions() );

		}

	}

	private	IEnumerator	CycleActions(){

		PhaseAction pAction = vActions[ iMaxActions - iActionsCount ];

//		if ( pAction. )



		iActionsCount--;

		yield return 0;

	}

}



////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////


public enum GAME_PHASE : byte { PLANNING, PLAYING }

public static class GamePhase {
	
	private	static	GAME_PHASE	iActuaPahse = GAME_PHASE.PLANNING;


	public static Vector3 P1Position, P2Position;

	public static void Switch() {
		
		if ( iActuaPahse == GAME_PHASE.PLAYING ) return;
		
		iActuaPahse = GAME_PHASE.PLAYING;
		
	}
	
	public static  GAME_PHASE GetCurrent() {
		return iActuaPahse;
	}
	
}