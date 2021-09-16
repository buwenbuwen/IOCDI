using System;
using System.Collections.Generic;
using System.Text;

namespace IOCDI.Container
{
    public class CustomContainerModel
    {
        //生命周期
        public LifetimeType Lifetime { get; set; }

        //要构造出来的类型
        public Type TargetType { get; set; }

        //保存单例
        public object SingletonInstance { get; set; }
    }
}
