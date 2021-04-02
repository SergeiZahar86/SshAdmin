using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;

namespace IncubeAdmin
{
    class Global
    {
        private static Global instance;
        private User User;
        private string connectionString;
        private string sqlExpression;

        public static Global getInstance() // возвращает singleton объекта Global
        {
            if (instance == null)
                instance = new Global();
            return instance;
        }

        private Global()
        {
            var appSettings = ConfigurationManager.AppSettings;
            connectionString = appSettings["connectionString"];

            sqlExpression = "SELECT * FROM Users";
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                /*SqliteCommand command = new SqliteCommand(sqlExpression, connection);
                using (SqliteDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows) // если есть данные
                    {
                        while (reader.Read())   // построчно считываем данные
                        {
                            var id = reader.GetValue(0);
                            var name = reader.GetValue(1);
                            var age = reader.GetValue(2);

                            Console.WriteLine($"{id} \t {name} \t {age}");
                        }
                    }
                }*/


            }

        }

    }
}
