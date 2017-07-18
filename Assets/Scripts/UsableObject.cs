using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public		enum			UsageType		{ NONE, INSTANT, ON_ACTION_END };

public class UsableObject : MonoBehaviour {

	public	Transform			Porta  = null;

	public	UsageType			iUseType = UsageType.NONE;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public	UsageType GetUseType() {
		return iUseType;
	}

	public void OnReset() {
		
		Porta.GetComponent<BoxCollider>().enabled = true;
		Porta.GetComponent<MeshRenderer>().enabled = true;

	}

	public void OnUse( Player User ) {

		Porta.GetComponent<BoxCollider>().enabled = false;
		Porta.GetComponent<MeshRenderer>().enabled = false;
		User.PathFinder.UpdateGrid();

		Debug.Log( "Usato" );


	}
}
