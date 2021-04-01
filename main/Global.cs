using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IncubeAdmin
{
    class Global
    {
        private static Global instance;
        private User User;
        private string connectionString;

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


            using (var connection = new SQLiteConnection("Data Source=usersdata.db"))
            {
                connection.Open();
            }
            //Console.Read();

        }

    }
}
