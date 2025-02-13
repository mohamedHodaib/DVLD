using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DVLD_DataAccessLayer;
namespace DVLD_BussinessLayer
{
    public class clsDrivingLicenseClass
    {
        private static void _AssignIsSystemErrorVariable()
        {
            clsGlobal.IsSystemError = DataAccessSettings.IsSystemError;
        }

        public static DataTable ListClasses()
        {
            DataTable dt = new DataTable();
            dt = clsClassesData.GetClasses();
            _AssignIsSystemErrorVariable();
            return dt;
        }

        public static int FindClassIDByClassName(string ClassName)
        {
            int ClassID =  clsClassesData.FindClassIDByClassName(ClassName);
            _AssignIsSystemErrorVariable();
            return ClassID;
        }
      public static string  FindClassNameByClassID(int ClassID)
        {
            string ClassName = clsClassesData.FindClassNameByClassID(ClassID);
            _AssignIsSystemErrorVariable();
            return ClassName;
        }
        public static int GetValidityYears(int LicenseClassID)
        {
            return clsClassesData.GetValidityYears(LicenseClassID);
        }

        public static double GetFees(int LicenseClassID)
        {
            return clsClassesData.GetFees(LicenseClassID);
        }
    }
}
