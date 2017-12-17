/********************************************************
 * Created by Xiangyu You
 * Date: 1/31/2013
 * Time: 1:19 AM
 ***************************************************/

 
using System;
using System.Text;
using System.Management;
using System.IO;
using System.Diagnostics;




namespace LAVS
{
	/// <summary>
	/// Description of OS_Checker.
	/// </summary>
	public class OS_Checker
	{
		public ManagementObject mo;
		public string OS_System_Type;
		
		public OS_Checker()
		{
			OS_System_Type = "";
			mo = null;
		}
		
		
		public void Check_the_OS_Version()
		{
			try
			{
           		OperatingSystem os = Environment.OSVersion;
           		Version vs = os.Version;
           		ManagementScope scope = new ManagementScope("\\\\.\\ROOT\\cimv2");
           		ObjectQuery query = new ObjectQuery("SELECT * FROM Win32_ComputerSystem");
           		ManagementObjectSearcher searcher = new ManagementObjectSearcher(scope, query);
           		ManagementObjectCollection queryCollection = searcher.Get();
           		foreach (ManagementObject m in queryCollection)
           		{
           			mo = m;
           		}
           		OS_System_Type = mo["systemtype"].ToString();
				}
			catch(Exception e)
			{
				Win_Log_Writter Log_Writter = new Win_Log_Writter();
				Log_Writter.write_to_log_AVdebug("Error to check the OS Version "+ e.ToString());
				
			}
		}
	
	}
}
