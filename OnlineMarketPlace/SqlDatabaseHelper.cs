using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace OnlineMarketPlace
{
    public class SqlDatabaseHelper
    {
        private readonly string connectionString;

        public SqlDatabaseHelper(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public DataTable ExecuteQuery(string query, SqlParameter[] parameters = null)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    if (parameters != null)
                    {
                        command.Parameters.AddRange(parameters);
                    }

                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);
                        return dataTable;
                    }
                }
            }
        }

        public int ExecuteNonQuery(string query, SqlParameter[] parameters = null)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    if (parameters != null)
                    {
                        command.Parameters.AddRange(parameters);
                    }

                    return command.ExecuteNonQuery();
                }
            }
        }

        public List<PickUpPoint> PullAllPUPs()
        {
            List<PickUpPoint> pickupPoints = new List<PickUpPoint>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT * FROM Пункты_выдачи;";
                SqlCommand command = new SqlCommand(query, connection);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        PickUpPoint pickupPoint = new PickUpPoint
                        {
                            /*
                            ID = reader.GetInt32(0),
                            Страна = reader.GetString(1),
                            Город = reader.GetString(2),
                            Улица = reader.GetString(3),
                            НомерУлицы = reader.GetString(4),
                            НомерКвартиры = reader.GetString(5),
                            НомерКомнаты = reader.GetString(6),
                            ПочтовыйИндекс = reader.GetString(7),
                            Рейтинг = reader.GetDecimal(8)
                            */
                            Id = reader.GetInt32(reader.GetOrdinal("ID")),
                            Country = reader.GetString(reader.GetOrdinal("СТРАНА")),
                            City = reader.GetString(reader.GetOrdinal("ГОРОД")),
                            Street = reader.GetString(reader.GetOrdinal("УЛИЦА")),
                            StreetNumber = reader.GetString(reader.GetOrdinal("НОМЕР_УЛИЦЫ")),
                            RoomNumber = reader.GetString(reader.GetOrdinal("НОМЕР_КОМНАТЫ")),
                            PostIndex = reader.GetString(reader.GetOrdinal("ПОЧТОВЫЙ_ИНДЕКС")),
                            Rating = reader.GetDecimal(reader.GetOrdinal("РЕЙТИНГ"))
                    };
                        pickupPoints.Add(pickupPoint);
                    }
                }
            }

            return pickupPoints;
        }

        public PickUpPoint PullPUPByID(int PUP_ID)
        {
            PickUpPoint pickupPoint = null;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT * FROM Пункты_выдачи WHERE ID = @PUP_ID;";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@PUP_ID", PUP_ID);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        pickupPoint = new PickUpPoint
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("ID")),
                            Country = reader.GetString(reader.GetOrdinal("СТРАНА")),
                            City = reader.GetString(reader.GetOrdinal("ГОРОД")),
                            Street = reader.GetString(reader.GetOrdinal("УЛИЦА")),
                            StreetNumber = reader.GetString(reader.GetOrdinal("НОМЕР_УЛИЦЫ")),
                            RoomNumber = reader.GetString(reader.GetOrdinal("НОМЕР_КОМНАТЫ")),
                            PostIndex = reader.GetString(reader.GetOrdinal("ПОЧТОВЫЙ_ИНДЕКС")),
                            Rating = reader.GetDecimal(reader.GetOrdinal("РЕЙТИНГ"))
                        };
                    }
                }
            }

            return pickupPoint;
        }

        public Employee PullEmployeeDetails(int employeeId)
        {
            Employee employee = null;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = @"
            SELECT 
                L.ID,
                L.ИМЯ AS Name,
                L.ФАМИЛИЯ AS Surname,
                L.ОТЧЕСТВО AS FathersName,
                L.ДАТА_РОЖДЕНИЯ AS DateOfBirth,
                L.ПОЛ AS Gender,
                L.СЕРИЯ_НОМЕР_ПАСПОРТА AS PassportData,
                SPV.ЗАРПЛАТА AS Wage,
                AVG(ZP.ОЦЕНКА) AS Rating
            FROM 
                Люди L
            JOIN 
                Сотрудники_по_пунктам_выдачи SPV ON L.ID = SPV.FK_РАБОТНИКА
            LEFT JOIN 
                Заказы_по_пунктам_выдачи ZP ON SPV.ID = ZP.FK_СОТРУДНИКА
            WHERE 
                L.ID = @EmployeeId
            GROUP BY 
                L.ID, L.ИМЯ, L.ФАМИЛИЯ, L.ОТЧЕСТВО, L.ДАТА_РОЖДЕНИЯ, 
                L.ПОЛ, L.СЕРИЯ_НОМЕР_ПАСПОРТА, SPV.ЗАРПЛАТА;";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@EmployeeId", employeeId);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        employee = new Employee
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("ID")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                            Surname = reader.GetString(reader.GetOrdinal("Surname")),
                            FathersName = reader.GetString(reader.GetOrdinal("FathersName")),
                            DateOfBirth = reader.GetDateTime(reader.GetOrdinal("DateOfBirth")),
                            Gender = reader.GetString(reader.GetOrdinal("Gender")),
                            PassportData = reader.GetString(reader.GetOrdinal("PassportData")),
                            Wage = reader.GetInt32(reader.GetOrdinal("Wage")),
                            Rating = reader.IsDBNull(reader.GetOrdinal("Rating"))
                                ? 0 : reader.GetDecimal(reader.GetOrdinal("Rating"))
                        };
                    }
                }
            }

            return employee;
        }

        public bool AuthenticateUser(string username, string password)
        {
            // ideally, passwords are hashed and then we pass hash
            /*
             * ALTER TABLE Сотрудники_по_пунктам_выдачи
                ADD Username NVARCHAR(50) NULL,
                    PasswordHash NVARCHAR(255) NULL;

                ALTER TABLE Сотрудники_по_пунктам_выдачи
                ADD CONSTRAINT UQ_Username UNIQUE (Username);
            */
            string query = @"IF EXISTS( SELECT 1 FROM Сотрудники_по_пунктам_выдачи
                        WHERE Username = {0}
                        AND PasswordHash = {1})
                        SELECT 'Authentication Successful' AS Result;
                        ELSE
                        SELECT 'Authentication Failed' AS Result;";

            return true;

        }




    }
}