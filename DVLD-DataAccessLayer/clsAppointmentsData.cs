using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace DVLD_DataAccessLayer
{
    public class clsAppointmentsData
    {
        public static DataTable GetAppointments(int LDLAppID, int TestTypeID)
        {
            DataAccessSettings.MakeIsSystemErrorFalseBeforeStart();
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DVLD.DB"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_GetAppointments", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@LDLAppID", LDLAppID);
                    cmd.Parameters.AddWithValue("@TTID", TestTypeID);
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

        public static bool GetTestAppointmentInfoByID(int TestAppointmentID,
           ref int TestTypeID, ref int LocalDrivingLicenseApplicationID,
           ref DateTime AppointmentDate, ref float PaidFees, ref int CreatedByUserID, ref bool IsLocked, ref int RetakeTestApplicationID)
        {
            bool isFound = false;
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DVLD.DB"].ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("sp_GetTestAppointmentInfoByID", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@TestAppointmentID", TestAppointmentID);
                    try
                    {
                        connection.Open();
                        SqlDataReader reader = command.ExecuteReader();
                        if (reader.Read())
                        {
                            isFound = true;
                            TestTypeID = (int)reader["TestTypeID"];
                            LocalDrivingLicenseApplicationID = (int)reader["LocalDrivingLicenseApplicationID"];
                            AppointmentDate = (DateTime)reader["AppointmentDate"];
                            CreatedByUserID = (int)reader["CreatedByUserID"];
                            PaidFees = Convert.ToSingle(reader["PaidFees"]);
                            IsLocked = (bool)reader["IsLocked"];
                            RetakeTestApplicationID = reader["RetakeTestApplicationID"] == DBNull.Value ? -1 : (int)reader["RetakeTestApplicationID"];
                        }
                        reader.Close();
                    }
                    catch (Exception ex)
                    {
                        isFound = false;
                    }
                }
            }
            return isFound;
        }

        public static bool UpdateAppointment(int AppointmentID, DateTime AppointmentDate)
        {
            DataAccessSettings.MakeIsSystemErrorFalseBeforeStart();
            int RowsAffected = 0;
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DVLD.DB"].ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("sp_UpdateAppointment", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@AppointmentID", AppointmentID);
                    command.Parameters.AddWithValue("@AppointmentDate", AppointmentDate);
                    try
                    {
                        connection.Open();
                        RowsAffected = command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        DataAccessSettings.IsSystemError = true;
                    }
                }
            }
            return RowsAffected > 0;
        }

        public static DateTime GetAppointmentDate(int AppointmentID)
        {
            DataAccessSettings.MakeIsSystemErrorFalseBeforeStart();
            DateTime dt = new DateTime();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DVLD.DB"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_GetAppointmentDate", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@AppointmentID", AppointmentID);
                    try
                    {
                        conn.Open();
                        object ob = cmd.ExecuteScalar();
                        if (ob != null)
                            dt = Convert.ToDateTime(ob);
                    }
                    catch (Exception ex)
                    {
                        DataAccessSettings.IsSystemError = true;
                    }
                }
            }
            return dt;
        }

        public static bool IsHaveAnActiveAppointment(int LDLAppID, int TestTypeID)
        {
            DataAccessSettings.MakeIsSystemErrorFalseBeforeStart();
            bool IsHaveAnActiveAppointment = false;
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DVLD.DB"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_IsHaveAnActiveAppointment", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@LDLAppID", LDLAppID);
                    cmd.Parameters.AddWithValue("@TestTypeID", TestTypeID);
                    try
                    {
                        conn.Open();
                        object ob = cmd.ExecuteScalar();
                        if (ob != null)
                            IsHaveAnActiveAppointment = true;
                    }
                    catch (Exception ex)
                    {
                        DataAccessSettings.IsSystemError = true;
                    }
                }
            }
            return IsHaveAnActiveAppointment;
        }

        public static int AddNewAppointmentandGetID(int TestTypeID, int LocalDrivingLicenseApplicationID, DateTime AppointmentDate, double PaidFees, int UserID, int RetakeTestApplicationID)
        {
            DataAccessSettings.MakeIsSystemErrorFalseBeforeStart();
            int AppointmentID = -1;
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DVLD.DB"].ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("sp_AddNewAppointmentandGetID", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@TestTypeID", TestTypeID);
                    command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
                    command.Parameters.AddWithValue("@AppointmentDate", AppointmentDate);
                    command.Parameters.AddWithValue("@PaidFees", PaidFees);
                    command.Parameters.AddWithValue("@CreatedByUserID", UserID);
                    command.Parameters.AddWithValue("@RetakeTestApplicationID", RetakeTestApplicationID == -1 ? DBNull.Value : (object)RetakeTestApplicationID);
                    try
                    {
                        connection.Open();
                        object result = command.ExecuteScalar();
                        if (result != null && int.TryParse(result.ToString(), out int InsertedID))
                        {
                            AppointmentID = InsertedID;
                        }
                    }
                    catch (Exception ex)
                    {
                        DataAccessSettings.IsSystemError = true;
                    }
                }
            }
            return AppointmentID;
        }

        public static bool LockTheAppointment(int AppointmentID)
        {
            DataAccessSettings.MakeIsSystemErrorFalseBeforeStart();
            int RowsAffected = 0;
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DVLD.DB"].ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("sp_LockTheAppointment", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@AppointmentID", AppointmentID);
                    try
                    {
                        connection.Open();
                        RowsAffected = command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        DataAccessSettings.IsSystemError = true;
                    }
                }
            }
            return RowsAffected > 0;
        }

        public static bool IsAppointmentLocked(int AppointmentID)
        {
            DataAccessSettings.MakeIsSystemErrorFalseBeforeStart();
            bool IsLocked = false;
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DVLD.DB"].ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("sp_IsAppointmentLocked", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@AppointmentID", AppointmentID);
                    try
                    {
                        connection.Open();
                        object ob = command.ExecuteScalar();
                        if (ob != null)
                            IsLocked = true;
                    }
                    catch (Exception ex)
                    {
                        DataAccessSettings.IsSystemError = true;
                    }
                }
            }
            return IsLocked;
        }
    }
}