/********************************************************
 * Created by Xiangyu You
 * Date: 1/31/2013
 * Time: 1:19 AM
 ***************************************************/


using System;
using System.IO;
using System.Security;
using System.Text.RegularExpressions;

namespace LAVS
{
    public class dontnetDetector
    {
        private const string FRAMEWORK_PATH = "\\Microsoft.NET\\Framework";
        private const string WINDIR1 = "windir";
        private const string WINDIR2 = "SystemRoot";
        private Win_Log_Writter log_Writter;


        public  dontnetDetector()
        {
            log_Writter = new Win_Log_Writter();
        }
        public  int FrameworkVersion()
        {
    
                try
                {
                	//bool tempbool = CheckDotNETVersion(NetFrameworkInstallationPath, 2);
                	//Console.WriteLine("dont net version 2 is installed " + tempbool);
                    return getHighestVersion(NetFrameworkInstallationPath);
                }
                catch (SecurityException e)
                {
                    log_Writter.write_to_log_AVdebug("Unable to allocated the .Net Framework Version, the program might not work properly " + e.ToString());
                    return -1;
                }
    
        }
        
        public  bool CheckFrameworkVersion(int versionNumber)
        {
    
                try
                {
                	return CheckDotNETVersion(NetFrameworkInstallationPath, versionNumber);
                	//Console.WriteLine("dont net version 2 is installed " + tempbool);
                	
           
                }
                catch (SecurityException e)
                {
                    log_Writter.write_to_log_AVdebug("Unable to allocated the .Net Framework Version, the program might not work properly " + e.ToString());
                    return false;
                }
    
        }   
        
        private  int getLowestVersion(string installationPath)
        {
            string[] versions = Directory.GetDirectories(installationPath, "v*");
            string version = "Unknown";

            int lowest= -1;
           	int tempInt = -1;

            
            for (int i = versions.Length - 1; i >= 0; i--)
            {
            
            
            	
              version = extractVersion(versions[i]);
  
              //Console.WriteLine("Test "+ removeDotInString(version));
              version = removeDotInString(version);
              if (isNumber(version))
              {
              	tempInt = Convert.ToInt32(version);
              	while(tempInt >= 10)
              	{
              		tempInt = (tempInt - (tempInt %10)) /10;
              	}
              	//Console.WriteLine("TempInt "+ tempInt);
               
            
              	if (tempInt == 1)
              	{
              		lowest =1 ;
              	}
              	else if  (tempInt > 0 &  lowest < tempInt)
              	{
              		lowest = tempInt;
              	}
              }
              else
              {
              	//Console.WriteLine(version + " is not a number");
              }
         
              
            }

            return lowest;
        }  
        

        private  int getHighestVersion(string installationPath)
        {
            string[] versions = Directory.GetDirectories(installationPath, "v*");
            string version = "Unknown";

            int highest= -1;
           	int tempInt = -1;

            
            for (int i = versions.Length - 1; i >= 0; i--)
            {
            
            
            	
              version = extractVersion(versions[i]);
  
              //Console.WriteLine("Test "+ removeDotInString(version));
              version = removeDotInString(version);
              if (isNumber(version))
              {
              	tempInt = Convert.ToInt32(version);
              	while(tempInt >= 10)
              	{
              		tempInt = (tempInt - (tempInt %10)) /10;
              	}
              //	Console.WriteLine("TempInt "+ tempInt);
               
              	if (	tempInt > highest)
              	{
              		highest = tempInt;
              	}
              	
              
              }
              else
              {
              //	Console.WriteLine(version + " is not a number");
              }
         
              
            }

            return highest;
        }
        
        
        
           private  bool CheckDotNETVersion(string installationPath, int dotNetNumber)
        {
            string[] versions = Directory.GetDirectories(installationPath, "v*");
            string version = "Unknown";

        
           	int tempInt = -1;

            
            for (int i = versions.Length - 1; i >= 0; i--)
            {
            
            
            	
              version = extractVersion(versions[i]);
  
            //  Console.WriteLine("Test "+ removeDotInString(version));
               version = removeDotInString(version);
              if (isNumber(version))
              {
              	tempInt = Convert.ToInt32(version);
              	while(tempInt >= 10)
              	{
              		tempInt = (tempInt - (tempInt %10)) /10;
              	}
              //	Console.WriteLine("TempInt "+ tempInt);
               
              	if (	tempInt == dotNetNumber)
              	{
              		return true;
              	}
              	
              
              }
              else
              {
           //   	Console.WriteLine(version + " is not a number");
              }  
            }

            	return false;


      
        }
           
           
           
        private string removeDotInString(string input)
        {
        //	char[] charToTrim = {'.'};
        	//char[] charString = input.ToCharArray();
        //	return input.TrimEnd(charToTrim);
        return input.Replace(@".", string.Empty);
        
        }
        

        private  string extractVersion(string directory)
        {
        	string temp;
            int startIndex = directory.LastIndexOf("\\") + 2;
        	
            temp  =  directory.Substring(startIndex, directory.Length - startIndex);
            return temp;
        }

        private  bool isNumber(string str)
        {
            return new Regex(@"^[0-9]+\.?[0-9]*$").IsMatch(str);
        }

        public  string NetFrameworkInstallationPath
        {
            get { return WindowsPath + FRAMEWORK_PATH; }
        }

        public  string WindowsPath
        {
            get
            {
                string winDir = Environment.GetEnvironmentVariable(WINDIR1);
                if (String.IsNullOrEmpty(winDir))
                    winDir = Environment.GetEnvironmentVariable(WINDIR2);

                return winDir;
            }
        }
    }
}
