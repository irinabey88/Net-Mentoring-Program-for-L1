using System;
using System.Collections.Generic;

namespace IocContainer.Interfaces
{
    /// <summary>
    /// Represents an interfacew <see cref="IObjectCreator"/>
    /// </summary>
    public interface IObjectCreator
    {
        /// <summary>
        /// Create the instance of object by the given key type object.
        /// </summary>
        /// <param name="type">The key value type</param>
        /// <param name="container">The container</param>
        /// <returns>Instance of the dependency object</returns>
        object CreateObject(Type type, Dictionary<Type, Type> container);

        /// <summary>
        /// Create the instance of object by the given key type object.
        /// </summary>
        /// <typeparam name="T">The key value type</typeparam>
        /// <param name="container">The container</param>
        /// <returns>Instance of the dependency object</returns>
        T CreateObject<T>(Dictionary<Type, Type> container);
    }
}