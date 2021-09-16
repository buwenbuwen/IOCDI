using System;
using System.Collections.Generic;
using System.Text;

namespace IOCDI.CustomAttributes
{
    [AttributeUsage(AttributeTargets.Constructor| AttributeTargets.Property)]
    public class CustomContainerAttribute : Attribute
    {
    }
}
