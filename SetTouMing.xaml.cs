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
    public partial class SetTouMing : Window
    {
        public SetTouMing()
        {
            InitializeComponent();
            ShowInTaskbar = false;
        }
        
        private void TouMing_slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            this.Owner.Opacity = 1 - TouMing_slider.Value;            
            Close_button.Content = Convert.ToInt32(TouMing_slider.Value * 100);
        }

        private void Close_button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        DispatcherTimer TouMing_timer = new DispatcherTimer();//using System.Windows.Threading;        
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.Opacity = 0;
            //DispatcherTimer TouMing_timer = new DispatcherTimer();//using System.Windows.Threading;
            TouMing_timer.Interval = TimeSpan.FromMilliseconds(100);
            TouMing_timer.Tick += Timer_Tick;
            TouMing_timer.Start();
        }
        private void Timer_Tick(object sender, System.EventArgs e)
        {
            if (this.Opacity < 1)
            {
                this.Opacity += 0.5;
            }
            else
            {
                TouMing_timer.Stop();
            }
        }
    }
}
