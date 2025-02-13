using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace DVLD_DataAccessLayer
{
    public class clsClassesData
    {
        public static DataTable GetClasses()
        {
            DataAccessSettings.MakeIsSystemErrorFalseBeforeStart();
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DVLD.DB"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_GetClasses", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    try
                    {
                        conn.Open();
                        SqlDataReader reader = cmd.ExecuteReader();
                        if (reader.HasRows) dt.Load(reader);
                        reader.Close();
                    }
                    catch (Exception ex)
                    {
                        DataAccessSettings.IsSystemError = true;
                    }
                }
            }
            return dt;
        }

        public static int GetValidityYears(int LicenseClassID)
        {
            DataAccessSettings.MakeIsSystemErrorFalseBeforeStart();
            int Years = 0;
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DVLD.DB"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_GetValidityYears", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ClassID", LicenseClassID);
                    try
                    {
                        conn.Open();
                        object ob = cmd.ExecuteScalar();
                        if (ob != null) Years = Convert.ToInt32(ob);
                    }
                    catch (Exception ex)
                    {
                        DataAccessSettings.IsSystemError = true;
                    }
                }
            }
            return Years;
        }

        public static int FindClassIDByClassName(string className)
        {
            DataAccessSettings.MakeIsSystemErrorFalseBeforeStart();
            int ClassID = -1;
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DVLD.DB"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_FindClassIDByClassName", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ClassName", className);
                    try
                    {
                        conn.Open();
                        object ob = cmd.ExecuteScalar();
                        if (ob != null) ClassID = Convert.ToInt32(ob);
                    }
                    catch (Exception ex)
                    {
                        DataAccessSettings.IsSystemError = true;
                    }
                }
            }
            return ClassID;
        }

        public static string FindClassNameByClassID(int classID)
        {
            DataAccessSettings.MakeIsSystemErrorFalseBeforeStart();
            string ClassName = "";
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DVLD.DB"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_FindClassNameByClassID", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@LicenseClassID", classID);
                    try
                    {
                        conn.Open();
                        object ob = cmd.ExecuteScalar();
                        if (ob != null) ClassName = ob.ToString();
                    }
                    catch (Exception ex)
                    {
                        DataAccessSettings.IsSystemError = true;
                    }
                }
            }
            return ClassName;
        }

        public static double GetFees(int ClassID)
        {
            DataAccessSettings.MakeIsSystemErrorFalseBeforeStart();
            double Fees = 0;
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DVLD.DB"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_GetFees", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ClassID", ClassID);
                    try
                    {
                        conn.Open();
                        object ob = cmd.ExecuteScalar();
                        if (ob != null) Fees = Convert.ToDouble(ob);
                    }
                    catch (Exception ex)
                    {
                        DataAccessSettings.IsSystemError = true;
                    }
                }
            }
            return Fees;
        }
    }
}