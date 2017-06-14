using ClickBlocksClient.UI;
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
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static ClickBlocksClient.App;

namespace ClickBlocksClient
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// 对话框是否已经返回结果
        /// </summary>
        private volatile bool isDialogReturn;
        /// <summary>
        /// 对话框返回的结果
        /// </summary>
        private DialogsResult aDialogsResult;
        /// <summary>
        /// 页面回退栈
        /// </summary>
        public Stack<Page> BackPages { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
            MWindow = this;
            isDialogReturn = false;
            aDialogsResult = DialogsResult.确定;
            BackPages = null;
            MainFrame.Content = new MainPage();
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            BackBtnView.Child = PathGrid.GetPathGrid("leftArrowPath", Colors.White);
        }

        #region 标题栏
        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MiniBtn_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void CloseBtn_Click(object sender, RoutedEventArgs e)
        {
            App.Current.Shutdown();
        }
        #endregion

        #region 对话框
        /// <summary>
        /// 显示对话框
        /// </summary>
        /// <returns>对话框返回的结果</returns>
        private async Task<DialogsResult> ShowDialogAsync()
        {
            isDialogReturn = false;
            MainFrame.IsEnabled = false;
            MainFrame.Effect = new BlurEffect() { Radius = 17, RenderingBias = RenderingBias.Performance };
            DialogsGrid.IsEnabled = true;
            DialogsGrid.Visibility = Visibility.Visible;
            MessageOKButton.Focus();
            await FinishDialogAsync();
            return aDialogsResult;
        }
        /// <summary>
        /// 监视对话框是否完成并对界面进行复原
        /// </summary>
        private async Task FinishDialogAsync()
        {
            while (true)
            {
                if (isDialogReturn) break;
                await Task.Delay(50);
            }
            isDialogReturn = false;
            MainFrame.IsEnabled = true;
            MainFrame.Effect = null;
        }
        /// <summary>
        /// 清除所有对话框
        /// </summary>
        public void ClearDialogs()
        {
            MessageGrid.Visibility = Visibility.Hidden;
            MessageCancelButton.Visibility = Visibility.Hidden;
            DialogsGrid.Visibility = Visibility.Hidden;
            DialogsGrid.IsEnabled = false;
            isDialogReturn = false;
            MainFrame.Effect = null;
            MainFrame.IsEnabled = true;
        }
        /// <summary>
        /// 显示消息框
        /// </summary>
        /// <param name="messageText">消息主体</param>
        /// <param name="messageTitle">消息标题</param>
        /// <param name="cancel">是否有取消</param>
        /// <returns>对话框结果</returns>
        public async Task<DialogsResult> ShowMessage(string messageText, string messageTitle = "", bool cancel = false)
        {
            if (cancel) MessageCancelButton.Visibility = Visibility.Visible;
            MessageGrid.Visibility = Visibility.Visible;
            MessageTitleTextBlock.Text = messageTitle;
            MessageTextTextBlock.Text = messageText;
            return await ShowDialogAsync();
        }

        private void MessageCancel_Click(object sender, RoutedEventArgs e)
        {
            aDialogsResult = DialogsResult.取消;
            isDialogReturn = true;
            DialogsGrid.IsEnabled = false;
            DialogsGrid.Visibility = Visibility.Hidden;
            MessageGrid.Visibility = Visibility.Hidden;
            MessageCancelButton.Visibility = Visibility.Hidden;
        }

        private void MessageOK_Click(object sender, RoutedEventArgs e)
        {
            aDialogsResult = DialogsResult.确定;
            isDialogReturn = true;
            DialogsGrid.IsEnabled = false;
            DialogsGrid.Visibility = Visibility.Hidden;
            MessageGrid.Visibility = Visibility.Hidden;
            MessageCancelButton.Visibility = Visibility.Hidden;
        }

        private void MessageGrid_KeyDown(object sender, KeyEventArgs e)
        {
            if ((Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl)) && Keyboard.IsKeyDown(Key.C))
            {
                Clipboard.SetDataObject(MessageTextTextBlock.Text);
            }
        }
        #endregion
    }
}
