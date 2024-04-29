using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Collections.ObjectModel;

namespace OnlineMarketPlace
{
    public class SqlDatabaseHelper
    {

        private readonly string connectionString;

        public SqlDatabaseHelper(string connectionString)
        {
            this.connectionString = connectionString;
        }
        public SqlDatabaseHelper()
        {
            // Server=myServerAddress;Database=myDataBase;User Id=myUsername;Password=myPassword;
            this.connectionString = @"Server = myServerAddress;
                                      Database = vip2023;
                                      User Id = BelovAA;
                                      Password = myPassword;";
            // fill with proper string
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

        public ObservableCollection<PickUpPoint> PullAllPUPs()
        {
            ObservableCollection<PickUpPoint> pickupPoints = new ObservableCollection<PickUpPoint>();

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

            string query = @"
        SELECT ID, СТРАНА, ГОРОД, УЛИЦА, НОМЕР_УЛИЦЫ, НОМЕР_КОМНАТЫ, ПОЧТОВЫЙ_ИНДЕКС, РЕЙТИНГ 
        FROM Пункты_выдачи 
        WHERE ID = @PUP_ID";

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@PUP_ID", PUP_ID);

                try
                {
                    connection.Open();
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
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
            }

            return pickupPoint;
        }

        public ObservableCollection<Product> PullProductsByEmployeePUPs(int employeeId)
        {
            ObservableCollection<Product> products = new ObservableCollection<Product>();

            string query = @"
        SELECT T.ID, T.НАЗВАНИЕ, T.ОПИСАНИЕ, T.ЦЕНА, T.РЕЙТИНГ, TPV.FK_ПУНКТА_ВЫДАЧИ AS PupId, TPZ.КОЛИЧЕСТВО
        FROM Товары T
        INNER JOIN Товары_по_пунктам_выдачи TPV ON T.ID = TPV.FK_ТОВАРА
        INNER JOIN Заказы_по_пунктам_выдачи ZPV ON TPV.FK_ПУНКТА_ВЫДАЧИ = ZPV.FK_ПУНКТА_ВЫДАЧИ
        INNER JOIN Товары_по_заказам TPZ ON T.ID = TPZ.FK_ТОВАРА
        WHERE ZPV.FK_СОТРУДНИКА = @EmployeeId";

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@EmployeeId", employeeId);

                try
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Product product = new Product
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("ID")),
                                Name = reader.GetString(reader.GetOrdinal("НАЗВАНИЕ")),
                                Description = reader.GetString(reader.GetOrdinal("ОПИСАНИЕ")),
                                Price = reader.GetDecimal(reader.GetOrdinal("ЦЕНА")),
                                Rating = reader.GetDecimal(reader.GetOrdinal("РЕЙТИНГ")),
                                PupId = reader.GetInt32(reader.GetOrdinal("PupId")),
                                Amount = reader.GetInt32(reader.GetOrdinal("КОЛИЧЕСТВО"))
                            };
                            products.Add(product);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
            }

