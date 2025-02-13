using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace DVLD_DataAccessLayer
{
    public class clsApplicationData
    {
        public static int AddNewApplicationandGetID(int ApplicantPersonID, DateTime ApplicationDate,
              int ApplicationTypeID, byte ApplicationStatus, DateTime LastStatusDate, double PaidFees, int UserID)
        {
            DataAccessSettings.MakeIsSystemErrorFalseBeforeStart();
            int ApplicationID = -1;

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DVLD.DB"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_AddNewApplication", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ApplicantPersonID", ApplicantPersonID);
                    cmd.Parameters.AddWithValue("@ApplicationDate", ApplicationDate);
                    cmd.Parameters.AddWithValue("@ApplicationTypeID", ApplicationTypeID);
                    cmd.Parameters.AddWithValue("@ApplicationStatus", ApplicationStatus);
                    cmd.Parameters.AddWithValue("@LastStatusDate", LastStatusDate);
                    cmd.Parameters.AddWithValue("@PaidFees", PaidFees);
                    cmd.Parameters.AddWithValue("@UserID", UserID);

                    try
                    {
                        conn.Open();
                        object result = cmd.ExecuteScalar();
                        if (result != null && int.TryParse(result.ToString(), out int InsertedID))
                        {
                            ApplicationID = InsertedID;
                        }
                    }
                    catch
                    {
                        DataAccessSettings.IsSystemError = true;
                    }
                }
            }
            return ApplicationID;
        }

        public static bool UpdateApplicationStatus(int ApplicationID, DateTime LastStatusDate, byte Status)
        {
            DataAccessSettings.MakeIsSystemErrorFalseBeforeStart();
            int affectedRows = 0;

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DVLD.DB"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_UpdateApplicationStatus", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ApplicationID", ApplicationID);
                    cmd.Parameters.AddWithValue("@NewStatus", Status);
                    cmd.Parameters.AddWithValue("@LastStatusDate", LastStatusDate);

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

        public static int GetPersonID(int AppID)
        {
            DataAccessSettings.MakeIsSystemErrorFalseBeforeStart();
            int PersonID = -1;

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DVLD.DB"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_GetPersonID", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ApplicationID", AppID);

                    try
                    {
                        conn.Open();
                        object result = cmd.ExecuteScalar();
                        if (result != null)
                        {
                            PersonID = Convert.ToInt32(result);
                        }
                    }
                    catch
                    {
                        DataAccessSettings.IsSystemError = true;
                    }
                }
            }
            return PersonID;
        }

        public static int GetApplicationStatus(int LDLAppID)
        {
            DataAccessSettings.MakeIsSystemErrorFalseBeforeStart();
            int Status = -1;

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DVLD.DB"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_GetApplicationStatus", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@LDLAppID", LDLAppID);

                    try
                    {
                        conn.Open();
                        object result = cmd.ExecuteScalar();
                        if (result != null)
                        {
                            Status = Convert.ToInt32(result);
                        }
                    }
                    catch
                    {
                        DataAccessSettings.IsSystemError = true;
                    }
                }
            }
            return Status;
        }

        public static bool DeleteApplication(int AppID)
        {
            DataAccessSettings.MakeIsSystemErrorFalseBeforeStart();
            int affectedRows = 0;

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DVLD.DB"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_DeleteApplication", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ApplicationID", AppID);

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

        public static bool IsPersonHasLicenseInThisClass(int PersonID, int ApplicationTypeID, int LicenseClassID)
        {
            DataAccessSettings.MakeIsSystemErrorFalseBeforeStart();
            bool HasLicense = false;

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DVLD.DB"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_IsPersonHasLicenseInThisClass", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@PersonID", PersonID);
                    cmd.Parameters.AddWithValue("@ApplicationTypeID", ApplicationTypeID);
                    cmd.Parameters.AddWithValue("@LicenseClassID", LicenseClassID);

                    try
                    {
                        conn.Open();
                        object result = cmd.ExecuteScalar();
                        HasLicense = result != null;
                    }
                    catch
                    {
                        DataAccessSettings.IsSystemError = true;
                    }
                }
            }
            return HasLicense;
        }

        public static DataTable FindApplicationByID(int ApplicationID)
        {
            DataAccessSettings.MakeIsSystemErrorFalseBeforeStart();
            DataTable dt = new DataTable();

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DVLD.DB"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_FindApplicationByID", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ApplicationID", ApplicationID);

                    try
                    {
                        conn.Open();
                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        adapter.Fill(dt);
                    }
                    catch
                    {
                        DataAccessSettings.IsSystemError = true;
                    }
                }
            }
            return dt;
        }

        public static bool UpdateApplication(int ApplicationID, byte ApplicationStatus)
        {
            DataAccessSettings.MakeIsSystemErrorFalseBeforeStart();
            int affectedRows = 0;

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DVLD.DB"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_UpdateApplication", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ApplicationID", ApplicationID);
                    cmd.Parameters.AddWithValue("@ApplicationStatus", ApplicationStatus);

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
