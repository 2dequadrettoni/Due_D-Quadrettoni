using UnityEngine;

public		enum			ActionType		{ MOVE, USE, WAIT, NONE };

public		delegate void	ActionEndCallBack();

interface iPlayerAction {

	// Set a callback called at end of action
	void SetEndActionCallback( ActionEndCallBack _Callback );
	
	// Execute the callback if set
	void ExecuteCallBack();

	// "override" of object.GetType()
	ActionType GetType();

	// Return the usable object if set
	UsableObject GetUsableObject();

	// Return the path if set
	Vector3 GetDestination();

}



public class PlayerAction : iPlayerAction {
	
	// WAIT, USE, MOVE
	private		ActionType		iType		= ActionType.NONE;

	// Action:USE
	private		UsableObject	pObject		= null;

	// Action:MOVE
	private		Vector3			vDestination = Vector3.zero;

	// Callback called at end of action
	private ActionEndCallBack	pCallback	= null;


	//////////////////////////////////////////////////////////////////
	//////////////////////////////////////////////////////////////////
	//////////////////////////////////////////////////////////////////
	
	// Create as User
	public		PlayerAction( UsableObject _Object ) { 
		iType			= ActionType.USE;
		pObject			= _Object;
	}

	// Create as Mover
	public		PlayerAction( Vector3 _Destination, UsableObject _Object = null ) {
		if ( _Destination == Vector3.zero ) return;

		iType			= ActionType.MOVE;
		vDestination	=  _Destination;
		pObject			= _Object;
	}

	// Create as waiter
	public		PlayerAction() { 
		iType			= ActionType.WAIT;
	}


	//////////////////////////////////////////////////////////////////

	// Set a callback called at end of action
	public void		SetEndActionCallback( ActionEndCallBack _Callback ) {
		pCallback = _Callback;
	}


	// Execute the callback if set
	public void		ExecuteCallBack() {

		if ( pCallback == null ) return;

		pCallback();
		pCallback = null;
	}


	// "override" of object.GetType()
	public new ActionType GetType() { return iType; }


	// Return the usable object if set
	public UsableObject	GetUsableObject() {

		return pObject;
	}

	// Return path start and end points
	public Vector3	GetDestination() {

		return vDestination;
	}

}