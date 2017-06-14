using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace ClickBlocksClient
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// 运行中的主窗体
        /// </summary>
        internal static MainWindow MWindow { get; set; }
        /// <summary>
        /// 对话框返回的结果
        /// </summary>
        public enum DialogsResult
        {
            确定,
            取消
        }
    }
}
