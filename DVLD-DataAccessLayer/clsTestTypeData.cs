using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace DVLD_DataAccessLayer
{
    public class clsTestTypeData
    {
        public static DataTable GetTestTypes()
        {
            DataAccessSettings.MakeIsSystemErrorFalseBeforeStart();
            DataTable dt = new DataTable();

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DVLD.DB"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("GetTestTypes", conn))
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

        public static bool GetTestType(int ID, ref string Title, ref string Description, ref float Fees)
        {
            bool IsFound = false;
            DataAccessSettings.MakeIsSystemErrorFalseBeforeStart();

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DVLD.DB"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("GetTestType", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID", ID);

                    try
                    {
                        conn.Open();
                        SqlDataReader reader = cmd.ExecuteReader();

                        if (reader.Read())
                        {
                            IsFound = true;
                            Title = reader["TestTypeTitle"].ToString();
                            Description = reader["TestTypeDescription"].ToString();
                            Fees = Convert.ToSingle(reader["TestTypeFees"]);
                        }

                        reader.Close();
                    }
                    catch (Exception ex)
                    {
                        DataAccessSettings.IsSystemError = true;
                    }
                }
            }

            return IsFound;
        }

        public static double GetTestTypeFees(int TestTypeID)
        {
            double Fees = 0;
            DataAccessSettings.MakeIsSystemErrorFalseBeforeStart();

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DVLD.DB"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("GetTestTypeFees", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID", TestTypeID);

                    try
                    {
                        conn.Open();
                        object result = cmd.ExecuteScalar();

                        if (result != null)
                        {
                            Fees = Convert.ToDouble(result);
                        }
                    }
                    catch (Exception ex)
                    {
                        DataAccessSettings.IsSystemError = true;
                    }
                }
            }

            return Fees;
        }

        public static bool Update(int ID, string Title, string Description, float Fees)
        {
            DataAccessSettings.MakeIsSystemErrorFalseBeforeStart();
            int AffectedRows = 0;

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DVLD.DB"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("UpdateTestType", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID", ID);
                    cmd.Parameters.AddWithValue("@Title", Title);
                    cmd.Parameters.AddWithValue("@Description", Description);
                    cmd.Parameters.AddWithValue("@Fees", Fees);

                    try
                    {
                        conn.Open();
                        AffectedRows = cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        DataAccessSettings.IsSystemError = true;
                    }
                }
            }

            return AffectedRows > 0;
        }
    }
}