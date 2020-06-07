using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace MVVMFramework.Commands
{
    /// <summary>
    /// UI元素可视属性的自定义命令
    /// </summary>
    public class VisibilityFrameworkElementCommand:BaseCommand
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public VisibilityFrameworkElementCommand() : base(Visibility) { }

        private static void Visibility(object para)
        {
            if (ReferenceEquals(para, null))
            {
                return;
            }
            if(para is FrameworkElement element)
            {
                if(element.Visibility == System.Windows.Visibility.Visible)
                {
                    element.Visibility = System.Windows.Visibility.Hidden;
                }
                else
                {
                    element.Visibility = System.Windows.Visibility.Visible;
                }
            }
        }
    }
}
