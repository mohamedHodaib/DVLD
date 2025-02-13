using DVLD_DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_BussinessLayer
{
    public class clsTestTypes
    {

        static DataTable _dt = new DataTable();
        public int ID { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }

        public float Fees { get; set; }

        public  enum enTestType {VisionTest= 1,WrittenTest = 2,StreetTest = 3};

       
        public clsTestTypes()
        {
            this.ID = 0;
            this.Title = string.Empty;
            this.Description = string.Empty;
            this.Fees = 0;
        }


        public clsTestTypes(int iD, string title,string description, float fees)
        {
            ID = iD;
            Title = title;
            Description = description;
            Fees = fees;
        }

        private static void _AssignIsSystemErrorVariable()
        {
            clsGlobal.IsSystemError = DataAccessSettings.IsSystemError;
        }
        public static DataTable GetTestTypes()
        {
            _dt = clsTestTypeData.GetTestTypes();
            _AssignIsSystemErrorVariable();
            return _dt;
        }

        public static clsTestTypes GetTestType(int ID)
        {
            string Title = string.Empty, Description = string.Empty;
            float Fees = 0;

            if (clsTestTypeData.GetTestType(ID, ref Title,ref Description, ref Fees))
                return new clsTestTypes(ID, Title,Description, Fees);
            else
                return null;
        }
        public static double GetTestTypeFees(clsTestTypes.enTestType TestType)
        {
            double Fees = clsTestTypeData.GetTestTypeFees((int)TestType);
            _AssignIsSystemErrorVariable();
            return Fees;
        }
        public  bool Save()
        {
            bool IsSaved = clsTestTypeData.Update(ID, Title,Description, Fees);
            _AssignIsSystemErrorVariable();
            return IsSaved;
        }
    }
}
