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
using System.Windows.Navigation;
using System.Windows.Shapes;
using SpeechLib;             //引用SpeechLib名称空间，导入语音识别库数据
using System.ComponentModel;
using System.Diagnostics;

namespace Recognition
{
	/// <summary>
	/// MainWindow.xaml 的交互逻辑
	/// </summary>
	public partial class MainWindow : Window
	{
        //包含了两个球队球员信息的List对象
		private List<Player> plRedList = new List<Player>()
			{
				new Player{PlName = "保罗", PlNum = "3", FoulTime = 0, Score = 0, Age = 28, Team = "Red"},
				new Player{PlName = "格里芬", PlNum = "13", FoulTime = 0, Score = 0, Age = 24, Team = "Red"},
                new Player{PlName = "巴恩斯", PlNum = "22", FoulTime = 0, Score = 0, Age = 29, Team = "Red"},
                new Player{PlName = "雷迪克", PlNum = "15", FoulTime = 0, Score = 0, Age = 29, Team = "Red"},
                new Player{PlName = "小乔丹", PlNum = "6", FoulTime = 0, Score = 0, Age = 30, Team = "Red"}
			};

         private List<Player> plBlueList = new List<Player>()
        {
            new Player{PlName = "韦德", PlNum = "3", FoulTime = 0, Score = 0, Age = 32, Team = "Blue"},
            new Player{PlName = "詹姆斯", PlNum = "6", FoulTime = 0, Score = 0, Age = 32, Team = "Blue"},
            new Player{PlName = "波什", PlNum = "1", FoulTime = 0, Score = 0, Age = 31, Team = "Blue"},
            new Player{PlName = "查莫斯", PlNum = "15", FoulTime = 0, Score = 0, Age = 29, Team = "Blue"},
            new Player{PlName = "巴蒂尔", PlNum = "33", FoulTime = 0, Score = 0, Age = 28, Team = "Blue"}
        };

		private SpeechLib.SpSharedRecoContext ssrc;                     //定义共享型识别引擎
		private ISpeechRecoGrammar srg;                                 //定义语法

        //主窗口绘制并初始化
		public MainWindow()
		{
			InitializeComponent();
		}

        //响应Start按钮的click事件
		private void Button_Click(object sender, RoutedEventArgs e)
		{
			ssrc = new SpSharedRecoContext();               //初始化共享引擎
			srg = ssrc.CreateGrammar(0);                   //创建语法模式
			srg.CmdLoadFromFile(".\\grammar.xml", SpeechLoadOption.SLODynamic);//读入规则
			ssrc.Recognition += new _ISpeechRecoContextEvents_RecognitionEventHandler(ssrc_Recognition);//添加识别事件     
			srg.CmdSetRuleState(srg.Rules.Item(0).Name, SpeechRuleState.SGDSActive);//激活规则  
		}

		void ssrc_Recognition(int StreamNumber, object StreamPosition, SpeechRecognitionType RecognitionType, ISpeechRecoResult Result)
		{
			textBox1.Text = Result.PhraseInfo.GetText(0, -1, true);             //获取语音识别结果将其存入string对象当中

			string text = Result.PhraseInfo.GetText(0, -1, true);
			int redscore = System.Convert.ToInt16(RedScore.Text);               //转化当前两队分数信息为int类型以便之后做计算
			int bluescore = System.Convert.ToInt16(BlueScore.Text);

            int index = 0;

            for(; index<plRedList.Count; index++)
            {
                if (text.IndexOf(plRedList[index].PlName) > -1)
                {
                    if (text.IndexOf("一分") > -1)
                    {
                        redscore += 1;
                        RedScore.Text = System.Convert.ToString(redscore);
                        plRedList[index].Score += 1;
                    }
                    else if (text.IndexOf("两分") > -1)
                    {
                        redscore += 2;
                        RedScore.Text = System.Convert.ToString(redscore);
                        plRedList[index].Score += 2;
                    }
                    else if (text.IndexOf("三分") > -1)
                    {
                        redscore += 3;
                        RedScore.Text = System.Convert.ToString(redscore);
                        plRedList[index].Score += 3;
                    }
                    else if(text.IndexOf("犯规") > -1)
                    {
                        plRedList[index].FoulTime += 1;
                    }
                    break;
                }

                if (text.IndexOf(plBlueList[index].PlName) > -1)
                {
                    if (text.IndexOf("一分") > -1)
                    {
                        bluescore += 1;
                        BlueScore.Text = System.Convert.ToString(bluescore);
                        plBlueList[index].Score += 1;
                    }
                    else if (text.IndexOf("两分") > -1)
                    {
                        bluescore += 2;
                        BlueScore.Text = System.Convert.ToString(bluescore);
                        plBlueList[index].Score += 2;
                    }
                    else if (text.IndexOf("三分") > -1)
                    {
                        bluescore += 3;
                        BlueScore.Text = System.Convert.ToString(bluescore);
                        plBlueList[index].Score += 3;
                    }
                    else if(text.IndexOf("犯规") > -1)
                    {
                        plBlueList[index].FoulTime += 1;
                    }
                    break;
                }
            }
		}

