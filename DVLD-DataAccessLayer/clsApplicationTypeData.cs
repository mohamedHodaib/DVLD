using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace DVLD_DataAccessLayer
{
    public class clsApplicationTypeData
    {
        public static DataTable GetApplicationTypes()
        {
            DataAccessSettings.MakeIsSystemErrorFalseBeforeStart();
            DataTable dt = new DataTable();

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DVLD.DB"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_GetApplicationTypes", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    try
                    {
                        conn.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.HasRows) dt.Load(reader);
                        }
                    }
                    catch
                    {
                        DataAccessSettings.IsSystemError = true;
                    }
                }
            }
            return dt;
        }

        public static bool GetApplicationType(int ID, ref string Title, ref float Fees)
        {
            bool IsFound = false;
            DataAccessSettings.MakeIsSystemErrorFalseBeforeStart();

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DVLD.DB"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_GetApplicationType", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ApplicationTypeID", ID);

                    try
                    {
                        conn.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                IsFound = true;
                                Title = reader["ApplicationTypeTitle"].ToString();
                                Fees = Convert.ToSingle(reader["ApplicationFees"]);
                            }
                        }
                    }
                    catch
                    {
                        DataAccessSettings.IsSystemError = true;
                    }
                }
            }
            return IsFound;
        }

        public static double GetApplicationTypeFees(int ApplicationTypeID)
        {
            DataAccessSettings.MakeIsSystemErrorFalseBeforeStart();
            double fees = 0;

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DVLD.DB"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_GetApplicationTypeFees", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ApplicationTypeID", ApplicationTypeID);

                    try
                    {
                        conn.Open();
                        object result = cmd.ExecuteScalar();
                        if (result != null)
                        {
                            fees = Convert.ToDouble(result);
                        }
                    }
                    catch
                    {
                        DataAccessSettings.IsSystemError = true;
                    }
                }
            }
            return fees;
        }

        public static bool Update(int ID, string Title, float Fees)
        {
            DataAccessSettings.MakeIsSystemErrorFalseBeforeStart();
            int affectedRows = 0;

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DVLD.DB"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_UpdateApplicationType", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ApplicationTypeID", ID);
                    cmd.Parameters.AddWithValue("@Title", Title);
                    cmd.Parameters.AddWithValue("@Fees", Fees);

                    try
                    {
                        conn.Open();
                        affectedRows = cmd.ExecuteNonQuery();
                    }
                    catch
                    {
                        DataAccessSettings.IsSystemError = true;
                    }
                }
            }
            return affectedRows > 0;
        }
    }
}
