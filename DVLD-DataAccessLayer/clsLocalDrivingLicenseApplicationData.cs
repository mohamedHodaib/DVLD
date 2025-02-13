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
    public class clsLocalDrivingLicenseApplicationData
    {
        public struct LDLAppInfo
        {
            public int ApplicationID { get; set; }
            public int LicenseClassID { get; set; }
        }

        public static LDLAppInfo lDLAppInfo = new LDLAppInfo();

        public static int AddNewApplicationandGetID(int ApplicationID, int LicenseClassID)
        {
            DataAccessSettings.MakeIsSystemErrorFalseBeforeStart();
            int LDLAppID = -1;
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DVLD.DB"].ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("AddNewApplicationandGetID", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
                    command.Parameters.AddWithValue("@LicenseClassID", LicenseClassID);

                    try
                    {
                        connection.Open();
                        object result = command.ExecuteScalar();
                        if (result != null && int.TryParse(result.ToString(), out int InsertedID))
                        {
                            LDLAppID = InsertedID;
                        }
                    }
                    catch (Exception ex)
                    {
                        // Handle exception
                    }
                }
            }
            return LDLAppID;
        }

        public static bool DoesAttendTestType(int LocalDrivingLicenseApplicationID, int TestTypeID)
        {
            bool IsFound = false;
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DVLD.DB"].ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("DoesAttendTestType", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
                    command.Parameters.AddWithValue("@TestTypeID", TestTypeID);

                    try
                    {
                        connection.Open();
                        object result = command.ExecuteScalar();
                        if (result != null)
                        {
                            IsFound = true;
                        }
                    }
                    catch (Exception ex)
                    {
                        // Handle exception
                    }
                }
            }
            return IsFound;
        }

        public static int GetLocalLicenseIDByLDLAppID(int LDLAppID)
        {
            DataAccessSettings.MakeIsSystemErrorFalseBeforeStart();
            int LicenseID = -1;
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DVLD.DB"].ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("GetLocalLicenseIDByLDLAppID", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@LDLAppID", LDLAppID);

                    try
                    {
                        connection.Open();
                        object result = command.ExecuteScalar();
                        if (result != null && int.TryParse(result.ToString(), out int InsertedID))
                        {
                            LicenseID = InsertedID;
                        }
                    }
                    catch (Exception ex)
                    {
                        // Handle exception
                    }
                }
            }
            return LicenseID;
        }

        public static int GetApplicationID(int LDLAppID)
        {
            DataAccessSettings.MakeIsSystemErrorFalseBeforeStart();
            int AppID = 0;
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DVLD.DB"].ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("GetApplicationID", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@LDLAppID", LDLAppID);

                    try
                    {
                        connection.Open();
                        object result = command.ExecuteScalar();
                        if (result != null)
                        {
                            AppID = Convert.ToInt32(result);
                        }
                    }
                    catch (Exception ex)
                    {
                        // Handle exception
                    }
                }
            }
            return AppID;
        }

        public static int GetPersonIDByLDLAppID(int LDLAppID)
        {
            DataAccessSettings.MakeIsSystemErrorFalseBeforeStart();
            int PersonID = -1;
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DVLD.DB"].ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("GetPersonIDByLDLAppID", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@LDLAppID", LDLAppID);

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
                        // Handle exception
                    }
                }
            }
            return PersonID;
        }

        public static bool IsPassedThisTestType(int LDLAppID, int TestTypeID)
        {
            DataAccessSettings.MakeIsSystemErrorFalseBeforeStart();
            bool IsPassedThisTestType = false;
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DVLD.DB"].ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("IsPassedThisTestType", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@LDLAppID", LDLAppID);
                    command.Parameters.AddWithValue("@TestTypeID", TestTypeID);

                    try
                    {
                        connection.Open();
                        object result = command.ExecuteScalar();
                        if (result != null)
                        {
                            IsPassedThisTestType = true;
                        }
                    }
                    catch (Exception ex)
                    {
                        // Handle exception
                    }
                }
            }
            return IsPassedThisTestType;
        }

        public static bool UpdateApplication(int LDLAppID, int ClassID)
        {
            DataAccessSettings.MakeIsSystemErrorFalseBeforeStart();
            int AffectedRows = 0;
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DVLD.DB"].ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("UpdateApplication", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@LDLAppID", LDLAppID);
                    command.Parameters.AddWithValue("@LicenseClassID", ClassID);

                    try
                    {
                        connection.Open();
                        AffectedRows = command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        // Handle exception
                    }
                }
            }
            return AffectedRows > 0;
        }

        public static int GetPersonID(int AppID)
        {
            DataAccessSettings.MakeIsSystemErrorFalseBeforeStart();
            int PersonID = -1;
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DVLD.DB"].ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("GetPersonID", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@AppID", AppID);

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
                        // Handle exception
                    }
                }
            }
            return PersonID;
        }

        public static bool FindLDLApplicationByID(int LocalDrivingLicenseApplicationID)
        {
            DataAccessSettings.MakeIsSystemErrorFalseBeforeStart();
            bool IsFound = false;
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DVLD.DB"].ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("FindLDLApplicationByID", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);

                    try
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                IsFound = true;
                                lDLAppInfo.ApplicationID = (int)reader["ApplicationID"];
                                lDLAppInfo.LicenseClassID = (int)reader["LicenseClassID"];
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        // Handle exception
                    }
                }
            }
            return IsFound;
        }

        public static bool DeleteLocalDrivingLicenseApplication(int LDLAppID)
        {
            DataAccessSettings.MakeIsSystemErrorFalseBeforeStart();
            int AffectedRows = 0;
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DVLD.DB"].ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("DeleteLocalDrivingLicenseApplication", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@LDLAppID", LDLAppID);

                    try
                    {
                        connection.Open();
                        AffectedRows = command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        // Handle exception
                    }
                }
            }
            return AffectedRows > 0;
        }

        public static DataTable ListLocalDrivingLicenseApplications()
        {
            DataAccessSettings.MakeIsSystemErrorFalseBeforeStart();
            DataTable dt = new DataTable();
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DVLD.DB"].ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("ListLocalDrivingLicenseApplications", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

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
                        // Handle exception
                    }
                }
            }
            return dt;
        }
    }
}