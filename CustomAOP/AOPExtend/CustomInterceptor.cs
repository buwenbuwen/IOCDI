using Castle.DynamicProxy;
using CustomAOP.AOPAttribute;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Reflection;

namespace CustomAOP.AOPExtend
{
    public class CustomInterceptor : StandardInterceptor
    {
        private readonly Type _type;

        public CustomInterceptor(Type type)
        {
            this._type = type;
        }

        /// <summary>
        /// 调用前执行
        /// </summary>
        /// <param name="invocation"></param>
        protected override void PreProceed(IInvocation invocation)
        {
            //Console.WriteLine("IOCAopExtend   调用前");
           
        }

        /// <summary>
        /// 调用时执行
        /// </summary>
        /// <param name="invocation"></param>
        protected override void PerformProceed(IInvocation invocation)
        {

            var method = invocation.Method;
            Action action = () => base.PerformProceed(invocation);

            if(method.IsDefined(typeof(BaseInterceptorAttribute),true))
            {
                foreach (var attribute in method.GetCustomAttributes<BaseInterceptorAttribute>().ToArray().Reverse())
                {
                    action = attribute.Do(invocation, action);
                }
            }
            if(this._type.IsDefined(typeof(BaseInterceptorAttribute),true))
            {
                foreach (var attribute in this._type.GetCustomAttributes<BaseInterceptorAttribute>().ToArray().Reverse())
                {
                    action = attribute.Do(invocation, action);
                }
            }
            action.Invoke();
  
        }

        /// <summary>
        /// 调用后执行
        /// </summary>
        /// <param name="invocation"></param>
        protected override void PostProceed(IInvocation invocation)
        {
            //Console.WriteLine("IOCAopExtend  调用后");
           
        }
    }
}
