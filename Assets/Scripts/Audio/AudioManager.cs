using UnityEngine.Audio;
using UnityEngine;
using System;
public class AudioManager : MonoBehaviour {
	
	public Sound[] sounds;
    
    public Sound[] music;

	public static AudioManager instance;
	// Use this for initialization
	void Awake () {
		if (instance == null)
			instance = this;
		else {
			Destroy (gameObject);
			return;
		}
        
		DontDestroyOnLoad (gameObject);
		foreach (Sound s in sounds) {

			s.source = gameObject.AddComponent<AudioSource> ();

			s.source.clip = s.clip;

			s.source.volume = s.volume;

			s.source.pitch = s.pitch;

			s.source.loop = s.loop;


		}


        DontDestroyOnLoad(gameObject);
        foreach (Sound m in music)
        {

            m.source = gameObject.AddComponent<AudioSource>();

            m.source.clip = m.clip;

            m.source.volume = m.volume;

            m.source.pitch = m.pitch;

            m.source.loop = m.loop;


        }

    }

	public AudioSource Play(string name, bool loop = false){
			
		Sound s = Array.Find (sounds, sound => sound.name == name);
        
        if (s == null ) {
			Debug.LogWarning("Sound " + name + "not found");
			return null;
		}
	
		s.source.loop = loop;
		s.source.Play();
		return s.source;

	}

    public void PlayMusic(string name)
    {
		
        Sound m = Array.Find(music, music => music.name == name);
        if ( m == null )
        {
            Debug.LogWarning("Music " + name + "not found");
            return;
        }
		
		m.source.loop= true;
        m.source.Play();

    }

    public	void StopAll() {

		foreach( Sound s in sounds ) if ( s != null && s.source != null ) s.source.Stop();

	}

    public void StopAllMusics()
    {

        foreach (Sound m in music ) if ( m != null && m.source != null ) m.source.Stop();

    }

}
			 