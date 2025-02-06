using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bogus;

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

        public void CreateTabelCategories()
        {
            string sql = File.ReadAllText(@"sql\tabels\tbl_categories.sql");
            try
            {
                var command = _conn.CreateCommand();
                command.CommandText = sql;
                int rows = command.ExecuteNonQuery(); //запит, який не вертає послідовність
                Console.WriteLine("Таблицю створено успішно");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Щось пішло не так {0}", ex.Message);
            }
        }

        public void DeleteAllTables()
        {
            string sql = File.ReadAllText(@"sql\dropAllTabels.sql");
            try
            {
                var command = _conn.CreateCommand();
                command.CommandText = sql;
                int rows = command.ExecuteNonQuery(); //запит, який не вертає послідовність
                Console.WriteLine("Taблиці успішно видалено");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Щось пішло не так {0}", ex.Message);
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
                lNameParam.Value=lName;
                command.Parameters.Add(lNameParam);

                command.Parameters.Add(new SqlParameter("@fName", fName));

                int rows = command.ExecuteNonQuery(); //запит, який не вертає послідовність
                Console.WriteLine("Запис додано "+rows);
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

            string [] names = faker.Commerce.Categories(count);
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

        public void Close()
        {
            _conn.Close();
        }
    }
}
