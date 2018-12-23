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
using System.Windows.Shapes;
using System.Windows.Threading;

namespace KaiXinXiaoMoNv
{
    /// <summary>
    /// SetTouMing.xaml 的交互逻辑
    /// </summary>
    public delegate void ChangeRiQihandler();//定义委托
    public partial class SetRiQi : Window
    {
        private Int32 choose_count = 0;
        public event ChangeRiQihandler ChangeRiQiEvent;
        

        public SetRiQi()
        {
            InitializeComponent();
            ShowInTaskbar = false;
        }
        
        private void RiQi_calendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            if (choose_count == 0)
            {
                choose_count++;
            }
            else
            {
                //this.Owner.yzm;
                MainWindow.choose_RiQi = Convert.ToDateTime(this.RiQi_calendar.SelectedDate);                
                this.Close();
            }
        }
                
        void StrikeEvent()//触发事件
        {
            if (ChangeRiQiEvent != null)
            {
                ChangeRiQiEvent();
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            StrikeEvent();
        }

        DispatcherTimer RiQi_timer = new DispatcherTimer();//using System.Windows.Threading;
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.Opacity = 0;
            //DispatcherTimer RiQi_timer = new DispatcherTimer();//using System.Windows.Threading;
            RiQi_timer.Interval = TimeSpan.FromMilliseconds(100);
            RiQi_timer.Tick += Timer_Tick;
            RiQi_timer.Start();
        }
        private void Timer_Tick(object sender,System.EventArgs e)
        {
            if (this.Opacity < 1)
            {
                this.Opacity += 0.25;
            }
            else
            {
                RiQi_timer.Stop();
            }
            
        }
    }
}
