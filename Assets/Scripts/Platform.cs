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

	public	float	fMoveSpeed			= 0.0f;

	// Use this for initialization
	void Start () {

		vStartPosition	= transform.GetChild( 0 ).position;
		vEndPosition	= transform.GetChild( 1 ).position;
		transform.position = vStartPosition;

	}
	
	// Update is called once per frame
	void UpdatePosition() {

		fInterpolant += ( Time.deltaTime * fMoveSpeed ) * iDirection;

		if ( ( fInterpolant > 0.9999f ) || ( fInterpolant < 0.1111f ) ) {
			iDirection *= -1;
			fInterpolant = Mathf.RoundToInt( fInterpolant );
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


	private void OnTriggerEnter( Collider other ) {
		
		if ( other.tag == "Player" ) {

			Player pScript = other.GetComponent<Player>();
			if ( !pScript.Linked ) {
				pScript.Link( this );
				HasPlayerInside = true;
			}
		}

	}

	private void OnTriggerExit( Collider other ) {
		
		if ( other.tag == "Player" ) {

			Player pScript = other.GetComponent<Player>();
			if ( pScript.Linked ) {
				pScript.UnLink( this );
				HasPlayerInside = false;
			}
		}

	}
}
