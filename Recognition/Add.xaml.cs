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

namespace Recognition
{
    /// <summary>
    /// Add.xaml 的交互逻辑
    /// </summary>
    
    //在主窗口点击Add按钮激活click事件并创建Add窗口，用于添加新的队员信息
    public partial class Add : Window
    {

        //创建MainWindow对象实例，用于将主窗口对象传入该子窗口当中用于修改主窗口的数据
        MainWindow c;
        //定义主窗口属性
        public MainWindow C
        {
            get
            {
                return c;
            }
            set
            {
                c = value;
            }
        }

        //构造器，用于将传入的主窗口对象赋值到该子窗口当中的承接对象的属性值上，并绘制和初始化子窗口
        public Add(MainWindow main)
        {
            C = main;
            InitializeComponent();
        }

        //响应Ensure按钮的click事件，用于确认并添加数据到存储的数据当中和
        private void Ensure_Click(object sender, RoutedEventArgs e)
        {
            ComboBoxItem item = Team.SelectedItem as ComboBoxItem;
            //在关闭当前子窗口的时候恢复主窗口的可编辑性
            c.IsEnabled = true;
            //c.UpdateLayout();
            if (pName.Text == string.Empty || pNum.Text == string.Empty || pAge.Text == string.Empty || item == null) 
			{
				this.Close();
				return;
			}

            else
                if (item.Content.ToString() == "Red")
                {
                    c.getRedList().Add(new Player(pName.Text, pNum.Text, 0, 0, System.Convert.ToInt32(pAge.Text), Team.SelectedValue.ToString()));

                    c.PlayerList.ItemsSource = null;
                    c.PlayerList.ItemsSource = c.getRedList();
                    c.PlayerList.DisplayMemberPath = "PlName";

                    c.age.SetBinding(TextBox.TextProperty, new Binding("SelectedItem.Age") { Source = c.PlayerList });
                    c.plNum.SetBinding(TextBox.TextProperty, new Binding("SelectedItem.PlNum") { Source = c.PlayerList });
                    c.foulTime.SetBinding(TextBox.TextProperty, new Binding("SelectedItem.FoulTime") { Source = c.PlayerList });
                    c.score.SetBinding(TextBox.TextProperty, new Binding("SelectedItem.Score") { Source = c.PlayerList });

                    c.Status.Content = "Red";
                }
                else if (item.Content.ToString() == "Blue")
                {
                    c.getBlueList().Add(new Player(pName.Text, pNum.Text, 0, 0, System.Convert.ToInt32(pAge.Text), Team.SelectedValue.ToString()));

                    c.PlayerList.ItemsSource = null;
                    c.PlayerList.ItemsSource = c.getBlueList();
                    c.PlayerList.DisplayMemberPath = "PlName";

                    c.age.SetBinding(TextBox.TextProperty, new Binding("SelectedItem.Age") { Source = c.PlayerList });
                    c.plNum.SetBinding(TextBox.TextProperty, new Binding("SelectedItem.PlNum") { Source = c.PlayerList });
                    c.foulTime.SetBinding(TextBox.TextProperty, new Binding("SelectedItem.FoulTime") { Source = c.PlayerList });
                    c.score.SetBinding(TextBox.TextProperty, new Binding("SelectedItem.Score") { Source = c.PlayerList });

                    c.Status.Content = "Blue";
                }
            this.Close();
        }

        //响应Cancel按钮的click事件，用于取消当前添加的子窗口，并返回主窗口
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            c.IsEnabled = true;
            this.Close();
        }

		private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			c.IsEnabled = true;
		}

    }
}
