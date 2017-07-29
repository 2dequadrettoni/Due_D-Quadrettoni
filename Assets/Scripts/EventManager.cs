using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour {

	private		List<Door>			vDoors			= null;
	private		List<Platform>		vPlatforms		= null;
	
	public	void	AddReceiver( Door o ) {
		if ( vDoors == null ) vDoors = new List<Door>();
		vDoors.Add( o );
	}

	public	void	AddReceiver( Platform o ) {
		if ( vPlatforms == null ) vPlatforms = new List<Platform>();
		vPlatforms.Add( o );
	}

	public void	SentEvent( Switcher o ) {

		foreach( Door d in vDoors )			d.OnEvent( o );

		foreach( Platform p in vPlatforms ) p.OnEvent( o );

	}

}
