﻿/****************************** Module Header ******************************\
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
using System.IO;
using Microsoft.Win32;

namespace LAVS {
    /// <summary>
    /// Description of Symantec_SEP.
    /// </summary>
    public class Symantec_SEP {
        public OS_Checker winOS_Checker;
        public string OS_System_Type;
        string definfo_path = null;
        string curDefs = null;
        string curDefs_year = null;
        string curDefs_month = null;
        string curDefs_day = null;
        string curDefs_rev = null;
        DateTime dt;
        private Win_Log_Writter Log_Writter;
        int curDefs_year_int = 0;
        int curDefs_month_int = 0;
        int curDefs_day_int = 0;
        //int curDefs_rev_int = 0;

        public Symantec_SEP () {
            winOS_Checker = new OS_Checker ();
            winOS_Checker.Check_the_OS_Version ();
            OS_System_Type = winOS_Checker.OS_System_Type;
            Log_Writter = new Win_Log_Writter ();

        }

        public bool check_symantec_av_exist () {

            definfo_path = @"C:\ProgramData\Symantec\Symantec Endpoint Protection\12.1.2015.2015.105\Data\Definitions\VirusDefs\definfo.dat";
            //Console.WriteLine(OS_System_Type);
            //Console.WriteLine(definfo_path);

            try {
                string[] lines = System.IO.File.ReadAllLines (@".\\SYMANTEC_SEP_PATH.txt");

                foreach (string line in lines) {

                    definfo_path = line + "\\definfo.dat";
                }

            } catch (Exception e) {

                Log_Writter.write_to_log_AVdebug ("SYMANTEC_SEP_PATH.txt is not find, using the default path C:\\ProgramData\\Symantec\\Symantec Endpoint Protection\\12.1.2015.2015.105\\Data\\Definitions\\VirusDefs\\definfo.dat. More Error info : " + e.ToString ());

            }

            if (File.Exists (definfo_path)) {
                return true;
            } else {
                return false;
            }
        }

        public bool aV_Definition_is_OutOfDate (int day_difference) {

            if (check_symantec_av_exist ()) {
                string[] lines = new string[3];
                int lines_index = 0;
                //= System.IO.File.ReadAllLines(definfo_path);       
                try {
                    // Create an instance of StreamReader to read from a file.
                    // The using statement also closes the StreamReader.
                    using (StreamReader sr = new StreamReader (definfo_path)) {
                        String line;
                        // Read and display lines from the file until the end of 
                        // the file is reached.
                        while ((line = sr.ReadLine ()) != null) {
                            lines[lines_index] = line;
                            lines_index++;
                        }
                    }
                } catch (Exception e) {
                    // Let the user know what went wrong.
                    Log_Writter.write_to_log_AVdebug ("definfo.dat File is not in correct format please check" + e.ToString ());
                    return false;
                    //Console.WriteLine(e.Message);
                }

                // Console.ReadLine();
                curDefs = lines[1];
            } else {
                Log_Writter.write_to_log_AVdebug ("The definfo.dat file does not exists, please check the installation of the Symantec SEP");
                return false;
            }

            //Console.WriteLine("this is it" + curDefs);       
            if (curDefs != null) {
                //Console.WriteLine("format!!!! "+curDefs);

                curDefs_year = curDefs.Substring (8, 4);

                curDefs_month = curDefs.Substring (12, 2);
                curDefs_day = curDefs.Substring (14, 2);
                curDefs_rev = curDefs.Substring (17, 3);

                try {
                    curDefs_year_int = Convert.ToInt32 (curDefs_year);
                    // Console.WriteLine("curDefs_year_int" + curDefs_year_int);
                    curDefs_month_int = Convert.ToInt32 (curDefs_month);
                    //  Console.WriteLine("curDefs_month_int" + curDefs_month_int);
                    curDefs_day_int = Convert.ToInt32 (curDefs_day);
                    //  Console.WriteLine("curDefs_day" + curDefs_day);
                    //  Console.WriteLine("curDefs_day_int" + curDefs_day_int);
                    //  Console.WriteLine("coverting test is " + Convert.ToInt32("10"));
                    //  Console.ReadLine();
                    dt = new DateTime (curDefs_year_int, curDefs_month_int, curDefs_day_int);
                    //   Console.WriteLine("datetime current is " + DateTime.Now);
                    //  Console.WriteLine("datatime of av is " + dt.ToString());

                    // Console.WriteLine( "test difference"+ (DateTime.Now - dt).TotalDays);

                    if ((DateTime.Now - dt).TotalDays >= day_difference) {

                        Log_Writter.write_to_log_Error ("Anti-virus definition is more than " + day_difference + " days old, please contact administrator to review the anti-virus definitions");
                        return true;

                    } else {
                        //Console.WriteLine("this is NOT out of the date");
                        Log_Writter.write_to_log_info ("Anti-virus definition is less than " + day_difference + " days old, No Action is required ");
                        return false;
                    }
                } catch (Exception e) {

                    Log_Writter.write_to_log_AVdebug ("Script is not working with following error : " + e.ToString ());

                    return false;
                }
            }

            return false;

        }

    }
}