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
using System.Diagnostics;
using System.IO;
using System.Management;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.Win32;

namespace LAVS {

    class Program {

        //   public bool debug_mode_enabled = false;
        // public int debug_switch = 0;
        public OS_Checker winOS_Checker;
        //public ManagementObject mo;
        public string OS_System_Type;
        public bool sym_av_corp_is_outOfDate;
        //[DefDates]
        //CurDefs=20130110.005
        //LastDefs=20130109.003
        int dotNetVersion = -1;
        Sym_AV_Corp Sym_av_coporation_edition;
        public bool AV_Present_onServer;

        [DllImport ("advapi32.dll", SetLastError = true)]
        static extern int RegDisableReflectionKey (IntPtr hBase);

        public Program () {
            AV_Present_onServer = false;
            RunScan ();
        }

        public void RunScan () {

            //check the dotnetversion
            dontnetDetector dot = new dontnetDetector ();
            sym_av_corp_is_outOfDate = false;
            Win_Log_Writter Log_Writter = new Win_Log_Writter ();
            dotNetVersion = dot.FrameworkVersion ();
            winOS_Checker = new OS_Checker ();
            winOS_Checker.Check_the_OS_Version ();

            //check for Sym_av_corp
            Sym_av_coporation_edition = new Sym_AV_Corp ();
            OS_System_Type = winOS_Checker.OS_System_Type;

            //************************************************
            // check for the Symantec
            //************************************************	
            if (Sym_av_coporation_edition.check_symantec_av_exist () == true) {

                Log_Writter.write_to_log_info ("Symantec Anti-virus Corp edition is Detected, Checking the AV defintion.... ");
                AV_Present_onServer = true;

                if (Sym_av_coporation_edition.aV_Definition_is_OutOfDate (2) == true) {
                    Log_Writter.write_to_log_Error ("Symantec Anti-virus Corp edition definition is more than " + 2 + " days old, please contact administrator to review the anti-virus definitions");

                } else {
                    Log_Writter.write_to_log_info ("Anti-virus definition Corp edition is less than " + 2 + " days old and no action is required");
                }

            } else {
                Log_Writter.write_to_log_info ("Symantec Anti-virus Corp edition is NOT Detected, Program will carry on searching for other AV softwares");
            }
            //************************************************
            // check for the FCS
            //************************************************	

            //************************************************
            // check for the Symantec SEP
            //************************************************	

            Symantec_SEP sym_sep = new Symantec_SEP ();

            if (sym_sep.check_symantec_av_exist () == true) {

                Log_Writter.write_to_log_info ("Symantec End point Protection AV software is Detected, Checking the AV defintion.... ");
                AV_Present_onServer = true;

                if (sym_sep.aV_Definition_is_OutOfDate (2) == true) {
                    Log_Writter.write_to_log_Error ("Symantec Anti-virus Corp edition definition is more than " + 2 + " days old, please contact administrator to review the anti-virus definitions");

                } else {
                    Log_Writter.write_to_log_info ("Anti-virus definition Corp edition is less than " + 2 + " days old and no action is required");
                }
            } else {
                Log_Writter.write_to_log_info ("Symantec End point Protection AV software is NOT Presented, Program will carry on searching for other AV softwares");
            }

            MS_FCS Forefront_Client_Security = new MS_FCS ();

            if (Forefront_Client_Security.check_MSFCS_av_exist ()) {

                Log_Writter.write_to_log_info ("Microsoft FCS AV software is Detected, Checking the AV/Malware defintion.... ");
                AV_Present_onServer = true;
                if (Forefront_Client_Security.aV_Definition_is_OutOfDate (2) == true) {
                    Log_Writter.write_to_log_Error ("Microosft FCS Anti-virus definition is more than " + 2 + " days old, please contact administrator to review the anti-virus definitions");
                } else {
                    Log_Writter.write_to_log_info ("Microosft FCS Anti-virus definition is less than " + 2 + " days old no action is required");

                }

                if (Forefront_Client_Security.aS_Definition_is_OutOfDate (2) == true) {
                    Log_Writter.write_to_log_Error ("Microosft FCS malware definition is more than " + 2 + " days old, please contact administrator to review the anti-virus definitions");
                } else {
                    Log_Writter.write_to_log_info ("Microosft FCS malware definition is less than " + 2 + " days old and no action is required");

                }

            } else {
                Log_Writter.write_to_log_info ("Microsoft FCS AV software is NOT Detected, Program will carry on searching for other AV softwares");
            }

            //************************************************
            // check for the FEP
            //************************************************	        		
            MS_FEP2010 FEP2010 = new MS_FEP2010 ();

            /*
                if (FEP2010.valid_InstallTime_Key() == true)
                {
                    Console.WriteLine("The Installation key is present"); 
                    Console.ReadLine();       
                }
                else
                {
                    Console.WriteLine("The Installation key is NOT Presnt"); 
                    Console.ReadLine();              		
                }
                if (FEP2010.retrive_InstallLocation_Key() != null)
                {
                    Console.WriteLine("The Installation key is present"  + FEP2010.retrive_InstallLocation_Key().ToString());
                    Console.ReadLine();    
                }
                else
                {
                    Console.WriteLine("The Installation key is NOT" );
                    Console.ReadLine();            		
                }
                if (Forefront_Client_Security.retrive_InstallLocation_Key() !=null)
                {
                    Console.WriteLine("The Installation key is present"  + Forefront_Client_Security.retrive_InstallLocation_Key());
                    Console.ReadLine();            		
                }
                else
                {
                    Console.WriteLine("The Installation key is NOT present");
                    Console.ReadLine();            		
                }
        		
       */

            if (FEP2010.check_FEP2010_av_exist ()) {
                Log_Writter.write_to_log_info ("Microsoft EndPoint Protection AV software is Detected, Checking the AV/Malware defintion....");
                AV_Present_onServer = true;
                if (FEP2010.aV_Definition_is_OutOfDate (2) == true) {
                    Log_Writter.write_to_log_Error ("Microosft EndPoint Protection Anti-virus definition is more than " + 2 + " days old, please contact administrator to review the anti-virus definitions");
                } else {
                    Log_Writter.write_to_log_info ("Microosft  EndPoint Protection Anti-virus definition is less than " + 2 + " days old and no action is required");

                }

                if (FEP2010.aS_Definition_is_OutOfDate (2) == true) {
                    Log_Writter.write_to_log_Error ("Microosft EndPoint Protection malware definition is more than " + 2 + " days old, please contact administrator to review the anti-virus definitions");
                } else {
                    Log_Writter.write_to_log_info ("Microosft EndPoint Protection malware definition is less than " + 2 + " days old and no action is required");

                }

            } else {
                Log_Writter.write_to_log_info ("Microsoft EndPoint Protection AV software is NOT Detected");
            }

            //************************************************
            // check for the Sophos
            //************************************************	

            Sophos_AV sop_av = new Sophos_AV ();

            if (sop_av.check_Sophos_av_exist () == true) {

                Log_Writter.write_to_log_info ("Sophos Anti-Virus software is Detected, Checking the AV defintion.... ");
                AV_Present_onServer = true;
                if (sop_av.aV_Definition_is_OutOfDate (2) == true) {
                    Log_Writter.write_to_log_Error ("Sophos Anti-Virus definition is more than " + 2 + " days old, please contact administrator to review the anti-virus definitions");

                } else {

                    Log_Writter.write_to_log_info ("Sophos Anti-Virus definition Corp edition is less than " + 2 + " days old and no action is required");
                }

                if (sop_av.aV_Definition_result_zero () == true) {
                    Log_Writter.write_to_log_info ("Sophos Anti-Virus definition is installed sucessfully no action required");

                } else {
                    Log_Writter.write_to_log_Error ("Sophos Anti-Virus definition has failed to install, Please contact administrator to review the Anti-virus configuration");

                }

            } else {
                Log_Writter.write_to_log_info ("Sophos Anti-Virus software is NOT Presented.");
            }

            //************************************************
            // no AV found
            //************************************************	
            if (AV_Present_onServer == false) {
                Log_Writter.write_to_log_Error ("No Anti-virus software is detected on the system or its never updated! Action Required!");
            }

        }
    }
}