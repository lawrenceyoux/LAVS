/*
 * Created by SharpDevelop.
 * User: Administrator
 * Date: 1/31/2013
 * Time: 1:30 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;
using System.Diagnostics;

namespace LAVS {
	/// <summary>
	/// Description of Win_Log_Writter.
	/// </summary>
	public class Win_Log_Writter {

		string sSource = "Symantec AntiVirus";
		string sLog = "Application";
		string sEvent = "Sample Event";

		string debugSource = " Anti-Virus LAVS";
		int eid = 5;

		public Win_Log_Writter () {

		}

		public Win_Log_Writter (string source, string log, string se, int eventID) {
			sSource = source;
			sLog = log;
			sEvent = se;
			eid = eventID;
		}

		public void write_to_log_Error (string Message) {
			string full_Message =
				"***********************************************************" + System.Environment.NewLine +
				"* Anti-virus Erorr  " + System.Environment.NewLine +
				"* Immediate action is Required            " + System.Environment.NewLine +
				"***********************************************************" +
				System.Environment.NewLine

				+
				Message;

			if (!EventLog.SourceExists (sSource)) {

				EventLog.CreateEventSource (sSource, sLog);

			}
			EventLog.WriteEntry (debugSource, full_Message, EventLogEntryType.Error, 1001);

		}

		public void write_to_log_info (string Message) {
			string full_Message =
				"***********************************************************" + System.Environment.NewLine +
				"* Anti-virus Information  " + System.Environment.NewLine +
				"* No Action is required           " + System.Environment.NewLine +
				"***********************************************************" +
				System.Environment.NewLine

				+
				Message;

			if (!EventLog.SourceExists (sSource)) {

				EventLog.CreateEventSource (sSource, sLog);

			}
			EventLog.WriteEntry (debugSource, full_Message, EventLogEntryType.Information, 1000);

		}

		public void write_to_log_AVdebug (string debug_string) {
			string full_debug_string =
				"***********************************************************" + System.Environment.NewLine +
				"* Anti-virus Debuging Information  " + System.Environment.NewLine +
				"* Immediate action is Required          " + System.Environment.NewLine +
				"***********************************************************" +
				System.Environment.NewLine

				+
				debug_string;

			if (!EventLog.SourceExists (debugSource)) {

				EventLog.CreateEventSource (debugSource, sLog);

			}
			EventLog.WriteEntry (debugSource, full_debug_string, EventLogEntryType.Information, 1002);

		}
	}
}