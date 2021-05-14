using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Shapes;
using System.Windows.Threading;
using IncubeAdmin.window;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using Newtonsoft.Json.Linq;
using Renci.SshNet;

namespace IncubeAdmin
{
    public partial class MainWindow : Window
    {
        string selectedRole;
        string nameClaster;
        bool setBoolNameClaster = false;
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

        // зеленый для элипсов
        byte r_GreenE = 36;
        byte g_GreenE = 201;
        byte b_GreenE = 133;

        // желтый
        byte r_Yellow = 255;
        byte g_Yellow = 241;
        byte b_Yellow = 118;


        // серый
        byte r_Grey = 160;
        byte g_Grey = 178;
        byte b_Grey = 187;

        // красный
        byte r_Red = 244;
        byte g_Red = 110;
        byte b_Red = 104;

        // серый для эллипса
        byte r = 66;
        byte g = 66;
        byte b = 66;

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
            //DataContext = applicationView;
            DataContext = applicationView.SelectedUser;
            stackPanel.DataContext = applicationView.SelectedUser;
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

            text_Gif_System.Visibility = Visibility.Hidden;
            image_Gif_System.Visibility = Visibility.Hidden;

            //MainGrid.Children.Remove(progressBar);
            stackPan_Nav.Children.Remove(chip_connect);

            name_node = new List<string>();
            isOk_node = new List<string>();
            ip_node = new List<string>();


            roles = new List<string> {"","Лесоруб","Врач","Космонавт" };
            this.DataContext = this;
            roles_combo.ItemsSource = null;
            roles_combo.ItemsSource = roles;
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
            DataContext = this;

            x0 = cnv.Width / 2;    // центр канваса
            y0 = cnv.Height / 2;   // центр канваса

