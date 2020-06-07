using System;
using System.Windows.Input;

namespace MVVMFramework.Commands
{
    /// <summary>
    /// 自定义命令的基类
    /// </summary>
    public class BaseCommand : ICommand
    {
        private Action<object> executeCode;
        /// <summary>
        /// 是否可以执行属性改变事件
        /// </summary>
        public event EventHandler CanExecuteChanged;

        /// <summary>
        /// 判断参数是否满足执行条件
        /// </summary>
        /// <param name="parameter">参数</param>
        /// <returns></returns>
        public virtual bool CanExecute(object parameter)
        {
            return true;
        }

        /// <summary>
        /// 执行自定义命令
        /// </summary>
        /// <param name="parameter"></param>
        public virtual void Execute(object parameter)
        {
            if (CanExecute(parameter))
            {
                executeCode?.Invoke(parameter);
            }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="executeCode">命令要执行的委托方法</param>
        public BaseCommand(Action<object> executeCode)
        {
            this.executeCode = executeCode;
        }
    }
}
