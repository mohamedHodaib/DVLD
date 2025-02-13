using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Threading.Tasks;
using static DVLD_DataAccessLayer.clsPersonData;

namespace DVLD_DataAccessLayer
{
    public class clsPersonData
    {
        public struct stPersonInfo
        {
            public int ID {  get; set; }
            public string NationalNo { get; set; }
            public string FName { get; set; }
            public string SName { get; set; }
            public string TName { get; set; }
            public string LName { get; set; }
            public string Phone { get; set; }
            public string Email { get; set; }
            public string Address { get; set; }
            public short Gender { get; set; }
            public DateTime DateOfBirth { get; set; }
            public string ImagePath { get; set; }
            public int CountryID { get; set; }
        }

        public static stPersonInfo PersonInfo;
        public static async Task<bool> FindByPersonID(int ID)
        {
            DataAccessSettings.MakeIsSystemErrorFalseBeforeStart();
            bool IsFound = false;

            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DVLD.DB"].ConnectionString))
            using (SqlCommand command = new SqlCommand("SP_GetPersonByID", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@ID", ID);
                try
                {
                    await connection.OpenAsync();
                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            IsFound = true;
                            PersonInfo.NationalNo = reader["NationalNo"] as string;
                            PersonInfo.FName = reader["FirstName"] as string;
                            PersonInfo.SName = reader["SecondName"] as string;
                            PersonInfo.TName = reader["ThirdName"] as string ?? "";
                            PersonInfo.LName = reader["LastName"] as string;
                            PersonInfo.Phone = reader["Phone"] as string;
                            PersonInfo.Email = reader["Email"] as string ?? "";
                            PersonInfo.Address = reader["Address"] as string;
                            PersonInfo.Gender = Convert.ToInt16(reader["Gendor"]);
                            PersonInfo.DateOfBirth = (DateTime)reader["DateOfBirth"];
                            PersonInfo.CountryID = (int)reader["NationalityCountryID"];
                            PersonInfo.ImagePath = reader["ImagePath"] as string ?? "";
                        }
                    }
                }
                catch
                {
                    DataAccessSettings.IsSystemError = true;
                }
            }
            return IsFound;
        }

