using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using System.Configuration;

namespace DVLD_DataAccessLayer
{
    public class clsLocalLicenseData
    {
        public static int AddNewLicenseandGetID(int ApplicationID, int DriverID, int LicenseClass,
                DateTime IssueDate, DateTime ExpirationDate, string Notes, double PaidFees, bool IsActive, byte IssueReason, int UserID)
        {
            DataAccessSettings.MakeIsSystemErrorFalseBeforeStart();
            int LicenseID = -1;
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DVLD.DB"].ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("AddNewLicenseandGetID", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
                    command.Parameters.AddWithValue("@DriverID", DriverID);
                    command.Parameters.AddWithValue("@LicenseClass", LicenseClass);
                    command.Parameters.AddWithValue("@IssueDate", IssueDate);
                    command.Parameters.AddWithValue("@ExpirationDate", ExpirationDate);
                    command.Parameters.AddWithValue("@Notes", string.IsNullOrEmpty(Notes) ? (object)DBNull.Value : Notes);
                    command.Parameters.AddWithValue("@PaidFees", PaidFees);
                    command.Parameters.AddWithValue("@IsActive", IsActive);
                    command.Parameters.AddWithValue("@IssueReason", IssueReason);
                    command.Parameters.AddWithValue("@UserID", UserID);
                    SqlParameter output = new SqlParameter("@LicenseID", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };

                    try
                    {
                        connection.Open();
                        command.ExecuteNonQuery();
                        if (output.Value != DBNull.Value)
                        {
                            LicenseID = Convert.ToInt32(output.Value);
                        }
                    }
                    catch (Exception ex)
                    {
                        DataAccessSettings.IsSystemError = true;
                    }
                }
            }
            return LicenseID;
        }

        public static int GetDriverID(int LicenseID)
        {
            DataAccessSettings.MakeIsSystemErrorFalseBeforeStart();
            int DriverID = -1;
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DVLD.DB"].ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("GetDriverID", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@LicenseID", LicenseID);

                    try
                    {
                        connection.Open();
                        object result = command.ExecuteScalar();
                        if (result != null)
                        {
                            DriverID = Convert.ToInt32(result);
                        }
                    }
                    catch (Exception ex)
                    {
                        DataAccessSettings.IsSystemError = true;
                    }
                }
            }
            return DriverID;
        }

        public static bool DeActive(int LicenseID)
        {
            DataAccessSettings.MakeIsSystemErrorFalseBeforeStart();
            int AffectedRows = 0;
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DVLD.DB"].ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("DeActive", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@LicenseID", LicenseID);

                    try
                    {
                        connection.Open();
                        AffectedRows = command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        DataAccessSettings.IsSystemError = true;
                    }
                }
            }
            return AffectedRows > 0;
        }

        public static bool IsExpired(int LicenseID)
        {
            DataAccessSettings.MakeIsSystemErrorFalseBeforeStart();
            bool IsExpired = false;
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DVLD.DB"].ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("IsExpired", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@LicenseID", LicenseID);

                    try
                    {
                        connection.Open();
                        object result = command.ExecuteScalar();
                        if (result != null)
                        {
                            IsExpired = true;
                        }
                    }
                    catch (Exception ex)
                    {
                        DataAccessSettings.IsSystemError = true;
                    }
                }
            }
            return IsExpired;
        }

        public static int IsDriverHasAnActiveLicenseInThisClass(int DriverID, int LicenseClassID, int CurrentLicenseID)
        {
            DataAccessSettings.MakeIsSystemErrorFalseBeforeStart();
            int LicenseID = -1;
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DVLD.DB"].ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("IsDriverHasAnActiveLicenseInThisClass", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@DriverID", DriverID);
                    command.Parameters.AddWithValue("@LicenseClassID", LicenseClassID);
                    command.Parameters.AddWithValue("@CurrentLicenseID", CurrentLicenseID);

                    try
                    {
                        connection.Open();
                        object result = command.ExecuteScalar();
                        if (result != null)
                        {
                            LicenseID = Convert.ToInt32(result);
                        }
                    }
                    catch (Exception ex)
                    {
                        DataAccessSettings.IsSystemError = true;
                    }
                }
            }
            return LicenseID;
        }

        public static DataTable ListLicenses(int PersonID)
        {
            DataAccessSettings.MakeIsSystemErrorFalseBeforeStart();
            DataTable dt = new DataTable();
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DVLD.DB"].ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("ListLicenses", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@PersonID", PersonID);

                    try
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                dt.Load(reader);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        DataAccessSettings.IsSystemError = true;
                    }
                }
            }
            return dt;
        }

        public static int GetPersonIDByLocalLicenseID(int LocalLicenseID)
        {
            DataAccessSettings.MakeIsSystemErrorFalseBeforeStart();
            int PersonID = -1;
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DVLD.DB"].ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("GetPersonIDByLocalLicenseID", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@LocalLicenseID", LocalLicenseID);

                    try
                    {
                        connection.Open();
                        object result = command.ExecuteScalar();
                        if (result != null)
                        {
                            PersonID = Convert.ToInt32(result);
                        }
                    }
                    catch (Exception ex)
                    {
                        DataAccessSettings.IsSystemError = true;
                    }
                }
            }
            return PersonID;
        }
    }
}