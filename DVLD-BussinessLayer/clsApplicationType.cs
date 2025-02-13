using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DVLD_DataAccessLayer;
namespace DVLD_BussinessLayer
{
    public class clsApplicationType
    {

        public int ID { get; set; }
         
        public string Title { get; set; }
         
        public float Fees { get; set; }
        static DataTable _dt = new DataTable();


        public clsApplicationType()
        {
            this.ID = 0;
            this.Title = string.Empty;
            this.Fees = 0;
        }


        public clsApplicationType(int iD, string title, float fees)
        {
            ID = iD;
            Title = title;
            Fees = fees;
        }

        private static void _AssignIsSystemErrorVariable()
        {
            clsGlobal.IsSystemError = DataAccessSettings.IsSystemError;
        }
        public static DataTable GetApplicationTypes()
        {
           _dt =   clsApplicationTypeData.GetApplicationTypes();
            _AssignIsSystemErrorVariable();
            return _dt;
        }

        public static clsApplicationType GetApplicationType(int ID)
        {
            string Title = string.Empty;
            float Fees = 0;

            if (clsApplicationTypeData.GetApplicationType(ID, ref Title, ref Fees))
                return new clsApplicationType(ID, Title, Fees);
            else
                return null;
        }
        public static double  GetApplicationTypeFees (clsApplication.enApplicationType ApplicationType)
        {
            double Fees = clsApplicationTypeData.GetApplicationTypeFees((int)ApplicationType);
            _AssignIsSystemErrorVariable();
            return Fees;
        }
     
        public  bool Save()
        {
            bool IsSaved =  clsApplicationTypeData.Update(this.ID, this.Title, this.Fees);
            _AssignIsSystemErrorVariable();
            return IsSaved;
        }

    }
}
