using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Configuration;

namespace DVLD_DataAccessLayer
{
    public  class clsUserData
    {

        public struct UserResult
        {
            public bool IsFound { get; set; }
            public int PersonID { get; set; }
            public string UserName { get; set; }
            public string Password { get; set; }
            public byte IsActive { get; set; }
        }


        public static bool FindUserByUserNameandPassword(string UserName, string Password,ref int UserID,ref int 
            PersonID,ref byte IsActive)
        {
           DataAccessSettings. MakeIsSystemErrorFalseBeforeStart();
                      bool IsFound = false;
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DVLD.DB"].ConnectionString))
            {

                using (SqlCommand cmd = new SqlCommand("SP_FindUserByUserNameandPassword", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@UserName", UserName);
                  cmd.Parameters.AddWithValue("@Password", Password);
                  try
                  {
                      conn.Open();
                        using (SqlDataReader Reader = cmd.ExecuteReader())
                        {

                            if (Reader.Read())
                            {
                                IsFound = true;
                                PersonID = (int)Reader["PersonID"];
                                UserID = (int)Reader["UserID"];
                                IsActive = Convert.ToByte(Reader["IsActive"]);
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

        public static async Task<UserResult> FindByUserIDAsync(int UserID)
        {
            DataAccessSettings.MakeIsSystemErrorFalseBeforeStart();
            UserResult result = new UserResult { IsFound = false };

            string connectionString = ConfigurationManager.ConnectionStrings["DVLD.DB"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("SP_FindByUserIDAsync", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@UserID", UserID);

                    try
                    {
                        await connection.OpenAsync();
                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                result.IsFound = true;
                                result.PersonID = (int)reader["PersonID"];
                                result.UserName = (string)reader["UserName"];
                                result.Password = (string)reader["Password"];
                                result.IsActive = Convert.ToByte(reader["IsActive"]);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        DataAccessSettings.IsSystemError = true;
                    }
                }
            }

            return result;
        }



        public static int AddNewUserandGetID(int PersonID,string  UserName,string Password,byte IsActive)
        {
           DataAccessSettings. MakeIsSystemErrorFalseBeforeStart ();
            int UserID = -1;
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DVLD.DB"].ConnectionString))
            {

                using (SqlCommand command = new SqlCommand("SP_AddNewUserandGetID", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@PersonID", PersonID);
                   command.Parameters.AddWithValue("@UserName", UserName);
                   command.Parameters.AddWithValue("@Password", Password);
                   command.Parameters.AddWithValue("@IsActive", IsActive);
                  
                   SqlParameter newIDParam = new SqlParameter("@UserID", SqlDbType.Int)
                   {
                       Direction = ParameterDirection.Output
                   };
                  
                   command.Parameters.Add(newIDParam);
                  
                   try
                   {
                       connection.Open();
                       command.ExecuteNonQuery(); //return the first column of the first row of the 
                                                                //last result Set
                       if (newIDParam.Value != DBNull.Value)
                       {
                           UserID = Convert.ToInt32(newIDParam.Value);
                       }
                   }
                   catch 
                   {
                         return -1;
                   }
        
                }
            }

            return UserID;
        }

        public static bool IsUserNameUsed(string UserName)
        {
            DataAccessSettings.MakeIsSystemErrorFalseBeforeStart();
            bool IsUsed = false;
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DVLD.DB"].ConnectionString))
            {

                using (SqlCommand cmd = new SqlCommand("SP_IsUserNameUsed", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@UserName", UserName);
                     try
                     {
                         conn.Open();
                         object ob = cmd.ExecuteScalar();
                         if (ob != DBNull.Value) IsUsed = true;
                     }
                     catch
                     {
                         DataAccessSettings.IsSystemError = true;
                     }
                     return IsUsed;

                }
            }
          }
        public static bool DeleteUser(int UserID)
        {
            DataAccessSettings.MakeIsSystemErrorFalseBeforeStart();
            int AffectedRows = 0;
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DVLD.DB"].ConnectionString))
            {

                using (SqlCommand cmd = new SqlCommand("SP_DeleteUser", conn))
                {

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@UserID", UserID);
                   
                   try
                   {
                       conn.Open();
                       AffectedRows = cmd.ExecuteNonQuery();
                   }
                   catch (Exception ex)
                   {
                       if (ex is SqlException Ex)
                       {
                           if (Ex.Number == 547)
                           {
                               DataAccessSettings.IsForeignKeyConstraintException = true;
                           }
                       }
                       else
                           DataAccessSettings.IsSystemError = true;
                   }
                }
            }
            return AffectedRows > 0;
        }
        public static DataTable ListUsers()
        {
            DataAccessSettings.MakeIsSystemErrorFalseBeforeStart();
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DVLD.DB"].ConnectionString))
            {

                using (SqlCommand cmd = new SqlCommand("SP_ListUsers", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    try
                    {
                        conn.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                            if (reader.HasRows) dt.Load(reader);

                    }
                    catch (Exception ex)
                    {
                        DataAccessSettings.IsSystemError = true;
                    }
                }
            }
            return dt;

        }
        public static bool IsPersonLinkedToUser(int ID)
        {
            DataAccessSettings.MakeIsSystemErrorFalseBeforeStart();
            bool IsLinked = false;
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DVLD.DB"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SP_IsPersonLinkedToUser", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@PersonID", ID);
                    try
                    {
                        conn.Open();
                        object ob = cmd.ExecuteScalar();
                        if (ob != null) IsLinked = true;
                    }
                    catch
                    {
                        DataAccessSettings.IsSystemError = true;
                    }
                }
                
            }
                  return IsLinked;
        }
        public static bool UpdateUser(int UserID, string UserName, string Password, byte IsActive)
        {
            DataAccessSettings.MakeIsSystemErrorFalseBeforeStart();
            int RowsAffected = 0;
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DVLD.DB"].ConnectionString))
            {

                using (SqlCommand command = new SqlCommand("SP_UpdateUser", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@UserID", UserID);
                     command.Parameters.AddWithValue("@UserName", UserName);
                     command.Parameters.AddWithValue("@Password", Password);
                     command.Parameters.AddWithValue("@IsActive", Convert.ToBoolean(IsActive));
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

        public static bool SavingNewPassword(int UserID, string Password)
        {
            DataAccessSettings.MakeIsSystemErrorFalseBeforeStart();
            int RowsAffected = 0;
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DVLD.DB"].ConnectionString))
            {

                using (SqlCommand command = new SqlCommand("SP_SavingNewPassword", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@UserID", UserID);
                    command.Parameters.AddWithValue("@Password", Password);
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
    }
}
