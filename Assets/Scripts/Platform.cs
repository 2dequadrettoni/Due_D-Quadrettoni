using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Platform : MonoBehaviour {

	public  bool    HasPlayerInside		= false;
	public	bool	CanUnLink			= false;
	private	Vector3	vStartPosition		= Vector3.zero;
	private	Vector3	vEndPosition		= Vector3.zero;
	private	float	fInterpolant		= 0.0f;

	private	int		iDirection			= 1;

	public	float	fMoveSpeed			= 5.0f;

	public	bool	bActive = false;

	// Use this for initialization
	void Start () {

		vStartPosition	= transform.GetChild( 0 ).position;
		vEndPosition	= transform.GetChild( 1 ).position;
		transform.position = vStartPosition;

	}
	
	// Update is called once per frame
	public void UpdatePosition() {

		if ( !bActive ) return;

		fInterpolant += ( Time.deltaTime * fMoveSpeed ) * iDirection;

		if ( ( fInterpolant < 0.0f ) || ( fInterpolant > 1.0f ) ) {
			bActive = false;
			iDirection *= -1;
			fInterpolant = Mathf.Clamp01( fInterpolant );
		}

		CanUnLink = true;
		if ( ( fInterpolant > 0.2f ) || ( fInterpolant < 0.8f ) ) CanUnLink = false;

		Vector3 vNewPosition = new Vector3(
			Mathf.Lerp( vStartPosition.x, vEndPosition.x, fInterpolant ),
			Mathf.Lerp( vStartPosition.y, vEndPosition.y, fInterpolant ),
			Mathf.Lerp( vStartPosition.z, vEndPosition.z, fInterpolant )
		);

		transform.position = vNewPosition;

	}

	public void	OnUse( Player User ) {
		print( "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA" );
		bActive = true;

	}


	private void OnTriggerEnter( Collider other ) {
		
		if ( bActive && other.tag == "Player" ) {

			Player pScript = other.GetComponent<Player>();
			if ( !pScript.Linked ) {
				pScript.Link( this );
				HasPlayerInside = true;
			}
		}

	}

	private void OnTriggerExit( Collider other ) {
		
		if ( bActive && other.tag == "Player" ) {

			Player pScript = other.GetComponent<Player>();
			if ( pScript.Linked ) {
				pScript.UnLink( this );
				HasPlayerInside = false;
			}
		}

	}
}
