using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GO_Position {
	public GameObject	obj					= null;
	public Vector3		Original_Position	= Vector3.zero;
	public Vector3		Decom_Position		= Vector3.zero;
	public float		Interpolant			= 0.0f;
	public float		Distance			= 0.0f;
	public bool			Done				= false;

	public GO_Position( GameObject oobj, Vector3 Original_Position, Vector3 Decom_Position ) {
		this.obj = oobj;
		this.Original_Position = Original_Position;
		this.Decom_Position = Decom_Position;
	}
}



public class GameManager : MonoBehaviour {

	private		FinalTile		pFinalTile1						= null;
	private		FinalTile		pFinalTile2						= null;

	public static bool			InTutorialSequence				= false;
	public static int			TutorialStep					= 0;

	[Header("Tutorial Sprites")]
	public		Sprite			pTutorial_0_MoveSprite			= null;
	public		Sprite			pTutorial_1_SwitchSprite		= null;
	public		Sprite			pTutorial_2_NextStageSprite		= null;
	public		Sprite			pTutorial_3_PlaySprite			= null;
	public		Sprite			pTutorial_4_RestartSprite		= null;

	private		List<GO_Position> vObjects						= null;

	private		bool			bInLevelTransition				= false;

	private void Start() {

		Fader.Hide();
		StartCoroutine( Fader.Show( 2.0f ) );

		pFinalTile1 = transform.GetChild( 0 ).GetComponent<FinalTile>();
		pFinalTile2 = transform.GetChild( 1 ).GetComponent<FinalTile>();

		pFinalTile1.iDesiredPlayerID = 1;
		pFinalTile2.iDesiredPlayerID = 2;

		int iSceneIndex = SceneManager.GetActiveScene().buildIndex;
		
		// Level Music
		AudioSource p = AudioManager.FindMusic( "Level" + iSceneIndex );
		if ( !p.isPlaying ) {

			AudioManager.StopAllMusics();

			p.loop = true;
			AudioManager.FadeInMusic( p, 8.0f );

		}

		GLOBALS.CurrentLevel = iSceneIndex;

		// Tutorials
		if ( iSceneIndex == 1 ) {
			InTutorialSequence				= true;
			UI.TutorialLvl					= true;
			Door.TutorialLvl				= true;
			FinalTile.TutorialLvl			= true;
			Switcher.TutorialLvl_Plane		= true;
			if ( TutorialStep < 4 )
				GLOBALS.TutorialOverride	= true;
			else {
				GLOBALS.TutorialOverride	= false;
			}
		}
		else {
			InTutorialSequence				= false;
			UI.TutorialLvl					= false;
			Door.TutorialLvl				= false;
			FinalTile.TutorialLvl			= false;
			Switcher.TutorialLvl_Plane		= false;
			GLOBALS.TutorialOverride		= false;
		}
		if ( iSceneIndex == 2 ) {
			Switcher.TutorialLvl			= true;
			Platform.TutorialLvl			= true;
		}
		else {
			Switcher.TutorialLvl			= false;
			Platform.TutorialLvl			= false;
		}
		if ( iSceneIndex == 6 ) {
			Key.TutorialLvl					= true;
		}
		else {
			Key.TutorialLvl					= false;
		}

		StartLevelComposition();

	}


	private	void	StartLevelComposition() {

		GameObject[] vKeys				= GameObject.FindGameObjectsWithTag("Key");
		GameObject[] vPlatforms			= GameObject.FindGameObjectsWithTag("Platform");
		GameObject[] vSwitchers			= GameObject.FindGameObjectsWithTag("Switcher");
		GameObject[] vSwitchers_Plane	= GameObject.FindGameObjectsWithTag("Plane_Switcher");
		GameObject[] vDoors				= GameObject.FindGameObjectsWithTag("Door");
		GameObject[] vTiles				= GameObject.FindGameObjectsWithTag("Tiles");
		GameObject[] vOthers			= GameObject.FindGameObjectsWithTag("Other");

		vObjects = new List<GO_Position>( vKeys.Length + vPlatforms.Length + vSwitchers.Length + vSwitchers_Plane.Length + vDoors.Length + vTiles.Length + vOthers.Length );

		{
			foreach( GameObject GO in vKeys				)	{ AddObj( GO ); }
			foreach( GameObject GO in vPlatforms		)	{ AddObj( GO ); }
			foreach( GameObject GO in vSwitchers		)	{ AddObj( GO ); }
			foreach( GameObject GO in vSwitchers_Plane	)	{ AddObj( GO ); }
			foreach( GameObject GO in vDoors			)	{ AddObj( GO ); }
			foreach( GameObject GO in vTiles			)	{ if ( !GO.transform.parent || ( GO.transform.parent && GO.transform.parent.tag != "Platform" ) ) AddObj( GO ); }
			foreach( GameObject GO in vOthers			)	{ AddObj( GO ); }
		}

		StartCoroutine( LevelCompositionCoroutine() );

	}


