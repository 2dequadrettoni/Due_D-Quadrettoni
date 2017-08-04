using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimationEvents : MonoBehaviour {

	private		bool	bGameStarted	= false;

	void	RemoveActiveObject()	{
		GLOBALS.StageManager.RemoveActiveObject();
	}


	void	PlaySpawnSound() {
		GLOBALS.AudioManager.Play( "PG_Spawn" );
	}

	void	PlayStepSound() {
		GLOBALS.AudioManager.Play( "Plan_Move" );
	}

	void	DoorOpened() {

		// Get collider child
		transform.parent.GetChild( 1 ).gameObject.SetActive(false);
	//	transform.parent.GetChild( 1 ).GetComponent<BoxCollider>().enabled = false;
//		transform.parent.gameObject.layer = 0;
		GLOBALS.PathFinder.UpdateGrid();

	}

	void	DoorClosed() {

		// Get collider child
		transform.parent.GetChild( 1 ).gameObject.SetActive(true);
//		transform.parent.GetComponent<BoxCollider>().enabled = true;
//		transform.parent.gameObject.layer = LayerMask.NameToLayer( "Unwalkable" );
		GLOBALS.PathFinder.UpdateGrid();

	}

	void	SetAsUsed() {


		UsableObject o = transform.parent.GetComponent<UsableObject>();
		if ( o is Switcher ) {
			( o as Switcher).SetUsed( true );
			return;
		}

		if ( o is Door ) {
			( o as Door ).SetClosed( false );
		}

	}

	void	SetAsNotUsed() {

		UsableObject o = transform.parent.GetComponent<UsableObject>();
		if ( o is Switcher ) {
			( o as Switcher).SetUsed( false );
			return;
		}

		if ( o is Door ) {
			( o as Door ).SetClosed( true );
		}

	}

	void	SetGameAsRunning() {

		if ( bGameStarted ) return;

		bGameStarted = true;
		GLOBALS.Player1.SetCursor( true );
		GLOBALS.StageManager.SelectedPlayer = 1;
		GLOBALS.Player1.CanParseInput = true;
		GLOBALS.UI.SelectPlayer( 1 );

	}

	void DisableObject() {
		
		Image pImage = GetComponent<Image>();
		if ( pImage ) pImage.enabled = false;

	}

	void EnableObject() {
		
		Image pImage = GetComponent<Image>();
		if ( pImage ) pImage.enabled = true;

	}

}
