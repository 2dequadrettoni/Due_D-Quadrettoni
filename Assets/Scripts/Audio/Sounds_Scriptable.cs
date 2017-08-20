 
using System.Collections.Generic;
using UnityEngine;


public class Sounds_Scriptable : ScriptableObject {


    public List<string> vSoundsClip;


#if UNITY_EDITOR
	private void Awake() {
		
		AudioClip[] vAudioClips = Resources.LoadAll<AudioClip>( "Audio/Sounds");
		vSoundsClip = new List<string>();
		foreach( AudioClip p in vAudioClips ) vSoundsClip.Add( "Audio/Sounds/" + p.name );

	}
#endif

}
