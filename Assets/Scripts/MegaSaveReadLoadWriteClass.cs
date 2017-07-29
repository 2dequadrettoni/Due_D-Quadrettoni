
using System;
using System.IO;

public class MegaSaveReadLoadWriteClass {

	const string SAVE_FILE_NAME = "lvl.asd";

	public int GetSavedlevel() {

		if ( !System.IO.File.Exists( SAVE_FILE_NAME ) ) return -1;

		string value = File.ReadAllText( SAVE_FILE_NAME );

		int iLevel = 0;

		if ( !Int32.TryParse( value, out iLevel ) ) return -1;

		return iLevel;


	}

	public void SaveLevel( int iCurrent_Level ) {
		
		File.WriteAllText( SAVE_FILE_NAME, iCurrent_Level.ToString() );

	}

}
