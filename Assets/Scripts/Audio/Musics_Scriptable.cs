 
using System.Collections.Generic;
using UnityEngine;


public class Musics_Scriptable : ScriptableObject {


	public List<string> vMusicsClip;


#if UNITY_EDITOR
	private void Awake() {
		
		AudioClip[] vAudioClips = Resources.LoadAll<AudioClip>( "Audio/Musics");
		vMusicsClip = new List<string>();
		foreach( AudioClip p in vAudioClips ) vMusicsClip.Add( "Audio/Musics/" + p.name );

	}
#endif

}
