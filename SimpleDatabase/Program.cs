using System.Data.SqlClient;
using System.Diagnostics;
using System.Text;
using SimpleDatabase.Services;

namespace SimpleDatabase
{
    internal class Program
    {

        //insert into tblProducts(CategoryId, Name, Description, Price, CreatedDate) 
        //Values(1, N'Молоко', N'Дуже смачне', 110, '2025-02-11 16:40:25');
        //

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
                Console.WriteLine("5.Додати n-категорій");
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
                    case 5:
                        Console.Write("Вкажіть кількість: ");
                        int count = int.Parse(Console.ReadLine());
                        Stopwatch stopWatch = new Stopwatch();
                        stopWatch.Start();
                        ds.InsertRandomCategories(count);
                        stopWatch.Stop();
                        // Get the elapsed time as a TimeSpan value.
                        TimeSpan ts = stopWatch.Elapsed;

                        // Format and display the TimeSpan value.
                        string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                            ts.Hours, ts.Minutes, ts.Seconds,
                            ts.Milliseconds / 10);
                        Console.WriteLine("RunTime " + elapsedTime);
                        break;
                }
            } while (operation != 0);
        }

        
    }
}
