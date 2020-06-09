using MVVMFramework.Configs;
using MVVMFramework.Interfaces;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

namespace MVVMFramework.Models
{
    /// <summary>
    /// ViewModel模型的基类
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
    public abstract class BaseModel : IViewModel
    {
        #region 接口属性
        public string UINameSpace { get; }

        public string UIElementName { get; private set; }

        public FrameworkElement UIElement { get; private set; }
        #endregion

        #region 接口事件
        public event EventHandler<RoutedEventArgs> OnElementLoaded;
        public event EventHandler<RoutedEventArgs> OnElementUnLoaded;
        public event EventHandler<CancelEventArgs> OnElementClosing;
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region 接口方法
        public void Close()
        {
            if (UIElement is Window window)
            {
                window.Close();
            }
            else
            {
                throw new FrameworkException(101, "只有窗体可以使用Close方法！");
            }
        }

        public IAsyncResult Execute(Action executeCode, Action callback)
        {
            return Execute<object>(delegate (object para)
            {
                executeCode?.Invoke();
            }, null, callback);
        }

        public IAsyncResult Execute<T>(Action<T> executeCode, T para, Action callback)
        {
            IAsyncResult result = executeCode?.BeginInvoke(para, delegate (IAsyncResult ar)
             {
                 executeCode?.EndInvoke(ar);
                 Invoke(callback);
             }, null);
            return result;
        }

        public IAsyncResult Execute<T1, T2>(Func<T1, T2> executeCode, T1 para, Action<T2> callback)
        {
            IAsyncResult result = executeCode?.BeginInvoke(para, delegate (IAsyncResult ar)
             {
                 T2 ret = default(T2);
                 if (executeCode != null)
                 {
                     ret = executeCode.EndInvoke(ar);
                 }
                 Invoke<T2>(callback, ret);
             }, null);
            return result;
        }

        public IAsyncResult Execute<T>(Func<T> executeCode, Action<T> callback)
        {
            return Execute<T, T>(delegate (T para)
             {
                 T ret = default(T);
                 if (executeCode != null)
                 {
                     ret = executeCode.Invoke();
                 }
                 return ret;
             }, default(T), callback);
        }

        public void Hide()
        {
            if (UIElement is Window window)
            {
                window.Hide();
            }
            else
            {
                UIElement.Visibility = Visibility.Hidden;
            }
        }

        public void Invoke(Action executeCode)
        {
            Invoke<object>(delegate (object para)
            {
                executeCode?.Invoke();
            }, null);
        }

        public void Invoke<T>(Action<T> executeCode, T para)
        {
            if (UIElement != null)
            {
                UIElement.Dispatcher.BeginInvoke(new Action(delegate ()
                {
                    executeCode?.Invoke(para);
                }), System.Windows.Threading.DispatcherPriority.Normal);
            }
            else
            {
                Application.Current.Dispatcher.BeginInvoke(new Action(delegate ()
                {
                    executeCode?.Invoke(para);
                }), System.Windows.Threading.DispatcherPriority.Normal);
            }
        }

        public void Show()
        {
            if (UIElement is Window window)
            {
                window.Show();
            }
            else
            {
                UIElement.Visibility = Visibility.Visible;
            }
        }

        public bool? ShowDialog()
        {
            if (UIElement is Window window)
            {
                return window.ShowDialog();
            }
            else
            {
                throw new FrameworkException(101, "只有窗体可以使用ShowDialog方法！");
            }
        }

        public virtual void ShowMessage(string message)
        {
            throw new FrameworkException(102, message);
        }

        public void ShowMessage(string message, Exception innerException)
        {
            ShowMessage(string.Format("发生{2}异常:{0}\r\n{1}.", message, innerException.Message, nameof(innerException)));
        }
        #endregion

        /// <summary>
        /// 获取程序集下一个类型的实例
        /// </summary>
        /// <param name="typeName">要获取类型实例的名称</param>
        /// <returns></returns>
        protected Type GetType(string typeName)
        {
            return GetType(UINameSpace, typeName);
        }

