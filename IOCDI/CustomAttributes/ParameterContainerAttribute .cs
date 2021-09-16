using System;
using System.Collections.Generic;
using System.Text;

namespace IOCDI.CustomAttributes
{
    /// <summary>
    /// 常量参数可以传值
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter)]
    public class ParameterContainerAttribute : Attribute
    {
    }
}
