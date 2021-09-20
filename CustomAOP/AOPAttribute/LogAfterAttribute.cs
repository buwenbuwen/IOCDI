using Castle.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Text;

namespace CustomAOP.AOPAttribute
{
    public class LogAfterAttribute : BaseInterceptorAttribute
    {
        public override Action Do(IInvocation invocation, Action action)
        {
            return () =>
            {
                Console.WriteLine($"This is LogAfterAttribute1  {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff")}");
                //去执行真实逻辑
                action.Invoke();
                Console.WriteLine($"This is LogAfterAttribute2  {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff")}");
            };
        }
    }
}
