using Castle.Windsor;

namespace TestFramework.IoC
{
    public class DependencyResolver
    {
        private static IWindsorContainer _container;

        //Initialize the container
        public static void Initialize()
        {
            if (_container == null)
            {                
                _container = new WindsorContainer();
                _container.Register(new ComponentRegistration());
            }            
        }

        //Resolve types
        public static T For<T>()
        {           
            return _container.Resolve<T>();
        }
    }
}
