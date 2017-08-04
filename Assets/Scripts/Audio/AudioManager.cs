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

	public void Play(string name){
			
		Sound s = Array.Find (sounds, sound => sound.name == name);
        
        if (s == null ) {
			Debug.LogWarning("Sound " + name + "not found");
			return;
		}
	
			
		s.source.Play();

	}

    public void PlayMusic(string name)
    {
       
        Sound m = Array.Find(music, music => music.name == name);

        if ( m == null)
        {
            Debug.LogWarning("Music " + name + "not found");
            return;
        }


        m.source.Play();

    }

    public	void StopAllSounds() {

		foreach( Sound s in sounds ) s.source.Stop();

	}

    public void StopAllMusics()
    {

        foreach (Sound m in music ) m.source.Stop();

    }

}
			 