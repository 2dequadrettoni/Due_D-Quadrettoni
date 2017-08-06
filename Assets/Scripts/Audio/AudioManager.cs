﻿
using System;
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

	// Use this for initialization
	public	static	void  Initialize ()
	{
		Debug.Log( "Audiomanager starting" );

		if ( bInitialized ) return;

		vSounds = new List<AudioSrc>();
		vMusics = new List<AudioSrc>();

		pAudioContainer = new GameObject( "AudioContainer" );

		// Sounds
		{
			AudioClip[] vSoundsClip = Resources.LoadAll<AudioClip>( "Audio/Sounds" );

			foreach( AudioClip pAudioClip in vSoundsClip ) {

				AudioSource pAudioSource = pAudioContainer.AddComponent<AudioSource>();
				pAudioSource.clip = pAudioClip;

				AudioSrc pAudioSrc = new AudioSrc();
				pAudioSrc.pSource	= pAudioSource;
				pAudioSrc.sName		= pAudioClip.name;

				vSounds.Add( pAudioSrc );
			}

			if ( vSounds == null || vSounds.Count == 0 ) Debug.LogWarning( "Error loading sounds" );

			Debug.Log( "sounds loaded " + vSounds.Count );

		}
		
		// Musics
		{
			AudioClip[] vMusicsClip = Resources.LoadAll<AudioClip>( "Audio/Musics" );

			foreach( AudioClip pAudioClip in vMusicsClip ) {

				AudioSource pAudioSource = pAudioContainer.AddComponent<AudioSource>();
				pAudioSource.clip = pAudioClip;

				AudioSrc pAudioSrc = new AudioSrc();
				pAudioSrc.pSource	= pAudioSource;
				pAudioSrc.sName		= pAudioClip.name;

				vMusics.Add( pAudioSrc );
			}

			if ( vMusics == null || vMusics.Count == 0 ) Debug.LogWarning( "Error loading musics" );

			Debug.Log( "musics loaded " + vMusics.Count );

		}

		bInitialized = true;

    }

	public	static	AudioSource	FindSound( string name ) {

		return vSounds.Find( sound => sound.sName == name ).pSource;

	}


	public	static	AudioSource Play( string name, bool loop = false )
	{
			
		AudioSource pAudioSource = FindSound( name );
        
        if ( pAudioSource == null ) {
			Debug.LogWarning( "Sound " + name + " not found" );
			return null;
		}
	
		pAudioSource.loop = loop;
		pAudioSource.Play();
		return pAudioSource;

	}


	public	static	AudioSource	FindMusic( string name ) {

		return vMusics.Find( sound => sound.sName == name ).pSource;

	}

    public	static	AudioSource PlayMusic( string name )
    {
		
		AudioSource pAudioSource = FindMusic( name );
        
        if ( pAudioSource == null ) {
			Debug.LogWarning( "Music " + name + " not found" );
			return null;
		}
	
		pAudioSource.loop = true;
		pAudioSource.Play();
		return pAudioSource;

    }

    public	static	void StopAllSounds() {

		foreach( AudioSrc s in vSounds ) if ( s.pSource != null ) s.pSource.Stop();

	}

    public	static	void StopAllMusics()
    {

       foreach( AudioSrc s in vMusics ) if ( s.pSource != null ) s.pSource.Stop();

    }

}
			 