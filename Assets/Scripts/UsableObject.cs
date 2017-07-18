using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UsableObject : MonoBehaviour {

	public Transform Porta  = null;


	public enum Usabletypes { ON_USE, ON_TURN_END }

	public Usabletypes Type;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
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
