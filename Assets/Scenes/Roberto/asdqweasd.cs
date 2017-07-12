using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class asdqweasd : MonoBehaviour {

	public Transform target;
	public NavMeshAgent agnt;
	public Camera cam;

	bool IsMoving = false;


	// Use this for initialization
	void Start () {
		

	}
	
	// Update is called once per frame
	void Update () {

		if ( Input.GetMouseButtonDown( 0 ) && !IsMoving ) {

			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit))
			{
				agnt.SetDestination( hit.point );

				IsMoving = true;
			}
		}

		if ( agnt.pathStatus==NavMeshPathStatus.PathComplete ) {

			IsMoving = false;

		}

	}


}
