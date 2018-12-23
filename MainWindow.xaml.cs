
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Drawing;
using System.Windows.Forms;
using System.Data;
using System.Data.OleDb;
using System.Threading;


namespace KaiXinXiaoMoNv
{
    
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    /// 
   
    public partial class MainWindow : Window
    {
       public static DateTime choose_RiQi = DateTime.Now.Date;
       public MainWindow()
        { 
            bool bExist;
            Mutex MyMutex = new Mutex(true, "OnlyRunOncetime", out bExist);
            bool aa = bExist;
            if (bExist)
            {
                InitializeComponent();
                ShowInTaskbar = false;
                this.Left = SystemParameters.PrimaryScreenWidth - this.Width;
                this.Top = SystemParameters.PrimaryScreenHeight - this.Height;
                Bind_message();
                InitialTray();
                MyMutex.ReleaseMutex();
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("程序已经运行！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                notifyIcon.Dispose();
                System.Windows.Application.Current.Shutdown();
            }            
        }

        private void Messages_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (this.Messages.IsReadOnly)
            {
                this.Messages.IsReadOnly = false;
            }
            else
            {
                this.Messages.IsReadOnly = true;
            }
        }        

        private System.Windows.Forms.NotifyIcon notifyIcon = null;
        private void InitialTray()
        {
            notifyIcon = new System.Windows.Forms.NotifyIcon();
            notifyIcon.BalloonTipText = "欢迎欢迎!";
            notifyIcon.Text = "便签";
            string aa = System.Windows.Forms.Application.StartupPath;
            notifyIcon.Icon = new System.Drawing.Icon(System.Windows.Forms.Application.StartupPath+ "\\Icon.ico");
            notifyIcon.Visible = true;
            notifyIcon.ShowBalloonTip(2000);
            notifyIcon.MouseClick += new System.Windows.Forms.MouseEventHandler(notifyIcon_MouseClick);
            //System.Windows.Forms.MenuItem menu1 = new System.Windows.Forms.MenuItem("菜单项1");
            //System.Windows.Forms.MenuItem menu2 = new System.Windows.Forms.MenuItem("菜单项2");
            //System.Windows.Forms.MenuItem menu = new System.Windows.Forms.MenuItem("菜单", new System.Windows.Forms.MenuItem[] { menu1, menu2 });
            //System.Windows.Forms.MenuItem exit = new System.Windows.Forms.MenuItem("exit");
            //exit.Click += new EventHandler(exit_Click);
            //System.Windows.Forms.MenuItem[] childen = new System.Windows.Forms.MenuItem[] { menu, exit };
            //notifyIcon.ContextMenu = new System.Windows.Forms.ContextMenu(childen);
            System.Windows.Forms.MenuItem exit = new System.Windows.Forms.MenuItem("退出");
            exit.Click += new EventHandler(exit_Click);
            System.Windows.Forms.MenuItem[] childen = new System.Windows.Forms.MenuItem[] { exit };
            notifyIcon.ContextMenu = new System.Windows.Forms.ContextMenu(childen);
            this.StateChanged += new EventHandler(SysTray_StateChanged);
        }

        private void SysTray_StateChanged(object sender,EventArgs e)
        {
            if (this.WindowState == WindowState.Minimized)
            {
                this.Visibility = Visibility.Hidden;
            }
        }

        private void exit_Click(object sender, EventArgs e)
        {
            if (System.Windows.MessageBox.Show("确定要关闭吗？", "退出", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.Yes)
            {
                notifyIcon.Dispose();
                System.Windows.Application.Current.Shutdown();
            }
        }

        private void notifyIcon_MouseClick(object sender,System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                if (this.Visibility == Visibility.Visible)
                {
                    this.Visibility = Visibility.Hidden;
                }
                else
                {
                    this.Visibility = System.Windows.Visibility.Visible;
                    this.Activate();
                }
            }
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Hidden;
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Messages.IsReadOnly = true;
            this.DragMove();            
        }            

