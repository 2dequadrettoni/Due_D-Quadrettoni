using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour {

	[SerializeField]	private		TilesManager	pTilesManager		= null;

						private		Tile []			vAdiacentTiles;
	/* Tile[ 1 ] = UP
	 * Tile[ 2 ] = RIGHT
	 * Tile[ 3 ] = DOWN
	 * Tile[ 4 ] = LEFT
	*/

	// Use this for initialization
	void Start () {

		pTilesManager = GameObject.Find( "Obj_TileManager" ).GetComponent<TilesManager>();
		if ( !pTilesManager ) {

			return;

		}

		vAdiacentTiles = new Tile[ 4 ] { null, null, null, null };


//		List < List< int > > vTileMap = pTilesManager.GetTileMap();





		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
