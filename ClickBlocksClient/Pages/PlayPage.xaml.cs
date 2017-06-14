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
    /// PlayPage.xaml 的交互逻辑
    /// </summary>
    public partial class PlayPage : Page
    {
        ClickBlocksGame game;
        private string gameMode;
        public PlayPage(string gameMode)
        {
            InitializeComponent();
            this.gameMode = gameMode;
            GameModeLabel.Content = gameMode;
            Loaded += PlayPage_Loaded;
            Unloaded += PlayPage_Unloaded;
        }

        private void PlayPage_Unloaded(object sender, RoutedEventArgs e)
        {
            InputMethod.SetIsInputMethodEnabled(MWindow, true);
        }

        private async void PlayPage_Loaded(object sender, RoutedEventArgs e)
        {
            MWindow.Title = Title;
            game = new ClickBlocksGame();
            PlayBorder.Child = game.Playground;
            (PlayBorder.Child as Grid).Focus();
            InputMethod.SetIsInputMethodEnabled(MWindow, false);
            while (true)
            {
                await GameStart();
                if ((await MWindow.ShowMessage(string.Format("本次得分是{0}\n是否再次挑战？", game.Score), "游戏结束", true) == DialogsResult.确定))
                {
                    game.ReStart(true);
                }
                else
                {
                    MWindow.NewPage(new MainPage());
                    break;
                }
            }
        }

        private async Task GameStart()
        {
            while (!game.IsStarted)
            {
                await Task.Delay(1);
            }
            GameInfo.Visibility = Visibility.Collapsed;
            await game.Start();
        }
    }
}
