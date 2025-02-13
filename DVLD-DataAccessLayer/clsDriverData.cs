using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace DVLD_DataAccessLayer
{
    public class clsDriverData
    {
        public static bool IsDriver(int PersonID)
        {
            DataAccessSettings.MakeIsSystemErrorFalseBeforeStart();
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DVLD.DB"].ConnectionString);
            bool IsDriver = false;
            SqlCommand cmd = new SqlCommand("sp_IsDriver", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@PersonID", PersonID);

            try
            {
                conn.Open();
                object ob = cmd.ExecuteScalar();
                if (ob != null)
                    IsDriver = true;
            }
            catch (Exception ex)
            {
                DataAccessSettings.IsSystemError = true;
            }
            finally
            {
                conn.Close();
            }
            return IsDriver;
        }

        public static int AddNewDriverandGetID(int PersonID, int UserID, DateTime CreatedDate)
        {
            DataAccessSettings.MakeIsSystemErrorFalseBeforeStart();
            int DriverID = -1;
            SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DVLD.DB"].ConnectionString);
            SqlCommand command = new SqlCommand("sp_AddNewDriverAndGetID", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@PersonID", PersonID);
            command.Parameters.AddWithValue("@UserID", UserID);
            command.Parameters.AddWithValue("@CreatedDate", CreatedDate);

            try
            {
                connection.Open();
                Object result = command.ExecuteScalar();
                if (result != null && int.TryParse(result.ToString(), out int InsertedID))
                {
                    DriverID = InsertedID;
                }
            }
            catch (Exception ex)
            {
                DataAccessSettings.IsSystemError = true;
            }
            finally
            {
                connection.Close();
            }
            return DriverID;
        }

        public static int GetDriverID(int PersonID)
        {
            DataAccessSettings.MakeIsSystemErrorFalseBeforeStart();
            int DriverID = -1;
            SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DVLD.DB"].ConnectionString);
            SqlCommand command = new SqlCommand("sp_GetDriverID", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@PersonID", PersonID);

            try
            {
                connection.Open();
                Object result = command.ExecuteScalar();
                if (result != null)
                {
                    DriverID = (int)result;
                }
            }
            catch (Exception ex)
            {
                DataAccessSettings.IsSystemError = true;
            }
            finally
            {
                connection.Close();
            }
            return DriverID;
        }

        public static DataTable FilterByPersonID(int ID)
        {
            DataAccessSettings.MakeIsSystemErrorFalseBeforeStart();
            DataTable dt = new DataTable();
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DVLD.DB"].ConnectionString);
            SqlCommand cmd = new SqlCommand("sp_FilterByPersonID", conn);
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
            finally
            {
                conn.Close();
            }
            return dt;
        }

        public static int GetPersonIDByDriverID(int DriverID)
        {
            DataAccessSettings.MakeIsSystemErrorFalseBeforeStart();
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DVLD.DB"].ConnectionString);
            int PersonID = -1;
            SqlCommand cmd = new SqlCommand("sp_GetPersonIDByDriverID", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ID", DriverID);

            try
            {
                conn.Open();
                object ob = cmd.ExecuteScalar();
                if (ob != null)
                    PersonID = Convert.ToInt32(ob);
            }
            catch (Exception ex)
            {
                DataAccessSettings.IsSystemError = true;
            }
            finally
            {
                conn.Close();
            }
            return PersonID;
        }

        public static DataTable FilterByNationalNumber(string NationalNumber)
        {
            DataAccessSettings.MakeIsSystemErrorFalseBeforeStart();
            DataTable dt = new DataTable();
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DVLD.DB"].ConnectionString);
            SqlCommand cmd = new SqlCommand("sp_FilterByNationalNo", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@NationalNumber", NationalNumber);

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
            finally
            {
                conn.Close();
            }
            return dt;
        }

        public static DataTable FilterByFullName(string FullName)
        {
            DataAccessSettings.MakeIsSystemErrorFalseBeforeStart();
            DataTable dt = new DataTable();
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DVLD.DB"].ConnectionString);
            SqlCommand cmd = new SqlCommand("sp_FilterByFullName", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@FullName", FullName);

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
            finally
            {
                conn.Close();
            }
            return dt;
        }

        public static DataTable FilterByDriverID(int ID)
        {
            DataAccessSettings.MakeIsSystemErrorFalseBeforeStart();
            DataTable dt = new DataTable();
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DVLD.DB"].ConnectionString);
            SqlCommand cmd = new SqlCommand("sp_FilterByDriverID", conn);
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
            finally
            {
                conn.Close();
            }
            return dt;
        }

        public static DataTable ListDrivers()
        {
            DataAccessSettings.MakeIsSystemErrorFalseBeforeStart();
            DataTable dt = new DataTable();
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DVLD.DB"].ConnectionString);
            SqlCommand cmd = new SqlCommand("sp_ListDrivers", conn);
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
            finally
            {
                conn.Close();
            }
            return dt;
        }

        public static bool GetDriverInfoByDriverID(int DriverID, ref int PersonID, ref int CreatedByUserID, ref DateTime CreatedDate)
        {
            bool isFound = false;
            SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DVLD.DB"].ConnectionString);
            SqlCommand command = new SqlCommand("sp_GetDriverInfoByDriverID", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@DriverID", DriverID);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    isFound = true;
                    PersonID = (int)reader["PersonID"];
                    CreatedByUserID = (int)reader["CreatedByUserID"];
                    CreatedDate = (DateTime)reader["CreatedDate"];
                }
                else
                {
                    isFound = false;
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                isFound = false;
            }
            finally
            {
                connection.Close();
            }

            return isFound;
        }
    }
}