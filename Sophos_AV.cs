using System;
using System.Collections.Generic;
using System.Text;

namespace LAVS {
    class Sophos_AV {

        public object LastUpdateTime;
        public object Result;
        private RegKey_Handler key_Handler;
        private Win_Log_Writter log_Writter;

        public int debug_switch = 0;

        public Sophos_AV () {

            try {
                key_Handler = new RegKey_Handler ();
                LastUpdateTime = key_Handler.Retrive_Sophos_RegKey_REGBINARY ("LastUpdateTime");
                Result = key_Handler.Retrive_Sophos_RegKey_REGBINARY ("Result");
                log_Writter = new Win_Log_Writter ();

            } catch (Exception e) {
                log_Writter.write_to_log_AVdebug ("Unable to retrive the regkey, the Program for MS SEP might not work properly " + e.ToString ());
            }
        }

        public bool check_Sophos_av_exist () {
            if (LastUpdateTime == null) {

                return false;
            } else {

                return true;
            }

        }

        public bool aV_Definition_is_OutOfDate (int day_difference) {

            DateTime unix_date = UnixTimeStampToDateTime (Convert.ToDouble (LastUpdateTime));
            try {

                if ((DateTime.Now - unix_date).Days >= day_difference) {
                    return true;
                } else {
                    return false;
                }
            } catch (Exception e) {
                log_Writter.write_to_log_AVdebug ("There is something wrong with regKey for microsoft Anti-virus, error info " + e.ToString ());
                return false;
            }
        }

        public bool aV_Definition_result_zero () {
            if (Convert.ToInt32 (Result) == 0) {
                return true;
            } else return false;
        }

        public static DateTime UnixTimeStampToDateTime (double updateTime) {
            System.DateTime dtDateTime = new DateTime (1970, 1, 1, 0, 0, 0, 0);
            dtDateTime = dtDateTime.AddSeconds (updateTime).ToLocalTime ();
            return dtDateTime;
        }

    }

}