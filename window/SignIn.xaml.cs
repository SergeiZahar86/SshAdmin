﻿using Microsoft.Data.Sqlite;
using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace IncubeAdmin.window
{
    public partial class SignIn : Window
    {
        private Global global;
        private string sqlExpression;
        private string secret;
        private DispatcherTimer dispatcherTimer;
        private bool error;

        private string name;
        private string ps;

        public SignIn()
        {
            InitializeComponent();
            secret = "kas;ldfu7392n.f(hjafl";
            grid_signIn.Visibility = Visibility.Visible;
            grid_signUp.Visibility = Visibility.Hidden;
            global = Global.getInstance();
            //host_string.Text = "10.90.0.29";
            //login_string.Text = "root";
            //pass_string.Password = "root26032021";
            global.host = "10.90.0.29";
            global.login = "root";
            global.password = "root26032021";



            CalculateMD5Hash("www");
            dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(OnTimedEvent);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 2);
            //dispatcherTimer.Start();
        }
        private void OnTimedEvent(Object source, EventArgs e)
        {
            grid_signIn.Visibility = Visibility.Visible;
            grid_signUp.Visibility = Visibility.Hidden;
            dispatcherTimer.Stop();
        }

        // Перемещение окна по экрану
        private void ColorZone_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            /*Application.Current.Shutdown(); // выход из программы
            Environment.Exit(0);*/
            this.Close();
        }
        /*private void myGif_MediaEnded(object sender, RoutedEventArgs e)
        {
            myGif.Position = new TimeSpan(0, 0, 1);
            myGif.Play();
        }*/

        private void MinButton_Click(object sender, RoutedEventArgs e) // сворачивание окна
        {
            this.WindowState = WindowState.Minimized;
        }

        private void signUp_Ok_Click(object sender, RoutedEventArgs e)
        {
            error = false;
            string log = login_string.Text;
            string pass = pass_string.Password;



            sqlExpression = $"SELECT * FROM Users where Name = '{log}'";
            using (var connection = new SqliteConnection(global.connectionString))
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
                            name = reader.GetString(0);
                            ps = reader.GetString(1);
                            //UsersGlobal.Add(new User { Id = f++, Name = name, Pass = pass });
                        }
                    }
                    else
                    {
                        error = true;
                        textError.FontSize = 20;
                        textError.Foreground = Brushes.Red;
                        textError.Text = "Пользователя с таким именем не существует";
                    }
                }
            }
            if (!error)
            {
                pass = CalculateMD5Hash(pass);
                if (pass == ps)
                {
                    Close();
                }
                else
                {
                    error = true;
                    textError.FontSize = 20;
                    textError.Foreground = Brushes.Red;
                    textError.Text = "Пароль введён неверно";
                }
            }

            name = "";
            ps = "";

            if (!error)
            {
                /*try
                {
                    global.sshClient = new SshClient(global.host, global.login, global.password);
                    global.sshClient.Connect();
                    if (global.sshClient.IsConnected == true)
                    {
                        global.isConnect = true;
                        this.Close();
                    }
                    else
                    {
                        textError.Text = "Соединение не установлено";
                    }
                }
                catch (Exception ee)
                {
                    textError.Text = $"Ошибка соединения. \n {ee}";
                }*/
                Close();
            }
        }

        private void signIn_cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            Application.Current.Shutdown(); // выход из программы
            Environment.Exit(0);
        }


        public string CalculateMD5Hash(string input)    // хэш-функция
        {
            // step 1, calculate MD5 hash from input
            input = input + secret;
            MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
            byte[] hash = md5.ComputeHash(inputBytes);
            // step 2, convert byte array to hex string
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }
            return sb.ToString();
        }

        private void signUp_Click(object sender, RoutedEventArgs e)  // зарегистрироваться
        {
            grid_signIn.Visibility = Visibility.Hidden;
            grid_signUp.Visibility = Visibility.Visible;
        }

        private void signUp_cancel_Click_1(object sender, RoutedEventArgs e)
        {
            grid_signIn.Visibility = Visibility.Visible;
            grid_signUp.Visibility = Visibility.Hidden;
        }

        public void setUsers(string lg, string pass)  // добавление пользователя в базу
        {
            var appSettings = ConfigurationManager.AppSettings;
            List<string> ImportedFiles = new List<string>();
            sqlExpression = $"INSERT INTO Users (Name, Pass) VALUES ('{lg}', '{pass}')";
            using (var connection = new SqliteConnection(global.connectionString))
            {
                connection.Open();
                SqliteCommand command = new SqliteCommand(sqlExpression, connection);
                try
                {
                    int number = command.ExecuteNonQuery();
                }
                catch 
                {
                    error = true;
                }
            }
        }

        private void signUp_Ok_Click_1(object sender, RoutedEventArgs e)
        {
            error = false;
            //string password = pass_string1.Password + secret;
            string str = CalculateMD5Hash(pass_string1.Password);
            setUsers(login_string1.Text, str);
            if (error)
            {
                textError1.FontSize = 20;
                textError1.Foreground = Brushes.Red;
                textError1.Text = "Пользователь с таким именем уже существует";
            }
            else
            {
                textError1.FontSize = 25;
                textError1.Foreground = Brushes.White;
                textError1.Text = "Вы успешно зарегистрировались";
                dispatcherTimer.Start();
            }
        }
    }
}