using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Attributes;
using Exceptions.Exceptions;
using IocContainer.Interfaces;

namespace IocContainer
{
    /// <summary>
    /// Represents an object of <see cref="Container"/>
    /// </summary>
    public class Container : IContainer
    {
        private readonly Dictionary<Type, Type> _container = new Dictionary<Type, Type>();
        private readonly IObjectCreator _objectCreator;

        /// <summary>
        /// Initialise an instance of <see cref="Container"/>
        /// </summary>
        /// <param name="objectCreator">A creator object.</param>
        public Container(IObjectCreator objectCreator)
        {
            _objectCreator = objectCreator ?? throw new ArgumentNullException(nameof(objectCreator));
        }

        #region Interface methods

        public void AddAssembly(Assembly assembly)
        {
            if (assembly == null)
            {
                throw new ArgumentNullException(nameof(assembly));
            }

            var loadedTypes = assembly.GetExportedTypes()
                .Where(type => IsTypeHasExportAttribute(type)
                               || IsTypeHasImportsAttributesProperties(type)
                               || IsTypeHasImportConstructorAttribute(type))
                               .ToList();

            foreach (var type in loadedTypes)
            {

                if (IsTypeHasExportAttribute(type))
                {
                    var attribute = (ExportAttribute)type.GetCustomAttribute(typeof(ExportAttribute));
                    if (attribute.Type != null)
                    {
                        AddType(attribute.Type, type);
                    }
                    else
                    {
                        AddType(type);
                    }
                }
                else
                {
                    AddType(type);
                }
            }
        }

        public void AddType(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            if (!type.IsClass)
            {
                throw new AddNotClassObjectException();
            }

            if (!_container.ContainsKey(type))
            {
                _container.Add(type, type);
            }
        }

        public void AddType(Type key, Type value)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            if (!key.IsInterface)
            {
                throw new AddNotInterfaceDependencyException();
            }

            if (!value.IsClass)
            {
                throw new AddNotClassObjectException();
            }

            if (!_container.ContainsKey(key))
            {
                _container.Add(key, value);
            }
            
       }

        public object CreateInstance(Type type)
        {
            return _objectCreator.CreateObject(type, _container);
        }

        public T CreateInstance<T>()
        {
            return _objectCreator.CreateObject<T>(_container);
        }

        #endregion

        #region Private methods

        private bool IsTypeHasExportAttribute(Type type)
        {
            return type.GetCustomAttribute(typeof(ExportAttribute)) != null;
        }

        private bool IsTypeHasImportConstructorAttribute(Type type)
        {
            return type.GetCustomAttribute(typeof(ImportConstructorAttribute)) != null;
        }

        private bool IsTypeHasImportsAttributesProperties(Type type)
        {
            return type.GetProperties().Any(property => property.GetCustomAttribute(typeof(ImportAttribute)) != null);
        }

        #endregion
    }
}