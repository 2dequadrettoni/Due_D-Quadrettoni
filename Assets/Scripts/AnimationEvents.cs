using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEvents : MonoBehaviour {

	private	StageManager	pStagemanage		= null;
	private	bool			bIsOK				= false;

	void Start () {

		pStagemanage = GLOBALS.StageManager;
		bIsOK = true;

	}

	void	AddActiveObject() {

		if ( !bIsOK ) return;
		pStagemanage.AddActiveObject();

	}

	void	RemoveActiveObject()	{

		if ( !bIsOK ) return;
		pStagemanage.RemoveActiveObject();

	}

	void	DoorOpened() {

		transform.parent.GetComponent<BoxCollider>().enabled = false;

	}
}