            main_Elipse();
            radius = 150;

        }
        public void cassandra_Name(string claster)
        {
            name_claster.Text = claster;
        }
        public void main_Elipse() // отрисовка круга для фона
        {
            Ellipse ellipse2 = new Ellipse();
            
            ellipse2.Fill = new SolidColorBrush(Color.FromRgb(r, g, b));
            ellipse2.Width = 450;
            ellipse2.Height = 450;
            ellipse2.Margin = new Thickness(x0 - 225, y0 - 225, 0, 0);
            ellipse2.Stroke = Brushes.Black;
            ellipse2.StrokeThickness = 0;
            

            DropShadowEffect shadowEffect = new DropShadowEffect();
            Color c3 = (Color)ColorConverter.ConvertFromString("#FF595959");
            shadowEffect.Color = c3;
            //shadowEffect.Color = Colors.Black;
            shadowEffect.Direction = 0;
            shadowEffect.BlurRadius = 10;
            shadowEffect.ShadowDepth = 0;
            shadowEffect.Opacity = 50;
            //effect.
            ellipse2.Effect = shadowEffect;
            cnv.Children.Add(ellipse2);
        }
        public void radius_Elipse_Casmon (List<Casmon> count) // отрисовка элипсов по окружности
        {
            
            double angle = 360 / (count.Count);
            double angle2 = angle;

            double radian = angle * Math.PI / 180;
            double radian2 = radian;
            for (int i = 0; i < count.Count; i++)
            {


                double x1 = x0 + (radius * Math.Cos(radian2));
                double y1 = y0 + (radius * Math.Sin(radian2));

                Border bord = new Border();
                bord.Width = 110;
                bord.Height = 110;
                bord.Margin = new Thickness(x1 - 55, y1 - 55, 0, 0);   // первый круг
                bord.CornerRadius = new CornerRadius(55);
                bord.BorderBrush = Brushes.Orange;
                bord.BorderThickness = new Thickness(5);

                if (global.casmons[i].check == "False")
                {
                    bord.BorderBrush = new SolidColorBrush(Color.FromRgb(r_Yellow, g_Yellow, b_Yellow));
                    bord.Background = new SolidColorBrush(Color.FromRgb(r_Yellow, g_Yellow, b_Yellow));
                    //bord.Tag = "yellow";
                    bord.Tag = global.casmons[i].node;
                }
                else
                {
                    bord.BorderBrush = new SolidColorBrush(Color.FromRgb(r_GreenE, g_GreenE, b_GreenE));
                    bord.Background = new SolidColorBrush(Color.FromRgb(r_GreenE, g_GreenE, b_GreenE));
                    //bord.Tag = "green";
                    bord.Tag = global.casmons[i].node;
                }

                bord.BorderThickness = new Thickness(0);
                bord.Focusable = true;
                

                TextBlock tBlock = new TextBlock();
                tBlock.FontSize = 14;
                tBlock.Inlines.Add(new Bold(new Run(count[i].node)));
                tBlock.Foreground = new SolidColorBrush(Color.FromRgb(48, 48, 48));
                tBlock.TextAlignment = TextAlignment.Center;
                tBlock.VerticalAlignment = VerticalAlignment.Center;
                tBlock.HorizontalAlignment = HorizontalAlignment.Center;
                tBlock.Padding = new Thickness(0, 0, 0, 1);

                bord.Child = tBlock;
                bord.MouseLeftButtonDown += bordLeftButton;
                //cnv.Children.Add(bord1);
                cnv.Children.Add(bord);

                //stckNmbr ++;
                angle2 += angle;
                radian2 = angle2 * Math.PI / 180;
            }
        }
        private void bordLeftButton(object sender, RoutedEventArgs e)
        {
            Border bord = (Border)sender;
            //MessageBox.Show("hello");
            ShowAllDisk showAll = new ShowAllDisk(bord.Tag.ToString());
            showAll.Owner = Window.GetWindow(this);
            showAll.Show();
        }
        public void radius_Elipse_SSH(List<SshClient> count) // отрисовка элипсов по окружности
        {

            double angle = 360 / (count.Count);
            double angle2 = angle;

            double radian = angle * Math.PI / 180;
            double radian2 = radian;
            for (int i = 0; i < count.Count; i++)
            {


                double x1 = x0 + (radius * Math.Cos(radian2));
                double y1 = y0 + (radius * Math.Sin(radian2));

                Border bord = new Border();
                bord.Width = 110;
                bord.Height = 110;
                bord.Margin = new Thickness(x1 - 55, y1 - 55, 0, 0);   // первый круг
                bord.CornerRadius = new CornerRadius(55);
                //bord.BorderBrush = Brushes.Orange;
                if (global.casmons[i].check == "False")
                {
                    bord.BorderBrush = new SolidColorBrush(Color.FromRgb(r_Yellow, g_Yellow, b_Yellow));
                    bord.Background = new SolidColorBrush(Color.FromRgb(r_Yellow, g_Yellow, b_Yellow));
                    //bord.Tag = "yellow";
                    bord.Tag = global.casmons[i].node;
                }
                else
                {
                    bord.BorderBrush = new SolidColorBrush(Color.FromRgb(r_GreenE, g_GreenE, b_GreenE));
                    bord.Background = new SolidColorBrush(Color.FromRgb(r_GreenE, g_GreenE, b_GreenE));
                    //bord.Tag = "green";
                    bord.Tag = global.casmons[i].node;
                }

                bord.BorderThickness = new Thickness(0);
                bord.Focusable = true;


                TextBlock tBlock = new TextBlock();
                tBlock.FontSize = 14;
                //tBlock.Inlines.Add(new Bold(new Run(count[i].node)));
                tBlock.Foreground = new SolidColorBrush(Color.FromRgb(48, 48, 48));
                tBlock.TextAlignment = TextAlignment.Center;
                tBlock.VerticalAlignment = VerticalAlignment.Center;
                tBlock.HorizontalAlignment = HorizontalAlignment.Center;
                tBlock.Padding = new Thickness(0, 0, 0, 1);

                bord.Child = tBlock;
                cnv.Children.Add(bord);

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



                    // Проверяем доступность и меняем цвет
                    this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
                    {
                        if (global.casmons.Count == 0)
                        {
                            global.casmons = casmon_list;
                            radius_Elipse_Casmon(global.casmons);
                        }
                        else
                        {
                            foreach (Casmon cas in global.casmons)
                            {
                                if (cas.check == "False")
                                {
                                    //string tag = cas.node;
                                    foreach (UIElement uI in cnv.Children)
                                    {
                                        if (uI is Border)
                                        {
                                            Border border = (Border)uI;

                                            SolidColorBrush red = new SolidColorBrush(Color.FromRgb(r_Red, g_Red, b_Red));
                                            SolidColorBrush border_color = (SolidColorBrush)border.Background;

                                            if (border.Tag.ToString() == cas.node.ToString() && red.Color != border_color.Color)
                                            {
                                                border.Background = new SolidColorBrush(Color.FromRgb(r_Yellow, g_Yellow, b_Yellow));
                                                //global.casmons = t.Result;
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    foreach (UIElement uI in cnv.Children)
                                    {
                                        if (uI is Border)
                                        {
                                            Border border = (Border)uI;

                                            SolidColorBrush red = new SolidColorBrush(Color.FromRgb(r_Red, g_Red, b_Red));
                                            SolidColorBrush border_color = (SolidColorBrush)border.Background;

                                            if (border.Tag.ToString() == cas.node.ToString() && red.Color != border_color.Color)
                                            {
                                                border.Background = new SolidColorBrush(Color.FromRgb(r_GreenE, g_GreenE, b_GreenE));
                                                //global.casmons = t.Result;
                                            }
                                        }
                                    }
                                }

                            }
                        }
                    });

                }
                catch (Exception ass)
                {
                    global.sshErrors.Add(new SshError(DateTime.Now.ToLocalTime().ToString(), ass.ToString()));
                    this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
                    {
                        datagrid_system.ItemsSource = null;
                        datagrid_system.SelectedItem = null;
                        datagrid_system.ItemsSource = global.sshErrors;
                        datagrid_system.SelectedItem = global.sshErrors;
                    });
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
                                Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
                                {
                                    progressBar.Visibility = Visibility.Visible;
                                });
                                global.sshClients[m].Connect();
                                Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
                                {
                                    progressBar.Visibility = Visibility.Hidden;
                                });
                                if (global.sshClients[m].IsConnected != true)
                                {
                                    //MessageBox.Show($"Соединение SSH по адресу {cassss[i].node} не получилось установить!");
                                    /*this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
                                    {
                                        error_text.Text = $"Соединение SSH по адресу {casmon.node} не получилось установить! \n{COUNT}";
                                    });
                                    COUNT++;*/
                                }
                            }
                            catch (Exception ee)
                            {
                                //string host = global.sshClients[m].ConnectionInfo.Host;
                                string host = casmon.node;
                                Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
                                {
                                    foreach (UIElement uI in cnv.Children)
                                    {
                                        if (uI is Border)
                                        {
                                            Border border = (Border)uI;
                                            if (border.Tag.ToString() == host.ToString())
                                            {
                                                border.Background = new SolidColorBrush(Color.FromRgb(r_Red, g_Red, b_Red));
                                            }
                                        }
                                    }
                                });




