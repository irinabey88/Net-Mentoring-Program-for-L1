using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Attributes;
using Exceptions.Exceptions;
using IocContainer.Interfaces;

namespace ReflectionObjectCreator
{
    public class ReflectionActivator : IObjectCreator
    {
        #region Interface methods

        public object CreateObject(Type type, Dictionary<Type, Type> container)
        {
            if (type == null)
            {
                throw new ArgumentNullException();
            }

            if (container == null)
            {
                throw new ArgumentNullException(nameof(container));
            }

            if (IsTypeHasExportAttribute(type))
            {
                return CreateExportedObject(type, container);
            }

            if (IsTypeHasImportsAttributesProperties(type))
            {
                return CreateObjectWithImportedProperties(type, container);
            }

            if (IsTypeHasImportConstructorAttribute(type))
            {
                return CreateObjectWithImportedConstructor(type, container);
            }

            if (container.ContainsKey(type))
            {
                return Activator.CreateInstance(container[type]);
            }

            return null;
        }

        public T CreateObject<T>(Dictionary<Type, Type> container)
        {
            var type = typeof(T);
            return (T)CreateObject(type, container);
        }

        #endregion

        #region Private methods

        private bool IsTypeHasImporAttribute(PropertyInfo type)
        {
            return type.GetCustomAttribute(typeof(ImportAttribute)) != null;
        }

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


        private object CreateExportedObject(Type type, Dictionary<Type, Type> container)
        {
            return CreateParamByType(type, container);
        }

        private object CreateObjectWithImportedProperties(Type type, Dictionary<Type, Type> container)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            if (container == null)
            {
                throw new ArgumentNullException(nameof(container));
            }

            if (!container.ContainsKey(type))
            {
                throw new UnknownDepencyException();
            }

            var resultInstance = Activator.CreateInstance(container[type]);
            var objectProperties = type.GetProperties()
                                       .Where(IsTypeHasImporAttribute)
                                       .ToList();

            foreach (var property in objectProperties)
            {
                if (!container.ContainsKey(property.PropertyType))
                {
                    throw new UnknownDepencyException();
                }

                property.SetValue(resultInstance, Activator.CreateInstance(container[property.PropertyType]));
            }

            return objectProperties.Count > 0 ? resultInstance : null;
        }

        private object CreateObjectWithImportedConstructor(Type type, Dictionary<Type, Type> container)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            if (container == null)
            {
                throw new ArgumentNullException(nameof(container));
            }

            if (!container.ContainsKey(type))
            {
                throw new UnknownDepencyException();
            }

            var constructors = type.GetConstructors()
                                   .Where(constructor => constructor.IsPublic && !constructor.IsStatic)
                                   .ToList();

            if (constructors.Count > 1)
            {
                throw new MultiplyConstructorsExceptions();
            }

            if (constructors.Count < 1)
            {
                return null;
            }

            if (constructors[0].GetParameters().Any(param => !container.ContainsKey(param.ParameterType)))
            {
                throw new UnknownDepencyException();
            }

            var createdParams = constructors[0]
                                  .GetParameters()
                                  .Select(param => CreateParamByType(param.ParameterType, container))
                                  .ToArray();

            return constructors[0].Invoke(createdParams);
        }

        private object CreateParamByType(Type type, Dictionary<Type, Type> container)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            if (container == null)
            {
                throw new ArgumentNullException(nameof(container));
            }

            if (!container.ContainsKey(type))
            {
                throw new UnknownDepencyException();
            }

            if (type.IsClass || type.IsInterface)
            {
                return Activator.CreateInstance(container[type]);
            }

            return null;
        }

        #endregion
    }
}