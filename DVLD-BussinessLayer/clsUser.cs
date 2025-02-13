using DVLD_DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static DVLD_DataAccessLayer.clsUserData;

namespace DVLD_BussinessLayer
{
    public class clsUser
    {
    private static DataTable _dt;
        public enum enMode { AddNew, Update }
        public enMode Mode = enMode.AddNew;
        public int UserID { get; set; }
        public int PersonID { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public byte IsActive { get; set; }
        public clsPerson PersonInfo { get; set; }
      
        private clsUser(int UserID, int PersonID,string UserName,string Password ,byte IsActive)
        {
            this.UserID = UserID;
            this.PersonID = PersonID;
            this.UserName = UserName;
            this.Password = Password;
            this.IsActive = IsActive;
            _LoadDataToPersonObject(PersonID);
            Mode = enMode.Update;
        }
        public clsUser()
        {
            this.UserID = -1;
            this.PersonID = -1;
            this.UserName = "";
            this.Password = "";
            this.IsActive = 0;
            Mode = enMode.AddNew;

        }
        private static void _AssignIsSystemErrorVariable()
        {
            clsGlobal.IsSystemError = DataAccessSettings.IsSystemError;
        }


        private async void _LoadDataToPersonObject(int UserID)
        {
            this.PersonInfo = await clsPerson.FindByPersonID(PersonID);
        }
        public static DataTable ListUsers()
        {
            _dt = clsUserData.ListUsers();
            _AssignIsSystemErrorVariable();
            return _dt;
        }
        public static string Encrypt(string plainText,string Key)
        {

            using(Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(Key);
                aes.IV = new byte[aes.BlockSize /  8];  

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key,aes.IV);

                
                using (var msEncrypt = new MemoryStream())
                {
                    using(var csEncrypt = new CryptoStream(msEncrypt,encryptor,CryptoStreamMode.Write))
                    {
                        using(var swEncrypt  = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(plainText);
                        }
                    }
                    return Convert.ToBase64String(msEncrypt.ToArray());
                }

            }

        }

        public static string Decrypt(string cipherText,string Key)
        {

            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(Key);
                aes.IV = new byte[aes.BlockSize / 8];

                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);


                using (var msDecrypt = new MemoryStream(Convert.FromBase64String(cipherText)))
                {
                    using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (var swDecrypt = new StreamReader(csDecrypt))
                        {
                            return swDecrypt.ReadToEnd();
                        }
                    }
                }

            }

        }
        public static string ComputeHash(string input)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                // Compute the hash value from the UTF-8 encoded input string
                byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));


                // Convert the byte array to a lowercase hexadecimal string
                return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            }
        }
        public static bool IsPersonLinkedToUser(int  ID)
        {
            bool IsLinked =  clsUserData.IsPersonLinkedToUser(ID) ;
            _AssignIsSystemErrorVariable() ;
            return IsLinked;
        }
        public static bool IsUserNameUsed(string UserName)
        {
            bool IsUserNameUsed = clsUserData.IsUserNameUsed(UserName);
            _AssignIsSystemErrorVariable() ;
            return IsUserNameUsed;
        }
        private bool _AddNewUser()
        {
            this.UserID = clsUserData.AddNewUserandGetID(this.PersonID,this.UserName,
                ComputeHash(this.Password),this.IsActive);
            return UserID != -1;
        }
        private bool _UpdateUser()
        {
            return clsUserData.UpdateUser(this.UserID, this.UserName,ComputeHash(this.Password),this.IsActive);
        }

        public static async Task<clsUser> FindByUserID(int UserID)
        {
            UserResult result = await clsUserData.FindByUserIDAsync(UserID);

            if (result.IsFound)
                return new clsUser(UserID, result.PersonID, result.UserName, result.Password, result.IsActive);
            else
                return null;
        }


        public static clsUser FindUserByUserNameandPassword(string UserName,string Password)
        {
            int PersonID = -1,UserID = -1;
            byte IsActive = 0;
            if (clsUserData.FindUserByUserNameandPassword(  UserName, ComputeHash(Password),ref UserID, ref PersonID, ref IsActive))
                return new clsUser(UserID, PersonID, UserName, Password, IsActive);
            else
                return null;
        }

        public static bool DeleteUser(int UserID)
        {
            if (!clsUserData.DeleteUser(UserID))
            {
                clsGlobal.IsForeignKeyConstraintException = DataAccessSettings.IsForeignKeyConstraintException;
                _AssignIsSystemErrorVariable();
                return false;
            }
            return true;
        }
        public  bool SavingNewPassword(string Password)
        {
            return clsUserData.SavingNewPassword(this.UserID,ComputeHash(Password));
        }
        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewUser())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                case enMode.Update:
                    return _UpdateUser();

                default:
                    return false;
            }
        }
    }
}
