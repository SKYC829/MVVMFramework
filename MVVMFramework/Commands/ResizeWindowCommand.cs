using MVVMFramework.Configs;
using System.Windows;

namespace MVVMFramework.Commands
{
    /// <summary>
    /// 最大化最小化窗体自定义命令
    /// </summary>
    public class ResizeWindowCommand : BaseCommand
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public ResizeWindowCommand() : base(Resize) { }

        private static void Resize(object para)
        {
            if (ReferenceEquals(para, null))
            {
                return;
            }

            if (para is Window window)
            {
                if (window.WindowState == WindowState.Maximized || window.WindowState == WindowState.Minimized)
                {
                    window.WindowState = WindowState.Normal;
                }
                else
                {
                    window.WindowState = WindowState.Maximized;
                }
            }
            else
            {
                throw new FrameworkException(101, "只有窗体可以使用Resize方法！");
            }
        }
    }
}
