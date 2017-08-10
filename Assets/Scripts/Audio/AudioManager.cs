using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct AudioSrc {

	public	AudioSource	pSource;
	public	string		sName;

}


public static class AudioManager {
	
	private static	List< AudioSrc >		vSounds			= null;
    
	private	static	List< AudioSrc >		vMusics			= null;

	private	static	GameObject				pAudioContainer	= null;

	private	static	bool					bInitialized	= false;

	private	static	AudioFader				pAudioFader		= null;

	public	static	bool					Loaded			= false;

	// Use this for initialization
	public	static	IEnumerator  LoadResources () {

		if ( bInitialized ) yield break;
		Loaded = true;
		bInitialized = true;

		vSounds = new List<AudioSrc>();
		vMusics = new List<AudioSrc>();

		pAudioContainer = new GameObject( "AudioContainer" );

		UnityEngine.Object.DontDestroyOnLoad( pAudioContainer );

		pAudioFader = pAudioContainer.AddComponent<AudioFader>();


		// Sounds
		{

			List<AudioClip> vSoundsClip = new List<AudioClip>();
			
			string[] sFileNames = System.IO.Directory.GetFiles(  Application.dataPath + "/Resources/Audio/Sounds" );

			if ( sFileNames == null )
				GLOBALS.Logger.Write ( "No Sounds" );
			else
				GLOBALS.Logger.Write ( "Sounds " + sFileNames.Length );

			foreach( string sFile in sFileNames ) {

				// Skip meta files
				if ( sFile[ sFile.Length - 1 ] == 'a' && sFile[ sFile.Length - 2 ] == 't' && sFile[ sFile.Length - 3 ] == 'e' && sFile[ sFile.Length - 4 ] == 'm' ) continue;

				// Get resource path
				int StartIndex = sFile.LastIndexOf( "Resources" ) + ("Resources/").Length;
				string sResourcesPath = sFile.Substring( StartIndex );
				sResourcesPath = sResourcesPath.Substring( 0, sResourcesPath.LastIndexOf( '.' ) );

				GLOBALS.Logger.Write ( "Loading " + sResourcesPath );

				// Async load
				ResourceRequest pResourceRequest = Resources.LoadAsync<AudioClip>( sResourcesPath );
				yield return pResourceRequest;

				if ( pResourceRequest.asset == null ) {
					Debug.Log( " Cannot load sound  " + sResourcesPath );
					continue;
				}
				else Debug.Log( " Loaded sound  " + sResourcesPath );

				AudioClip pAudioClip = pResourceRequest.asset as AudioClip;
				vSoundsClip.Add( pAudioClip );

				GLOBALS.Logger.Write ( "Loaded " );

				yield return null;

			}

			foreach( AudioClip pAudioClip in vSoundsClip ) {

				AudioSource pAudioSource = pAudioContainer.AddComponent<AudioSource>();
				pAudioSource.clip = pAudioClip;

				AudioSrc pAudioSrc = new AudioSrc();
				pAudioSrc.pSource	= pAudioSource;
				pAudioSrc.sName		= pAudioClip.name;

				vSounds.Add( pAudioSrc );
			}

			if ( vSounds == null || vSounds.Count == 0 ) Debug.LogWarning( "Error loading sounds" );


		}

		/*
		// Musics
		{

			List<AudioClip> vMusicsClip = new List<AudioClip>();

			string[] sFileNames = System.IO.Directory.GetFiles(  Application.dataPath + "/Resources/Audio/Musics" );

			GLOBALS.Logger.Write ( "Musics " + sFileNames.Length );
			
			foreach( string sFile in sFileNames ) {

				if ( sFile[ sFile.Length - 1 ] == 'a' && sFile[ sFile.Length - 2 ] == 't' && sFile[ sFile.Length - 3 ] == 'e' && sFile[ sFile.Length - 4 ] == 'm' ) continue;

				int StartIndex = sFile.LastIndexOf( "Resources" ) + ("Resources/").Length;

				string sResourcesPath = sFile.Substring( StartIndex );

				sResourcesPath = sResourcesPath.Substring( 0, sResourcesPath.LastIndexOf( '.' ) );

				GLOBALS.Logger.Write ( "Loading " + sResourcesPath );

				ResourceRequest pResourceRequest = Resources.LoadAsync<AudioClip>( sResourcesPath );
				yield return pResourceRequest;

				if ( pResourceRequest.asset == null ) {
					Debug.Log( " Cannot load music  " + sResourcesPath );
					continue;
				}
				Debug.Log( " Loaded music  " + sResourcesPath );

				AudioClip pAudioClip = pResourceRequest.asset as AudioClip;
				vMusicsClip.Add( pAudioClip );

				GLOBALS.Logger.Write ( "Loaded " );

				yield return null;

			}

			foreach( AudioClip pAudioClip in vMusicsClip ) {

				AudioSource pAudioSource = pAudioContainer.AddComponent<AudioSource>();
				pAudioSource.clip = pAudioClip;

				AudioSrc pAudioSrc = new AudioSrc();
				pAudioSrc.pSource	= pAudioSource;
				pAudioSrc.sName		= pAudioClip.name;

				vMusics.Add( pAudioSrc );
			}

			if ( vMusics == null || vMusics.Count == 0 ) Debug.LogWarning( "Error loading musics" );

		}
		*/
		

	}

