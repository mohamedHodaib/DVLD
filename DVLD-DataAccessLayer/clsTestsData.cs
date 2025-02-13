using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DVLD_DataAccessLayer
{
    public class clsTestsData
    {
        public static int GetTrials(int LDLAppID, int TestTypeID)
        {
            int Trials = 0;
            DataAccessSettings.MakeIsSystemErrorFalseBeforeStart();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DVLD.DB"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("GetTrials", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@LDLAppID", LDLAppID);
                    cmd.Parameters.AddWithValue("@TestTypeID", TestTypeID);

                    try
                    {
                        conn.Open();
                        object ob = cmd.ExecuteScalar();
                        if (ob != null)
                        {
                            Trials = Convert.ToInt32(ob);
                        }
                    }
                    catch (Exception ex)
                    {
                        DataAccessSettings.IsSystemError = true;
                    }
                }
            }
            return Trials;
        }

        public static short GetPassedTests(int LDLAppID)
        {
            DataAccessSettings.MakeIsSystemErrorFalseBeforeStart();
            short PassedTests = 0;
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DVLD.DB"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("GetPassedTests", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@LDLAppID", LDLAppID);

                    try
                    {
                        conn.Open();
                        object ob = cmd.ExecuteScalar();
                        if (ob != null)
                        {
                            PassedTests = Convert.ToInt16(ob);
                        }
                    }
                    catch (Exception ex)
                    {
                        DataAccessSettings.IsSystemError = true;
                    }
                }
            }
            return PassedTests;
        }

        public static int AddNewTestandGetID(int TestAppointmentID, bool TestResult, string Notes, int UserID)
        {
            DataAccessSettings.MakeIsSystemErrorFalseBeforeStart();
            int TestID = -1;
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DVLD.DB"].ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("AddNewTestandGetID", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@TestAppointmentID", TestAppointmentID);
                    command.Parameters.AddWithValue("@TestResult", TestResult);
                    command.Parameters.AddWithValue("@Notes", string.IsNullOrEmpty(Notes) ? (object)DBNull.Value : Notes);
                    command.Parameters.AddWithValue("@UserID", UserID);
                    SqlParameter output = new SqlParameter("@TestID", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };
                    try
                    {
                        connection.Open();
                        command.ExecuteNonQuery();
                        if (output.Value != DBNull.Value)
                        {
                            TestID = Convert.ToInt32(output.Value);
                        }
                    }
                    catch (Exception ex)
                    {
                        DataAccessSettings.IsSystemError = true;
                    }
                }
            }
            return TestID;
        }

        public static bool FindTest(int TestAppointmentID, ref int TestID, ref bool TestResult, ref string Notes, ref int UserID)
        {
            DataAccessSettings.MakeIsSystemErrorFalseBeforeStart();
            bool IsFound = false;
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DVLD.DB"].ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("FindTest", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@TestAppointmentID", TestAppointmentID);

                    try
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                IsFound = true;
                                TestID = (int)reader["TestID"];
                                TestResult = Convert.ToBoolean(reader["TestResult"]);
                                UserID = (int)reader["UserID"];
                                Notes = reader["Notes"] != DBNull.Value ? (string)reader["Notes"] : "";
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
    }
}