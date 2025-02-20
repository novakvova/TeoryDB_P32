using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bogus;
using SimpleDatabase.Models;

namespace SimpleDatabase.Services
{
    public class DatabaseService
    {
        private static string conn_str = "Data Source=.;Integrated Security=True;Initial Catalog=p32;";
        //private static string conn_str = "Data Source=vova_lutsk.mssql.somee.com;User=neyojef748_SQLLogin_1;Password=hk8wyz31xm;Initial Catalog=vova_lutsk";

        private readonly SqlConnection _conn;

        public DatabaseService()
        {
            _conn = new SqlConnection(conn_str);
            try
            {
                _conn.Open();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Щось пішло не так {0}", ex.Message);
            }
        }

        public void ReadAllTabels()
        {
            string sql = "SELECT * FROM sys.tables";

            try
            {
                var command = _conn.CreateCommand();
                command.CommandText = sql;
                using (var reader = command.ExecuteReader()) //Створюємо обєкт для читання даних
                {
                    while (reader.Read()) //читаємо кожен рядок
                    {
                        string id = reader["object_id"].ToString();
                        string name = reader["name"].ToString();
                        Console.WriteLine($"Id: {id}\tName: {name}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Щось пішло не так {0}", ex.Message);
            }
        }

        public void CreateTabels()
        {
            string[] tabels = { "tbl_categories", "tbl_products", "tbl_users", 
                "tbl_order_statuses", "tbl_orders", "tbl_order_items" };
            foreach (string tabel in tabels)
            {
                string sql = File.ReadAllText(@$"sql\tabels\{tabel}.sql");
                try
                {
                    var command = _conn.CreateCommand();
                    command.CommandText = sql;
                    int rows = command.ExecuteNonQuery(); //запит, який не вертає послідовність
                    Console.WriteLine($"Таблицю створено успішно {tabel}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Щось пішло не так {0}", ex.Message);
                }
            }
        }

        public void DeleteAllTables()
        {
            var sqlLines = File.ReadAllLines(@"sql\dropAllTabels.sql");
            foreach (var line in sqlLines)
            {
                try
                {
                    var command = _conn.CreateCommand();
                    command.CommandText = line;
                    int rows = command.ExecuteNonQuery(); //запит, який не вертає послідовність
                    Console.WriteLine("Taблиці успішно видалено");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Щось пішло не так {0}", ex.Message);
                }
            }
        }

        public void CreateCategory()
        {
            string fName, lName;
            Console.Write("Вкажіть назву категорії: ");
            fName = Console.ReadLine();
            Console.Write("Вкажіть опис категорії: ");
            lName = Console.ReadLine();
            //такий запит підається sql інєкції наприклад ось - '); DROP TABLE tbl_categories;--
            //string sql = @$"
            //    INSERT INTO tbl_categories 
            //    (FName,LName)
            //    VALUES (N'{fName}', N'{lName}');
            //    ";
            string sql = @$"
                INSERT INTO tbl_categories 
                (FName,LName)
                VALUES (@fName, @lName);
                ";
            try
            {
                var command = _conn.CreateCommand();
                command.CommandText = sql;

                SqlParameter lNameParam = new SqlParameter("@lName", System.Data.SqlDbType.NVarChar);
                lNameParam.Value = lName;
                command.Parameters.Add(lNameParam);

                command.Parameters.Add(new SqlParameter("@fName", fName));

                int rows = command.ExecuteNonQuery(); //запит, який не вертає послідовність
                Console.WriteLine("Запис додано " + rows);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Щось пішло не так {0}", ex.Message);
            }
        }

        public void InsertRandomCategories(int count)
        {
            var faker = new Faker("en");

            string sql = @$"
                INSERT INTO tbl_categories 
                (FName,LName)
                VALUES (@fName, @lName);
                ";

            string[] names = faker.Commerce.Categories(count);
            for (int i = 0; i < count; i++)
            {
                var description = faker.Lorem.Text();
                try
                {
                    var command = _conn.CreateCommand();
                    command.CommandText = sql;

                    SqlParameter lNameParam = new SqlParameter("@lName", System.Data.SqlDbType.NVarChar)
                    {
                        Value = description
                    };
                    command.Parameters.Add(lNameParam);
                    command.Parameters.Add(new SqlParameter("@fName", names[i]));

                    int rows = command.ExecuteNonQuery(); //запит, який не вертає послідовність
                    //Console.WriteLine("Запис додано " + rows);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Щось пішло не так {0}", ex.Message);
                }
            }
        }

        #region ---------Робота з користувачами------------
        
        public void InsertRandomSpeed(int count)
        {
            List<User> users = GenerateUsers(count);
            BulkInsertUsers(users);
        }

        private List<User> GenerateUsers(int count)
        {
            var faker = new Faker<User>()
                .RuleFor(u => u.FirstName, f => f.Name.FirstName())
                .RuleFor(u => u.LastName, f => f.Name.LastName())
                .RuleFor(u => u.Email, f => f.Internet.Email())
                .RuleFor(u => u.PhoneNumber, f => f.Phone.PhoneNumber())
                .RuleFor(u => u.Password, f => f.Internet.Password());

            return faker.Generate(count);
        }

        private void BulkInsertUsers(List<User> users)
        {
            using (SqlBulkCopy bulkCopy = new SqlBulkCopy(_conn))
            {
                bulkCopy.DestinationTableName = "tbl_users";
                bulkCopy.ColumnMappings.Add("FirstName", "FirstName");
                bulkCopy.ColumnMappings.Add("LastName", "LastName");
                bulkCopy.ColumnMappings.Add("Email", "Email");
                bulkCopy.ColumnMappings.Add("PhoneNumber", "PhoneNumber");
                bulkCopy.ColumnMappings.Add("Password", "Password");

                DataTable table = new DataTable();
                table.Columns.Add("FirstName", typeof(string));
                table.Columns.Add("LastName", typeof(string));
                table.Columns.Add("Email", typeof(string));
                table.Columns.Add("PhoneNumber", typeof(string));
                table.Columns.Add("Password", typeof(string));

                foreach (var user in users)
                {
                    table.Rows.Add(user.FirstName, user.LastName, user.Email, user.PhoneNumber, user.Password);
                }

                bulkCopy.WriteToServer(table);
            }
        }

        /// <summary>
        /// Повертає кількість користувачів у БД
        /// </summary>
        /// <returns>Повертає число</returns>
        public int GetCountUsers()
        {
            string sql = "SELECT COUNT(Id) as count_users FROM tbl_users";

            try
            {
                var command = _conn.CreateCommand();
                command.CommandText = sql;
                using (var reader = command.ExecuteReader()) //Створюємо обєкт для читання даних
                {
                    if (reader.Read()) //читаємо кожен рядок
                    {
                        int count =int.Parse(reader["count_users"].ToString());
                        return count;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Щось пішло не так {0}", ex.Message);
            }
            return -1;
        }

        /// <summary>
        /// Читання усіх користувачів із БД
        /// </summary>
        /// <returns>Повертає список</returns>
        public List<User> GetAllUsers()
        {
            string sql = "SELECT * FROM tbl_users " +
                "ORDER BY Id " +
                "OFFSET 20 ROWS " +
                "FETCH NEXT 20 ROWS ONLY;";
            List <User> users = new List<User>();
            try
            {
                var command = _conn.CreateCommand();
                command.CommandText = sql;
                using (var reader = command.ExecuteReader()) //Створюємо обєкт для читання даних
                {
                    while (reader.Read()) //читаємо кожен рядок
                    {
                        var user = new User();
                        user.Id = reader["Id"].ToString();
                        user.FirstName = reader["FirstName"].ToString();
                        user.LastName = reader["LastName"].ToString();
                        user.Email = reader["Email"].ToString();
                        user.PhoneNumber = reader["PhoneNumber"].ToString();
                        user.Password = reader["Password"].ToString();
                        users.Add(user);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Щось пішло не так {0}", ex.Message);
            }
            return users;
        }

        #endregion
        public void Close()
        {
            _conn.Close();
        }
    }
}
