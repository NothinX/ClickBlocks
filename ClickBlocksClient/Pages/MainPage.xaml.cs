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
using static ClickBlocksClient.Statics;

namespace ClickBlocksClient
{
    /// <summary>
    /// MainPage.xaml 的交互逻辑
    /// </summary>
    public partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
            Loaded += MainPage_Loaded;
        }

        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            MWindow.Title = Title;
            if (UserName!=null)
            {
                UserNameText.Text = UserName;
                AfterLoginGrid.Visibility = Visibility.Visible;
                BeforeLoginGrid.Visibility = Visibility.Collapsed;
            }
            else
            {
                AfterLoginGrid.Visibility = Visibility.Collapsed;
                BeforeLoginGrid.Visibility = Visibility.Visible;
            }
        }

        private void LoginBtn_Click(object sender, RoutedEventArgs e)
        {
            MWindow.NewPage(new LoginPage());
        }

        private void RegisterBtn_Click(object sender, RoutedEventArgs e)
        {
            MWindow.NewPage(new LoginPage(true));
        }

        private void PlayBtn_Click(object sender, RoutedEventArgs e)
        {
            MWindow.NewPage(new PlayPage());
        }

        private void RankBtn_Click(object sender, RoutedEventArgs e)
        {
            MWindow.NewPage(new RankPage());
        }
    }
}
