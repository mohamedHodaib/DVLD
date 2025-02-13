using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DVLD_BussinessLayer;
using DVLD_DataAccessLayer;
namespace DVLD_DataAccessLayer
{
    public  class clsCountry
    {
        private static DataTable _dt;
        private static void _CheckForSystemError()
        {
            if (_dt.Rows.Count == 0)
            {
                if (DataAccessSettings.IsSystemError) clsGlobal.IsSystemError = true;
            }
        }
        public static DataTable ListCountries()
        {
            _dt = clsCountryData.ListCountries();
            _CheckForSystemError();
            return _dt;
        }
        public static int FindCountryID(string CountryName)
        {
            return clsCountryData.FindCountryID(CountryName);
        }
        public static string FindCountryName(int CountryID)
        {
            return clsCountryData.FindCountryName(CountryID);
        }
    }
}
