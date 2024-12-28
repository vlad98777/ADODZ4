using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADODZ4._1
{
    class Program
    {
          private static string connectionString = ConfigurationManager.ConnectionStrings["VegetablesAndFruitsConnection"].ConnectionString;

        static async Task Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("Выберите действие:");
                Console.WriteLine("1. Подключиться к базе данных");
                Console.WriteLine("2. Вывести всю информацию из таблицы");
                Console.WriteLine("3. Вывести все названия овощей и фруктов");
                Console.WriteLine("4. Вывести все цвета");
                Console.WriteLine("5. Показать максимальную калорийность");
                Console.WriteLine("6. Показать минимальную калорийность");
                Console.WriteLine("7. Показать среднюю калорийность");
                Console.WriteLine("8. Показать количество овощей");
                Console.WriteLine("9. Показать количество фруктов");
                Console.WriteLine("10. Показать количество овощей и фруктов заданного цвета");
                Console.WriteLine("11. Показать овощи и фрукты с калорийностью ниже указанной");
                Console.WriteLine("12. Показать овощи и фрукты с калорийностью выше указанной");
                Console.WriteLine("13. Показать овощи и фрукты с калорийностью в указанном диапазоне");
                Console.WriteLine("14. Показать все овощи и фрукты, у которых цвет желтый или красный");
                Console.WriteLine("0. Выйти");
                Console.Write("Введите номер действия: ");
                string input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        await ConnectToDatabaseAsync();
                        break;
                    case "2":
                        await ShowAllProductsAsync();
                        break;
                    case "3":
                        await ShowNamesAsync();
                        break;
                    case "4":
                        await ShowColorsAsync();
                        break;
                    case "5":
                        await ShowMaxCaloricContentAsync();
                        break;
                    case "6":
                        await ShowMinCaloricContentAsync();
                        break;
                    case "7":
                        await ShowAverageCaloricContentAsync();
                        break;
                    case "8":
                        await ShowCountOfVegetablesAsync();
                        break;
                    case "9":
                        await ShowCountOfFruitsAsync();
                        break;
                    case "10":
                        await ShowCountByColorAsync();
                        break;
                    case "11":
                        await ShowProductsBelowCaloricContentAsync();
                        break;
                    case "12":
                        await ShowProductsAboveCaloricContentAsync();
                        break;
                    case "13":
                        await ShowProductsInCaloricRangeAsync();
                        break;
                    case "14":
                        await ShowProductsByColorAsync();
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Неверный ввод. Попробуйте еще раз.");
                        break;
                }
            }
        }

        private static async Task ConnectToDatabaseAsync()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                Stopwatch stopwatch = Stopwatch.StartNew();
                try
                {
                    await connection.OpenAsync();
                    Console.WriteLine("Подключение успешно установлено.");
                }
                catch (SqlException ex)
                {
                    Console.WriteLine("Ошибка подключения: " + ex.Message);
                }
                finally
                {
                    if (connection.State == ConnectionState.Open)
                    {
                        connection.Close();
                        Console.WriteLine("Подключение закрыто.");
                    }
                    stopwatch.Stop();
                    Console.WriteLine($"Время подключения: {stopwatch.Elapsed.TotalSeconds} секунд");
                }
            }
        }

        private static async Task ShowAllProductsAsync()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();

                SqlCommand command = new SqlCommand("SELECT * FROM Products", connection);
                SqlDataReader reader = await command.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    Console.WriteLine($"ID: {reader["Id"]}, Название: {reader["Name"]}, Тип: {reader["Type"]}, Цвет: {reader["Color"]}, Калорийность: {reader["CaloricContent"]}");
                }
                await reader.CloseAsync();
            }
        }

        private static async Task ShowNamesAsync()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();

                SqlCommand command = new SqlCommand("SELECT Name FROM Products", connection);
                SqlDataReader reader = await command.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    Console.WriteLine($"Название: {reader["Name"]}");
                }
                await reader.CloseAsync();
            }
        }

        private static async Task ShowColorsAsync()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();

                SqlCommand command = new SqlCommand("SELECT DISTINCT Color FROM Products", connection);
                SqlDataReader reader = await command.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    Console.WriteLine($"Цвет: {reader["Color"]}");
                }
                await reader.CloseAsync();
            }
        }

        private static async Task ShowMaxCaloricContentAsync()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();

                SqlCommand command = new SqlCommand("SELECT MAX(CaloricContent) AS MaxCaloric FROM Products", connection);
                var result = await command.ExecuteScalarAsync();
                Console.WriteLine($"Максимальная калорийность: {result}");
            }
        }

        private static async Task ShowMinCaloricContentAsync()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();

                SqlCommand command = new SqlCommand("SELECT MIN(CaloricContent) AS MinCaloric FROM Products", connection);
                var result = await command.ExecuteScalarAsync();
                Console.WriteLine($"Минимальная калорийность: {result}");
            }
        }

        private static async Task ShowAverageCaloricContentAsync()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();

                SqlCommand command = new SqlCommand("SELECT AVG(CaloricContent) AS AvgCaloric FROM Products", connection);
                var result = await command.ExecuteScalarAsync();
                Console.WriteLine($"Средняя калорийность: {result}");
            }
        }

        private static async Task ShowCountOfVegetablesAsync()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();

                SqlCommand command = new SqlCommand("SELECT COUNT(*) FROM Products WHERE Type = 'овощ'", connection);
                var count = await command.ExecuteScalarAsync();
                Console.WriteLine($"Количество овощей: {count}");
            }
        }

        private static async Task ShowCountOfFruitsAsync()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();

                SqlCommand command = new SqlCommand("SELECT COUNT(*) FROM Products WHERE Type = 'фрукт'", connection);
                var count = await command.ExecuteScalarAsync();
                Console.WriteLine($"Количество фруктов: {count}");
            }
        }

        private static async Task ShowCountByColorAsync()
        {
            Console.Write("Введите цвет: ");
            string color = Console.ReadLine();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();

                SqlCommand command = new SqlCommand("SELECT COUNT(*) FROM Products WHERE Color = @Color", connection);
                command.Parameters.AddWithValue("@Color", color);
                var count = await command.ExecuteScalarAsync();
                Console.WriteLine($"Количество овощей и фруктов цвета '{color}': {count}");
            }
        }

        private static async Task ShowProductsBelowCaloricContentAsync()
        {
            Console.Write("Введите калорийность: ");
            int caloricContent = int.Parse(Console.ReadLine());

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();

                SqlCommand command = new SqlCommand("SELECT * FROM Products WHERE CaloricContent < @CaloricContent", connection);
                command.Parameters.AddWithValue("@CaloricContent", caloricContent);
                SqlDataReader reader = await command.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    Console.WriteLine($"Название: {reader["Name"]}, Калорийность: {reader["CaloricContent"]}");
                }
                await reader.CloseAsync();
            }
        }

        private static async Task ShowProductsAboveCaloricContentAsync()
        {
            Console.Write("Введите калорийность: ");
            int caloricContent = int.Parse(Console.ReadLine());

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();

                SqlCommand command = new SqlCommand("SELECT * FROM Products WHERE CaloricContent > @CaloricContent", connection);
                command.Parameters.AddWithValue("@CaloricContent", caloricContent);
                SqlDataReader reader = await command.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    Console.WriteLine($"Название: {reader["Name"]}, Калорийность: {reader["CaloricContent"]}");
                }
                await reader.CloseAsync();
            }
        }

        private static async Task ShowProductsInCaloricRangeAsync()
        {
            Console.Write("Введите минимальную калорийность: ");
            int minCaloric = int.Parse(Console.ReadLine());
            Console.Write("Введите максимальную калорийность: ");
            int maxCaloric = int.Parse(Console.ReadLine());

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();

                SqlCommand command = new SqlCommand("SELECT * FROM Products WHERE CaloricContent BETWEEN @MinCaloric AND @MaxCaloric", connection);
                command.Parameters.AddWithValue("@MinCaloric", minCaloric);
                command.Parameters.AddWithValue("@MaxCaloric", maxCaloric);
                SqlDataReader reader = await command.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    Console.WriteLine($"Название: {reader["Name"]}, Калорийность: {reader["CaloricContent"]}");
                }
                await reader.CloseAsync();
            }
        }

        private static async Task ShowProductsByColorAsync()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();

                SqlCommand command = new SqlCommand("SELECT * FROM Products WHERE Color IN ('желтый', 'красный')", connection);
                SqlDataReader reader = await command.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    Console.WriteLine($"Название: {reader["Name"]}, Цвет: {reader["Color"]}, Калорийность: {reader["CaloricContent"]}");
                }
                await reader.CloseAsync();
            }
        }
    }
}