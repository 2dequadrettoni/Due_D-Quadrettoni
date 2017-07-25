using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
[RequireComponent(typeof( UsableObject ))]
public class DoorBySwitchers : MonoBehaviour {

	[SerializeField]
	private			UsableObject[]		vSwitchers		= null;

	private			UsableObject		pThisUsable		= null;

	private			bool				bActivated		= false;

	// Use this for initialization
	void Start () {

	}

	bool	AreSwitcherUsed() {
		foreach( UsableObject o in vSwitchers ) {
			if ( !o.Used ) return false;
		}
		return true;
	}
	
	// Update is called once per frame
	void Update () {
		
		if ( !bActivated && AreSwitcherUsed() )
			bActivated = true;
			pThisUsable.OnUse( null );
	}
}
*/