        public static async Task<bool> FindByNationalNo(string NationalNo)
        {
            DataAccessSettings.MakeIsSystemErrorFalseBeforeStart();
            bool IsFound = false;

            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DVLD.DB"].ConnectionString))
            using (SqlCommand command = new SqlCommand("SP_GetPersonByNationalNo", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@NationalNo", NationalNo);
                try
                {
                    await connection.OpenAsync();
                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            IsFound = true;
                            PersonInfo.ID = Convert.ToInt32(reader["PersonID"]);
                            PersonInfo.NationalNo = reader["NationalNo"] as string;
                            PersonInfo.FName = reader["FirstName"] as string;
                            PersonInfo.SName = reader["SecondName"] as string;
                            PersonInfo.TName = reader["ThirdName"] as string ?? "";
                            PersonInfo.LName = reader["LastName"] as string;
                            PersonInfo.Phone = reader["Phone"] as string;
                            PersonInfo.Email = reader["Email"] as string ?? "";
                            PersonInfo.Address = reader["Address"] as string;
                            PersonInfo.Gender = Convert.ToInt16(reader["Gendor"]);
                            PersonInfo.DateOfBirth = (DateTime)reader["DateOfBirth"];
                            PersonInfo.CountryID = (int)reader["NationalityCountryID"];
                            PersonInfo.ImagePath = reader["ImagePath"] as string ?? "";
                        }
                    }
                }
                catch
                {
                    DataAccessSettings.IsSystemError = true;
                }
            }
            return IsFound;
        }

        public static DataTable FilterByPhone(string Phone)
        {
            DataAccessSettings.MakeIsSystemErrorFalseBeforeStart();
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DVLD.DB"].ConnectionString))
            using (SqlCommand cmd = new SqlCommand("SP_FilterByPhone", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Phone", Phone + "%");
                try
                {
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows) dt.Load(reader);
                    }
                }
                catch
                {
                    DataAccessSettings.IsSystemError = true;
                }
            }
            return dt;
        }

        public static DataTable FilterByNationalNumber(string NationalNo)
        {
            DataAccessSettings.MakeIsSystemErrorFalseBeforeStart();
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DVLD.DB"].ConnectionString))
            using (SqlCommand cmd = new SqlCommand("SP_FilterByNationalNo", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@NationalNo", NationalNo + "%");
                try
                {
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows) dt.Load(reader);
                    }
                }
                catch
                {
                    DataAccessSettings.IsSystemError = true;
                }
            }
            return dt;
        }
        public static DataTable ListPeople()
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DVLD.DB"].ConnectionString))
            using (SqlCommand cmd = new SqlCommand("SP_ListPeople", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                try
                {
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows) dt.Load(reader);
                    }
                }
                catch
                {
                    DataAccessSettings.IsSystemError = true;
                }
            }
            return dt;
        }

        public static bool DeletePerson(int ID)
        {
            DataAccessSettings.MakeIsSystemErrorFalseBeforeStart();
            int AffectedRows = 0;
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DVLD.DB"].ConnectionString))
            using (SqlCommand cmd = new SqlCommand("SP_DeletePerson", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PersonID", ID);
                try
                {
                    conn.Open();
                    AffectedRows = cmd.ExecuteNonQuery();
                }
                catch (SqlException ex) when (ex.Number == 547)
                {
                    DataAccessSettings.IsForeignKeyConstraintException = true;
                }
                catch
                {
                    DataAccessSettings.IsSystemError = true;
                }
            }
            return AffectedRows > 0;
        }

        static public int AddNewPersonandGetID(string NationalNo, string FName, string SName, string TName,
         string LName, DateTime DateOfBirth, short Gender, string Address
          , string Phone, string Email, int CountryID, string ImagePath)
        {
            DataAccessSettings.MakeIsSystemErrorFalseBeforeStart();
            int PersonID = -1;
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DVLD.DB"].ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("SP_Add_New_Person", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@NationalNo", NationalNo);
                    command.Parameters.AddWithValue("@FirstName", FName);
                    command.Parameters.AddWithValue("@SecondName", SName);
                    if (TName == "") command.Parameters.AddWithValue("@ThirdName", DBNull.Value);
                    else command.Parameters.AddWithValue("@ThirdName", TName);
                    command.Parameters.AddWithValue("@LastName", LName);
                    if (Email == "") command.Parameters.AddWithValue("@Email", DBNull.Value);
                    else command.Parameters.AddWithValue("@Email", Email);
                    command.Parameters.AddWithValue("@Phone", Phone);
                    command.Parameters.AddWithValue("@Address", Address);
                    command.Parameters.AddWithValue("@CountryID", CountryID);
                    command.Parameters.AddWithValue("@Gender", Gender);
                    command.Parameters.AddWithValue("@DateOfBirth", DateOfBirth);
                    if (ImagePath == "") command.Parameters.AddWithValue("@ImagePath", DBNull.Value);
                    else command.Parameters.AddWithValue("@ImagePath", ImagePath);

                    SqlParameter outputIdParam = new SqlParameter("@NewPersonID", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };

                    command.Parameters.Add(outputIdParam);

                    try
                    {
                        connection.Open();
                        command.ExecuteNonQuery();
                        //return the first column of the first row of the 
                        //last result Set
                        if (outputIdParam.Value != DBNull.Value)
                        {
                            PersonID = Convert.ToInt32(outputIdParam.Value);
                        }
                    }
                    catch
                    {
                        return -1;
                    }
                }
            }
            return PersonID;
        }

        public static async Task<bool> UpdatePerson(stPersonInfo person)
        {
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DVLD.DB"].ConnectionString))
            using (SqlCommand command = new SqlCommand("SP_UpdatePerson", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@NationalNo", person.NationalNo);
                command.Parameters.AddWithValue("@FirstName", person.FName);
                command.Parameters.AddWithValue("@SecondName", person.SName);
                command.Parameters.AddWithValue("@ThirdName", person.TName);
                command.Parameters.AddWithValue("@LastName", person.LName);
                command.Parameters.AddWithValue("@Phone", person.Phone);
                command.Parameters.AddWithValue("@Email", person.Email);
                command.Parameters.AddWithValue("@Address", person.Address);
                command.Parameters.AddWithValue("@Gendor", person.Gender);
                command.Parameters.AddWithValue("@DateOfBirth", person.DateOfBirth);
                command.Parameters.AddWithValue("@ImagePath", person.ImagePath);
                command.Parameters.AddWithValue("@NationalityCountryID", person.CountryID);

                try
                {
                    await connection.OpenAsync();
                    int rowsAffected = await command.ExecuteNonQueryAsync();
                    return rowsAffected > 0;
                }
                catch
                {
                    DataAccessSettings.IsSystemError = true;
                    return false;
                }
            }
        }

        public static string GetPath(int ID)
        {
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DVLD.DB"].ConnectionString))
            using (SqlCommand command = new SqlCommand("SP_GetPersonImagePath", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@ID", ID);
                try
                {
                    connection.Open();
                    return command.ExecuteScalar()?.ToString();
                }
                catch
                {
                    DataAccessSettings.IsSystemError = true;
                    return string.Empty;
                }
            }
        }
    }
}
