using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public string host;
        public string login;
        public string password;
        public SftpClient sftp;
        public SshClient sshClient;
        public bool isConnect;
        public bool isProgressBar;    // видимость прогресс-бара

        public List<Node> nodes;
        

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
            //sshClient = new SshClient();
            UsersGlobal = new List<User>();

            getUsers();

        }

        public void getUsers()
        {
            var appSettings = ConfigurationManager.AppSettings;
            //connectionString = "Data Source = MySqlite.db;Cache=Shared;Mode=ReadWrite;";
            //connectionString = appSettings["connectionString"];
            List<string> ImportedFiles = new List<string>();
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

        /*private void UpdateTextWrong() // Получение начальных сведений от виртуальной машины
        {

            this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
            {
                //left_ssh_text.Text += "Вставить новый текст \n";

                try
                {


                    if (global.sshClient.IsConnected == true)
                    {
                        // имя узла
                        using (var command = global.sshClient.CreateCommand("nodetool status | awk '/^(U|D)(N|L|J|M)/{print $8}'"))
                        {
                            string fff = command.Execute();
                            string[] words = fff.Split(new char[] { '\n' });
                            for (int i = 0; i < words.Length - 1; i++)
                            {
                                //left_ssh_text.Text += (words[i] + "\n");
                                name_node.Add(words[i]);
                            }
                            //radius_Elipse(words);
                            *//*foreach (string s in words)
                            {
                                ssh_text.Text += (s + "\n");
                            }*//*
                            //Console.Write(command.Execute());
                            //Console.ReadLine();
                        }
                        // доступность узла
                        using (var command = global.sshClient.CreateCommand("nodetool status | awk '/^(U|D)(N|L|J|M)/{print $1}'"))
                        {
                            string fff = command.Execute();
                            string[] words = fff.Split(new char[] { '\n' });
                            for (int i = 0; i < words.Length - 1; i++)
                            {
                                //left_ssh_text.Text += (words[i] + "\n");
                                isOk_node.Add(words[i]);
                            }
                            //Console.Write(command.Execute());
                            //Console.ReadLine();
                        }
                        // ip адрес узла
                        using (var command = global.sshClient.CreateCommand("nodetool status | awk '/^(U|D)(N|L|J|M)/{print $2}'"))
                        {
                            string fff = command.Execute();
                            string[] words = fff.Split(new char[] { '\n' });
                            for (int i = 0; i < words.Length - 1; i++)
                            {
                                //left_ssh_text.Text += (words[i] + "\n");
                                ip_node.Add(words[i]);
                            }
                            //Console.Write(command.Execute());
                            //Console.ReadLine();
                        }

                        for (int i = 0; i < name_node.Count; i++)
                        {
                            global.nodes.Add(new Node(name_node[i], ip_node[i], isOk_node[i]));
                        }

                        radius_Elipse(global.nodes);

                        // добавление элементов в стекпанель
                        for (int i = 0; i < global.nodes.Count; i++)
                        {
                            StackPanel stack = new StackPanel();
                            stack.Orientation = Orientation.Horizontal;

                            Border bord = new Border();
                            bord.Width = 14;
                            bord.Height = 14;
                            bord.Margin = new Thickness(5, 5, 15, 5);   // первый круг
                            bord.CornerRadius = new CornerRadius(15);
                            //bord.BorderBrush = Brushes.Orange;
                            if (global.nodes[i].Status == "DN")
                            {
                                bord.BorderBrush = new SolidColorBrush(Color.FromRgb(r_Yellow, g_Yellow, b_Yellow));
                                bord.Background = new SolidColorBrush(Color.FromRgb(r_Yellow, g_Yellow, b_Yellow));
                            }
                            else
                            {
                                bord.BorderBrush = new SolidColorBrush(Color.FromRgb(r_Green, g_Green, b_Green));
                                bord.Background = new SolidColorBrush(Color.FromRgb(r_Green, g_Green, b_Green));
                            }
                            bord.BorderThickness = new Thickness(0);
                            bord.Focusable = true;
                            //bord.Tag = count[i]; // для поиска метки по клику правой кнопки мыши


                            TextBlock tBlock = new TextBlock();
                            tBlock.FontSize = 14;
                            tBlock.Inlines.Add(new Span(new Run(global.nodes[i].Name + "   " + global.nodes[i].Ip + "   " + global.nodes[i].Status)));
                            //tBlock.Foreground = ;
                            tBlock.TextAlignment = TextAlignment.Center;
                            tBlock.VerticalAlignment = VerticalAlignment.Center;
                            tBlock.HorizontalAlignment = HorizontalAlignment.Center;
                            tBlock.Padding = new Thickness(0, 0, 0, 1);



                            stack.Children.Add(bord);
                            stack.Children.Add(tBlock);
                            third_stack_right.Children.Add(stack);
                        }


                    }
                    //wait.Close();
                }
                catch (Exception dddd)
                {
                    left_ssh_text.Text += (dddd.ToString() + " \n");
                }




                progressBar.Visibility = Visibility.Hidden;
            }
            );








            *//*using (var command = global.sshClient.CreateCommand("nodetool status | awk '/^(U|D)(N|L|J|M)/{print $8}'"))
            {
                string fff = command.Execute();
                string[] words = fff.Split(new char[] { '\n' });
                for (int i = 0; i < words.Length - 1; i++)
                {
                    //left_ssh_text.Text += (words[i] + "\n");
                    name_node.Add(words[i]);
                }
                //radius_Elipse(words);
                *//*foreach (string s in words)
                {
                    ssh_text.Text += (s + "\n");
                }*//*
                //Console.Write(command.Execute());
                //Console.ReadLine();
            }
            // Эмулирует некоторую работу посредством пятисекундной задержки
            while (true)
            {
                Thread.Sleep(TimeSpan.FromSeconds(1));

                // Получить диспетчер от текущего окна и использовать его для вызова кода обновления
                this.Dispatcher.BeginInvoke(DispatcherPriority.Normal,
                    (ThreadStart)delegate ()
                    {
                        left_ssh_text.Text += "Вставить новый текст \n";
                    }
                );
            }
        }*/


        /*private void Worker(object state)
        {
            for (var i = 0; i < 100; i++)
            {
                var t = i.ToString();
                this.Dispatcher.Invoke(() => { left_ssh_text.Text += (t + "  "); });
                Thread.Sleep(1000);
            }*//*
    }*/


    }
}
