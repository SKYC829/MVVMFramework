using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace MVVMFramework.Configs
{
    /// <summary>
    /// 自定义异常信息
    /// </summary>
    public class FrameworkException : Exception, ISerializable
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public FrameworkException()
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="message">异常信息</param>
        public FrameworkException(string message) : base(message)
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="errorCode">异常错误码</param>
        /// <param name="message">异常信息</param>
        public FrameworkException(int errorCode,string message) : base(message)
        {
            HResult = errorCode;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="message">异常信息</param>
        /// <param name="innerException">内部异常信息</param>
        public FrameworkException(string message, Exception innerException) : base(message, innerException)
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="errorCode">异常错误码</param>
        /// <param name="message">异常信息</param>
        /// <param name="paras">异常信息参数</param>
        public FrameworkException(int errorCode,string message,params object[] paras):this(errorCode,string.Format(message,paras))
        {            
        }

        /// <summary>
        /// 自定义序列化信息的构造函数
        /// </summary>
        /// <param name="info">序列化信息</param>
        /// <param name="context">序列化数据流的上下文信息</param>
        protected FrameworkException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        /// <summary>
        /// 自定义序列化信息的获取属性的值的方法
        /// </summary>
        /// <param name="info">序列化信息</param>
        /// <param name="context">序列化数据流的上下文信息</param>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }

        /// <summary>
        /// 静态创建异常信息的方法
        /// </summary>
        /// <param name="errorCode">异常错误码</param>
        /// <returns></returns>
        public static FrameworkException Create(int errorCode)
        {
            return Create(errorCode, string.Empty);
        }

        /// <summary>
        /// 静态创建异常信息的方法
        /// </summary>
        /// <param name="errorCode">异常错误码</param>
        /// <param name="message">异常信息</param>
        /// <returns></returns>
        public static FrameworkException Create(int errorCode,string message)
        {
            return Create(errorCode, message, null);
        }

        /// <summary>
        /// 静态创建异常信息的方法
        /// </summary>
        /// <param name="errorCode">异常错误码</param>
        /// <param name="message">异常信息</param>
        /// <param name="paras">异常信息的参数</param>
        /// <returns></returns>
        public static FrameworkException Create(int errorCode,string message,params object[] paras)
        {
            return new FrameworkException(errorCode, message, paras);
        }
    }
}