	public	static	AudioSource	FindSound( string name ) {

		return vSounds.Find( sound => sound.sName == name ).pSource;

	}

	public	static	AudioSource Play( AudioSource pAudioSource, bool loop = false ) {

		pAudioSource.loop = loop;
		pAudioSource.Play();

		return pAudioSource;

	}

	public	static	AudioSource Play( string name, bool loop = false ) {
			
		AudioSource pAudioSource = FindSound( name );
        
		if ( pAudioSource == null ) {
			Debug.LogWarning( "Sound " + name + " not found" );
			return null;
		}
	
		return Play( pAudioSource, loop );

	}


	public	static	AudioSource	FindMusic( string name ) {

		return vMusics.Find( music => music.sName == name ).pSource;

	}

	public	static	AudioSource FadeInMusic( AudioSource pAudioSource, float FadeTime = 3.0f ) {

		pAudioSource.loop = true;
		pAudioFader.FadeIn( pAudioSource, FadeTime );

		return pAudioSource;
	}

	public	static	AudioSource FadeInMusic( string name, float FadeTime = 3.0f )
	{
		
		AudioSource pAudioSource = FindMusic( name );
        
		if ( pAudioSource == null ) {
			Debug.LogWarning( "Music " + name + " not found" );
			return null;
		}

		return FadeInMusic( pAudioSource, FadeTime );

	}


	public	static	AudioSource FadeOutMusic( AudioSource pAudioSource, float FadeTime = 3.0f ) {

		pAudioSource.loop = true;
		pAudioFader.FadeOut( pAudioSource, FadeTime );

		return pAudioSource;
	}


	public	static	AudioSource FadeOutMusic( string name, float FadeTime = 3.0f ) {
		
		AudioSource pAudioSource = FindMusic( name );
        
		if ( pAudioSource == null ) {
			Debug.LogWarning( "Music " + name + " not found" );
			return null;
		}

		return FadeOutMusic( pAudioSource, FadeTime );

	}


	public	static	void StopAllSounds() {

		foreach( AudioSrc s in vSounds ) if ( s.pSource != null ) s.pSource.Stop();

	}

	public	static	void StopAllMusics( bool bInstant = true ) {

		foreach( AudioSrc s in vMusics ) if ( s.pSource != null )
			if ( bInstant )
				s.pSource.Stop();
			else
				FadeOutMusic( s.pSource );

	}

}
			 