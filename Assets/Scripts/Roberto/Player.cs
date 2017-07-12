// Ref: http://wiki.unity3d.com/index.php/GridMove

using System.Collections; // IEnumerator
using UnityEngine;


public class Player : MonoBehaviour {

	[SerializeField]	private 	float		fMoveSpeed				= 3.0f;
    [SerializeField]	private 	float		fGridSize				= 1.0f;
						private		bool		bAllowDiagonals			= true;
						private		bool		bCorrectDiagonalSpeed	= true;

						private		Vector2		vUserInput				= Vector2.zero;
						private		bool		bIsMoving				= false;
 
    public void Update() {

		// if actually player is not moving
		if ( !bIsMoving ) {

			// Get user input
            vUserInput = new Vector2( Input.GetAxis( "Horizontal" ), Input.GetAxis( "Vertical" ) );

			// if user give an input
            if ( vUserInput != Vector2.zero ) {

				// if diagonal not allowed give priority to axis with major input value
				// ( because used GetAxis input will move from 0 to 1 and viceversa, actually is not intant )
				if ( !bAllowDiagonals ) {

					if ( Mathf.Abs( vUserInput.x ) > Mathf.Abs( vUserInput.y ) ) {
						vUserInput.y = 0;
					}
					else {
						vUserInput.x = 0;
					}

				}

				// Start a coroutine that manage he player movement
                StartCoroutine( Action_Move( this.transform ) );

            }

        }

    }

    public IEnumerator Action_Move(Transform transform) {
		
		// Set as moving
		bIsMoving = true;
		float fInterpolant = 0.0f;

		// Calculate end position
		Vector3 vEndPosition = transform.position + ( new Vector3( System.Math.Sign( vUserInput.x ), System.Math.Sign( vUserInput.y ) ) * fGridSize );

		// if Diagonal movement enabled, scale movement
		float fDiagonalFactor = 1.0f;
		if( bAllowDiagonals && ( vUserInput.x != 0.0f ) && ( vUserInput.y != 0.0f ) ) {
			fDiagonalFactor = 0.7071f;
		}

		// While interpolant is not full
		while ( fInterpolant < 1.0f ) {

			// Increase interpolant
			fInterpolant += Time.deltaTime * ( fMoveSpeed / fGridSize ) * fDiagonalFactor;

			// Set next lerped position
			transform.position = Vector3.Lerp( transform.position, vEndPosition, fInterpolant );

			yield return null;

		}

		// Now that movement is finished, set as not moving and return
		bIsMoving = false;
		yield return 0;
    }

}
