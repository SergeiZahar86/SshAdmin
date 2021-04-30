using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using IncubeAdmin.main;
using Microsoft.Data.Sqlite;
using Renci.SshNet;


namespace IncubeAdmin
{
    class Global
    {
        private static Global instance;
        private User User;
        public string connectionString;
        private string sqlExpression;
        public List<User> UsersGlobal;                             // список пользователей из базы данных
        public List<SystemCassandra> systemCassandras;
        public List<AllDisk> allDisks;                             // список всех дисков на конкретном
        public string NameClasterCassandra;                        // название кластера кассандры
        public List<Casmon> casmons;                               // список узлов кассандры
        public List<Sysmon> sysmons;                               // список Sysmon
        public List<SshError> sshErrors;                           // список ошибок

        public string host;                                        // ip вводимое при запуске программы
        public string login;                                       // login вводимый при запуске программы
        public string password;                                    // password вводимый при запуске программы
        //public SftpClient sftp;
        public List<SshClient> sshClients;                         // список соединений по ssh к компьютерам
        public bool isConnect;                                     // проверка соединения по ssh для отображения вверху окна
        public bool isProgressBar;                                 // видимость прогресс-бара

        public List<Node> nodes;                                   // список узлов Cassandra
        public List<Host> hosts;                                   // список хостов
        public byte[] array;

/*        private string passPhrase = "MyTestPassphrase";        //Может быть любой строкой
        private string saltValue = "MyTestSaltValue";        // Может быть любой строкой
        private string hashAlgorithm = "SHA256";             // может быть "MD5"
        private int passwordIterations = 3;                //Может быть любым числом
        private string initVector = "!1A3g2D4s9K556g7"; // Должно быть 16 байт
        private int keySize = 256;                // Может быть 192 или 128
*/

        public static Global getInstance() // возвращает singleton объекта Global
        {
            if (instance == null)
                instance = new Global();
            return instance;
        }

        private Global()
        {
            sshErrors = new List<SshError>();
            casmons = new List<Casmon>();
            sysmons = new List<Sysmon>();
            sshClients = new List<SshClient>();
            connectionString = "Data Source = ../../MySqlite.db;Cache=Shared;Mode=ReadWrite;";
            nodes = new List<Node>();
            //hosts = new List<Host>();
            //sshClient = new SshClient();
            UsersGlobal = new List<User>();
            systemCassandras = new List<SystemCassandra>();
            allDisks = new List<AllDisk>(); 

            getUsersSQLite();
            getSystemsCassandra();
            getAllDisk();
        }

        


        public void getHosts(string nameUser) // выборка всех компьютеров по имени пользователя
        {
            sqlExpression = $"SELECT  Hosts.host AS host, Hosts.login as login , Hosts.pass as pass FROM Users " +
                $"INNER JOIN UsersHosts ON Users.id = UsersHosts.user " +
                $"LEFT JOIN Hosts ON UsersHosts.host = Hosts.id WHERE Users.name = '{nameUser}'";
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
                            string ip = reader.GetString(0);
                            string login = reader.GetString(1);
                            string pass = reader.GetString(2);
                            hosts.Add(new Host(ip, login, pass));
                        }
                    }
                }
            }
        }

        public void getUsersSQLite()
        {
            //var appSettings = ConfigurationManager.AppSettings;
            //connectionString = "Data Source = MySqlite.db;Cache=Shared;Mode=ReadWrite;";
            //connectionString = appSettings["connectionString"];
            //List<string> ImportedFiles = new List<string>();
            sqlExpression = "SELECT * FROM Users";
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                SqliteCommand command = new SqliteCommand(sqlExpression, connection);
                using (SqliteDataReader reader = command.ExecuteReader())
                {
                    int f = 1;
                    if (reader.HasRows) // если есть данные
                    {
                        while (reader.Read())   // построчно считываем данные
                        {
                            string name = reader.GetString(0);
                            string pass = reader.GetString(1);
                            UsersGlobal.Add(new User { Id = f++, Name = name, Pass = pass });
                        }
                    }
                }
            }
        }

        public void getUsers()
        {

        }
        private void getSystemsCassandra()
        {
            for(int i = 1; i < 4; i++)
            {
                systemCassandras.Add(new SystemCassandra(i, "server", $"10.90.90.{i}",
                    "16380", "10000", "54%", "500", "300", "DB", $"10.90.90.{i}", "UN"));
            }
        }
        private void getAllDisk()
        {
            double ss = 11000;
            for(int i = 1; i <6; i++)
            {
                allDisks.Add(new AllDisk($"10.90.90.{i}",$"sssss{i}", $"{ss}", "10000"));
                ss += 1000;
            }
        }
    }
}
