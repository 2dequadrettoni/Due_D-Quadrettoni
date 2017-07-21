using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Toggler : MonoBehaviour {

	[SerializeField]
	private	bool	bActive	= false;
	/*
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	*/
	public void OnUse( Player User, bool Internal = true ) {
		print( "Obj Used" );

		bActive = !bActive;

		if ( bActive ) {
			GetComponent<BoxCollider>().enabled = true;
			GetComponent<MeshRenderer>().enabled = true;
		}
		else {
			GetComponent<BoxCollider>().enabled = false;
			GetComponent<MeshRenderer>().enabled = false;
		}
		User.PathFinder.UpdateGrid();
	}

	public void OnReset() {
		print( "Obj reset" );

		GetComponent<BoxCollider>().enabled = true;
		GetComponent<MeshRenderer>().enabled = true;

	}
}