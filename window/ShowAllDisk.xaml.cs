using IncubeAdmin.main;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
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
    /// <summary>
    /// Логика взаимодействия для ShowAllDisk.xaml
    /// </summary>
   
    public partial class ShowAllDisk : Window
    {
        private Global global;
        byte r_Grey = 160;
        byte g_Grey = 178;
        byte b_Grey = 187;

        public ShowAllDisk()
        {
            InitializeComponent();
            global = Global.getInstance();

        }

        private void ColorZone_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)  // Перемещение окна по экрану
        {
            this.DragMove();
        }

        private void MinButton_Click(object sender, RoutedEventArgs e)    // сворачивание окна
        {
            this.WindowState = WindowState.Minimized;
        }


        private void cancel_Click(object sender, RoutedEventArgs e)   // Закрыть окно и выход из программы
        {
            this.Close();
            
        }
        public void getTooltip_allDisk(List<AllDisk> alls)
        {
            StackPanel stack = new StackPanel();
            stack.Orientation = Orientation.Horizontal;
            TextBlock text = new TextBlock();
            text.FontSize = 14;
            text.Inlines.Add(new Span(new Run(" ID     IP        DiskMemory")));
            //tBlock.Foreground = ;
            text.TextAlignment = TextAlignment.Center;
            text.VerticalAlignment = VerticalAlignment.Center;
            text.HorizontalAlignment = HorizontalAlignment.Center;
            text.Padding = new Thickness(0, 0, 0, 1);
            text.Margin = new Thickness(10, 0, 0, 0);
            text.Foreground = new SolidColorBrush(Color.FromRgb(r_Grey, g_Grey, b_Grey));

            //stack.Children.Add(bord);
            stack.Children.Add(text);
            stk_disks.Children.Add(stack);
            for (int i = 0; i < alls.Count; i++)
            {
                StackPanel stk = new StackPanel();
                stk.Orientation = Orientation.Horizontal;
                TextBlock text1 = new TextBlock();
                text1.FontSize = 14;
                //text1.Inlines.Add(new Span(new Run(global.allDisks[i].Id )));
                //tBlock.Foreground = ;
                text1.TextAlignment = TextAlignment.Center;
                text1.VerticalAlignment = VerticalAlignment.Center;
                text1.HorizontalAlignment = HorizontalAlignment.Center;
                text1.Padding = new Thickness(0, 0, 0, 1);
                text1.Margin = new Thickness(10, 0, 0, 0);
                text1.Foreground = new SolidColorBrush(Color.FromRgb(r_Grey, g_Grey, b_Grey));

                //stack.Children.Add(bord);
                stk.Children.Add(text1);
                stk_disks.Children.Add(stk);
            }
        }


        private void signUp_cancel_Click(object sender, RoutedEventArgs e) // Вернуться на форму авторизации
        {
            
        }


        private void signUp_Ok_Click(object sender, RoutedEventArgs e)  // OK регистрация
        {
           
        }

        private void close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}

