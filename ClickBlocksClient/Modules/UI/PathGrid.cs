using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace ClickBlocksClient.UI
{
    /// <summary>
    /// PathGrid模块
    /// </summary>
    class PathGrid
    {
        /// <summary>
        /// 获得资源字典中用Grid包裹的路径集
        /// </summary>
        /// <param name="name">路径的名称</param>
        /// <param name="color">路径填充的颜色</param>
        /// <param name="isThinkness">是否要调整路径的位置</param>
        /// <returns>处理好的路径</returns>
        public static Grid GetPathGrid(string name, Color color, bool isThinkness = false)
        {
            if (name == "") return null;
            var dictionary = new ResourceDictionary()
            {
                Source = new Uri("/Dictionarys/pathDictionary.xaml", UriKind.Relative)
            };
            Grid grid = (Grid)dictionary[name];
            if (isThinkness)
            {
                grid.Margin = GetThickness(name);
            }
            foreach (var item in grid.Children)
            {
                if (item is Path)
                {
                    (item as Path).Fill = new SolidColorBrush(color);
                }
            }
            return grid;
        }
        /// <summary>
        /// 获得资源字典中用Grid包裹的路径集，根据加载的地方调整路径集的位置
        /// </summary>
        /// <param name="name">路径的名称</param>
        /// <param name="color">路径填充的颜色</param>
        /// <param name="where">路径要加载的地方</param>
        /// <returns>处理好的路径</returns>
        public static Grid GetPathGrid(string name, Color color, string where)
        {
            var dictionary = new ResourceDictionary()
            {
                Source = new Uri("/Dictionarys/pathDictionary.xaml", UriKind.Relative)
            };
            Grid grid = (Grid)dictionary[name];
            grid.Margin = GetThickness(name + "_" + where);
            foreach (var item in grid.Children)
            {
                if (item is Path)
                {
                    (item as Path).Fill = new SolidColorBrush(color);
                }
            }
            return grid;
        }
        /// <summary>
        /// 获得一个Thickness
        /// </summary>
        /// <param name="name">Thinkness在Resources里面的名称</param>
        /// <returns>转换后的Thinkness</returns>
        public static Thickness GetThickness(string name)
        {
            string[] doubles = R.GetString(name).Split('#');
            return new Thickness(double.Parse(doubles[0]), double.Parse(doubles[1]), double.Parse(doubles[2]), double.Parse(doubles[3]));
        }
    }
}
