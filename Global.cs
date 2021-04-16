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
        public List<User> UsersGlobal; // список пользователей из базы данных

        public string login;
        public string password;
        public SftpClient sftp;
        public SshClient sshClient;
        public bool isConnect;
        public bool isProgressBar;    // видимость прогресс-бара

        public List<Node> nodes;      // список узлов Cassandra
        public List<Host> hosts;      // список хостов
        public string host;
        public byte[] array;

        private string passPhrase = "MyTestPassphrase";        //Может быть любой строкой
        private string saltValue = "MyTestSaltValue";        // Может быть любой строкой
        private string hashAlgorithm = "SHA256";             // может быть "MD5"
        private int passwordIterations = 3;                //Может быть любым числом
        private string initVector = "!1A3g2D4s9K556g7"; // Должно быть 16 байт
        private int keySize = 256;                // Может быть 192 или 128


        public static Global getInstance() // возвращает singleton объекта Global
        {
            if (instance == null)
                instance = new Global();
            return instance;
        }

        private Global()
        {
            connectionString = "Data Source = ../../MySqlite.db;Cache=Shared;Mode=ReadWrite;";
            nodes = new List<Node>();
            //hosts = new List<Host>();
            //sshClient = new SshClient();
            UsersGlobal = new List<User>();

            getUsers();

        }

        
        public string MyEncrypt(string str)   // шифрование
        {
            string cipherText = RijndaelAlgorithm.Encrypt
            (
                str,
                passPhrase,
                saltValue,
                hashAlgorithm,
                passwordIterations,
                initVector,
                keySize
            );
            return cipherText;
        }

        public string MyDecrypt(string str)    // дешифрование
        {
            string plainText = RijndaelAlgorithm.Decrypt
            (
                str,
                passPhrase,
                saltValue,
                hashAlgorithm,
                passwordIterations,
                initVector,
                keySize
            );
            return plainText;
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

        public void getUsers()
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



    }
}
