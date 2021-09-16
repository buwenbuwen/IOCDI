using IOCDI.CustomAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace IOCDI.Container
{
    public class CustomContainer : ICustomContainer
    {

        private Dictionary<string, CustomContainerModel> _dict = new Dictionary<string, CustomContainerModel>();

        private Dictionary<string, object[]> _valueDict = new Dictionary<string, object[]>();

        /// <summary>
        /// 作用域单例
        /// </summary>
        private Dictionary<string, object> _scopeDict = new Dictionary<string, object>();

        //设置统一的key
        private string GetKey(string FullName, string shortName) => $"{FullName}_{shortName}";

        public CustomContainer()
        {

        }

        private CustomContainer(Dictionary<string, CustomContainerModel> dict, 
            Dictionary<string, object[]> valueDict, 
            Dictionary<string, object> scopeDict)
        {
            this._dict = dict;
            this._valueDict = valueDict;
            this._scopeDict = scopeDict;
        }
  

        public ICustomContainer CreateScope()
        {
            // 要new一个新的
            return new CustomContainer(this._dict, this._valueDict, new Dictionary<string, object>());
        }

        //注入到字典里
        public void Register<TFrom, TTo>(string shortName = null, object[] paraList = null, LifetimeType lifetimeType = LifetimeType.Transient) where TTo : TFrom
        {
            this._dict.Add(GetKey(typeof(TFrom).FullName, shortName), new CustomContainerModel
            {
                Lifetime = lifetimeType,
                TargetType = typeof(TTo)
            });

            if(paraList!=null && paraList.Length > 0)
            {
                this._valueDict.Add(GetKey(typeof(TFrom).FullName,shortName), paraList);
            }
        }

        //生成对象
        public TFrom Resolve<TFrom>(string shortName = null)
        {

            return (TFrom)this.ResolveObject(typeof(TFrom), shortName);
        }

        //无线层级获取
        private object ResolveObject(Type abstractType, string shortName = null)
        {
            string key = this.GetKey(abstractType.FullName, shortName);
            CustomContainerModel model = this._dict[key];


            //LifetimeType
            switch (model.Lifetime)
            {
                case LifetimeType.Transient:
                    Console.WriteLine("Transient 开始");
                    break;
                case LifetimeType.Singleton:

                    Console.WriteLine("Singleton 开始");

                    if (model.SingletonInstance != null)
                    {
                        return model.SingletonInstance;
                    }
                    break;
                case LifetimeType.Scope:
                    Console.WriteLine("Scope  开始");
                    if(this._scopeDict.ContainsKey(key))
                    {
                        return this._scopeDict[key];
                    }
                    break;
                default:
                    break;
            }

            Type type = model.TargetType;

            //优先用特性
            ConstructorInfo ctor = type.GetConstructors().FirstOrDefault(p => p.IsDefined(typeof(CustomContainerAttribute), true));
            if(ctor == null)
            {
                //获取参数最多参数的构造函数
                ctor = type.GetConstructors().OrderByDescending(p => p.GetParameters().Length).First();
            }


            List<object> paraList = new List<object>();//构造函数参数
            object[] paraConstant = this._valueDict.ContainsKey(key) ? this._valueDict[key] : null;//常量
            int index = 0;

            //获取所有参数
            foreach (var para in ctor.GetParameters())
            {

                if(para.IsDefined(typeof(ParameterContainerAttribute),true))
                {
                    paraList.Add(paraConstant[index++]);
                }
                else
                {
                    Type paraType = para.ParameterType;//获取参数类型
                    string paraShortName = this.GetShortName(para); //获取多个实现的名字
                    object paraInstance = ResolveObject(paraType, paraShortName);//开始递归一直递归到没有参数的类型

                    paraList.Add(paraInstance);
                }  
            }

            object oInstance = Activator.CreateInstance(type, paraList.ToArray());

            #region 属性注入
            foreach(var prop in type.GetProperties().Where(p=>p.IsDefined(typeof(CustomContainerAttribute),true)))
            {
                Type propType = prop.PropertyType;
                string paraShortName = GetShortName(prop);
                object propInstance = ResolveObject(propType, paraShortName);
                prop.SetValue(oInstance, propInstance);
            }
            #endregion


            switch (model.Lifetime)
            {
                case LifetimeType.Transient:
                    Console.WriteLine("Transient  结束");
                    break;
                case LifetimeType.Singleton:

                    Console.WriteLine("Singleton  结束");

                    model.SingletonInstance = oInstance;
                    break;
                case LifetimeType.Scope:

                    Console.WriteLine("Scope  结束");

                    this._scopeDict[key] = oInstance;
                    break;
                default:
                    break;
            }

            return oInstance;
        }

        /// <summary>
        /// 获取对应多个实现的名字（标记属性的特性和标记参数的特性合并）  
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        private string GetShortName(ICustomAttributeProvider provider)
        {
            if (provider.IsDefined(typeof(ParameterShortNameContainerAttribute), true))
            {

                var attribute = (ParameterShortNameContainerAttribute)provider.GetCustomAttributes(typeof(ParameterShortNameContainerAttribute), true)[0];

                return attribute.ShortName;
            }
            return null;
        }
        //private string GetShortName(ParameterInfo info)
        //{
        //    if(info.IsDefined(typeof(ParameterShortNameContainerAttribute),true))
        //    {
        //        return info.GetCustomAttribute<ParameterShortNameContainerAttribute>().ShortName;
        //    }
        //    return null;
        //}
        //private string GetShortName(PropertyInfo info)
        //{
        //    if (info.IsDefined(typeof(ParameterShortNameContainerAttribute), true))
        //    {
        //        return info.GetCustomAttribute<ParameterShortNameContainerAttribute>().ShortName;
        //    }
        //    return null;
        //}


        public void RegisterType(Type typeFrom, Type typeTo, LifetimeType lifetimeType = LifetimeType.Transient)
        {
            throw new NotImplementedException();
        }
        public object Resolve(Type type)
        {
            throw new NotImplementedException();
        }
    }
}
