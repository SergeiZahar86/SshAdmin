using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using IncubeAdmin.main;
using IncubeAdmin.window;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;

namespace IncubeAdmin
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Global global;
        ApplicationViewModel applicationView;
        Border border;
        int stckNmbr;
        TextBlock textBlock;
        double radius;                            // радиус круга для узлов 
        private Color с3;
        double x0;                                // центр канваса
        double y0;                                // центр канваса
        string remoteDirectory = "/";

        List<string> name_node;
        List<string> isOk_node;
        List<string> ip_node;

        // зеленый
        byte r_Green = 112;
        byte g_Green = 218;
        byte b_Green = 201;


        // желтый
        byte r_Yellow = 255;
        byte g_Yellow = 241;
        byte b_Yellow = 118;

        
        public MainWindow()
        {

            InitializeComponent();
            global = Global.getInstance();
            applicationView = ApplicationViewModel.getInstance();
            DataContext = applicationView;
            datagrid.ItemsSource = applicationView.Users;
            datagrid.SelectedItem = applicationView.SelectedUser;

            second.Visibility = Visibility.Hidden;
            third.Visibility = Visibility.Hidden;
            fourth.Visibility = Visibility.Hidden;
            progressBar.Visibility = Visibility.Hidden;

            //MainGrid.Children.Remove(progressBar);
            stackPan_Nav.Children.Remove(chip_connect);

            name_node = new List<string>();
            isOk_node = new List<string>();
            ip_node = new List<string>();



            /*var thread = new Thread(Worker) { IsBackground = true };
            thread.Start();*/



            // для диаграммы
            PointLabel = chartPoint => string.Format("{0} ({1:P})", chartPoint.Y, chartPoint.Participation);
            DataContext = this;



            // создание объектов для диаграмм
            SeriesCollection = new SeriesCollection      
            {
                new PieSeries
                {
                    Title = "Chrome",
                    Values = new ChartValues<ObservableValue> { new ObservableValue(8) },
                    DataLabels = true
                },
                new PieSeries
                {
                    Title = "Mozilla",
                    Values = new ChartValues<ObservableValue> { new ObservableValue(6) },
                    DataLabels = true
                },
                new PieSeries
                {
                    Title = "Opera",
                    Values = new ChartValues<ObservableValue> { new ObservableValue(10) },
                    DataLabels = true
                },
                new PieSeries
                {
                    Title = "Explorer",
                    Values = new ChartValues<ObservableValue> { new ObservableValue(4) },
                    DataLabels = true
                }
            };

            //adding values or series will update and animate the chart automatically
            //SeriesCollection.Add(new PieSeries());
            //SeriesCollection[0].Values.Add(5);

            DataContext = this;

            x0 = cnv.Width / 2;    // центр канваса
            y0 = cnv.Height / 2;   // центр канваса

            // серый
            byte r = 66;
            byte g = 66;
            byte b = 66;

            



            /*Ellipse ellipse = new Ellipse();
            ellipse.Fill = new SolidColorBrush(Color.FromRgb(r, g, b));
            ellipse.Width = 550;
            ellipse.Height = 550;
            ellipse.Margin = new Thickness(x0 - 275, y0 - 275, 0, 0);
            ellipse.Stroke = Brushes.Black;
            ellipse.StrokeThickness = 0;*/

            /*DropShadowEffect effect = new DropShadowEffect();
            Color c3 = (Color)ColorConverter.ConvertFromString("#ededed");
            //effect.Color = Colors.Orange;
            effect.Color = c3;
            effect.Direction = 315;
            effect.BlurRadius = 40;
            effect.ShadowDepth = 20;
            effect.Opacity = 50;
            //effect.
            ellipse.Effect = effect;*/

            //cnv.Children.Add(ellipse);


            Ellipse ellipse2 = new Ellipse();
            //Color c2 = Color.FromArgb(246, 246, 248, 0);
            //ellipse2.Fill = new SolidColorBrush(c);
            ellipse2.Fill = new SolidColorBrush(Color.FromRgb(r, g, b));
            ellipse2.Width = 550;
            ellipse2.Height = 550;
            ellipse2.Margin = new Thickness(x0 - 275, y0 - 275, 0, 0);
            ellipse2.Stroke = Brushes.Black;
            ellipse2.StrokeThickness = 0;


            DropShadowEffect shadowEffect = new DropShadowEffect();
            //Color c3 = (Color)ColorConverter.ConvertFromString("#ededed");
            //shadowEffect.Color = c3;
            shadowEffect.Color = Colors.Black;
            shadowEffect.Direction = 0;
            shadowEffect.BlurRadius = 10;
            shadowEffect.ShadowDepth = 0;
            shadowEffect.Opacity = 50;
            //effect.
            ellipse2.Effect = shadowEffect;
            cnv.Children.Add(ellipse2);



            radius = 200;
            //radius_Elipse(4);

        }

        public void radius_Elipse (List<Node> count) // отрисовка элипсов по окружности
        {
            /*double x0 = cnv.Width / 2;    // центр канваса
            double y0 = cnv.Height / 2;   // центр канваса*/


            //stckNmbr = 1;


            /*for(int i = 0; i <count.Length - 1; i++)
            {

            }*/

            /*border = new Border();
            border.Width = 50;
            border.Height = 50;


            border.Margin = new Thickness(x0 - 25, y0 + radius - 25, 0, 0);   // первый круг
            //var ddd = border.Margin.Left;
            border.CornerRadius = new CornerRadius(25);
            border.BorderBrush = Brushes.Red;
            border.Background = Brushes.LightPink;
            border.BorderThickness = new Thickness(2);
            border.Focusable = true;
            border.Tag = stckNmbr.ToString(); // для поиска метки по клику правой кнопки мыши


            textBlock = new TextBlock();
            textBlock.FontSize = 10;
            textBlock.Inlines.Add(new Bold(new Run(stckNmbr.ToString())));
            textBlock.Foreground = Brushes.Black;
            textBlock.TextAlignment = TextAlignment.Center;
            textBlock.VerticalAlignment = VerticalAlignment.Center;
            textBlock.HorizontalAlignment = HorizontalAlignment.Center;
            textBlock.Padding = new Thickness(0, 0, 0, 1);

            border.Child = textBlock;
            cnv.Children.Add(border);*/
            



            double angle = 360 / (count.Count);
            double angle2 = angle;

            double radian = angle * Math.PI / 180;
            double radian2 = radian;
            for (int i = 0; i < count.Count; i++)
            {


                double x1 = x0 + (radius * Math.Cos(radian2));
                double y1 = y0 + (radius * Math.Sin(radian2));

                Border bord = new Border();
                bord.Width = 100;
                bord.Height = 100;
                bord.Margin = new Thickness(x1 - 50, y1 - 50, 0, 0);   // первый круг
                bord.CornerRadius = new CornerRadius(50);
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
                bord.Tag = count[i].Name; // для поиска метки по клику правой кнопки мыши

                /*DropShadowEffect shadowEffect = new DropShadowEffect();
                //Color c3 = (Color)ColorConverter.ConvertFromString("#ededed");
                //shadowEffect.Color = c3;
                shadowEffect.Color = Colors.White;
                shadowEffect.Direction = 135;
                shadowEffect.BlurRadius = 15;
                shadowEffect.ShadowDepth = 15;
                shadowEffect.Opacity = 50;
                //effect.
                bord.Effect = shadowEffect;*/

                /*Border bord1 = new Border();
                bord1.Width = 100;
                bord1.Height = 100;
                bord1.Margin = new Thickness(x1 - 50, y1 - 50, 0, 0);   // первый круг
                bord1.CornerRadius = new CornerRadius(50);
                bord1.BorderBrush = Brushes.Orange;
                bord1.Background = Brushes.Orange;
                bord1.BorderThickness = new Thickness(2);
                bord1.Focusable = true;
                bord1.Tag = stckNmbr.ToString(); // для поиска метки по клику правой кнопки мыши*/

               /* DropShadowEffect effect3 = new DropShadowEffect();
                Color c3 = (Color)ColorConverter.ConvertFromString("#ededed");
                //effect.Color = Colors.Orange;
                effect3.Color = c3;
                effect3.Direction = 315;
                effect3.BlurRadius = 10;
                effect3.ShadowDepth = 8;
                effect3.Opacity = 50;
                bord1.Effect = effect3;*/

                TextBlock tBlock = new TextBlock();
                tBlock.FontSize = 20;
                tBlock.Inlines.Add(new Bold(new Run(count[i].Name)));
                tBlock.Foreground = Brushes.Black;
                tBlock.TextAlignment = TextAlignment.Center;
                tBlock.VerticalAlignment = VerticalAlignment.Center;
                tBlock.HorizontalAlignment = HorizontalAlignment.Center;
                tBlock.Padding = new Thickness(0, 0, 0, 1);

                bord.Child = tBlock;
                //cnv.Children.Add(bord1);
                cnv.Children.Add(bord);

                //stckNmbr ++;
                angle2 += angle;
                radian2 = angle2 * Math.PI / 180;
            }
        }


        private void but_Click(object sender, RoutedEventArgs e)                            // добавление User в таблицу
        {
            User user = new User();
            applicationView.Users.Insert(0, user);
            datagrid.ItemsSource = null;
            datagrid.ItemsSource = applicationView.Users;
        }



        public Func<ChartPoint, string> PointLabel { get; set; }               // для диаграммы
        private void Chart_OnDataClick(object sender, ChartPoint chartpoint)   // для диаграммы
        {
            var chart = (LiveCharts.Wpf.PieChart)chartpoint.ChartView;

            //clear selected slice.
            foreach (PieSeries series in chart.Series)
                series.PushOut = 0;

            var selectedSeries = (PieSeries)chartpoint.SeriesView;
            selectedSeries.PushOut = 20;
        }





        private void CloseButton_Click(object sender, RoutedEventArgs e)                    // выход из программы
        {
            Application.Current.Shutdown(); // выход из программы
            Environment.Exit(0);
        }  
        private void ExpandButton_Click(object sender, RoutedEventArgs e)                   // окно во весь экран
        {
            if (WindowState == WindowState.Normal)
            {
                this.WindowState = WindowState.Maximized;
                this.Padding = new Thickness(100);
                this.Margin = new Thickness(100);
                HorizontalAlignment = HorizontalAlignment.Center;

            } else
            {
                WindowState = WindowState.Normal;
            }
        }
        private void MinButton_Click(object sender, RoutedEventArgs e)                      // сворачивание окна
        {
            this.WindowState = WindowState.Minimized;
        }
        private void ColorZone_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)   // Перемещение окна по экрану
        {
            this.DragMove();
        }





        private void radio1_Click(object sender, RoutedEventArgs e)                         // переключение страниц
        {
            first.Visibility = Visibility.Visible;
            second.Visibility = Visibility.Hidden;
            third.Visibility = Visibility.Hidden;
            fourth.Visibility = Visibility.Hidden;

        }
        private void radio2_Click(object sender, RoutedEventArgs e)                         // переключение страниц
        {
            first.Visibility = Visibility.Hidden;
            second.Visibility = Visibility.Visible;
            third.Visibility = Visibility.Hidden;
            fourth.Visibility = Visibility.Hidden;

        }
        private void radio3_Click(object sender, RoutedEventArgs e)                         // переключение страниц
        {
            first.Visibility = Visibility.Hidden;
            second.Visibility = Visibility.Hidden;
            third.Visibility = Visibility.Visible;
            fourth.Visibility = Visibility.Hidden;

        }
        private void radio4_Click(object sender, RoutedEventArgs e)                         // переключение страниц
        {
            first.Visibility = Visibility.Hidden;
            second.Visibility = Visibility.Hidden;
            third.Visibility = Visibility.Hidden;
            fourth.Visibility = Visibility.Visible;
        }
        private void ScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            ScrollViewer scv = (ScrollViewer)sender;
            scv.ScrollToVerticalOffset(scv.VerticalOffset - e.Delta);
            e.Handled = true;
        }












        /// //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public SeriesCollection SeriesCollection { get; set; }

        private void UpdateAllOnClick(object sender, RoutedEventArgs e)
        {
            var r = new Random();

            foreach (var series in SeriesCollection)
            {
                foreach (var observable in series.Values.Cast<ObservableValue>())
                {
                    observable.Value = r.Next(0, 10);
                }
            }
        }

        private void AddSeriesOnClick(object sender, RoutedEventArgs e)
        {
            var r = new Random();
            var c = SeriesCollection.Count > 0 ? SeriesCollection[0].Values.Count : 5;

            var vals = new ChartValues<ObservableValue>();

            for (var i = 0; i < c; i++)
            {
                vals.Add(new ObservableValue(r.Next(0, 10)));
            }

            SeriesCollection.Add(new PieSeries
            {
                Values = vals
            });
        }

        private void RemoveSeriesOnClick(object sender, RoutedEventArgs e)
        {
            if (SeriesCollection.Count > 0)
                SeriesCollection.RemoveAt(0);
        }

        private void AddValueOnClick(object sender, RoutedEventArgs e)
        {
            var r = new Random();
            foreach (var series in SeriesCollection)
            {
                series.Values.Add(new ObservableValue(r.Next(0, 10)));
            }
        }

        private void RemoveValueOnClick(object sender, RoutedEventArgs e)
        {
            foreach (var series in SeriesCollection)
            {
                if (series.Values.Count > 0)
                    series.Values.RemoveAt(0);
            }
        }

        private void RestartOnClick(object sender, RoutedEventArgs e)
        {
            Chart.Update(true, true);
        }
        /// /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////





        
        private void datagrid_SelectionChanged(object sender, SelectionChangedEventArgs e)   // выбор строки в таблице
        {
            try
            {
                if (datagrid.SelectedItem != null)
                {
                    if (datagrid.SelectedItem is User)
                    {
                        var row = (User)datagrid.SelectedItem;

                        if (row != null)
                        {
                            stackPanel.DataContext = row;

                        }
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        private void system_Click(object sender, RoutedEventArgs e)  // определение размеров окна
        {
            try
            {
                
                double a = this.Width;
                double b = this.Height;
                left_ssh_text.Text += (a + "  " + b).ToString();


                /*Thread thread = new Thread(UpdateTextWrong);
                thread.Start();*/
                //new MainWindow().ShowDialog();

                //progressBar.Visibility = Visibility.Visible;

                /*Wait wait = new Wait();
                wait.Width = this.Width;
                wait.Height = this.Height;
                wait.Show();*/

                /*if (WindowState == WindowState.Normal)
                {
                    this.WindowState = WindowState.Maximized;
                    this.Padding = new Thickness(100);
                    this.Margin = new Thickness(100);
                    HorizontalAlignment = HorizontalAlignment.Center;

                }
                else
                {
                    WindowState = WindowState.Normal;
                }*/


            }
            catch(Exception eee)
            {

            }
        }

        /*private void UpdateTextWrong()
        {
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
            }
        }*/


        private void signUp_Click(object sender, RoutedEventArgs e) // окно создания подключения
        {
            try
            {
                stackPan_Nav.Children.Remove(chip_connect); // маркер определения состояния подключения
                stackPan_Nav.Children.Add(chip_block);  // маркер определения состояния подключения
            }
            catch { }


            try
            {
                SignUp sighUp = new SignUp();
                sighUp.Owner = Window.GetWindow(this);
                sighUp.ShowDialog();
                if (global.isConnect == true)
                {
                    stackPan_Nav.Children.Remove(chip_block); // маркер определения состояния подключения
                    stackPan_Nav.Children.Add(chip_connect);  // маркер определения состояния подключения
                }
                global.isConnect = false;
            }
            catch { }
        }

        private void system1_Click(object sender, RoutedEventArgs e) // вывести информацию о узлах
        {
            //progressBar.Visibility = Visibility.Visible;
            /*MainGrid.Children.Add(progressBar);
            progressBar.Visibility = Visibility.Visible;
            Thread.Sleep(2000);*/
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
                            left_ssh_text.Text += (words[i] + "\n");
                            name_node.Add(words[i]);
                        }
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
                            left_ssh_text.Text += (words[i] + "\n");
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
                            left_ssh_text.Text += (words[i] + "\n");
                            ip_node.Add(words[i]);
                        }
                        //Console.Write(command.Execute());
                        //Console.ReadLine();
                    }

                    using (var command = global.sshClient.CreateCommand("nodetool status"))
                    {
                        string fff = command.Execute();
                        string[] words = fff.Split(new char[] { '\n' });
                        foreach (string s in words)
                        {
                            left_ssh_text.Text += (s + "\n");
                        }
                        //Console.Write(command.Execute());
                        //Console.ReadLine();
                    }
                }

            }
            catch (Exception eee)
            {
                left_ssh_text.Text = eee.ToString();
            }

            /*MainGrid.Children.Remove(progressBar);
            progressBar.Visibility = Visibility.Hidden;*/
        }
        private void system4_Click(object sender, RoutedEventArgs e)
        {
            /*progressBar.Visibility = Visibility.Visible;
            Thread.Sleep(4000);
            progressBar.Visibility = Visibility.Hidden;*/
        }

        private void Window_Loaded(object sender, RoutedEventArgs e) // после загрузки главного окна
        {
            try
            {
                /*Wait wait = new Wait();
                wait.Width = this.Width;
                wait.Height = this.Height;
                wait.Owner = Window.GetWindow(this);
                wait.Show();*/

                progressBar.Visibility = Visibility.Visible;

                SignUp sighUp = new SignUp();
                sighUp.Owner = Window.GetWindow(this);
                sighUp.ShowDialog();
                if (global.isConnect == true)
                {
                    stackPan_Nav.Children.Remove(chip_block);
                    stackPan_Nav.Children.Add(chip_connect);
                }
                global.isConnect = false;


                



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
                        /*foreach (string s in words)
                        {
                            ssh_text.Text += (s + "\n");
                        }*/
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

                    for(int i = 0; i < name_node.Count; i++)
                    {
                        global.nodes.Add( new Node( name_node[i], ip_node[i], isOk_node[i]));
                    }

                    radius_Elipse(global.nodes);

                    // добавление элементов в стекпанель
                    for (int i = 0; i <global.nodes.Count; i++)
                    {
                        StackPanel stack = new StackPanel();
                        stack.Orientation = Orientation.Horizontal;

                        Border bord = new Border();
                        bord.Width = 14;
                        bord.Height = 14;
                        bord.Margin = new Thickness(5, 5, 15, 5);   // первый круг
                        bord.CornerRadius = new CornerRadius(15);
                        //bord.BorderBrush = Brushes.Orange;
                        if(global.nodes[i].Status == "DN")
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
                        tBlock.Inlines.Add(new Span(new Run(global.nodes[i].Name + "   "+ global.nodes[i].Ip + "   " + global.nodes[i].Status)));
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
            catch(Exception dddd)
            {
                left_ssh_text.Text += (dddd.ToString() + " \n");
            }
            progressBar.Visibility = Visibility.Hidden;
        }

    }
}

