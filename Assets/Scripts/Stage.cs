
using UnityEngine;

public class Stage {

	// Waiter as default
	PlayerAction Player1Action = null;
	PlayerAction Player2Action = null;

	/// <summary>Check if both PGs have actions</summary>
	public	bool	IsOK() {
		return ( ( Player1Action != null ) && ( Player2Action != null ) );
	}

	public	void	Default() {

		if ( Player1Action == null )	Player1Action = new PlayerAction();
		if ( Player2Action == null )	Player2Action = new PlayerAction();

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