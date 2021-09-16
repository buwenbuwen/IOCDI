using System;
using System.Collections.Generic;
using System.Text;

namespace IOCDI.CustomAttributes
{
    /// <summary>
    /// 常量参数可以传值
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter|AttributeTargets.Property)]
    public class ParameterShortNameContainerAttribute : Attribute
    {
        public string ShortName { get; private set; }
        public ParameterShortNameContainerAttribute(string shortName)
        {
            this.ShortName = shortName;
        }
    }
}
