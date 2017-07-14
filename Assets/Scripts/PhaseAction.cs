using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
using NodeList = System.Collections.Generic.List<Node>;
*/
public		enum			ActionType		{ WAIT, USE, MOVE };
public class PhaseAction {
	

	private		ActionType		iType		= 0;

	// Action:USE
	private		GameObject		pObject		= null;

	// Action:MOVE
	private		List<Node>		vNodeList	= null;







	// Create as User
	public		PhaseAction( GameObject _Object ) { 
		iType	= ActionType.USE;
		pObject = _Object;
	}

	// Create as Mover
	public		PhaseAction( List<Node> _PathList ) { 
		iType	= ActionType.USE;
		vNodeList = _PathList;
	}

	// Create as waiter
	public		PhaseAction() { 
		iType	= ActionType.WAIT;
	}

	public ActionType GetType() { return iType; }

}