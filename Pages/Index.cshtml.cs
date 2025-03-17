using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Data;
using System.Data.SqlClient;//это в nuget

namespace курсовая.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        private string ConectDb(string tableName)
        {
            Console.WriteLine(tableName);
            string connectionString = "Server=DESKTOP-5VUOBQO,1433;Database=база данных кп2;Trusted_connection=yes;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    Console.WriteLine("Подключение успешно установлено.");

                    // Запрос для получения всех таблиц в базе данных
                    Console.WriteLine();
                    string query = $"SELECT * FROM {tableName}";
                    Console.WriteLine(query);
                    string result = "";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            Console.WriteLine("результат запроса:" + reader);
                            for (int i = 0; reader.Read(); i++)
                            {
                                for (int j = 0; j < reader.FieldCount; j++)
                                {
                                    result += reader[j] + "; ";
                                }
                                result += "| ";

                            }
                            return result;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Ошибка: " + ex.Message);
                }
                finally
                {
                    connection.Close();
                    Console.WriteLine("Подключение закрыто.");
                }
            }
            return "no data found";

        }


        public string Id { get; set; } // Свойство для хранения значения id

        public IActionResult OnGet()
        {
            // Получаем значение параметра "id" из GET-запроса
            Id = ConectDb(Request.Query["dataTables"]);

            // Логика обработки значения id
            if (!string.IsNullOrEmpty(Id))
            {
                _logger.LogInformation($"data: {Id}"); // Логируем значение
            }

            return Page(); // Возвращаем страницу
        }
    }
}

