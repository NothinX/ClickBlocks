﻿using System;
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
        private string gameMode;
        public PlayPage(string gameMode)
        {
            InitializeComponent();
            this.gameMode = gameMode;
            GameModeLabel.Content = gameMode;
            Loaded += PlayPage_Loaded;
        }

        private void PlayPage_Loaded(object sender, RoutedEventArgs e)
        {
            MWindow.Title = Title;
        }
    }
}
