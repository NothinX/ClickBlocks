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
    /// LoginPage.xaml 的交互逻辑
    /// </summary>
    public partial class LoginPage : Page
    {
        public LoginPage(bool isRegister=false)
        {
            InitializeComponent();
            if (isRegister)
            {
                Title = "注册";
            }
            else
            {
                Title = "登录";
            }
            Loaded += LoginPage_Loaded;
        }

        private void LoginPage_Loaded(object sender, RoutedEventArgs e)
        {
            MWindow.Title = Title;
            LoginBtn.Content = Title;
        }

        private async void LoginBtn_Click(object sender, RoutedEventArgs e)
        {
            if (Title=="登录")
            {
                await LoginingAsync();
            } 
            else if (Title == "注册")
            {
                await RegisteringAsync();
            }
        }

        private async Task RegisteringAsync()
        {
            if (UsernameText.Text == "")
            {
                await MWindow.ShowMessage("请输入账号！", "错误");
                VUsernameText.Focus();
            }
            else if (PasswordText.Password == "")
            {
                await MWindow.ShowMessage("请输入密码！", "错误");
                VPasswordText.Focus();
            }
            else
            {
                UserName = UsernameText.Text;
                MWindow.NewPage(new MainPage());
            }
        }

        private async Task LoginingAsync()
        {
            if (UsernameText.Text == "")
            {
                await MWindow.ShowMessage("请输入账号！", "错误");
                VUsernameText.Focus();
            }
            else if (PasswordText.Password == "")
            {
                await MWindow.ShowMessage("请输入密码！", "错误");
                VPasswordText.Focus();
            }
            else
            {
                UserName = UsernameText.Text;
                MWindow.NewPage(new MainPage());
            }
        }

        #region 账户密码输入框交互逻辑，实现输入提示，密码框回车即登录
        private void VUsernameText_GotFocus(object sender, RoutedEventArgs e)
        {
            VUsernameText.Visibility = Visibility.Hidden;
            VUsernameText.IsEnabled = false;
            UsernameText.Visibility = Visibility.Visible;
            UsernameText.Focus();
        }

        private void VPasswordText_GotFocus(object sender, RoutedEventArgs e)
        {
            VPasswordText.Visibility = Visibility.Hidden;
            VPasswordText.IsEnabled = false;
            PasswordText.Visibility = Visibility.Visible;
            PasswordText.Focus();
        }

        private void UsernameText_LostFocus(object sender, RoutedEventArgs e)
        {
            if (UsernameText.Text == "")
            {
                VUsernameText.IsEnabled = true;
                VUsernameText.Visibility = Visibility.Visible;
                UsernameText.Visibility = Visibility.Hidden;
            }
        }

        private void PasswordText_LostFocus(object sender, RoutedEventArgs e)
        {
            if (PasswordText.Password == "")
            {
                VPasswordText.IsEnabled = true;
                VPasswordText.Visibility = Visibility.Visible;
                PasswordText.Visibility = Visibility.Hidden;
            }
        }

        private async void PasswordText_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                await LoginingAsync();
            }
        }
        #endregion

    }
}
