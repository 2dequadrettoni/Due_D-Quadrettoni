using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

using System.IO;

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

	private	static	bool					bLoaded			= false;

	public	static	bool					Loaded {
		get { return bLoaded; }
	}

	public	static	void	Initialize() {

		if ( bInitialized ) return;

		bInitialized = true;

		vSounds = new List<AudioSrc>();
		vMusics = new List<AudioSrc>();

		pAudioContainer = new GameObject( "AudioContainer" );

		UnityEngine.Object.DontDestroyOnLoad( pAudioContainer );

		pAudioFader = pAudioContainer.AddComponent<AudioFader>();

	}


	// Use this for initialization
	public	static	IEnumerator  LoadResources () {

		if ( !bInitialized ) yield break;
		
		GLOBALS.Logger.Write( "AudioManager:Loading audio resources" );

		// Sounds
		{
			GLOBALS.Logger.Write( "AudioManager:Loading Sounds" );

			ResourceRequest pResourceRequest = Resources.LoadAsync ( "Sounds" );

			while( !pResourceRequest.isDone ) yield return pResourceRequest;

			Sounds_Scriptable pSoundsResources = pResourceRequest.asset as Sounds_Scriptable;

			foreach( string sAudioClip in pSoundsResources.vSoundsClip ) { 

				if ( sAudioClip == null ) {
					Debug.Log( "music loading failed" );
					continue;
				}

				GLOBALS.Logger.Write( "AudioManager:Loading " + sAudioClip );

				ResourceRequest pResourceRequestAsset = Resources.LoadAsync<AudioClip>( sAudioClip );

				while( !pResourceRequestAsset.isDone ) yield return pResourceRequestAsset;

				AudioClip pAudioClip = pResourceRequestAsset.asset as AudioClip;

				AudioSource pAudioSource = pAudioContainer.AddComponent<AudioSource>();
				pAudioSource.clip = pAudioClip;

				AudioSrc pAudioSrc = new AudioSrc();
				pAudioSrc.pSource	= pAudioSource;
				pAudioSrc.sName		= pAudioClip.name;

				vSounds.Add( pAudioSrc );

				yield return null;

//				Debug.Log( " Loaded  " + pAudioClip.name );
				
			}

			if ( vSounds == null || vSounds.Count == 0 ) Debug.LogWarning( "Error loading sounds" );

			GLOBALS.Logger.Write( "AudioManager:Sounds loaded" );

		}
		
		// Musics
		{
			GLOBALS.Logger.Write( "AudioManager:Loading Musics" );

			ResourceRequest pResourceRequest = Resources.LoadAsync ( "Musics" );
			
			while( !pResourceRequest.isDone ) yield return pResourceRequest;

			Musics_Scriptable pMusicsResources = pResourceRequest.asset as Musics_Scriptable;

			foreach( string sAudioClip in pMusicsResources.vMusicsClip ) { 

				if ( sAudioClip == null ) {
					Debug.Log( "music loading failed" );
					continue;
				}

				GLOBALS.Logger.Write( "AudioManager:Loading " + sAudioClip );

				ResourceRequest pResourceRequestAsset = Resources.LoadAsync<AudioClip>( sAudioClip );

				while( !pResourceRequestAsset.isDone ) yield return pResourceRequestAsset;

				AudioClip pAudioClip = pResourceRequestAsset.asset as AudioClip;

				AudioSource pAudioSource = pAudioContainer.AddComponent<AudioSource>();
				pAudioSource.clip = pAudioClip;

				AudioSrc pAudioSrc = new AudioSrc();
				pAudioSrc.pSource	= pAudioSource;
				pAudioSrc.sName		= pAudioClip.name;

				vMusics.Add( pAudioSrc );

				yield return null;

//				Debug.Log( " Loaded  " + pAudioClip.name );
				
			}

			if ( vMusics == null || vMusics.Count == 0 ) Debug.LogWarning( "Error loading musics" );

			GLOBALS.Logger.Write( "AudioManager:Musics loaded" );

		}
		
		Debug.Log( "loaded" );
		bLoaded = true;

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

	public	static	AudioSource FadeInMusic( AudioSource pAudioSource, float FadeTime = 4.0f ) {

		pAudioSource.loop = true;
		pAudioFader.FadeIn( pAudioSource, FadeTime );

		return pAudioSource;
	}

	public	static	AudioSource FadeInMusic( string name, float FadeTime = 4.0f )
	{
		
		AudioSource pAudioSource = FindMusic( name );
        
		if ( pAudioSource == null ) {
			Debug.LogWarning( "Music " + name + " not found" );
			return null;
		}

		return FadeInMusic( pAudioSource, FadeTime );

	}


	public	static	AudioSource FadeOutMusic( AudioSource pAudioSource, float FadeTime = 4.0f ) {

		pAudioSource.loop = true;
		pAudioFader.FadeOut( pAudioSource, FadeTime );

		return pAudioSource;
	}


	public	static	AudioSource FadeOutMusic( string name, float FadeTime = 4.0f ) {
		
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

		foreach( AudioSrc s in vMusics )
			if ( s.pSource != null && s.pSource.isPlaying ) {
				if ( bInstant )
					s.pSource.Stop();
				else
					FadeOutMusic( s.pSource );
			}

	}

}
			 