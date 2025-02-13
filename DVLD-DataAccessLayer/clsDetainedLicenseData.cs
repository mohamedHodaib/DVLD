using System;
using System.Collections.Generic;
using System.Collections.Specialized;
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
    public class clsDetainedLicenseData
    {
        public static int AddNewDetainedLicenseAndGetID(int LicenseID, DateTime DetainDate, double FineFees, int CreatedByUserID)
        {
            DataAccessSettings.MakeIsSystemErrorFalseBeforeStart();
            int DetainID = -1;
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DVLD.DB"].ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("sp_AddNewDetainedLicense", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@DetainDate", DetainDate);
                    command.Parameters.AddWithValue("@FineFees", FineFees);
                    command.Parameters.AddWithValue("@UserID", CreatedByUserID);
                    SqlParameter output = new SqlParameter("@LicenseID", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };

                    command.Parameters.Add(output);

                    try
                    {
                        connection.Open();
                        command.ExecuteNonQuery();
                        if (output.Value != DBNull.Value)
                        {
                            DetainID = Convert.ToInt32(output.Value);
                        }
                    }
                    catch (Exception ex)
                    {
                        DataAccessSettings.IsSystemError = true;
                    }
                }
            }
            return DetainID;
        }

        public static DataTable FilterByApplicationID(int ApplicationID)
        {
            return ExecuteFilterProcedure("sp_FilterByApplicationID", "@AppID", ApplicationID);
        }

        public static DataTable FilterByDetainID(int DetainID)
        {
            return ExecuteFilterProcedure("sp_FilterByDetainID", "@DetainID", DetainID);
        }

        public static DataTable FilterByNationalNumber(string NationalNumber)
        {
            return ExecuteFilterProcedure("sp_FilterByNationalNumber", "@NationalNo", NationalNumber + "%");
        }

        public static DataTable FilterByFullName(string FullName)
        {
            return ExecuteFilterProcedure("sp_FilterByFullName", "@FullName", FullName + "%");
        }

        public static DataTable FilterByIsReleased(string IsReleased)
        {
            int isReleasedValue = (IsReleased == "Yes") ? 1 : 0;
            return ExecuteFilterProcedure("sp_FilterByIsReleased", "@IsReleased", isReleasedValue);
        }

        public static DataTable ListDetainedLicenses()
        {
            return ExecuteFilterProcedure("sp_ListDetainedLicenses", null, null);
        }

        public static bool GetDetainedLicense(int LicenseID, ref int DetainID, ref int CreatedByUserID, ref DateTime DetainDate, ref double FineFees)
        {
            DataAccessSettings.MakeIsSystemErrorFalseBeforeStart();
            bool IsFound = false;
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DVLD.DB"].ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("sp_GetDetainedLicense", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@LicenseID", LicenseID);

                    try
                    {
                        connection.Open();
                        using (SqlDataReader Reader = command.ExecuteReader())
                        {
                            if (Reader.Read())
                            {
                                IsFound = true;
                                DetainID = (int)Reader["DetainID"];
                                DetainDate = (DateTime)Reader["DetainDate"];
                                FineFees = Convert.ToDouble(Reader["FineFees"]);
                                CreatedByUserID = (int)Reader["CreatedByUserID"];
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        DataAccessSettings.IsSystemError = true;
                    }
                }
            }
            return IsFound;
        }

        public static bool Release(int DetainID, int ReleaseByUserID, int AppID)
        {
            DataAccessSettings.MakeIsSystemErrorFalseBeforeStart();
            int AffectedRows = ExecuteNonQueryProcedure("sp_ReleaseDetainedLicense", (cmd) =>
            {
                cmd.Parameters.AddWithValue("@DetainID", DetainID);
                cmd.Parameters.AddWithValue("@UserID", ReleaseByUserID);
                cmd.Parameters.AddWithValue("@AppID", AppID);
            });
            return AffectedRows > 0;
        }

        public static bool IsLicenseDetained(int LicenseID)
        {
            bool IsDetained = false;
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DVLD.DB"].ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("sp_IsLicenseDetained", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@LicenseID", LicenseID);

                    SqlParameter output = new SqlParameter("@ResultID", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.ReturnValue
                    };

                    command.Parameters.Add(output);

                    try
                    {
                        connection.Open();
                        command.ExecuteNonQuery();
                        if(output.Value != DBNull.Value)
                        {
                            IsDetained = Convert.ToBoolean(output.Value);
                        }
                    }
                    catch (Exception ex)
                    {
                        DataAccessSettings.IsSystemError = true;
                    }
                }
            }
            return IsDetained;
        }

        private static DataTable ExecuteFilterProcedure(string procedureName, string paramName, object paramValue)
        {
            DataAccessSettings.MakeIsSystemErrorFalseBeforeStart();
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DVLD.DB"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(procedureName, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    if (paramName != null)
                    {
                        cmd.Parameters.AddWithValue(paramName, paramValue);
                    }
                    try
                    {
                        conn.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.HasRows) dt.Load(reader);
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

        private static int ExecuteNonQueryProcedure(string procedureName, Action<SqlCommand> addParams)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DVLD.DB"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(procedureName, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    addParams(cmd);
                    try
                    {
                        conn.Open();
                        return cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        DataAccessSettings.IsSystemError = true;
                        return 0;
                    }
                }
            }
        }
    }
}