        /// <summary>
        /// 获取程序集下一个类型的实例
        /// </summary>
        /// <param name="assemblyName">程序集名称</param>
        /// <param name="typeName">要获取类型实例的名称</param>
        /// <returns></returns>
        protected Type GetType(string assemblyName, string typeName)
        {
            Assembly assembly = Assembly.Load(assemblyName);
            Type result = assembly.GetTypes().FirstOrDefault(p => p.Name.Equals(typeName.Replace(string.Format("{0}.", assemblyName), ""))); //assembly.GetType(typeName, true, false);
            return result;
        }

        /// <summary>
        /// 获取对象的实例
        /// </summary>
        /// <typeparam name="T">对象实例的类型</typeparam>
        /// <returns></returns>
        protected T Get<T>()
        {
            return Get<T>(string.Format("{0}.{1}", UINameSpace, UIElementName));
        }

        /// <summary>
        /// 获取对象的实例
        /// </summary>
        /// <typeparam name="T">对象实例的类型</typeparam>
        /// <param name="elementFullName">要获取对象的全称</param>
        /// <returns></returns>
        protected T Get<T>(string elementFullName)
        {
            Type elementType = GetType(elementFullName);
            T result = default(T);
            result = (T)Activator.CreateInstance(elementType);
            return result;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public BaseModel()
        {
            UINameSpace = GetProjectNameSpace();
            SetUIElement();
            UIElement.DataContext = this;
        }

        /// <summary>
        /// 获取引用此项目的命名空间
        /// </summary>
        /// <returns></returns>
        private string GetProjectNameSpace()
        {
            StackTrace stackTrace = new StackTrace(true);
            //GetFrames[0]为当前，1为调用方法，2为其上上层，依次类推
            //因为我们要获取引用了这个dll的命名空间，所以是当前的调用方法的上层，所以是2
            //之所以要Split一下是因为有些类有可能是放在命名空间下的某个文件夹下的,就会出现命名空间.文件夹这个情况
            //这个情况会导致加载程序集失败,所以要先Split一下,把他去除掉
            return stackTrace.GetFrames()[2].GetMethod().DeclaringType.Namespace.Split('.').FirstOrDefault();
        }

        /// <summary>
        /// 获取并创建WPF的UI元素
        /// </summary>
        private void SetUIElement()
        {
            UIElementName = GetType().Name;
            UIElementName = UIElementName.Replace("Vm_", ""); //去除命名规范的前缀
            UIElementName = UIElementName.Replace("`1", ""); //去除泛型的特定标识
            if (UIElementName.StartsWith("Window"))
            {
                UIElementName = UIElementName.TrimStart("Window".ToCharArray());
                UIElement = Get<Window>();
                (UIElement as Window).Closing += delegate (object sender, CancelEventArgs e)
                {
                    OnElementClosing?.Invoke(sender, e);
                };
            }
            else if (UIElementName.StartsWith("Page"))
            {
                UIElementName = UIElementName.TrimStart("Page".ToCharArray());
                UIElement = Get<Page>();
            }
            else if (UIElementName.StartsWith("UC"))
            {
                UIElementName = UIElementName.TrimStart("UC".ToCharArray());
                UIElement = Get<UserControl>();
            }
            else
            {
                throw new FrameworkException(103, string.Format("元素[{0}]不符合命名规范！", UIElementName));
            }
            UIElement.Loaded += delegate (object sender, RoutedEventArgs e)
            {
                OnElementLoaded?.Invoke(sender, e);
            };
            UIElement.Unloaded += delegate (object sender, RoutedEventArgs e)
            {
                OnElementUnLoaded?.Invoke(sender, e);
            };
        }

        /// <summary>
        /// 通知UI线程某个属性的值被改变了方法
        /// <para>用于更新UI显示</para>
        /// </summary>
        /// <param name="propertyName">改变了的属性的名称，为空时默认调用的方法名或属性名</param>
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
