using System;
using System.Reflection;

namespace IocContainerReflectionEmit.Interfaces
{
    public interface IContainer
    {
        void AddAssembly(Assembly assembly);

        void AddType(Type type);

        void AddType(Type key, Type value);

        object CreateInstance(Type type);

        T CreateInstance<T>();
    }
}