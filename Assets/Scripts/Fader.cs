
using System.Collections.Generic;
using System.Collections; // IEnumerator
using UnityEngine;
using UnityEngine.UI;

#pragma warning disable CS0162 // Unreachable code detected
#pragma warning disable CS0414 // Var assigned but never used

public static class Fader {

	private	static	Color	_StartColor;

	private	static	Color	_EndColor;

	private	static	float	fColorinterpolant	= 0.0f;
	public	static	float	ColorIntrerpolant {
		get { return fColorinterpolant; }
	}

	private	static	Image	pMainImage			= null;

	private	static	bool	bFadeCompleted		= false;
	public	static	bool	FadeCompleted {
		get { return bFadeCompleted; }
	}

	private	static	bool	bInitialized		= false;



	public	static	void	Initialize() {

		if ( bInitialized ) return;
		bInitialized = true;

		_StartColor = _EndColor = Color.black;

		GameObject pImageOBJ = MonoBehaviour.Instantiate( Resources.Load( "Fader" ) ) as GameObject;
		pMainImage = pImageOBJ.transform.GetChild(0).GetComponent<Image>();
		pMainImage.color = new Color ( 0, 0, 0, 0 );

		UnityEngine.Object.DontDestroyOnLoad( pImageOBJ );

	}




	public	static	void	SetColors ( Color StartColor, Color EndColor ) {
		_StartColor = StartColor;
		_EndColor = EndColor;
	}


	/// <summary>Black image disappear showing everything</summary>
	public	static	IEnumerator	Show( float fFadeTime = 2.0f, System.Action pEndCallback = null ) {

		if ( !bInitialized ) Initialize();

		bFadeCompleted = false;
		pMainImage.raycastTarget = true;

		Debug.Log( "Starting fade out of black image" );
		while ( pMainImage.color.a > 0.0f ) {

			pMainImage.color = new Color ( 0, 0, 0, pMainImage.color.a - Time.unscaledDeltaTime / fFadeTime );

			yield return null;

		}

		Debug.Log( "Finished fade out of black image" );
		pMainImage.color = new Color ( 0, 0, 0, 0 );
		pMainImage.raycastTarget = false;
		bFadeCompleted = true;

		if ( pEndCallback != null ) pEndCallback();

	}


	/// <summary>Black image appear hiding everything</summary>
	public	static	IEnumerator	Hide( float fFadeTime = 2.0f, System.Action pEndCallback = null ) {

		if ( !bInitialized ) Initialize();

		bFadeCompleted = false;
		pMainImage.raycastTarget = true;

		Debug.Log( "Starting fade in of black image" );
		while ( pMainImage.color.a < 1.0f ) {;

			pMainImage.color = new Color ( 0, 0, 0, pMainImage.color.a + Time.unscaledDeltaTime / fFadeTime );

			yield return null;

		}
		Debug.Log( "Finished fade in of black image" );

		pMainImage.color = new Color ( 0, 0, 0, 1 );
		pMainImage.raycastTarget = false;
		bFadeCompleted = true;

		if ( pEndCallback != null ) pEndCallback();

	}


	public	static	void	Show() {

		pMainImage.raycastTarget = false;
		pMainImage.color = new Color ( 0, 0, 0, 0 );

	}


	public	static	void	Hide() {

		pMainImage.raycastTarget = true;
		pMainImage.color = new Color ( 0, 0, 0, 1 );

	}





}

#pragma warning restore CS0414 // Var assigned but never used
#pragma warning restore CS0162 // Unreachable code detected