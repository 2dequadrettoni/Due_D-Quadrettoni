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
	
	private		Vector3			vStartPoint	= Vector3.zero;

	// WAIT, USE, MOVE
	private		ActionType		iType		= ActionType.WAIT;

	// Action:USE
	private		UsableObject	pObject		= null;

	// Action:MOVE
	private		Vector3			vDestination = Vector3.zero;

	// Callback called at end of action
	private ActionEndCallBack	pCallback	= null;

	// Start as a valid action
	private	bool				bIsValid	= true;


	//////////////////////////////////////////////////////////////////
	//////////////////////////////////////////////////////////////////
	//////////////////////////////////////////////////////////////////
	
	// Create as User
	/// <summary>Object use</summary>
	public		PlayerAction( Vector3 _CurrentPosition, UsableObject _Object ) { 
		iType			= ActionType.USE;
		vStartPoint		= _CurrentPosition;
		pObject			= _Object;
	}

	// Create as Mover
	/// <summary>Move action, secondary is usable object at movement anction</summary>
	public		PlayerAction( Vector3 _StartPoint, Vector3 _Destination, UsableObject _Object = null ) {
		if ( _Destination == Vector3.zero ) {
			bIsValid = false;
			return;
		}

		iType			= ActionType.MOVE;
		vStartPoint		= _StartPoint;
		vDestination	=  _Destination;
		pObject			= _Object;
	}

	// Create as waiter
	/// <summary>Alias for WAIT action</summary>
	public		PlayerAction( Vector3 _CurrentPosition ) {
		vStartPoint		= _CurrentPosition;
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


	public	Vector3	GetStartPoint() {

		return vStartPoint;

	}

	// Return path start and end points
	public Vector3	GetDestination() {

		return vDestination;

	}


	public	bool	IsValid() {
		return bIsValid;
	}


	public	void	Invalidate() {
		bIsValid = false;
	}

}