using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Renci.SshNet;

namespace IncubeAdmin
{
    class Global
    {
        private static Global instance;
        private User User;
        private string connectionString;
        private string sqlExpression;
        public List<User> UsersGlobal; // список пользователей из базы данных

        public string host;
        public string login;
        public string password;
        public SftpClient sftp;
        public SshClient sshClient;
        public bool isConnect;
        public bool isProgressBar;    // видимость прогресс-бара

        public static Global getInstance() // возвращает singleton объекта Global
        {
            if (instance == null)
                instance = new Global();
            return instance;
        }

        private Global()
        {


            //sshClient = new SshClient();
            UsersGlobal = new List<User>();
            var appSettings = ConfigurationManager.AppSettings;
            connectionString = appSettings["connectionString"];
            List<string> ImportedFiles = new List<string>();
            sqlExpression = "SELECT * FROM Users";


            /*using (SQLiteConnection connect = new SQLiteConnection("Data Source = MySqlite.db;Cache=Shared;Mode=ReadOnly;"))
            {
                connect.Open();
                using (SQLiteCommand fmd = connect.CreateCommand())
                {
                    fmd.CommandText = sqlExpression;
                    fmd.CommandType = CommandType.Text;
                    SQLiteDataReader r = fmd.ExecuteReader();
                    while (r.Read())
                    {
                        var id = r.GetValue(0);
                        var name = r.GetValue(1);
                        var age = r.GetValue(2);

                        //ImportedFiles.Add(Convert.ToString(r["FileName"]));
                    }
                }
            }*/
            //return ImportedFiles;


            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                SqliteCommand command = new SqliteCommand(sqlExpression, connection);






                using (SqliteDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows) // если есть данные
                    {
                        while (reader.Read())   // построчно считываем данные
                        {
                            int id = reader.GetInt32(0);
                            string name = reader.GetString(1);
                            string pass = reader.GetString(2);
                            UsersGlobal.Add(new User { Id = id, Name = name, Pass = pass });
                        }
                    }
                }


            }

        }

    }
}
