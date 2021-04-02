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
        public SignUp()
        {
            InitializeComponent();
        }


        // Перемещение окна по экрану
        private void ColorZone_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown(); // выход из программы
            Environment.Exit(0);
        }
        private void ExpandButton_Click(object sender, RoutedEventArgs e) // сворачивание окна
        {
            if (WindowState == WindowState.Normal)
            {
                this.WindowState = WindowState.Maximized;
                this.Padding = new Thickness(100);
                this.Margin = new Thickness(100);
                HorizontalAlignment = HorizontalAlignment.Center;

            }
            else
            {
                WindowState = WindowState.Normal;
            }
        }

        private void MinButton_Click(object sender, RoutedEventArgs e) // сворачивание окна
        {
            this.WindowState = WindowState.Minimized;
        }


    }
}
