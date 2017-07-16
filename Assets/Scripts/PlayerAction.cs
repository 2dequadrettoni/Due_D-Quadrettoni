using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Similar to typedef
using NodeList = System.Collections.Generic.List<Node>;

public		enum			ActionType		{ WAIT, USE, MOVE };
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
	NodeList GetPath();

	// Return the final position if path is set ( return value is a NODE )
	Node GetFinalPosition();

}



public class PlayerAction : iPlayerAction {
	
	// WAIT, USE, MOVE
	private		ActionType		iType		= 0;

	// Action:USE
	private		UsableObject	pObject		= null;

	// Action:MOVE
	private		NodeList		vNodeList	= null;

	// Callback called at end of action
	private ActionEndCallBack	pCallback	= null;


	//////////////////////////////////////////////////////////////////
	//////////////////////////////////////////////////////////////////
	//////////////////////////////////////////////////////////////////

	
	// Create as User
	public		PlayerAction( UsableObject _Object ) { 
		iType		= ActionType.USE;
		pObject		= _Object;
	}

	// Create as Mover
	public		PlayerAction( NodeList _PathList ) { 
		iType		= ActionType.MOVE;
		vNodeList	=  new NodeList( _PathList );
	}

	// Create as waiter
	public		PlayerAction() { 
		iType		= ActionType.WAIT;
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



	// Return the path if set
	public NodeList	GetPath() {

		if ( vNodeList.Count < 1 ) return null;

		return vNodeList;
	}

	public Node GetFinalPosition() {

		if ( vNodeList.Count < 1 ) return null;

		return vNodeList[ vNodeList.Count - 1 ];

	}

}