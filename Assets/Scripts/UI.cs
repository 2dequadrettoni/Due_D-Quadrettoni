using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour {

	// UI
	private		Canvas			pCanvas			= null;
	private		Sprite			pTable			= null;

	private		Sprite[]		vIcons			= null;
	private		List<Sprite>	vIconsP1		= null;
	private		List<Sprite>	vIconsP2		= null;

	// Docking
	private		Vector2			vStartPosition	= new Vector2 ( -367.3f, -99.5f );
	private		float			fOffset			= 30.7f;

	// Use this for initialization
	void Start () {

		// Canvas
		Transform   pCanvasObject = transform.GetChild( 0 );
		pCanvas		= pCanvasObject.GetComponent<Canvas>();

		// Table sprite
		pTable		= pCanvasObject.GetChild( 0 ).gameObject.GetComponent<Image>().sprite;

		// Actions icons
		Transform	pIcons = pCanvasObject.GetChild( 1 );
		vIcons = new Sprite[ 3 ];
		for ( int i = 0; i < 3; i++ ) {
			vIcons[i] = pIcons.GetChild( i ).gameObject.GetComponent<Image>().sprite;
		}

		vIconsP1 = new List<Sprite>();
		vIconsP2 = new List<Sprite>();

	}
	

	public	void	UpdateUI( int _SelectedPlayer, int _ActionType, int _CurrentStage ) {

		if ( vIconsP1.Count < _CurrentStage ) {

			
//			vIconsP1.Add( vIcons[ _ActionType ] );

		}

		if ( vIconsP1.Count > _CurrentStage ) {

//			vIconsP1.RemoveAt( vIconsP1.Count - 1 );

		}

//		if ( vIconsP2.Count < _CurrentStage ) vIconsP2.Add( vIcons[ _ActionType ] );
//		if ( vIconsP2.Count > _CurrentStage ) vIconsP2.RemoveAt( vIconsP2.Count - 1 );

	}


	// Update is called once per frame
	void Update () {
		
	}
}
