using System.Data.SqlClient;
using System.Text;

namespace SimpleDatabase
{
    internal class Program
    {
        private static string conn_str = "Data Source=.;Integrated Security=True;Initial Catalog=p32;";
        //private static string conn_str = "Data Source=vova_lutsk.mssql.somee.com;User=neyojef748_SQLLogin_1;Password=hk8wyz31xm;Initial Catalog=vova_lutsk";
        static void Main(string[] args)
        {
            Console.InputEncoding = Encoding.UTF8;
            Console.OutputEncoding = Encoding.UTF8;

            int operation = 0;
            do
            {
                Console.WriteLine("Оберіть операцію:");
                Console.WriteLine("0.Вихід");
                Console.WriteLine("1.Показати список таблиць в БД");
                Console.WriteLine("2.Створити в БД таблиці");
                Console.WriteLine("3.Видалити усі таблиці в БД");
                Console.Write("->_");
                operation = int.Parse(Console.ReadLine());
                switch(operation)
                {
                    case 1:
                        Console.WriteLine("---------Список таблиць у БД------");
                        ReadAllTabels(); 
                        break;
                    case 2:
                        Console.WriteLine("---------Створення таблиці в БД------");
                        CreateTabelCategories();
                        break;
                    case 3:
                        Console.WriteLine("---------Очистити таблиці з БД------");
                        DeleteAllTables();
                        break;
                }
            } while (operation != 0);
        }

        private static void ReadAllTabels()
        {
            string sql = "SELECT * FROM sys.tables";

            //Підключаємося до БД
            var _conn = new SqlConnection(conn_str);
            try
            {
                _conn.Open();
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
                //Є комданди, які вертають список, є команди, які не вератють список
                //Console.WriteLine("З'яднання з БД успішне :)");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Щось пішло не так {0}", ex.Message);
            }
        }

        private static void CreateTabelCategories()
        {
            string sql = File.ReadAllText(@"sql\tabels\tbl_categories.sql");
            //Підключаємося до БД
            var _conn = new SqlConnection(conn_str);
            try
            {
                _conn.Open();
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

        private static void DeleteAllTables()
        {
            string sql = File.ReadAllText(@"sql\dropAllTabels.sql");
            //Підключаємося до БД
            var _conn = new SqlConnection(conn_str);
            try
            {
                _conn.Open();
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
    }
}