        public DataTable GetDatatable(string tablename)
        {
            DataTable DTable = new DataTable();
            OleDbConnection myConn = new OleDbConnection(Properties.Resources.Connection);
            try
            {
                myConn.Open();
                if (myConn.State == System.Data.ConnectionState.Open)
                {
                    string acc_command = "select ID,NeiRong,RiQi,XiaoMoNv from " + tablename + " order by RiQi desc";                    
                    OleDbDataAdapter adapter1 = new OleDbDataAdapter(acc_command, myConn);
                    DataTable DataTab = new DataTable();
                    adapter1.Fill(DTable);
                    myConn.Close();
                }
            }
            catch (System.Data.OleDb.OleDbException ex)
            {                
                string[] message_err = ex.Message.ToString().Split('\'');
                System.Windows.Forms.MessageBox.Show("数据库错误：" + message_err[0], "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                notifyIcon.Dispose();
                System.Windows.Application.Current.Shutdown();            
            }
            finally
            {
                myConn.Close();
            }
            return DTable;
        }

        private void XieRu_Click(object sender, RoutedEventArgs e)
        {
            Int32 check_ID = 0;
            Int32 XiaoMoNv_ID = 0;
            string mess_NeiRong = inSQL(Messages.Text);
            DateTime toda = DateTime.Now;            
            OleDbConnection myConn = new OleDbConnection(Properties.Resources.Connection);
            try
            {

                myConn.Open();
                string check_command = "select * from NameTab where ( RiQi between #" + toda.Date.ToString("yyyy-MM-dd") + "# and #3000-01-01# ) order by RiQi desc";
                OleDbCommand check_com = new OleDbCommand(check_command, myConn);
                OleDbDataReader check_reader = check_com.ExecuteReader();
                if (check_reader.HasRows)
                {
                    while (check_reader.Read())
                    {
                        check_ID = Convert.ToInt32(check_reader[0]);
                        XiaoMoNv_ID = Convert.ToInt32(check_reader[3]) + 1;
                    }
                }
                string message_today = "insert into NameTab(NeiRong,RiQi,XiaoMoNv) values('" + mess_NeiRong + "','" + toda + "','" + XiaoMoNv_ID + "')";
                if (check_ID > 0)
                {
                    message_today = "update NameTab set NeiRong = '" + mess_NeiRong + "' , RiQi = '" + toda + "' , XiaoMoNv = '" + XiaoMoNv_ID + "' where ID = " + check_ID + "";
                }

                OleDbCommand cmd = new OleDbCommand(message_today, myConn);
                cmd.ExecuteNonQuery();
                myConn.Close();
                Bind_message();
            }
            catch (System.Data.OleDb.OleDbException ex)
            {
                string[] message_err = ex.Message.ToString().Split('\'');
                System.Windows.Forms.MessageBox.Show("数据库错误：" + message_err[0], "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            finally
            {
                myConn.Close();
            }
        }

        private void Bind_message()
        {
            if (GetDatatable("NameTab").Rows.Count > 0)
            {
                Messages.Text = outSQL(GetDatatable("NameTab").Rows[0][1].ToString());
                this.Date_label.Content = Convert.ToDateTime(GetDatatable("NameTab").Rows[0][2]).ToString("yyyy-MM-dd") + "(" + GetDatatable("NameTab").Rows[0][3].ToString() + ")";
            }
            else
            {
                Messages.Text = "双击添加信息！";
            }
            this.Messages.IsReadOnly = true;
        }
        
        private void ShuaXin_Click(object sender, RoutedEventArgs e)
        {
            Bind_message();
            choose_RiQi = DateTime.Now.Date;
        }

        public static string inSQL(string formatStr)
        {
            string rStr = formatStr;
            if (formatStr != null && formatStr != string.Empty)
            {
                rStr = rStr.Replace("'", "''");
                rStr = rStr.Replace("\"", "\"\"");
            }
            return rStr;
        }
        
        public static string outSQL(string formatStr)
        {
            string rStr = formatStr;
            if (rStr != null)
            {
                rStr = rStr.Replace("''", "'");
                rStr = rStr.Replace("\"\"", "\"");
            }
            return rStr;
        }

        private void exit_Click(object sender, RoutedEventArgs e)
        {
            if (System.Windows.MessageBox.Show("确定要关闭吗？", "退出", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.Yes)
            {
                notifyIcon.Dispose();
                System.Windows.Application.Current.Shutdown();
            }
        }

        private void TouMing_Click(object sender, RoutedEventArgs e)
        {
            SetTouMing TouMing = new SetTouMing();
            TouMing.Owner = this;
            System.Windows.Point P_mouse = Mouse.GetPosition(e.Source as FrameworkElement);
            TouMing.Left = (e.Source as FrameworkElement).PointToScreen(P_mouse).X - (TouMing.Width / 2);
            TouMing.Top = (e.Source as FrameworkElement).PointToScreen(P_mouse).Y - (TouMing.Height / 2);
            TouMing.TouMing_slider.Value = 1 - this.Opacity;
            TouMing.ShowDialog();          
        }

        private void RiQi_Click(object sender, RoutedEventArgs e)
        {
            SetRiQi RiQi = new SetRiQi();
            RiQi.Owner = this;
            System.Windows.Point P_mouse = Mouse.GetPosition(e.Source as FrameworkElement);
            RiQi.Left = (e.Source as FrameworkElement).PointToScreen(P_mouse).X - (RiQi.Width / 2);
            RiQi.Top = (e.Source as FrameworkElement).PointToScreen(P_mouse).Y - (RiQi.Height / 2);
            RiQi.RiQi_calendar.SelectedDate = choose_RiQi;
            RiQi.ChangeRiQiEvent += new ChangeRiQihandler(RiQiClosed);
            RiQi.ShowDialog();
        }

        void RiQiClosed()
        {
            DateTime choose_Cal = choose_RiQi;
            string RiQi_command = "select * from NameTab where ( RiQi between #" + choose_RiQi.Date.ToString("yyyy-MM-dd") + "# and #" + choose_RiQi.Date.AddDays(1).ToString("yyyy-MM-dd") + "# ) order by RiQi desc";
            Bind_RQMessage(RiQi_command);
        }
        public void Bind_RQMessage(string RiQi_command)
        {
            OleDbConnection myRQConn = new OleDbConnection(Properties.Resources.Connection);
            myRQConn.Open();
            if (myRQConn.State == System.Data.ConnectionState.Open)
            {
                OleDbCommand RiQi_com = new OleDbCommand(RiQi_command, myRQConn);
                OleDbDataReader RiQi_reader = RiQi_com.ExecuteReader();
                if (RiQi_reader.HasRows)
                {
                    while (RiQi_reader.Read())
                    {
                        this.Messages.Text = outSQL(RiQi_reader[1].ToString());
                        this.Date_label.Content = Convert.ToDateTime(RiQi_reader[2]).ToString("yyyy-MM-dd");
                    }
                }
                else
                {
                    this.Messages.Text = "你好，该日期没有任务记录！";
                    this.Date_label.Content = "welcome";
                }
                myRQConn.Close();
            }
        }
    }
}
