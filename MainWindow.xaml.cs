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
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Windows.Threading;
using System.Timers;
using Renci.SshNet;

namespace IncubeAdmin
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int COUNT;
        List<Casmon> casmon_list;
        public List<Disk> disks;
        private Global global;
        public List<string> roles;
        ApplicationViewModel applicationView;
        DispatcherTimer getCasMon;                // таймер получение данных с casmon
        DispatcherTimer getSysMon;                // таймер получение данных с sysmon
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


        // серый
        byte r_Grey = 160;
        byte g_Grey = 178;
        byte b_Grey = 187;



        

        public MainWindow()
        {

            InitializeComponent();
            global = Global.getInstance();

            casmon_list = new List<Casmon>();
            disks = new List<Disk>();

            getCasMon = new DispatcherTimer();
            getCasMon.Tick += new EventHandler(GetConnectCassandras);
            getCasMon.Interval = new TimeSpan(0, 0, 1);

            getSysMon = new DispatcherTimer();
            getSysMon.Tick += new EventHandler(GetConnectCassandras);
            getSysMon.Interval = new TimeSpan(0, 0, 3);

            applicationView = ApplicationViewModel.getInstance();
            DataContext = applicationView;
            datagrid_users.ItemsSource = applicationView.Users;
            datagrid_users.SelectedItem = applicationView.SelectedUser;
            //datagrid_system.ItemsSource = applicationView.Cassandras;
            //datagrid_system.SelectedItem = applicationView.SelectedSystemCassandra;
            datagrid_system.ItemsSource = global.sshErrors;
            datagrid_system.SelectedItem = global.sshErrors;
           

            users_Page.Visibility = Visibility.Hidden;
            //system_Page.Visibility = Visibility.Hidden;
            directories_Page.Visibility = Visibility.Hidden;
            progressBar.Visibility = Visibility.Hidden;
            //none.Visibility = Visibility.Hidden;

            //MainGrid.Children.Remove(progressBar);
            stackPan_Nav.Children.Remove(chip_connect);

            name_node = new List<string>();
            isOk_node = new List<string>();
            ip_node = new List<string>();


            roles = new List<string> {"4","5","Hello" };
            roles.Add("1");
            roles.Add("2");
            this.DataContext = this;
            roles_combo.ItemsSource = null;
            roles_combo.ItemsSource = roles;

            
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
            ellipse2.Width = 300;
            ellipse2.Height = 300;
            ellipse2.Margin = new Thickness(x0 - 150, y0 - 150, 0, 0);
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



            radius = 115;
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
                bord.Width = 50;
                bord.Height = 50;
                bord.Margin = new Thickness(x1 - 25, y1 - 25, 0, 0);   // первый круг
                bord.CornerRadius = new CornerRadius(25);
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
                tBlock.FontSize = 16;
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


        private void butUser_Click(object sender, RoutedEventArgs e)                            // добавление User в таблицу
        {
            User user = new User();
            applicationView.Users.Insert(0, user);
            datagrid_users.ItemsSource = null;
            datagrid_users.ItemsSource = applicationView.Users;
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


        private void usersPage_Click(object sender, RoutedEventArgs e)                         // переключение страниц
        {
            users_Page.Visibility = Visibility.Visible;
            system_Page.Visibility = Visibility.Hidden;
            directories_Page.Visibility = Visibility.Hidden;
        }
        private void systemPage_Click(object sender, RoutedEventArgs e)                         // переключение страниц
        {
            users_Page.Visibility = Visibility.Hidden;
            system_Page.Visibility = Visibility.Visible;
            directories_Page.Visibility = Visibility.Hidden;
        }
        private void directoriesPage_Click(object sender, RoutedEventArgs e)                         // переключение страниц
        {
            users_Page.Visibility = Visibility.Hidden;
            system_Page.Visibility = Visibility.Hidden;
            directories_Page.Visibility = Visibility.Visible;
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
            //Chart.Update(true, true);
        }
        /// /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////





        
        private void datagrid_SelectionChanged(object sender, SelectionChangedEventArgs e)   // выбор строки в таблице
        {
            try
            {
                if (datagrid_users.SelectedItem != null)
                {
                    if (datagrid_users.SelectedItem is User)
                    {
                        var row = (User)datagrid_users.SelectedItem;

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

        private void sys_Click(object sender, RoutedEventArgs e)  // определение размеров окна
        {
            try
            {
                
                /*double a = this.Width;
                double b = this.Height;
                left_ssh_text.Text += (a + "  " + b).ToString();
                progressBar.Visibility = Visibility.Visible;


                Thread thread = new Thread(GetDataCassandra);
                thread.Start();*/

            }
            catch(Exception eee)
            {

            }
        }

/*        private void GetDataCassandra() // Получение начальных сведений от виртуальной машины
        {

            try
            {
                if (global.sshClient.IsConnected == true)
                {
                    // имя узла
                    //using (var command = global.sshClient.CreateCommand("nodetool status | awk '/^(U|D)(N|L|J|M)/{print $8}'"))
                    using (var command = global.sshClient.CreateCommand("hostnamectl | grep hostname | cut -d : -f 2"))
                    {
                        string fff = command.Execute();
                        string[] words = fff.Split(new char[] { '\n' });
                        for (int i = 0; i < words.Length - 1; i++)
                        {
                            name_node.Add(words[i]);
                        }
                    }

                    // доступность узла
                    using (var command = global.sshClient.CreateCommand("nodetool status | awk '/^(U|D)(N|L|J|M)/{print $1}'"))
                    {
                        string fff = command.Execute();
                        string[] words = fff.Split(new char[] { '\n' });
                        for (int i = 0; i < words.Length - 1; i++)
                        {
                            isOk_node.Add(words[i]);
                        }
                    }

                    // ip адрес узла
                    using (var command = global.sshClient.CreateCommand("nodetool status | awk '/^(U|D)(N|L|J|M)/{print $2}'"))
                    {
                        string fff = command.Execute();
                        string[] words = fff.Split(new char[] { '\n' });
                        for (int i = 0; i < words.Length - 1; i++)
                        {
                            ip_node.Add(words[i]);
                        }
                    }

                    for (int i = 0; i < name_node.Count; i++)
                    {
                        global.nodes.Add(new Node(name_node[i], ip_node[i], isOk_node[i]));
                    }



                    // вызвать диспетчер патока главного окна и сделать изменения в GUI. Если вместо BeginInvoke() 
                    // применить Invoke(). Метод Invoke() останавливает поток до тех пор, пока диспетчер выполняет код.
                    // Метод Invoke() можно использовать, если нужно приостановить асинхронную операцию до тех пор, пока
                    // от пользователя не поступит какой-нибудь отклик.
                    this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
                    {
                        radius_Elipse(global.nodes);
                    });


                    // вызвать диспетчер патока главного окна и сделать изменения в GUI.
                    this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
                    {

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
                            //third_stack_right.Children.Add(stack);
                        }
                    });

                    // вызвать диспетчер патока главного окна и сделать изменения в GUI.
                    this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
                    {
                        //progressBar.Visibility = Visibility.Hidden;
                    });
                }
            }
            catch (Exception dddd)
            {
                // вызвать диспетчер патока главного окна и сделать изменения в GUI.
                this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
                {
                    //left_ssh_text.Text += (dddd.ToString() + " \n");
                });
            }
        }
*/
        private void GetDataHost() // получить данные о хостах
        {
            // вызвать диспетчер патока главного окна и сделать изменения в GUI.
            this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
            {

                // добавление элементов в стекпанель вторая сверху
                for (int i = 0; i < global.hosts.Count; i++)
                {
                    StackPanel stack = new StackPanel();
                    stack.Orientation = Orientation.Horizontal;

                    /*Border bord = new Border();
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
                    bord.Focusable = true;*/
                    //bord.Tag = count[i]; // для поиска метки по клику правой кнопки мыши

                    TextBlock tBlock = new TextBlock();
                    tBlock.FontSize = 14;
                    tBlock.Inlines.Add(new Span(new Run(global.hosts[i].Ip + "     " + global.hosts[i].Login)));
                    //tBlock.Foreground = ;
                    tBlock.TextAlignment = TextAlignment.Center;
                    tBlock.VerticalAlignment = VerticalAlignment.Center;
                    tBlock.HorizontalAlignment = HorizontalAlignment.Center;
                    tBlock.Padding = new Thickness(0, 0, 0, 1);
                    tBlock.Margin = new Thickness(10, 0, 0, 0);
                    tBlock.Foreground = new SolidColorBrush(Color.FromRgb(r_Grey, g_Grey, b_Grey));

                    //stack.Children.Add(bord);
                    stack.Children.Add(tBlock);
                    //third_stack_right1.Children.Add(stack);
                }

                // добавление элементов в стекпанель вторая сверху
               /* for (int i = 0; i < global.hosts.Count; i++)
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
                    tBlock.Inlines.Add(new Span(new Run(global.hosts[i].Ip + "     " + global.hosts[i].Login)));
                    //tBlock.Foreground = ;
                    tBlock.TextAlignment = TextAlignment.Center;
                    tBlock.VerticalAlignment = VerticalAlignment.Center;
                    tBlock.HorizontalAlignment = HorizontalAlignment.Center;
                    tBlock.Padding = new Thickness(0, 0, 0, 1);
                    tBlock.Margin = new Thickness(10, 0, 0, 0);
                    tBlock.Foreground = new SolidColorBrush(Color.FromRgb(r_Grey, g_Grey, b_Grey));

                    //stack.Children.Add(bord);
                    stack.Children.Add(tBlock);
                    third_stack_right.Children.Add(stack);
                }*/
            });
        }

        private void connect_Click(object sender, RoutedEventArgs e) // окно создания подключения
        {
            try
            {
                stackPan_Nav.Children.Remove(chip_connect); // маркер определения состояния подключения
                stackPan_Nav.Children.Add(chip_block);  // маркер определения состояния подключения
            }
            catch { }


            try
            {
                SignIn sighIn = new SignIn();
                sighIn.Owner = Window.GetWindow(this);
                sighIn.ShowDialog();
                if (global.isConnect == true)
                {
                    stackPan_Nav.Children.Remove(chip_block); // маркер определения состояния подключения
                    stackPan_Nav.Children.Add(chip_connect);  // маркер определения состояния подключения
                }
                global.isConnect = false;
            }
            catch { }
        }

/*        private void sys1_Click(object sender, RoutedEventArgs e) // вывести информацию о узлах
        {
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

                    using (var command = global.sshClient.CreateCommand("nodetool status"))
                    {
                        string fff = command.Execute();
                        string[] words = fff.Split(new char[] { '\n' });
                        foreach (string s in words)
                        {
                            //left_ssh_text.Text += (s + "\n");
                        }
                        //Console.Write(command.Execute());
                        //Console.ReadLine();
                    }
                }

            }
            catch (Exception eee)
            {
                //left_ssh_text.Text = eee.ToString();
            }

            *//*MainGrid.Children.Remove(progressBar);
            progressBar.Visibility = Visibility.Hidden;*//*
        }
*/
        private void sys4_Click(object sender, RoutedEventArgs e)
        {
            /*progressBar.Visibility = Visibility.Visible;
            Thread.Sleep(4000);
            progressBar.Visibility = Visibility.Hidden;*/
        }

        private void Window_Loaded(object sender, RoutedEventArgs e) // после загрузки главного окна
        {

            SignIn sighIn = new SignIn();
            sighIn.Owner = Window.GetWindow(this);
            sighIn.ShowDialog();
            if (global.isConnect == true)
            {
                stackPan_Nav.Children.Remove(chip_block);
                stackPan_Nav.Children.Add(chip_connect);
            }
            global.isConnect = false;
            getCasMon.Start();
            //Thread thread = new Thread(GetDataCassandra);
            //thread.Start();
            //Thread thread = new Thread(GetConnectCassandras);
            //thread.Start();

            //Thread thread = new Thread(GetDataHost);
            //thread.Start();

        }
        //int i = 1;
        private async void GetConnectCassandras(Object source, EventArgs e)  // Метод в таймере для casmon и создания всех соединений по SSH
        {

            DispatcherTimer timer = (DispatcherTimer)source;
            if (null != timer.Tag)
            {
                return;
            }
            try
            {
                Task<List<Casmon>> t = Task<List<Casmon>>.Run(get_cass);
                timer.Tag = t;
                await t;
                global.casmons = t.Result;
                //i++;
                //getCasMon.Interval = new TimeSpan(0,0,0,i);
            }
            catch (Exception ass)
            {
                return;
            }
            finally
            {
                timer.Tag = null;
            }

            // получение файла casmon из начального соединения и извлечение всех доступных адресов
            List<Casmon> get_cass()
            {
                casmon_list.Clear();
                //var command = global.sshClients[0].CreateCommand("cd /etc/cassandra/; /opt/rust-bin/casmon");
                //var fff = command.Execute();
                try
                {
                    using (SshCommand ddd = global.sshClients[0].RunCommand("cd /etc/cassandra/; /opt/rust-bin/casmon"))
                    {
                        string res = ddd.Result;
                        JObject eee = JObject.Parse(res);
                        JArray list = (JArray)eee["seeds"];
                        global.NameClasterCassandra = (string)eee["clusetr_name"];
                        string node = "";
                        string check = "";
                        foreach (JObject content in list.Children<JObject>())
                        {
                            foreach (JProperty prop in content.Properties())
                            {
                                if (prop.Name.ToString() == "node")
                                {
                                    node = prop.Value.ToString();
                                }
                                else
                                {
                                    check = prop.Value.ToString();
                                }
                            }
                            casmon_list.Add(new Casmon(node, check));
                        }
                    }
                }
                catch (Exception ass)
                {
                    this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
                    {
                        error_text.Text = $"Ошибка при получении данных с начального узла! \n{COUNT}\n{ass}";
                    });
                    COUNT++;
                }
                // делаем остальные соединения по ssh если их еще не сделали
                int m = global.sshClients.Count -1 ;
                if(global.sshClients.Count < casmon_list.Count) // если количество соединений меньше возможных
                {
                    foreach(Casmon casmon in casmon_list)
                    {
                        if(casmon.node != global.host)
                        {
                            m++;
                            try
                            {
                                global.sshClients.Add(new SshClient(casmon.node, global.login, global.password));
                                global.sshClients[m].Connect();
                                if (global.sshClients[m].IsConnected != true)
                                {
                                    //MessageBox.Show($"Соединение SSH по адресу {cassss[i].node} не получилось установить!");
                                    this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
                                    {
                                        error_text.Text = $"Соединение SSH по адресу {casmon.node} не получилось установить! \n{COUNT}";
                                    });
                                    COUNT++;
                                }
                            }
                            catch (Exception ee)
                            {
                                global.sshErrors.Add(new SshError(DateTime.Now.ToLongTimeString(), ee.ToString()));
                                this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
                                {
                                    Console.WriteLine(casmon.node);
                                    //MessageBox.Show($"Ошибка соединения по SSH {cassss[i].node} . \n {ee}");
                                    error_text.Text = $"Ошибка соединения по SSH {casmon.node} . \n{COUNT}\n {ee}";
                                });
                                COUNT++;
                            }
                        }
                    }
                    int o = 0; // просто так. для точки остановки
                }
                else if(global.sshClients.Count == casmon_list.Count)  // проверка на IsConnected 
                {
                    foreach (SshClient ssh_client in global.sshClients)
                    {
                        if(ssh_client.IsConnected == false)
                        {
                            try
                            {
                                ssh_client.Connect();
                            }
                            catch (Exception ass)
                            {
                                this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
                                {
                                    error_text.Text = $"Ошибка при получении данных с начального узла! \n{COUNT}\n{ass}";
                                });
                                COUNT++;
                            }
                        }
                    }
                }

                global.sysmons.Clear();
                // получение всех sysmon файлов
                foreach(SshClient ssh in global.sshClients)
                {
                    disks.Clear();
                    try
                    {
                        using (SshCommand ddd = ssh.RunCommand("cd /etc/cassandra/; /opt/rust-bin/sysmon"))
                        {
                            string res = ddd.Result;
                            JObject eee = JObject.Parse(res);

                            string host = (string)eee["host"];
                            string ip = (string)eee["ip"];
                            string os = (string)eee["os"];
                            string version = (string)eee["version"];
                            int mem_total = (int)eee["mem_total"];
                            int mem_used = (int)eee["mem_used"];


                            JArray list = (JArray)eee["disks"];
                            string name = "";
                            string mount_point = "";
                            double total = 0;
                            double used = 0;
                            foreach (JObject content in list.Children<JObject>())
                            {
                                foreach (JProperty prop in content.Properties())
                                {
                                    if (prop.Name.ToString() == "name")
                                    {
                                        name = prop.Value.ToString();
                                    }
                                    else if (prop.Name.ToString() == "mount_point")
                                    {
                                        mount_point = prop.Value.ToString();
                                    }
                                    else if (prop.Name.ToString() == "total")
                                    {
                                        total = (double)prop.Value;
                                    }
                                    else if (prop.Name.ToString() == "used")
                                    {
                                        used = (double)prop.Value;
                                    }
                                }
                                disks.Add(new Disk(name, mount_point, total, used));
                            }
                            global.sysmons.Add(new Sysmon(host, ip, os, version, mem_total, mem_used, disks));
                        }
                    }
                    catch (Exception ass)
                    {
                        this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
                        {
                            error_text.Text = $"Ошибка при получении данных с {ssh.ConnectionInfo.Host}! \n{COUNT}\n{ass}";
                        });
                        COUNT++;
                    }
                }

                return casmon_list;
            }
        }

        private void TextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ShowAllDisk showAllDisk = new ShowAllDisk();
            showAllDisk.ShowDialog();
        }

       
    }
}

