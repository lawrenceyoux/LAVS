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

namespace LAVS
{
	/// <summary>
	/// Description of MS_SCEP2012.
	/// </summary>
	public class MS_SCEP2012
	{
		
	
		private object ASSignatureApplied_regkey_value;
		private RegKey_Handler key_Handler;
		private Win_Log_Writter log_Writter;
		private object AVSignatureApplied_regkey_value;		
		public int debug_switch = 0;
  
		
		public MS_SCEP2012(int debug)
		{
			debug_switch = debug;

            try
            {
                key_Handler = new RegKey_Handler();
                AVSignatureApplied_regkey_value = key_Handler.Retrive_FEP_RegKey_REGBINARY("AVSignatureApplied");
                ASSignatureApplied_regkey_value = key_Handler.Retrive_FEP_RegKey_REGBINARY("ASSignatureApplied");
                log_Writter = new Win_Log_Writter();
            }
            catch (Exception e)
            {
                log_Writter.write_to_log_AVdebug("Unable to retrive the regkey, the Program for MS SEP might not work properly " + e.ToString());
            }
		}
		
		
		
		public bool check_SCEP2012_av_exist()
		{
			if (AVSignatureApplied_regkey_value == null)
			{
                return false;
			}
			else
			{
				return true;
			}
					
				
		
		}
		
		
		
		public bool aV_Definition_is_OutOfDate(int day_difference)
		{

			if(retrive_InstallLocation_Key() != null & AVSignatureApplied_regkey_value == null)
			{
				return true; 
			}			
			
			if (AVSignatureApplied_regkey_value != null)
			{		
				try
				{
					byte[] bytes = (byte[])AVSignatureApplied_regkey_value;
					long seconds  =   bytes[7] * (long)Math.Pow(2, 56)
	              					+ bytes[6] * (long)Math.Pow(2, 48)
	              					+ bytes[5] * (long)Math.Pow(2, 40)
		          					+ bytes[4] * (long)Math.Pow(2, 32)
		          					+ bytes[3] * (long)Math.Pow(2, 24)
		          					+ bytes[2] * (long)Math.Pow(2, 16)
		          					+ bytes[1] * (long)Math.Pow(2, 8)
		          					+ bytes[0] ;
					double days = seconds/(1e7*86400);
					DateTime date = new DateTime(1601, 1, 1); 	
					date = date.AddDays(days);

					if ((DateTime.Now - date).Days >= day_difference)
					{
						return true;
					}
					else
					{
						return false;
					}
 					
				}
				catch (Exception e)
				{
					log_Writter.write_to_log_AVdebug("There is something wrong with regKey for microsoft Anti-virus, error info " + e.ToString());
					return false;
				}
			}
			else
			{
				log_Writter.write_to_log_AVdebug("The ReKey for Microsoft Anti-virus doesnt exits, error info, Please manully check the regKey");
				return false;
			}
			
		}
				
			
		
		
				
		public bool aS_Definition_is_OutOfDate(int day_difference)
		{
			if(retrive_InstallLocation_Key() != null & ASSignatureApplied_regkey_value == null)
			{
				return true; 
			}
			
			if (AVSignatureApplied_regkey_value != null)
			{		
				try
				{
					byte[] bytes = (byte[])ASSignatureApplied_regkey_value;
					long seconds  =   bytes[7] * (long)Math.Pow(2, 56)
	              					+ bytes[6] * (long)Math.Pow(2, 48)
	              					+ bytes[5] * (long)Math.Pow(2, 40)
		          					+ bytes[4] * (long)Math.Pow(2, 32)
		          					+ bytes[3] * (long)Math.Pow(2, 24)
		          					+ bytes[2] * (long)Math.Pow(2, 16)
		          					+ bytes[1] * (long)Math.Pow(2, 8)
		          					+ bytes[0] ;
					double days = seconds/(1e7*86400);
					DateTime date = new DateTime(1601, 1, 1); 	
					date = date.AddDays(days);

					if ((DateTime.Now - date).Days >= day_difference)
					{
						return true;
					}
					else
					{
						return false;
					}
 					
				}
				catch (Exception e)
				{
					if (debug_switch == 1)
					{
						log_Writter.write_to_log_AVdebug("There is something wrong with regKey for microsoft Anti-virus, error info " + e.ToString());
					}
					return false;
				}
			}
			else
			{
				if(debug_switch == 1)
				{
					log_Writter.write_to_log_AVdebug("The ReKey for Microsoft Anti-virus doesnt exits, error info, Please manully check the regKey");
				}
				return false;
			}
			
		}
		
		
		public object retrive_InstallTime_Key()
		{
			try
			{
				return key_Handler.Retrive_FEP_RegKey_InstallTime_REGBINARY();
			}
			catch(Exception e)
			{	
				if(debug_switch == 1)
				{
					log_Writter.write_to_log_AVdebug("Error when retriving SCEO2012 InstallTime: " + e.ToString());				
				}return null;
			}
		}
		
		
		
		public object retrive_InstallLocation_Key()
		{
			try
			{
				return key_Handler.Retrive_FEP_RegKey_InstallLocation_REGSZ();
			}
			catch(Exception e)
			{
				if(debug_switch ==1)
				{
					log_Writter.write_to_log_AVdebug("Error when retriving FEP InstallTime: " + e.ToString());				
				}
				return null;
			}
		}
				
				
				
		public bool valid_InstallTime_Key()
		{
			if ( retrive_InstallTime_Key() != null)
			{
				return true;
			}
			else
			{
				return false;
			}
		}
				
				
		
		
		
		
		
	}
}
