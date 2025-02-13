using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace DVLD_DataAccessLayer
{
    public class clsInternationalLicenseData
    {
        public static DataTable ListPersonLicenses(int PersonID)
        {
            DataAccessSettings.MakeIsSystemErrorFalseBeforeStart();
            DataTable dt = new DataTable();

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DVLD.DB"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("ListPersonLicenses", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@PersonID", PersonID);

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

        public static int GetActiveInternationalLicenseIDByDriverID(int DriverID)
        {
            int InternationalLicenseID = -1;

            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DVLD.DB"].ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("GetActiveInternationalLicenseIDByDriverID", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@DriverID", DriverID);

                    try
                    {
                        connection.Open();
                        object result = command.ExecuteScalar();

                        if (result != null && int.TryParse(result.ToString(), out int insertedID))
                        {
                            InternationalLicenseID = insertedID;
                        }
                    }
                    catch (Exception ex)
                    {
                        // Handle exception
                    }
                }
            }

            return InternationalLicenseID;
        }

        public static DataTable ListAllLicenses()
        {
            DataAccessSettings.MakeIsSystemErrorFalseBeforeStart();
            DataTable dt = new DataTable();

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DVLD.DB"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("ListAllLicenses", conn))
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

        public static DataTable FilterByILID(int ID)
        {
            DataAccessSettings.MakeIsSystemErrorFalseBeforeStart();
            DataTable dt = new DataTable();

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DVLD.DB"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("FilterByILID", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID", ID);

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

        public static DataTable FilterByAppID(int ID)
        {
            DataAccessSettings.MakeIsSystemErrorFalseBeforeStart();
            DataTable dt = new DataTable();

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DVLD.DB"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("FilterByAppID", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID", ID);

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

        public static DataTable FilterByLLID(int ID)
        {
            DataAccessSettings.MakeIsSystemErrorFalseBeforeStart();
            DataTable dt = new DataTable();

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DVLD.DB"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("FilterByLLID", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID", ID);

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

        public static DataTable FilterByDriverID(int ID)
        {
            DataAccessSettings.MakeIsSystemErrorFalseBeforeStart();
            DataTable dt = new DataTable();

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DVLD.DB"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("FilterByDriverID", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID", ID);

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

        public static DataTable FilterByActivation(string IsActiveFilter)
        {
            DataAccessSettings.MakeIsSystemErrorFalseBeforeStart();
            DataTable dt = new DataTable();

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DVLD.DB"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("FilterByActivation", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@IsActive", IsActiveFilter == "Yes" ? 1 : 0);

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

        public static int FindInternationalLicense(int LLicenseID)
        {
            DataAccessSettings.MakeIsSystemErrorFalseBeforeStart();
            int ILID = -1;

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DVLD.DB"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("FindInternationalLicense", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@LLicenseID", LLicenseID);

                    try
                    {
                        conn.Open();
                        object result = cmd.ExecuteScalar();
                        if (result != null) ILID = Convert.ToInt32(result);
                    }
                    catch (Exception ex)
                    {
                        DataAccessSettings.IsSystemError = true;
                    }
                }
            }

            return ILID;
        }

        public static int GetPersonIDByILID(int ILID)
        {
            DataAccessSettings.MakeIsSystemErrorFalseBeforeStart();
            int PersonID = -1;

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DVLD.DB"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("GetPersonIDByILID", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ILID", ILID);

                    try
                    {
                        conn.Open();
                        object result = cmd.ExecuteScalar();
                        if (result != null) PersonID = Convert.ToInt32(result);
                    }
                    catch (Exception ex)
                    {
                        DataAccessSettings.IsSystemError = true;
                    }
                }
            }

            return PersonID;
        }

        public static int AddNewIL(int ApplicationID, int DriverID, int LLID, DateTime IssueDate, DateTime ExpirationDate, bool IsActive, int UserID)
        {
            DataAccessSettings.MakeIsSystemErrorFalseBeforeStart();
            int ILID = -1;

            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DVLD.DB"].ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("AddNewIL", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
                    command.Parameters.AddWithValue("@DriverID", DriverID);
                    command.Parameters.AddWithValue("@LLID", LLID);
                    command.Parameters.AddWithValue("@IssueDate", IssueDate);
                    command.Parameters.AddWithValue("@ExpDate", ExpirationDate);
                    command.Parameters.AddWithValue("@IsActive", IsActive);
                    command.Parameters.AddWithValue("@UserID", UserID);

                    try
                    {
                        connection.Open();
                        object result = command.ExecuteScalar();

                        if (result != null && int.TryParse(result.ToString(), out int InsertedID))
                        {
                            ILID = InsertedID;
                        }
                    }
                    catch (Exception ex)
                    {
                        DataAccessSettings.IsSystemError = true;
                    }
                }
            }

            return ILID;
        }
    }
}