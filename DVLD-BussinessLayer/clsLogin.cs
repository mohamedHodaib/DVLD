using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DVLD_DataAccessLayer;
using System.IO;
using System.Threading;
using Microsoft.Win32;
using System.Configuration;
namespace DVLD_BussinessLayer
{
    public class clsLogin
    {
        public static short FaildLoginCount = 0;

        public static int _PrevWait = 5;
        public static int _CurrentWait = 5;
        public static int _CountOfLogin = 1;
        public static bool Login(string Username, string Password)
        {
            if (FaildLoginCount == 3) FaildLoginCount = 0;
            clsGlobal.CurrentUser = clsUser.FindUserByUserNameandPassword(Username,Password);
            clsGlobal.IsSystemError = DataAccessSettings.IsSystemError;
            if (clsGlobal.CurrentUser == null)
            {
                FaildLoginCount++;
                return false;
            }
           else
            {
            clsGlobal.CurrentUserID = clsGlobal.CurrentUser.UserID;

                return true;
            }
        }

        public static void IncreaseCountOfLogin()
        { _CountOfLogin++; }

        public static void CalculateCurrentWait()
        { _CurrentWait = _PrevWait * 2; }

        public static void AssignCurrentWaitToPrevWait()
        {
            _PrevWait = _CurrentWait;
        }

        public static void DecreaseTheCurrentWait()
        {
            _CurrentWait--;
        }

        public static bool WriteToRegistery(string UserName,string Password,string path)
        {
            try
            {
                Registry.SetValue(path,"DVLD_Credentials", UserName + "#//#" + clsUser.Encrypt(Password, ConfigurationManager.AppSettings["Key"]), RegistryValueKind.String);
                return true;
            }
            catch(Exception ex)
            {
                 return false; 
            }
          
        }
        
        public static void DeletedCurrentCredentials(string path)
        {
            Registry.SetValue(path, "DVLD_Credentials", "", RegistryValueKind.String);
        }
    }
}