	private void AddObj( GameObject o ) {


		Vector3 vDecom_Position = Camera.main.ViewportToWorldPoint(
			new Vector3(
				Random.Range( 1.1f, 3.5f ) * ( Random.Range( 0, 100 ) > 50 ? 1 : -1 ),
				Random.Range( 1.1f, 3.5f ) * ( Random.Range( 0, 100 ) > 50 ? 1 : -1 ),
				Random.Range( 1.1f, 3.5f ) * ( Random.Range( 0, 100 ) > 50 ? 1 : -1 )
			)
		);

		/*
		Vector3 vDecom_Position = new Vector3 ( 
			Random.Range( -0.2f, 0.3f ) * vObjects.Count + Random.Range( -20, 21 ),
			0.0f,
			Random.Range( -0.2f, 0.3f ) * vObjects.Count + Random.Range( -20, 21 )
		);
		*/
		GO_Position p = new GO_Position( o, o.transform.position, vDecom_Position );
		p.Distance = Vector3.Distance( o.transform.position, vDecom_Position );
		vObjects.Add( p );

		o.transform.position = vDecom_Position;

	}

	IEnumerator LevelCompositionCoroutine() {

		int iCurrentObjectLeft = vObjects.Count;


		GLOBALS.Player1.Hide();
		GLOBALS.Player2.Hide();


		UnityEditor.Selection.activeObject = GLOBALS.Player2.transform.GetChild(0).gameObject;

		while ( iCurrentObjectLeft > 0 ) {

			for ( int i = 0; i < vObjects.Count; i++ ) {
				GO_Position p = vObjects[i];

				if ( p.Done ) continue;

				p.Interpolant += Time.unscaledDeltaTime / p.Distance * 50f;
					
				if ( p.Interpolant > 1.0f ) {
					p.Done = true;
					iCurrentObjectLeft--;
					print( iCurrentObjectLeft );
				}
				
				p.obj.transform.position = Vector3.Lerp( p.Decom_Position, p.Original_Position, p.Interpolant );

			}

			yield return null;
		}

		for ( int i = 0; i < vObjects.Count; i++ ) vObjects[i].Done = false;

		GLOBALS.Player1.Show();
		GLOBALS.Player2.Show();

		GLOBALS.Player1.Spawn();
		GLOBALS.Player2.Spawn();

		bInLevelTransition = false;

	}

	IEnumerator LevelDecompositionCoroutine() {

		bInLevelTransition = true;

		int iCurrentObjectLeft = vObjects.Count;

		yield return new WaitForSecondsRealtime( 1.0f );

		while ( iCurrentObjectLeft > 0 ) {

			for ( int i = 0; i < vObjects.Count; i++ ) {
				GO_Position p = vObjects[i];

				if ( p.Done ) continue;

				p.Interpolant += Time.unscaledDeltaTime / p.Distance * 200f;
					
				if ( p.Interpolant > 1.0f ) {
					p.Done = true;
					p.Interpolant = 0.0f;
					iCurrentObjectLeft--;
					print( iCurrentObjectLeft );
				}
				
				p.obj.transform.position = Vector3.Lerp( p.Original_Position, p.Decom_Position, p.Interpolant );

			}

			yield return null;
		}

		GLOBALS.Player1.Hide();
		GLOBALS.Player2.Hide();

	}

	public	void	NextTutorial() {

		TutorialStep++;
		switch( TutorialStep ) {

			case 1: {  GLOBALS.TutorialSlot.sprite = pTutorial_0_MoveSprite;		break; }
			case 2: {  GLOBALS.TutorialSlot.sprite = pTutorial_1_SwitchSprite;		break; }
			case 3: {  GLOBALS.TutorialSlot.sprite = pTutorial_2_NextStageSprite;	break; }
			case 4: {  GLOBALS.TutorialSlot.sprite = pTutorial_3_PlaySprite;		break; }
			case 5: {  GLOBALS.TutorialSlot.sprite = pTutorial_4_RestartSprite;		break; }
			default: { GLOBALS.TutorialSlot.sprite = null;							break; }
		}

	}

	static public void NextScene() {

		if ( GLOBALS.StageManager.IsPlaying )
				GLOBALS.StageManager.Stop( true );

		SceneManager.LoadScene( GLOBALS.CurrentLevel + 1 );

	}

	private void Update() {


		if ( Input.GetKeyDown( KeyCode.Escape ) ) GLOBALS.UI.OnPause();


		if ( GLOBALS.StageManager.IsPlaying && pFinalTile1.IsInside && pFinalTile2.IsInside ) {

			SaveLoad.SaveLevel( GLOBALS.CurrentLevel );

			AudioManager.Play( "Level_End" );

			GLOBALS.StageManager.Stop( false );

			GLOBALS.Player1.IsInAnimationOverride = true;
			GLOBALS.Player1.PlayWinAnimation();

			GLOBALS.Player2.IsInAnimationOverride = true;
			GLOBALS.Player2.PlayWinAnimation();

			StartCoroutine ( Fader.Hide( 2, () => GameManager.NextScene() ) );

			StartCoroutine( LevelDecompositionCoroutine() );

		}

	}



	public void RestartGame() {

		if ( bInLevelTransition ) return;

		if ( GameManager.InTutorialSequence && GameManager.TutorialStep < 5 ) return;

		AudioManager.StopAllSounds();
//		AudioManager.StopAllMusics();

		GLOBALS.CurrentLevel--;

		StartCoroutine( LevelDecompositionCoroutine() );

		StartCoroutine ( Fader.Hide( 2, () => GameManager.NextScene() ) );

	}




	public static void	Exit( string LogMessage = null ) {

		if ( LogMessage != null ) GLOBALS.Logger.Write( LogMessage );
		Exit();

	}

	public static void	Exit() {

#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
#else
		Application.Quit();
#endif

	}


	private void OnDestroy() {
		
		AudioManager.StopAllSounds();
		AudioManager.StopAllMusics();

	}


}
