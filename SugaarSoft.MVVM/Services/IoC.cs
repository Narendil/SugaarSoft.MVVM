using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SugaarSoft.MVVM.Services
{
    public interface IIoCContainer
    {
        T Resolve<T>(bool forceNewInstance = false);
        void RegisterType<T>();
        void RegisterType<T, U>();
        void RegisterInstance<T>(T instance);
    }

    public class IoC : IIoCContainer
    {
        #region Properties

        private readonly Dictionary<Type, Type> RegisteredTypes = new Dictionary<Type, Type>();
        private readonly Dictionary<Type, object> RegisteredObjs = new Dictionary<Type, object>();

        #endregion

        #region Ctor

        private static IoC _instance;

        private IoC() { }

        public static IoC Current
        {
            get
            {
                if (_instance == null)
                    _instance = new IoC();
                return _instance;
            }
        }

        #endregion

        #region IIoCContainer

        public T Resolve<T>(bool forceNewInstance = false)
        {
            return (T)Resolve(typeof(T), forceNewInstance);
        }

        public void RegisterType<T>()
        {
            RegisterType<T, T>();
        }

        public void RegisterType<T, U>()
        {
            if (RegisteredTypes.ContainsKey(typeof(T)))
                RegisteredTypes.Remove(typeof(T));
            RegisteredTypes.Add(typeof(T), typeof(U));
        }

        public void RegisterInstance<T>(T obj)
        {
            RegisterInstance(typeof(T), obj);
        }

        public void RegisterInstance<T, U>(U obj) where U : T
        {
            RegisterInstance(typeof(T), obj);
        }

        private void RegisterInstance(Type type, object obj)
        {
            //UN SEGUNDO REGISTRO MACHACA EL INICIAL
            if (RegisteredObjs.ContainsKey(type))
                RegisteredObjs.Remove(type);
            if (RegisteredTypes.ContainsKey(type))
                RegisteredTypes.Remove(type);
            RegisteredTypes.Add(type, obj.GetType());
            RegisteredObjs.Add(type, obj);
        }

        private object Resolve(Type typeSource, bool forceNewInstance = false)
        {
            var typeToResolve = typeSource;
            if (RegisteredTypes.ContainsKey(typeSource))
                typeToResolve = RegisteredTypes[typeSource];

            if (!forceNewInstance)
                if (RegisteredObjs.ContainsKey(typeToResolve))
                    return RegisteredObjs[typeToResolve];

            var constructors = typeToResolve.GetConstructors();
            var parameterlessConstructors = constructors.Where(ctor => ctor.GetParameters().Count() == 0);
            if (parameterlessConstructors.Count() != 0)
            {
                var obj = parameterlessConstructors.First().Invoke(new object[] { });
                RegisterInstance(typeToResolve, obj);
                return obj;
            }

            foreach (var constructor in constructors)
                try
                {
                    var parametersInfo = constructor.GetParameters();
                    var parameters = new List<object>();
                    foreach (var parameterInfo in parametersInfo)
                        parameters.Add(Resolve(parameterInfo.ParameterType));

                    var obj = constructor.Invoke(parameters.ToArray());
                    RegisterInstance(typeToResolve, obj);
                    return obj;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("EXCEPCIÓN EN LA RESOLUCIÓN DE TIPOS:" + ex.Message);
                }
            throw new Exception(String.Format("No se ha podido generar el tipo {0}", typeSource.Name));
        }

        #endregion
    }
}
