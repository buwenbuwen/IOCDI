using IOCDI.Container;
using IOCDI.CustomAttributes;
using System;

namespace App.Test
{
    class Program
    {
        static void Main(string[] args)
        {

            ICustomContainer container = new CustomContainer();
            //container.Register<IPerson, Person>(lifetimeType: LifetimeType.Scope);
            //container.Register<IStudent, Student>();
            //container.Register<IDepartment, Department>();
            //container.Register<IPerson, Person2>(shortName: "Mysql", paraList: new object[] { "小明", 45 });
            container.Register<IService, Service>(lifetimeType: LifetimeType.Scope);

            //var t = container.Resolve<IPerson>();
            //t.Say();
            //var tt = container.Resolve<IPerson>("Mysql");
            //tt.Say();


            var ner = container.CreateScope();
            var n1 = ner.Resolve<IService>();
            var n2 = ner.Resolve<IService>();

            var wer = container.CreateScope();
            var w1 = wer.Resolve<IService>();
            var w2 = wer.Resolve<IService>();

            Console.WriteLine(object.ReferenceEquals(n1, n2));
            Console.WriteLine(object.ReferenceEquals(w1, w2));
            Console.WriteLine(object.ReferenceEquals(n1, w1));
            Console.WriteLine(object.ReferenceEquals(n1, w2));
            Console.WriteLine(object.ReferenceEquals(n2, w1));
            Console.WriteLine(object.ReferenceEquals(n2, w2));
            Console.ReadKey();


        }
    }

    public interface IService
    {

    }
    public class Service : IService
    {

    }



    public interface IPerson
    {
        void Say();
    }
    public class Person2 : IPerson
    {
        private readonly IDepartment _sdt;

        public Person2(IDepartment sdt,[ParameterContainerAttribute] string s,[ParameterContainerAttribute] int i)
        {
            Console.WriteLine("----------------------------------------");
            Console.WriteLine($"{s}:{i}");
            this._sdt = sdt;
            
        }
        public void Say()
        {
            Console.WriteLine("我是Person2");
            _sdt.Hi();
        }
    }
    public class Person : IPerson
    {
        private readonly IStudent _sdt;
        private readonly IDepartment _department;

        [CustomContainerAttribute]
        public IStudent student { get; set; }

       
        public Person(IStudent sdt)
        {
            this._sdt = sdt;
        }
        public Person(IStudent sdt, IDepartment department)
        {
            this._sdt = sdt;
            this._department = department;
        }
        public void Say()
        {
            //student.Hello();
            //_sdt.Hello();
            //_department.Hi();
            //Console.WriteLine("Hello Word!");
        }
    }
    public interface IStudent
    {
        void Hello();
    }
    public class Student : IStudent
    {
        private readonly IPerson _p;

        public Student([ParameterShortNameContainer("Mysql")]IPerson p)
        {
            this._p = p;
        }


        public void Hello()
        {
            
            Console.WriteLine("学生");
            _p.Say();
        }
    }
    public interface IDepartment
    {
        void Hi();
    }
    public class Department : IDepartment
    {
        public void Hi()
        {
            Console.WriteLine("我是部门");
        }
    }
}
