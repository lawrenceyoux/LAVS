/****************************** Module Header ******************************\
Module Name:  .cs
Project:      LAVS
Author:       Xiangyu You 

/// <summary>  
///  
/// </summary>  

THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED 
WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
\***************************************************************************/


using System;
using System.Reflection;
using System.Runtime.InteropServices;
using Microsoft.Win32;

namespace LAVS {
	/// <summary>
	/// Description of RegKey_Handler.
	/// </summary>
	public class RegKey_Handler {
		private Win_Log_Writter Log_Writter;
		object value1 = "";
		RegistryKey key;
		object installTime_Object;

		[DllImport ("advapi32.dll", SetLastError = true)]
		static extern int RegDisableReflectionKey (IntPtr hBase);

		public RegKey_Handler () {
			Log_Writter = new Win_Log_Writter ();
			installTime_Object = null;

		}

		public object Retrive_FEP_RegKey_REGBINARY (string key_name) {
			try {

				key = Registry.LocalMachine.OpenSubKey ("SOFTWARE\\Microsoft\\Microsoft Antimalware\\Signature Updates\\");
				Type type = typeof (RegistryKey);
				FieldInfo fi = type.GetField (
					"hkey",
					BindingFlags.NonPublic | BindingFlags.Instance);
				SafeHandle handle = (SafeHandle) fi.GetValue (key);
				IntPtr realHandle = handle.DangerousGetHandle ();
				int errorCode = RegDisableReflectionKey (handle.DangerousGetHandle ());

				value1 = key.GetValue (key_name);
				return value1;

			} catch (Exception e) {
				Log_Writter.write_to_log_AVdebug ("Registery Read failed for Microsoft End Point Protection, if this software is not installed, you can safely ignore this message");

				return null;
			}

		}

		public object Retrive_FCS_RegKey_REGBINARY (string key_name) {
			try {

				key = Registry.LocalMachine.OpenSubKey ("SOFTWARE\\Microsoft\\Microsoft Forefront\\Client Security\\1.0\\AM\\Signature Updates\\");
				Type type = typeof (RegistryKey);
				FieldInfo fi = type.GetField (
					"hkey",
					BindingFlags.NonPublic | BindingFlags.Instance);
				SafeHandle handle = (SafeHandle) fi.GetValue (key);
				IntPtr realHandle = handle.DangerousGetHandle ();
				int errorCode = RegDisableReflectionKey (handle.DangerousGetHandle ());

				/*
      			if (key == null)
      			{
       				Console.WriteLine("The Key is null, something wrong here"); 
      			}
 
      	*/
				value1 = key.GetValue (key_name);
				//Console.WriteLine(value1);
				//Console.ReadLine();
				return value1;

			} catch (Exception e) {
				Log_Writter.write_to_log_AVdebug ("Registery Read failed for microsoft FCS, if this software is not installed, you can safely ignore this message");

				return null;
			}

		}

		public object Retrive_FEP_RegKey_InstallTime_REGBINARY () {
			try {

				key = Registry.LocalMachine.OpenSubKey ("SOFTWARE\\Microsoft\\Microsoft Antimalware\\");
				Type type = typeof (RegistryKey);
				FieldInfo fi = type.GetField (
					"hkey",
					BindingFlags.NonPublic | BindingFlags.Instance);
				SafeHandle handle = (SafeHandle) fi.GetValue (key);
				IntPtr realHandle = handle.DangerousGetHandle ();
				int errorCode = RegDisableReflectionKey (handle.DangerousGetHandle ());

				installTime_Object = key.GetValue ("InstallTime");

				return installTime_Object;

			} catch (Exception e) {
				Log_Writter.write_to_log_AVdebug ("Registery Read failed for microsoft FEP Install Time, if this software is not installed, you can safely ignore this message");

				return null;
			}

		}

		public object Retrive_FEP_RegKey_InstallLocation_REGSZ () {

			try {

				key = Registry.LocalMachine.OpenSubKey ("SOFTWARE\\Microsoft\\Microsoft Antimalware\\");
				Type type = typeof (RegistryKey);
				FieldInfo fi = type.GetField (
					"hkey",
					BindingFlags.NonPublic | BindingFlags.Instance);
				SafeHandle handle = (SafeHandle) fi.GetValue (key);
				IntPtr realHandle = handle.DangerousGetHandle ();
				int errorCode = RegDisableReflectionKey (handle.DangerousGetHandle ());

				installTime_Object = key.GetValue ("InstallLocation");

				return installTime_Object;

			} catch (Exception e) {
				Log_Writter.write_to_log_AVdebug ("Registery Read failed for microsoft FEP Install Location");
				return null;
			}

		}

		public object Retrive_FCS_RegKey_InstallLocation_REGSZ () {
			try {

				key = Registry.LocalMachine.OpenSubKey ("SOFTWARE\\Microsoft\\Microsoft Forefront\\Client Security\\1.0\\AMs\\");
				Type type = typeof (RegistryKey);
				FieldInfo fi = type.GetField (
					"hkey",
					BindingFlags.NonPublic | BindingFlags.Instance);
				SafeHandle handle = (SafeHandle) fi.GetValue (key);
				IntPtr realHandle = handle.DangerousGetHandle ();
				int errorCode = RegDisableReflectionKey (handle.DangerousGetHandle ());

				installTime_Object = key.GetValue ("InstallLocation");

				return installTime_Object;

			} catch (Exception e) {
				Log_Writter.write_to_log_AVdebug ("Registery Read failed for microsoft FCS Install Location");
				return null;
			}

		}

		public void Validate_RegKey () {

		}

		/*******************************************************************
		 * Retrive_Sophos_RegKey_REGBINARY
		 *******************************************************************/

		public object Retrive_Sophos_RegKey_REGBINARY (string key_name) {
			string value64 = string.Empty;
			try {
				RegistryKey localKey = RegistryKey.OpenBaseKey (Microsoft.Win32.RegistryHive.LocalMachine, RegistryView.Registry64);

				key = localKey.OpenSubKey ("SOFTWARE\\Wow6432Node\\Sophos\\AutoUpdate\\UpdateStatus\\");
				Type type = typeof (RegistryKey);
				FieldInfo fi = type.GetField (
					"hkey",
					BindingFlags.NonPublic | BindingFlags.Instance);
				SafeHandle handle = (SafeHandle) fi.GetValue (key);
				IntPtr realHandle = handle.DangerousGetHandle ();
				int errorCode = RegDisableReflectionKey (handle.DangerousGetHandle ());

				value1 = key.GetValue (key_name);
				return value1;

				/*
                if (key != null)
                {
                    value64 = key.GetValue("Result").ToString();
                }
               Console.WriteLine(String.Format("Result [value64]: {0}", value64));
               return value64;

                */

			} catch (Exception e) {
				Log_Writter.write_to_log_AVdebug ("Registery Read failed for Sohpos, if this software is not installed, you can safely ignore this message");

				return null;
			}

		}
	}
}