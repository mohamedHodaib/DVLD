using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace DVLD_DataAccessLayer
{
    public class clsDriverInternationalLicenseInfoData
    {
        public static bool FindLicenseInfo(int ILID, ref int LLID, ref int DriverID, ref int ApplciationID,
                ref DateTime IssueDate, ref DateTime ExpirationDate, ref bool IsActive)
        {
            DataAccessSettings.MakeIsSystemErrorFalseBeforeStart();
            bool IsFound = false;
            SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DVLD.DB"].ConnectionString);

            // Use the stored procedure name instead of the SQL query
            string storedProcedureName = "spFindLicenseInfoByID";
            SqlCommand command = new SqlCommand(storedProcedureName, connection);
            command.CommandType = System.Data.CommandType.StoredProcedure;

            // Add parameters for the stored procedure
            command.Parameters.AddWithValue("@InternationalLicenseID", ILID);
            command.Parameters.Add(new SqlParameter("@ApplicationID", System.Data.SqlDbType.Int) { Direction = System.Data.ParameterDirection.Output });
            command.Parameters.Add(new SqlParameter("@DriverID", System.Data.SqlDbType.Int) { Direction = System.Data.ParameterDirection.Output });
            command.Parameters.Add(new SqlParameter("@LocalLicenseID", System.Data.SqlDbType.Int) { Direction = System.Data.ParameterDirection.Output });
            command.Parameters.Add(new SqlParameter("@IssueDate", System.Data.SqlDbType.DateTime) { Direction = System.Data.ParameterDirection.Output });
            command.Parameters.Add(new SqlParameter("@ExpirationDate", System.Data.SqlDbType.DateTime) { Direction = System.Data.ParameterDirection.Output });
            command.Parameters.Add(new SqlParameter("@IsActive", System.Data.SqlDbType.Bit) { Direction = System.Data.ParameterDirection.Output });

            try
            {
                connection.Open();
                command.ExecuteNonQuery(); // Execute the stored procedure

                // Retrieve output parameters
                ApplciationID = (int)command.Parameters["@ApplicationID"].Value;
                DriverID = (int)command.Parameters["@DriverID"].Value;
                LLID = (int)command.Parameters["@LocalLicenseID"].Value;
                IssueDate = (DateTime)command.Parameters["@IssueDate"].Value;
                ExpirationDate = (DateTime)command.Parameters["@ExpirationDate"].Value;
                IsActive = (bool)command.Parameters["@IsActive"].Value;

                IsFound = true; // If no exception, the record was found
            }
            catch (Exception ex)
            {
                // Handle exceptions
                DataAccessSettings.IsSystemError = true;
            }
            finally
            {
                connection.Close(); // Ensure the connection is closed
            }
            return IsFound;
        }
    }
}