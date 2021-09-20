using Castle.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace CustomAOP.AOPAttribute
{
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    public class MonitorAttribute : BaseInterceptorAttribute
    {
        public override Action Do(IInvocation invocation, Action action)
        {
            return () =>
            {
                Stopwatch stopwatch = new Stopwatch();
                Console.WriteLine("This is MonitorAttribute 1");
                stopwatch.Start();

                //去执行真实逻辑
                action.Invoke();
                //想做个缓存拦截，觉得自己能搞定刷个1

                stopwatch.Stop();
                Console.WriteLine($"This is MonitorAttribute 2本次方法花费时间{stopwatch.ElapsedMilliseconds}ms");
            };
        }
    }
}
