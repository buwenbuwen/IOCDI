using Castle.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace CustomAOP.AOPExtend
{
    public static class IOCAopExtend
    {
        public static object AOP(this object obj,Type type)
        {
            ProxyGenerator generator = new ProxyGenerator();
            CustomInterceptor interceptor = new CustomInterceptor(type);
            return generator.CreateInterfaceProxyWithTarget(type, obj, interceptor);
          }

    }
}
