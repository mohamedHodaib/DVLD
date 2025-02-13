using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using DVLD_DataAccessLayer;
using System.IO;
using Microsoft.SqlServer.Server;
namespace DVLD_BussinessLayer
{
    public class clsPerson
    {
        public enum enMode { AddNew, Update }
       public  enMode Mode = enMode.AddNew;
        private static DataTable _dt;
        public int ID { get; set; }
        public string NationalNo { get; set; }
        public string FName { get; set; }
        public string SName { get; set; }
        public string TName { get; set; }
        public string LName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public short Gender {  get; set; }
        public DateTime DateOfBirth { get; set; }
        public string ImagePath { get; set; }
        public int CountryID { get; set; }
        public string FullName
        {
            get { return FName + " " + SName + " " + TName + " " + LName; }
        }

        clsPersonData.stPersonInfo personInfo;
        private clsPerson(int ID, string NationalNo, string FName, string SName, string TName, string LName, string Phone,
  string Email, string Address,short Gender, DateTime DateOfBirth, int CountryID, string ImagePath)
        {
            this.ID = ID;
            this.NationalNo = NationalNo;
            this.FName = FName;
            this.SName = SName;
            this.TName = TName;
            this.LName = LName;
            this.Phone = Phone;
            this.Email = Email;
            this.Address = Address;
            this.Gender = Gender;
            this.DateOfBirth = DateOfBirth;
            this.CountryID = CountryID;
            this.ImagePath = ImagePath;
            Mode = enMode.Update;
        }
        public clsPerson()
        {
            this.ID = -1;
            this.NationalNo = "";
            this.FName = "";
            this.SName = "";
            this.TName = "";
            this.LName = "";
            this.Phone = "";
            this.Email = "";
            this.Address = "";
            this.Gender = -1;
            this.DateOfBirth = DateTime.Now;
            this.CountryID = -1;
            this.ImagePath = "";
            Mode = enMode.AddNew;

        }
        public static async Task<clsPerson> FindByPersonID(int ID)
        {
            if (await clsPersonData.FindByPersonID(ID))
                return new clsPerson( ID,  clsPersonData.PersonInfo.NationalNo,  clsPersonData.PersonInfo.FName,  clsPersonData.PersonInfo.SName
                    ,clsPersonData.PersonInfo.TName,  clsPersonData.PersonInfo.LName,  clsPersonData.PersonInfo.Phone,
                    clsPersonData.PersonInfo.Email,clsPersonData.PersonInfo.Address,  clsPersonData.PersonInfo.Gender,  clsPersonData.PersonInfo.DateOfBirth, 
                    clsPersonData.PersonInfo.CountryID, clsPersonData.PersonInfo.ImagePath);
            else
                return null;
        }

        public static async  Task<clsPerson> FindByNationalNumber(string NationalNo)
        {

            if (await clsPersonData.FindByNationalNo(NationalNo))
                return new clsPerson(clsPersonData.PersonInfo.ID, clsPersonData.PersonInfo.NationalNo,
                    clsPersonData.PersonInfo.FName, clsPersonData.PersonInfo.SName, clsPersonData.PersonInfo.TName,
                    clsPersonData.PersonInfo.LName, clsPersonData.PersonInfo.Phone,
               clsPersonData.PersonInfo.Email, clsPersonData.PersonInfo.Address, clsPersonData.PersonInfo.Gender, clsPersonData.PersonInfo.DateOfBirth, 
               clsPersonData.PersonInfo.CountryID,  clsPersonData.PersonInfo.ImagePath);
            else
                return null;
        }
        private static void _AssignSystemErrorVariable()
        {

                clsGlobal.IsSystemError = DataAccessSettings.IsSystemError;
            
        }
        public static DataTable ListPeople()
        {
            _dt = clsPersonData.ListPeople();
            _AssignSystemErrorVariable();
            return _dt;
        }
        public static bool IsNationalNumberExist(string NationalNumber)
        {
            _dt = clsPersonData.FilterByNationalNumber(NationalNumber);
               _AssignSystemErrorVariable();
            return _dt.Rows.Count != 0;
        }


        public static bool IsPhoneNumberExist(string NationalNumber)
        {
            _dt = clsPersonData.FilterByPhone(NationalNumber);
            _AssignSystemErrorVariable();
            return _dt.Rows.Count != 0;
        }
        public static bool DeletePerson(int ID)
        {
            if(! DeleteImageFromImagesFolder(ID))
            {
                clsGlobal.IsSystemError = true;
                return false;
            }
            if (!clsPersonData.DeletePerson(ID))
            {
                clsGlobal.IsForeignKeyConstraintException = 
                    DataAccessSettings.IsForeignKeyConstraintException;
                _AssignSystemErrorVariable();
                return false;   
            }
            return true;
        }
      
        //public static bool IsPersonExist(int ID)
        //{
        //    return clsDataAccessLayer.IsExist(ID);
        //}
        private bool _AddNewPerson()
        { 
            this.ID = clsPersonData.AddNewPersonandGetID(this.NationalNo,this.FName,this.SName,this.TName,
              this.LName , this.DateOfBirth,this.Gender, this.Address
               , this.Phone, this.Email, this.CountryID, this.ImagePath);
            return ID != -1;
        }
        private async Task<bool> _UpdatePerson()
        {
            _FillPersonInfoStruct();
            bool IsSuccess = await clsPersonData.UpdatePerson(personInfo);
            return IsSuccess;
        }

        private void _FillPersonInfoStruct()
        {
            personInfo = new clsPersonData.stPersonInfo();
            personInfo.ID = this.ID;
            personInfo.NationalNo = this.NationalNo;
            personInfo.FName = this.FName;
            personInfo.SName = this.SName;
            personInfo.TName = this.TName;
            personInfo.LName = this.LName;
            personInfo.DateOfBirth = this.DateOfBirth;
            personInfo.Gender = this.Gender;
            personInfo.Address = this.Address;
            personInfo.Phone = this.Phone;
            personInfo.Email = this.Email;
            personInfo.CountryID = this.CountryID;
            personInfo.ImagePath = this.ImagePath;
        }
        private static bool DeleteImageFromImagesFolder(int ID)
        {
            string Path = clsPersonData.GetPath(ID);
            if(Path != "")
            {
                try
                {
                 File.Delete(Path);
                    return true;
                }
                catch
                {
                    clsGlobal.IsSystemError = true;
                    return false;
                }
            }
            return true;
        }
        public async Task<bool> Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewPerson())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                case enMode.Update:
                    return await _UpdatePerson();

                default:
                    return false;
            }
        }

   
    }
}
