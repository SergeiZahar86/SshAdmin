using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.Linq;
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

namespace IncubeAdmin.window
{
    /// <summary>
    /// Логика взаимодействия для SignUp.xaml
    /// </summary>
    public partial class SignUp : Window
    {
        private Global global;
        public SignUp()
        {
            InitializeComponent();
            global = Global.getInstance();
            host_string.Text = "10.90.0.29";
            login_string.Text = "admin";
            pass_string.Password = "admin26032021";
            global.host = "10.90.0.29";
            global.login = "admin";
            global.password = "admin26032021";
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
        

        private void MinButton_Click(object sender, RoutedEventArgs e) // сворачивание окна
        {
            this.WindowState = WindowState.Minimized;
        }

        private void signUp_Ok_Click(object sender, RoutedEventArgs e)
        {
            /*global.host = host_string.Text;
            global.login = login_string.Text;
            global.password = pass_string.Password;*/
            try
            {
                global.sftp = new SftpClient(global.host, global.login, global.password);
                global.sftp.Connect();
                if (global.sftp.IsConnected == true)
                {
                    global.isConnect = true;
                    //this.Close();

                }
                else
                {
                    textError.Text = "Соединение не установлено";
                }


                global.sshClient = new SshClient(global.host, global.login, global.password);
                global.sshClient.Connect();
                if(global.sshClient.IsConnected == true)
                {
                    //global.isConnect = true;
                    this.Close();
                }


            }
            catch(Exception ee)
            {
                textError.Text = $"Ошибка соединения. \n {ee}";
            }
        }

        private void signUp_cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
