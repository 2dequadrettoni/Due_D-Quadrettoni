
using UnityEngine;

#if UNITY_EDITOR
	using UnityEditor;
#endif


public class AudioResources {

	#if UNITY_EDITOR
	[UnityEditor.MenuItem("Assets/Create/Create audio assets")]
	static void Create() {

			AssetDatabase.CreateAsset( ScriptableObject.CreateInstance<Sounds_Scriptable>(), "Assets/Resources/Sounds.asset");
			AssetDatabase.CreateAsset( ScriptableObject.CreateInstance<Musics_Scriptable>(), "Assets/Resources/Musics.asset");

	}

	[UnityEditor.MenuItem("Assets/Create/Save assets")]
	static void Save() {
		AssetDatabase.SaveAssets();
	}
	#endif


}
 