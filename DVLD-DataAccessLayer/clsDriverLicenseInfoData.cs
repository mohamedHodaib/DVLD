using System;
using System.Data.SqlClient;
using System.Configuration;

namespace DVLD_DataAccessLayer
{
    public class clsDriverLicenseInfoData
    {
        public static bool FindLicenseInfo(int LicenseID, ref int DriverID, ref DateTime IssueDate, ref DateTime ExpirationDate, ref string Notes, ref bool IsActive, ref byte IssueReason, ref int LicenseClass)
        {
            bool isFound = false;

            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DVLD.DB"].ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("FindLicenseInfo", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@LicenseID", LicenseID);

                    try
                    {
                        connection.Open();
                        SqlDataReader reader = command.ExecuteReader();

                        if (reader.Read())
                        {
                            isFound = true;
                            DriverID = (int)reader["DriverID"];
                            LicenseClass = (int)reader["LicenseClass"];
                            IssueDate = (DateTime)reader["IssueDate"];
                            ExpirationDate = (DateTime)reader["ExpirationDate"];
                            Notes = reader["Notes"] == DBNull.Value ? "" : (string)reader["Notes"];
                            IsActive = (bool)reader["IsActive"];
                            IssueReason = (byte)reader["IssueReason"];
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

        public static bool GetActiveDriverLicenseWithClass3Info(int LicenseID, ref int DriverID, ref DateTime IssueDate, ref DateTime ExpirationDate, ref string Notes, ref bool IsActive, ref byte IssueReason, ref int LicenseClass, ref bool IsDetained)
        {
            DataAccessSettings.MakeIsSystemErrorFalseBeforeStart();
            bool IsFound = false;

            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DVLD.DB"].ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("GetActiveDriverLicenseWithClass3Info", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@LicenseID", LicenseID);

                    try
                    {
                        connection.Open();
                        SqlDataReader Reader = command.ExecuteReader();

                        if (Reader.Read())
                        {
                            IsFound = true;
                            DriverID = Convert.ToInt32(Reader[0]);
                            IssueDate = Convert.ToDateTime(Reader[1]);
                            ExpirationDate = Convert.ToDateTime(Reader[2]);
                            Notes = Convert.ToString(Reader[3]);
                            IsActive = Convert.ToBoolean(Reader[4]);
                            IssueReason = (byte)Reader[5];
                            LicenseClass = Convert.ToInt32(Reader[6]);
                            IsDetained = Convert.ToBoolean(Reader[7]);
                        }

                        Reader.Close();
                    }
                    catch (Exception ex)
                    {
                        DataAccessSettings.IsSystemError = true;
                    }
                }
            }

            return IsFound;
        }

        public static int GetActiveLicenseIDPersonID(int PersonID, int LicenseClassID)
        {
            DataAccessSettings.MakeIsSystemErrorFalseBeforeStart();
            int LicenseID = -1;

            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DVLD.DB"].ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("GetActiveLicenseIDPersonID", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@PersonID", PersonID);
                    command.Parameters.AddWithValue("@LicenseClassID", LicenseClassID);

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
    }
}