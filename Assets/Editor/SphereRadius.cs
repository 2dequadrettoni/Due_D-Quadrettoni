using UnityEditor;
using UnityEngine;

//Select the dependencies of the found GameObject
public class SphereRadius : EditorWindow
{
	GameObject			pObject		= null;
	SphereCollider		pCollider	= null;
	Camera				pCamera		= null;
	float				fRadius		= 0.5f;

	// Menu Tree
    [MenuItem("Window/Tools/Sphere Radius")]

	// Window creation
    static void Init() {

		EditorWindow window = GetWindow( typeof( SphereRadius ) );
		window.position = new Rect( 800, 400, 400, 0 );
		window.Show();

	}

	// Video update
	private void OnInspectorUpdate() {

		Repaint();

	}

	// Window polulation
	private void OnGUI() {

		pCamera = SceneView.lastActiveSceneView.camera;

		if ( pObject ) {

			// Bind Slider to sphere collider radius
			GUILayout.Label ( "Radius", EditorStyles.boldLabel );
			fRadius = EditorGUILayout.Slider ( "Slider", fRadius, 0, 10 );

			pCollider.radius = fRadius;
		}

		if( ( !pObject ) &&  GUILayout.Button( "Create one" ) ) {

			// Create object
			pObject = new GameObject( "Sphere", typeof( SphereCollider) );

			// Get Collider component
			pCollider = pObject.GetComponent<SphereCollider>();

			// If no onject selected
			if ( !Selection.activeObject ) {

				// Set position to center of editor view
				pObject.transform.position = new Vector2( pCamera.transform.position.x, pCamera.transform.position.y );

			}

			else {

				// set position to object one
				pObject.transform.position = new Vector2( 
					( ( GameObject ) Selection.activeObject ).transform.position.x, 
					( ( GameObject ) Selection.activeObject ).transform.position.y
				);
			}

			Selection.activeObject = pObject;

			// Focus on it
			SceneView.lastActiveSceneView.FrameSelected();

		}
	}

	// Called when destroyed
	private void OnDestroy() {
		
		DestroyImmediate( pObject );

	}


}