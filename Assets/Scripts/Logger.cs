using System.Collections;
using System.Collections.Generic;


using System;
using System.IO;


/// <summary>
///  My logger class
/// </summary>
public class Logger {

	private	const string LOG_FILE_NAME = "game.log";

	private	StreamWriter pWriter = null;

	public Logger(  ) {

		pWriter = new StreamWriter( LOG_FILE_NAME );
	}

	public void Write( string logMessage ) {
		pWriter.WriteLine( logMessage + "\n" );
		pWriter.Flush();
	}

	public void Flush() {
		pWriter.Flush();
	}

	public void Close() {
		pWriter.Close();
	}

}
