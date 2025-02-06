using System.Data.SqlClient;
using System.Text;
using SimpleDatabase.Services;

namespace SimpleDatabase
{
    internal class Program
    {
       
        static void Main(string[] args)
        {
            Console.InputEncoding = Encoding.UTF8;
            Console.OutputEncoding = Encoding.UTF8;
            
            DatabaseService ds = new DatabaseService();

            int operation = 0;
            do
            {
                Console.WriteLine("Оберіть операцію:");
                Console.WriteLine("0.Вихід");
                Console.WriteLine("1.Показати список таблиць в БД");
                Console.WriteLine("2.Створити в БД таблиці");
                Console.WriteLine("3.Видалити усі таблиці в БД");
                Console.WriteLine("4.Додати категорію");
                Console.Write("->_");
                operation = int.Parse(Console.ReadLine());
                switch(operation)
                {
                    case 1:
                        Console.WriteLine("---------Список таблиць у БД------");
                        ds.ReadAllTabels(); 
                        break;
                    case 2:
                        Console.WriteLine("---------Створення таблиці в БД------");
                        ds.CreateTabelCategories();
                        break;
                    case 3:
                        Console.WriteLine("---------Очистити таблиці з БД------");
                        ds.DeleteAllTables();
                        break;
                    case 4:
                        ds.CreateCategory();
                        break;
                }
            } while (operation != 0);
        }

        
    }
}
