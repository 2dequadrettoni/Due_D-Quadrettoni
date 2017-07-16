
using UnityEngine;

public class Stage {

	// Waiter as default
	PlayerAction Player1Action = null;
	PlayerAction Player2Action = null;

	public	bool	IsOK() {
		return ( ( Player1Action != null ) && ( Player2Action != null ) );
	}

	public void SetAction( int PlayerID, PlayerAction pAction ) {

		if ( PlayerID == 1 ) 
			Player1Action = pAction;
		else {
			Player2Action = pAction;
		}

	}

	public	PlayerAction	GetAction( int PlayerID ) {

		if ( PlayerID == 1 ) 
			return Player1Action;
		else {
			return Player2Action;
		}

	}

}