                                global.sshErrors.Add(new SshError(DateTime.Now.ToLocalTime().ToString(), ee.ToString()));
                                this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
                                {
                                    progressBar.Visibility = Visibility.Hidden;
                                    datagrid_system.ItemsSource = null;
                                    datagrid_system.SelectedItem = null;
                                    datagrid_system.ItemsSource = global.sshErrors;
                                    datagrid_system.SelectedItem = global.sshErrors;
                                });
                            }
                        }
                    }
                }
                else if(global.sshClients.Count == casmon_list.Count)  // проверка на IsConnected 
                {
                    foreach (SshClient ssh_client in global.sshClients)
                    {
                        if(ssh_client.IsConnected == false)
                        {
                            try
                            {
                                Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
                                {
                                    text_Gif_System.Visibility = Visibility.Visible;
                                    image_Gif_System.Visibility = Visibility.Visible;
                                });
                                ssh_client.Connect();

                                // Если соединение установлено то красим в желтый
                                string host = ssh_client.ConnectionInfo.Host;
                                Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
                                {
                                    foreach (UIElement uI in cnv.Children)
                                    {
                                        if (uI is Border)
                                        {
                                            Border border = (Border)uI;
                                            if (border.Tag.ToString() == host.ToString())
                                            {
                                                border.Background = new SolidColorBrush(Color.FromRgb(r_Yellow, g_Yellow, b_Yellow));
                                            }
                                        }
                                    }
                                });

                                Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
                                {
                                    text_Gif_System.Visibility = Visibility.Hidden;
                                    image_Gif_System.Visibility = Visibility.Hidden;
                                });

                            }
                            catch (Exception ass)
                            {
                                //======================================================================================================================
                                string host = ssh_client.ConnectionInfo.Host;
                                Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
                                {
                                    foreach (UIElement uI in cnv.Children)
                                    {
                                        if (uI is Border)
                                        {
                                            Border border = (Border)uI;
                                            if (border.Tag.ToString() == host.ToString())
                                            {
                                                border.Background = new SolidColorBrush(Color.FromRgb(r_Red, g_Red, b_Red));
                                            }
                                        }
                                    }
                                });


                                global.sshErrors.Add(new SshError(DateTime.Now.ToLocalTime().ToString(), ass.ToString()));
                                this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
                                {
                                    text_Gif_System.Visibility = Visibility.Hidden;
                                    image_Gif_System.Visibility = Visibility.Hidden;
                                    datagrid_system.ItemsSource = null;
                                    datagrid_system.SelectedItem = null;
                                    datagrid_system.ItemsSource = global.sshErrors;
                                    datagrid_system.SelectedItem = global.sshErrors;
                                });
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
                                disks.Add(new Disk(name, mount_point, total.ToString(), used.ToString()));
                            }
                            global.sysmons.Add(new Sysmon(host, ip, os, version, mem_total, mem_used, disks));
                            if (!setBoolNameClaster)  // устанавливаем имя кластера
                            {
                                nameClaster = global.sysmons[0].host;
                                this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
                                {
                                    cassandra_Name(nameClaster);
                                });
                                setBoolNameClaster = true;
                            }
                        }
                    }
                    catch (Exception ass)
                    {
                        global.sshErrors.Add(new SshError(DateTime.Now.ToLocalTime().ToString(), ass.ToString()));
                        this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
                        {
                            datagrid_system.ItemsSource = null;
                            datagrid_system.SelectedItem = null;
                            datagrid_system.ItemsSource = global.sshErrors;
                            datagrid_system.SelectedItem = global.sshErrors;
                        });
                    }
                }
                return casmon_list;
            }
        }

        private void roles_combo_SelectionChanged(object sender, SelectionChangedEventArgs e) // выбор роли
        {
            int selectedIndex = roles_combo.SelectedIndex;
            selectedRole = roles_combo.SelectedItem.ToString();
        }
    }
}