        //响应Quit按钮的click事件
		private void Quit_Click(object sender, RoutedEventArgs e)
		{
			App.Current.Shutdown();
		}

        //初始化下，所有数据对象是不可编辑的，但是可以通过以下代码进行编辑针对识别出错的情况
		private void EditRed_Click(object sender, RoutedEventArgs e)
		{
			RedScore.IsHitTestVisible = true;
			RedScore.IsReadOnly = false;
			RedScore.Focus();
			RedScore.SelectAll();
		}

		private void EditBlue_Click(object sender, RoutedEventArgs e)
		{
			BlueScore.IsHitTestVisible = true;
			BlueScore.IsReadOnly = false;
			BlueScore.Focus();
			BlueScore.SelectAll();
		}

        //响应红队分数的失去焦点事件，在对象TextBox失去焦点后关闭编辑功能防止误编辑
		private void RedScore_LostFocus(object sender, RoutedEventArgs e)
		{
			RedScore.IsHitTestVisible = false;
			RedScore.IsReadOnly = true;
		}

		private void BlueScore_LostFocus(object sender, RoutedEventArgs e)
		{
			BlueScore.IsHitTestVisible = false;
			BlueScore.IsReadOnly = true;
		}

        //响应Red按钮的click事件，click之后打印球队信息列表
		private void Red_Click(object sender, RoutedEventArgs e)
		{
            this.Status.Content = "Red";

			this.PlayerList.ItemsSource = plRedList;
			this.PlayerList.DisplayMemberPath = "PlName";

            //绑定队员信息数据到各个TextBox的Text属性上
			this.age.SetBinding(TextBox.TextProperty, new Binding("SelectedItem.Age") { Source = this.PlayerList });
			this.plNum.SetBinding(TextBox.TextProperty, new Binding("SelectedItem.PlNum") { Source = this.PlayerList });
			this.foulTime.SetBinding(TextBox.TextProperty, new Binding("SelectedItem.FoulTime") { Source = this.PlayerList });
			this.score.SetBinding(TextBox.TextProperty, new Binding("SelectedItem.Score") { Source = this.PlayerList });
		}

        private void Blue_Click(object sender, RoutedEventArgs e)
        {
            this.Status.Content = "Blue";
            
            this.PlayerList.ItemsSource = plBlueList;
            this.PlayerList.DisplayMemberPath = "PlName";

            this.age.SetBinding(TextBox.TextProperty, new Binding("SelectedItem.Age") { Source = this.PlayerList });
            this.plNum.SetBinding(TextBox.TextProperty, new Binding("SelectedItem.PlNum") { Source = this.PlayerList });
            this.foulTime.SetBinding(TextBox.TextProperty, new Binding("SelectedItem.FoulTime") { Source = this.PlayerList });
            this.score.SetBinding(TextBox.TextProperty, new Binding("SelectedItem.Score") { Source = this.PlayerList });
        }

        //响应Add按钮的click事件，用于弹出添加球员信息的窗口
        private void Add_Click(object sender, RoutedEventArgs e)
        {
            Window win = new Add(main);
            win.Show();
            main.IsEnabled = false;             //在弹出Add子窗口以后将主窗口的Enable属性设置为false，也就是关闭主窗口的编辑以便出现误编辑
        }

        //响应Delete按钮的click事件，用于直接删除指定球员对象信息
        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (Status.Content.ToString() == "Red")
            {
                plRedList.RemoveAt(PlayerList.SelectedIndex);

                this.PlayerList.ItemsSource = null;
                this.PlayerList.ItemsSource = plRedList;
                this.PlayerList.DisplayMemberPath = "PlName";

                //绑定队员信息数据到各个TextBox的Text属性上
                this.age.SetBinding(TextBox.TextProperty, new Binding("SelectedItem.Age") { Source = this.PlayerList });
                this.plNum.SetBinding(TextBox.TextProperty, new Binding("SelectedItem.PlNum") { Source = this.PlayerList });
                this.foulTime.SetBinding(TextBox.TextProperty, new Binding("SelectedItem.FoulTime") { Source = this.PlayerList });
                this.score.SetBinding(TextBox.TextProperty, new Binding("SelectedItem.Score") { Source = this.PlayerList });
            }
            else if (Status.Content.ToString() == "Blue")
            {
                plBlueList.RemoveAt(PlayerList.SelectedIndex);

                this.PlayerList.ItemsSource = null;
                this.PlayerList.ItemsSource = plBlueList;
                this.PlayerList.DisplayMemberPath = "PlName";

                this.age.SetBinding(TextBox.TextProperty, new Binding("SelectedItem.Age") { Source = this.PlayerList });
                this.plNum.SetBinding(TextBox.TextProperty, new Binding("SelectedItem.PlNum") { Source = this.PlayerList });
                this.foulTime.SetBinding(TextBox.TextProperty, new Binding("SelectedItem.FoulTime") { Source = this.PlayerList });
                this.score.SetBinding(TextBox.TextProperty, new Binding("SelectedItem.Score") { Source = this.PlayerList });
            }
        }

        public List<Player> getRedList()
        {
            return plRedList;
        }

        public List<Player> getBlueList()
        {
            return plBlueList;
        }
	}
}
 