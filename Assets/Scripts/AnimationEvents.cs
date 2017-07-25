using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEvents : MonoBehaviour {

	private	StageManager	pStagemanage		= null;
	private	bool			bIsOK				= false;

	void Start () {

		pStagemanage = GLOBALS.StageManager;

	}

	void	RemoveActiveObject()	{
		pStagemanage.RemoveActiveObject();

	}

	void	DoorOpened() {

		transform.parent.GetComponent<BoxCollider>().enabled = false;
		transform.parent.gameObject.layer = 0;
		GLOBALS.PathFinder.UpdateGrid();

	}

	void	SetGameAsRunning() {
		
		GLOBALS.StageManager.SelectedPlayer = 1;
		GLOBALS.Player1.CanParseInput = true;
		GLOBALS.UI.SelectPlayer( 1 );

	}


}
