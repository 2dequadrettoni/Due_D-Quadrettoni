﻿using System.Collections;
using System.Collections.Generic;


using System;
using System.IO;


/// <summary>
///  My logger class
/// </summary>
public class Logger {

	private	const string LOG_FILE_NAME = "game.log";

	public Logger(  ) {

		new FileStream( LOG_FILE_NAME, FileMode.OpenOrCreate, FileAccess.ReadWrite ).Close();

		StreamWriter pWriter = new StreamWriter( LOG_FILE_NAME );
		pWriter.WriteLine( "Logger Initialized" );
		pWriter.Flush();
		pWriter.Close();
	}

	public void Write( string logMessage ) {
		StreamWriter pWriter = new StreamWriter( LOG_FILE_NAME, true );
		pWriter.WriteLine( UnityEngine.Time.unscaledTime + " - " + logMessage );
		pWriter.Flush();
		pWriter.Close();
	}

}
