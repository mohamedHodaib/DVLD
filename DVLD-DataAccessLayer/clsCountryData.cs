using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace DVLD_DataAccessLayer
{
    public class clsCountryData
    {
        public static int FindCountryID(string CountryName)
        {
            int CountryID = -1;
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DVLD.DB"].ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("FindCountryID", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CountryName", CountryName);
                SqlParameter outputParam = new SqlParameter("@CountryID", SqlDbType.Int) { Direction = ParameterDirection.Output };
                cmd.Parameters.Add(outputParam);

                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    if (outputParam.Value != DBNull.Value)
                        CountryID = (int)outputParam.Value;
                }
                catch (Exception ex)
                {
                    DataAccessSettings.IsSystemError = true;
                }
            }
            return CountryID;
        }

        public static string FindCountryName(int CountryID)
        {
            string CountryName = "";
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DVLD.DB"].ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("FindCountryName", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CountryID", CountryID);
                SqlParameter outputParam = new SqlParameter("@CountryName", SqlDbType.NVarChar, 255) { Direction = ParameterDirection.Output };
                cmd.Parameters.Add(outputParam);

                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    if (outputParam.Value != DBNull.Value)
                        CountryName = outputParam.Value.ToString();
                }
                catch (Exception ex)
                {
                    DataAccessSettings.IsSystemError = true;
                }
            }
            return CountryName;
        }

        public static DataTable ListCountries()
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DVLD.DB"].ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("ListCountries", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                try
                {
                    conn.Open();
                  using(  SqlDataReader reader = cmd.ExecuteReader())
                    if (reader.HasRows) dt.Load(reader);
                 
                }
                catch (Exception ex)
                {
                    DataAccessSettings.IsSystemError = true;
                }
            }
            return dt;
        }
    }
}
