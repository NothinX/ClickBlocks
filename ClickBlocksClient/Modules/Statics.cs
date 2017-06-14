using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClickBlocksClient
{
    /// <summary>
    /// Statics模块
    /// </summary>
    static class Statics
    {
        /// <summary>
        /// 运行中的主窗体
        /// </summary>
        internal static MainWindow MWindow { get; set; }
        /// <summary>
        /// 当前登录用户名
        /// </summary>
        internal static string UserName { get; set; }
        /// <summary>
        /// 当前登录用户积分
        /// </summary>
        internal static int UserPoints { get; set; }
        /// <summary>
        /// 对话框返回的结果
        /// </summary>
        internal enum DialogsResult
        {
            确定,
            取消
        }
    }
}