            return products;
        }

        public Employee PullEmployeeDetails(int employeeId)
        {
            Employee employee = null;

            string query = @"
        SELECT 
            L.ID,
            L.ИМЯ,
            L.ФАМИЛИЯ,
            L.ОТЧЕСТВО,
            L.ДАТА_РОЖДЕНИЯ,
            L.ПОЛ,
            L.СЕРИЯ_НОМЕР_ПАСПОРТА,
            SPV.ЗАРПЛАТА,
            AVG(ZP.ОЦЕНКА) AS РЕЙТИНГ
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

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@EmployeeId", employeeId);

                try
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            employee = new Employee
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("ID")),
                                Name = reader.GetString(reader.GetOrdinal("ИМЯ")),
                                Surname = reader.GetString(reader.GetOrdinal("ФАМИЛИЯ")),
                                FathersName = reader.GetString(reader.GetOrdinal("ОТЧЕСТВО")),
                                DateOfBirth = reader.GetDateTime(reader.GetOrdinal("ДАТА_РОЖДЕНИЯ")),
                                Gender = reader.GetString(reader.GetOrdinal("ПОЛ")),
                                PassportData = reader.GetString(reader.GetOrdinal("СЕРИЯ_НОМЕР_ПАСПОРТА")),
                                Wage = reader.GetInt32(reader.GetOrdinal("ЗАРПЛАТА")),
                                Rating = reader.IsDBNull(reader.GetOrdinal("РЕЙТИНГ")) ? 0 : reader.GetDecimal(reader.GetOrdinal("РЕЙТИНГ"))
                            };
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
            }

            return employee;
        }

        public bool AuthenticateUser(string username, string password, out bool isAdmin)
        {
            /*
             ALTER TABLE Сотрудники_по_пунктам_выдачи
                ADD Username NVARCHAR(50) NULL,
                    PasswordHash NVARCHAR(255) NULL,
                    IsAdmin BIT NULL;
             */
            isAdmin = false;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Construct the query with parameterized values
                string query = @"SELECT PasswordHash, IsAdmin
                         FROM Сотрудники_по_пунктам_выдачи
                         WHERE Username = @Username;";

                SqlCommand command = new SqlCommand(query, connection);

                // Add parameters to the query
                command.Parameters.AddWithValue("@Username", username);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        // Retrieve the stored password hash and isAdmin status
                        string storedPasswordHash = reader.GetString(0);
                        isAdmin = reader.GetBoolean(1);

                        // Verify the provided password against the stored hash
                        if (VerifyPassword(password, storedPasswordHash))
                        {
                            return true; // Authentication successful
                        }
                    }
                }
            }
            return false; // Authentication failed
        }

        private bool VerifyPassword(string password, string storedPasswordHash)
        {
            // Implement password hashing and verification logic here
            // Compare the provided password hash with the stored password hash
            // True if they match, False otherwise
            return password == storedPasswordHash;
        }

        public void SaveProductChanges(ObservableCollection<Product> productsToAdd,
                                       ObservableCollection<Product> productsToUpdate,
                                       ObservableCollection<Product> productsToDelete)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Handle products to add
                foreach (var product in productsToAdd)
                {
                    string insertQuery = "INSERT INTO Products (Name, Description, Price, Rating, Amount) " +
                                         "VALUES (@Name, @Description, @Price, @Rating, @Amount)";
                    SqlCommand insertCommand = new SqlCommand(insertQuery, connection);
                    insertCommand.Parameters.AddWithValue("@Name", product.Name);
                    insertCommand.Parameters.AddWithValue("@Description", product.Description);
                    insertCommand.Parameters.AddWithValue("@Price", product.Price);
                    insertCommand.Parameters.AddWithValue("@Rating", product.Rating);
                    insertCommand.Parameters.AddWithValue("@Amount", product.Amount);
                    insertCommand.ExecuteNonQuery();
                }

                // Handle products to update
                foreach (var product in productsToUpdate)
                {
                    string updateQuery = "UPDATE Products SET Name = @Name, Description = @Description, " +
                                         "Price = @Price, Rating = @Rating, Amount = @Amount " +
                                         "WHERE Id = @ProductId";
                    SqlCommand updateCommand = new SqlCommand(updateQuery, connection);
                    updateCommand.Parameters.AddWithValue("@Name", product.Name);
                    updateCommand.Parameters.AddWithValue("@Description", product.Description);
                    updateCommand.Parameters.AddWithValue("@Price", product.Price);
                    updateCommand.Parameters.AddWithValue("@Rating", product.Rating);
                    updateCommand.Parameters.AddWithValue("@Amount", product.Amount);
                    updateCommand.Parameters.AddWithValue("@ProductId", product.Id);
                    updateCommand.ExecuteNonQuery();
                }

                // Handle products to delete
                foreach (var product in productsToDelete)
                {
                    string deleteQuery = "DELETE FROM Products WHERE Id = @ProductId";
                    SqlCommand deleteCommand = new SqlCommand(deleteQuery, connection);
                    deleteCommand.Parameters.AddWithValue("@ProductId", product.Id);
                    deleteCommand.ExecuteNonQuery();
                }
            }
        }


        /*
        public void SaveProductChanges(
            ObservableCollection<Product> productsToAdd,
            ObservableCollection<Product> productsToDelete)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                foreach (var product in productsToAdd)
                {
                    string insertQuery = "INSERT INTO Products (Name, Description, Price, Rating, Amount) " +
                                         "VALUES (@Name, @Description, @Price, @Rating, @Amount)";
                    SqlCommand insertCommand = new SqlCommand(insertQuery, connection);
                    insertCommand.Parameters.AddWithValue("@Name", product.Name);
                    insertCommand.Parameters.AddWithValue("@Description", product.Description);
                    insertCommand.Parameters.AddWithValue("@Price", product.Price);
                    insertCommand.Parameters.AddWithValue("@Rating", product.Rating);
                    insertCommand.Parameters.AddWithValue("@Amount", product.Amount);
                    insertCommand.ExecuteNonQuery();
                }

                foreach (var product in productsToDelete)
                {
                    string deleteQuery = "DELETE FROM Products WHERE Id = @ProductId";
                    SqlCommand deleteCommand = new SqlCommand(deleteQuery, connection);
                    deleteCommand.Parameters.AddWithValue("@ProductId", product.Id);
                    deleteCommand.ExecuteNonQuery();
                }
            }
        }
        */

        public ObservableCollection<Employee> PullAllEmployees()
        {
            ObservableCollection<Employee> employees = new ObservableCollection<Employee>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT * FROM Employees;";
                SqlCommand command = new SqlCommand(query, connection);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Employee employee = new Employee
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("ID")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                            Surname = reader.GetString(reader.GetOrdinal("Surname")),
                            FathersName = reader.GetString(reader.GetOrdinal("FathersName")),
                            DateOfBirth = reader.GetDateTime(reader.GetOrdinal("DateOfBirth")),
                            Wage = reader.GetDecimal(reader.GetOrdinal("Wage")),
                            Rating = reader.GetDecimal(reader.GetOrdinal("Rating"))
                        };

                        employees.Add(employee);
                    }
                }
            }

            return employees;
        }

        public ObservableCollection<Employee> PullEmployeesByPUP(int PUP_ID)
        {
            ObservableCollection<Employee> employees = new ObservableCollection<Employee>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT * FROM Employees WHERE PUP_ID = @PUP_ID;";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@PUP_ID", PUP_ID);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Employee employee = new Employee
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("ID")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                            Surname = reader.GetString(reader.GetOrdinal("Surname")),
                            FathersName = reader.GetString(reader.GetOrdinal("FathersName")),
                            DateOfBirth = reader.GetDateTime(reader.GetOrdinal("DateOfBirth")),
                            Wage = reader.GetDecimal(reader.GetOrdinal("Wage")),
                            Rating = reader.GetDecimal(reader.GetOrdinal("Rating"))
                        };

                        employees.Add(employee);
                    }
                }
            }

            return employees;
        }

        // update for updated employees
        public void SaveEmployeeChanges(ObservableCollection<Employee> employeesToAdd, ObservableCollection<Employee> employeesToDelete)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                foreach (var employee in employeesToAdd)
                {
                    string insertQuery = "INSERT INTO Employees (Name, Surname, FathersName, DateOfBirth, Wage, Rating) " +
                                         "VALUES (@Name, @Surname, @FathersName, @DateOfBirth, @Wage, @Rating)";
                    SqlCommand insertCommand = new SqlCommand(insertQuery, connection);
                    insertCommand.Parameters.AddWithValue("@Name", employee.Name);
                    insertCommand.Parameters.AddWithValue("@Surname", employee.Surname);
                    insertCommand.Parameters.AddWithValue("@FathersName", employee.FathersName);
                    insertCommand.Parameters.AddWithValue("@DateOfBirth", employee.DateOfBirth);
                    insertCommand.Parameters.AddWithValue("@Wage", employee.Wage);
                    insertCommand.Parameters.AddWithValue("@Rating", employee.Rating);
                    insertCommand.ExecuteNonQuery();
                }

                foreach (var employee in employeesToDelete)
                {
                    string deleteQuery = "DELETE FROM Employees WHERE Id = @EmployeeId";
                    SqlCommand deleteCommand = new SqlCommand(deleteQuery, connection);
                    deleteCommand.Parameters.AddWithValue("@EmployeeId", employee.Id);
                    deleteCommand.ExecuteNonQuery();
                }
            }
        }

        public void UpdateProductAmount(int productId, int newAmount)
        {
            // This method allows non-admin users to only change the amount of a product in the database

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string updateQuery = "UPDATE Products SET Amount = @NewAmount WHERE Id = @ProductId";
                SqlCommand updateCommand = new SqlCommand(updateQuery, connection);
                updateCommand.Parameters.AddWithValue("@NewAmount", newAmount);
                updateCommand.Parameters.AddWithValue("@ProductId", productId);
                updateCommand.ExecuteNonQuery();
            }
        }

        public ObservableCollection<Order> GetAllOrdersForAllPickUpPoints()
        {
            ObservableCollection<Order> orders = new ObservableCollection<Order>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = @"SELECT 
                                    O.ID AS OrderId,
                                    O.ЦЕНА AS Price,
                                    O.ДАТА_СОЗДАНИЯ AS CreationDate,
                                    O.FK_ЗАКАЗЧИКА AS CustomerId,
                                    L.ИМЯ + ' ' + L.ФАМИЛИЯ AS CustomerName,
                                    L.КОНТАКТНЫЙ_ТЕЛЕФОН AS CustomerContactInfo,
                                    T.ID AS ProductId,
                                    T.НАЗВАНИЕ AS ProductName,
                                    T.ОПИСАНИЕ AS ProductDescription,
                                    T.ЦЕНА AS ProductPrice,
                                    T.РЕЙТИНГ AS ProductRating,
                                    T.КОЛИЧЕСТВО AS ProductAmount,
                                    P.ID AS PickUpPointId,
                                    P.СТРАНА AS Country,
                                    P.ГОРОД AS City,
                                    P.УЛИЦА AS Street,
                                    P.НОМЕР_УЛИЦЫ AS StreetNumber,
                                    P.ПОЧТОВЫЙ_ИНДЕКС AS PostalCode
                                FROM 
                                    Заказы O
                                INNER JOIN 
                                    Люди L ON O.FK_ЗАКАЗЧИКА = L.ID
                                INNER JOIN 
                                    Товары_по_заказам TP ON O.ID = TP.FK_ЗАКАЗА
                                INNER JOIN 
                                    Товары T ON TP.FK_ТОВАРА = T.ID
                                INNER JOIN 
                                    Заказы_по_пунктам_выдачи ZP ON O.ID = ZP.FK_ЗАКАЗА
                                INNER JOIN 
                                    Пункты_выдачи P ON ZP.FK_ПУНКТА_ВЫДАЧИ = P.ID";

                SqlCommand command = new SqlCommand(query, connection);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        int orderId = Convert.ToInt32(reader["OrderId"]);

                        Order existingOrder = orders.FirstOrDefault(o => o.Id == orderId);

                        if (existingOrder == null)
                        {
                            existingOrder = new Order
                            {
                                Id = orderId,
                                Price = Convert.ToDecimal(reader["Price"]),
                                CreationDate = Convert.ToDateTime(reader["CreationDate"]),
                                CustomerId = Convert.ToInt32(reader["CustomerId"]),
                                CustomerName = reader["CustomerName"].ToString(),
                                CustomerContactInfo = reader["CustomerContactInfo"].ToString(),
                                OrderItems = new List<Product>(),
                                PickUpPoints = new List<PickUpPoint>()
                            };

                            orders.Add(existingOrder);
                        }

                        Product product = new Product
                        {
                            Id = Convert.ToInt32(reader["ProductId"]),
                            Name = reader["ProductName"].ToString(),
                            Description = reader["ProductDescription"].ToString(),
                            Price = Convert.ToDecimal(reader["ProductPrice"]),
                            Rating = Convert.ToDecimal(reader["ProductRating"]),
                            Amount = Convert.ToInt32(reader["ProductAmount"])
                        };

                        existingOrder.OrderItems.Add(product);

                        PickUpPoint pickUpPoint = new PickUpPoint
                        {
                            Id = Convert.ToInt32(reader["PickUpPointId"]),
                            Country = reader["Country"].ToString(),
                            City = reader["City"].ToString(),
                            Street = reader["Street"].ToString(),
                            StreetNumber = reader["StreetNumber"].ToString(),
                            PostIndex = reader["PostalCode"].ToString()
                        };

                        existingOrder.PickUpPoints.Add(pickUpPoint);
                    }

                    reader.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
            }

            return orders;
        }

        public ObservableCollection<Order> PullPUPOrders(int employeeId)
        {
            ObservableCollection<Order> orders = new ObservableCollection<Order>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = @"SELECT Orders.ID, Orders.Price, Orders.CreationDate, Orders.FK_ЗАКАЗЧИКА, 
                                        Люди.ИМЯ, Люди.ФАМИЛИЯ, Люди.КОНТАКТНЫЙ_ТЕЛЕФОН
                                 FROM Orders
                                 JOIN Люди ON Orders.FK_ЗАКАЗЧИКА = Люди.ID
                                 JOIN Заказы_по_пунктам_выдачи ON Orders.ID = Заказы_по_пунктам_выдачи.FK_ЗАКАЗА
                                 JOIN Сотрудники_по_пунктам_выдачи ON Заказы_по_пунктам_выдачи.FK_СОТРУДНИКА = Сотрудники_по_пунктам_выдачи.ID
                                 WHERE Сотрудники_по_пунктам_выдачи.FK_РАБОТНИКА = @EmployeeId";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@EmployeeId", employeeId);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        Order order = new Order
                        {
                            Id = reader.GetInt32(0),
                            Price = reader.GetDecimal(1),
                            CreationDate = reader.GetDateTime(2),
                            CustomerId = reader.GetInt32(3),
                            CustomerName = reader.GetString(4) + " " + reader.GetString(5),
                            CustomerContactInfo = reader.GetString(6)
                        };

                        orders.Add(order);
                    }
                }
                catch (Exception ex)
                {
                    // Handle exceptions
                    Console.WriteLine("Error: " + ex.Message);
                }
            }

            return orders;
        }
    }
}