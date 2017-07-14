using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UsableObject : MonoBehaviour {

	// object size
	private float	radius;
	// però esendo un solido quello che può essere usato per gli oggetti, mettendo un collider non triggerato.
	// Da solo dovrebbe oscacolare il path del giocatore


	private	bool	bCanbeUsed;
	// decide se si può utilizzare


	private	bool	bActivated;
	// decide se si può utilizzare




	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OnUse() {

		// se non può esere usato skipp
		if ( !bCanbeUsed ) return;

		// Use it once
		bCanbeUsed = false;


	}
}
