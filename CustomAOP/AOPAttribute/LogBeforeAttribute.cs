using Castle.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Text;

namespace CustomAOP.AOPAttribute
{
    public class LogBeforeAttribute : BaseInterceptorAttribute
    {
        public override Action Do(IInvocation invocation, Action action)
        {
            return () =>
            {
                Console.WriteLine($"This is LogBeforeAttribute1 {invocation.Method.Name} {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff")}");
                //去执行真实逻辑
                action.Invoke();
                //写个日志---参数检查--能做的事儿已经很多了
                Console.WriteLine($"This is LogBeforeAttribute2 {invocation.Method.Name} {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff")}");
            };
        }
    }
}
