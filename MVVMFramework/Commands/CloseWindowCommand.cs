using MVVMFramework.Configs;
using System.Windows;

namespace MVVMFramework.Commands
{
    /// <summary>
    /// 关闭窗口自定义命令
    /// </summary>
    public class CloseWindowCommand : BaseCommand
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public CloseWindowCommand() : base(Close)
        {

        }

        /// <summary>
        /// 关闭命令，传入窗体
        /// </summary>
        /// <param name="para">要关闭的窗体</param>
        private static void Close(object para)
        {
            if (para is Window window)
            {
                window?.Close();
            }
            else
            {
                throw new FrameworkException(101, "只有窗体可以使用Close方法！");
            }
        }
    }
}
