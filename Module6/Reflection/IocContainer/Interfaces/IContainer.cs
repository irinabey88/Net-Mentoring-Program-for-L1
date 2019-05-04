using System;
using System.Reflection;

namespace IocContainer.Interfaces
{
    /// <summary>
    /// Represents functionality of <see cref="Container"/>
    /// </summary>
    public interface IContainer
    {
        /// <summary>
        /// Builds dependency for assembly
        /// </summary>
        /// <param name="assembly">An assemble <see cref="Assembly"/></param>
        void AddAssembly(Assembly assembly);

        /// <summary>
        /// Adds dependency for type.
        /// </summary>
        /// <param name="type">A type <see cref="Type"/></param>
        void AddType(Type type);

        /// <summary>
        /// Add type dependency to the container <see cref="Container"/>
        /// </summary>
        /// <param name="key">The key type.</param>
        /// <param name="value">The value type</param>
        void AddType(Type key, Type value);

        /// <summary>
        /// Create the instance of object by the given key type object.
        /// </summary>
        /// <param name="type">The key value type</param>
        /// <returns>Instance of the dependency object</returns>
        object CreateInstance(Type type);

        /// <summary>
        /// Create the instance of object by the given key type object.
        /// </summary>
        /// <typeparam name="T">The key value type</typeparam>
        /// <returns>Instance of the dependency object</returns>
        T CreateInstance<T>();
    }
}