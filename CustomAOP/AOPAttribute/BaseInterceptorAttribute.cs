using Castle.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Text;

namespace CustomAOP.AOPAttribute
{
    public abstract class BaseInterceptorAttribute:Attribute
    {
        public abstract Action Do(IInvocation invocation, Action action);
    }
}
