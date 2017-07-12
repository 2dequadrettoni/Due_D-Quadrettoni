using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilesManager : MonoBehaviour {


	List < List< Tile > > vTileMap;


	// Use this for initialization
	void Start () {

		vTileMap = new List < List< Tile > >();
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	/*

	public bool AddTile( Tile pTile, Vector2 vPosition ) {

		if ( vPosition.x > vTileMap.Count ) {
			vTileMap.Add( new List<Tile>() );
		}

		vTileMap[ (int)vPosition.x ].Add( pTile );

		return false;

	}


	public Tile GetTile( Vector2 vPosition ) {

		if ( vPosition.x < vTileMap.Count ) {

			List< Tile > vVerticals = vTileMap[ (int) vPosition.x ];

			if ( vPosition.y < vVerticals.Count )

				return vVerticals[ (int) vPosition.y ];

		}

		return null;

	}

	*/

	public bool CanMove( Vector2 vPosition, Vector2 vDirection ) {

		return false;

	}


}
