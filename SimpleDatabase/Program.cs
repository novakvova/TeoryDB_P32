﻿using System.Data.SqlClient;
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
                Console.WriteLine("6.Додати n-користувачів");
                Console.WriteLine("7.Показати кількість користувачів");
                Console.WriteLine("8.Читання користувачів");
                Console.WriteLine("9.Заповнити БД даними");
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
                        ds.CreateTabels();
                        break;
                    case 3:
                        Console.WriteLine("---------Очистити таблиці з БД------");
                        ds.DeleteAllTables();
                        break;
                    case 4:
                        ds.CreateCategory();
                        break;
                    case 5:
                        {
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

                    case 6:
                        {
                            Console.Write("Вкажіть кількість: ");
                            int count = int.Parse(Console.ReadLine());
                            Stopwatch stopWatch = new Stopwatch();
                            stopWatch.Start();
                            ds.InsertRandomSpeed(count);
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

                    case 7:
                        {
                            Stopwatch stopWatch = new Stopwatch();
                            stopWatch.Start();
                            int count = ds.GetCountUsers();
                            Console.WriteLine($"Кількість користувачів у БД: {count}");
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

                    case 8:
                        {
                            //Stopwatch stopWatch = new Stopwatch();
                            //stopWatch.Start();
                            var users = ds.GetAllUsers();
                            foreach(var user in users)
                            {
                                Console.WriteLine(user);
                            }
                            
                            //stopWatch.Stop();
                            //// Get the elapsed time as a TimeSpan value.
                            //TimeSpan ts = stopWatch.Elapsed;

                            //// Format and display the TimeSpan value.
                            //string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                            //    ts.Hours, ts.Minutes, ts.Seconds,
                            //    ts.Milliseconds / 10);
                            //Console.WriteLine("RunTime " + elapsedTime);
                            break;
                        }

                    case 9:
                        Console.WriteLine("---------Очистити таблиці з БД------");
                        ds.InsertAllTables();
                        break;
                }
            } while (operation != 0);
        }

        
    }
}
