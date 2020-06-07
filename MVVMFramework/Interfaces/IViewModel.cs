using System;
using System.ComponentModel;
using System.Windows;

namespace MVVMFramework.Interfaces
{
    /// <summary>
    /// ViewModel模型的接口规范
    /// <para>所有ViewModel必须遵循以下规范</para>
    /// <list type="table">
    /// <item>
    /// <term>Window窗体</term>
    /// <description>Vm_Window+窗体名</description>
    /// </item>
    /// <item>
    /// <term>Page页面</term>
    /// <description>Vm_Page+页面名</description>
    /// </item>
    /// <item>
    /// <term>UserControl用户控件</term>
    /// <description>Vm_UC+控件名</description>
    /// </item>
    /// </list>
    /// <para>所有规范标准大小写必须一致</para>
    /// </summary>
    public interface IViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// UI元素加载完成事件
        /// </summary>
        event EventHandler<RoutedEventArgs> OnElementLoaded;

        /// <summary>
        /// UI元素移除完成事件
        /// </summary>
        event EventHandler<RoutedEventArgs> OnElementUnLoaded;

        /// <summary>
        /// UI元素关闭事件
        /// </summary>
        event EventHandler<CancelEventArgs> OnElementClosing;

        /// <summary>
        /// UI元素命名空间
        /// <para>用于反射创建UI元素用</para>
        /// </summary>
        string UINameSpace { get; }

        /// <summary>
        /// UI元素名称
        /// <para>用于反射创建UI元素用</para>
        /// </summary>
        string UIElementName { get; }

        /// <summary>
        /// UI元素的引用
        /// </summary>
        FrameworkElement UIElement { get; }

        /// <summary>
        /// 显示UI元素
        /// </summary>
        void Show();

        /// <summary>
        /// 以模态窗体的形式显示UI元素
        /// </summary>
        /// <returns></returns>
        bool? ShowDialog();

        /// <summary>
        /// 关闭UI元素
        /// </summary>
        void Close();

        /// <summary>
        /// 隐藏UI元素
        /// </summary>
        void Hide();

        /// <summary>
        /// 显示消息
        /// </summary>
        /// <param name="message">消息内容</param>
        void ShowMessage(string message);

        /// <summary>
        /// 显示消息
        /// </summary>
        /// <param name="message">消息内容</param>
        /// <param name="innerException">错误信息</param>
        void ShowMessage(string message, Exception innerException);

        /// <summary>
        /// 交换到线程来按线程优先级执行一个委托方法
        /// </summary>
        /// <param name="executeCode">委托方法</param>
        void Invoke(Action executeCode);

        /// <summary>
        /// 交换到线程来按线程优先级执行一个委托方法
        /// </summary>
        /// <typeparam name="T">委托方法参数的类型</typeparam>
        /// <param name="executeCode">委托方法</param>
        /// <param name="para">委托方法的参数</param>
        void Invoke<T>(Action<T> executeCode, T para);

        /// <summary>
        /// 异步执行一个委托方法,并在完成时执行回掉函数
        /// </summary>
        /// <param name="executeCode">委托方法</param>
        /// <param name="callback">回掉函数</param>
        /// <returns></returns>
        IAsyncResult Execute(Action executeCode, Action callback);

        /// <summary>
        /// 异步执行一个委托方法,并在完成时执行回掉函数
        /// </summary>
        /// <typeparam name="T">委托方法的参数的类型</typeparam>
        /// <param name="executeCode">委托方法</param>
        /// <param name="para">委托方法的参数</param>
        /// <param name="callback">回掉函数</param>
        /// <returns></returns>
        IAsyncResult Execute<T>(Action<T> executeCode, T para, Action callback);

        /// <summary>
        /// 异步执行一个委托方法,并在完成时执行回掉函数
        /// </summary>
        /// <typeparam name="T1">委托方法的参数的类型</typeparam>
        /// <typeparam name="T2">回调函数的参数的类型</typeparam>
        /// <param name="executeCode">委托方法</param>
        /// <param name="para">委托方法的参数</param>
        /// <param name="callback">回掉函数</param>
        /// <returns></returns>
        IAsyncResult Execute<T1, T2>(Func<T1, T2> executeCode, T1 para, Action<T2> callback);

        /// <summary>
        /// 异步执行一个委托方法,并在完成时执行回掉函数
        /// </summary>
        /// <typeparam name="T">回调函数的参数的类型</typeparam>
        /// <param name="executeCode">委托方法</param>
        /// <param name="callback">回掉函数</param>
        /// <returns></returns>
        IAsyncResult Execute<T>(Func<T> executeCode, Action<T> callback);
    }
}
