using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using Attributes;
using Exceptions.Exceptions;
using IocContainerReflectionEmit.Interfaces;

namespace IocContainerReflectionEmit
{
    public class Container : IContainer
    {
        private readonly Dictionary<Type, Type> _itemsList = new Dictionary<Type, Type>();

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
                    var attribute = (ExportAttribute) type.GetCustomAttribute(typeof(ExportAttribute));
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

            if (!_itemsList.ContainsKey(type))
            {
                _itemsList.Add(type, type);
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

            if (!_itemsList.ContainsKey(key))
            {
                _itemsList.Add(key, value);
            }

        }

        public object CreateInstance(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException();
            }

            if (!_itemsList.ContainsKey(type))
            {
                throw new UnknownDepencyException();
            }

            var resultType = _itemsList[type];
            var res = CreateObject(resultType.GetProperties().Select(property => property.Name).ToArray(),
                                          resultType.GetProperties().Select(property => property.PropertyType).ToArray(),
                                          resultType.Name);

            return res;
        }

        public T CreateInstance<T>()
        {
            var type = typeof(T);

            if (type == null)
            {
                throw new ArgumentNullException();
            }

            if (!_itemsList.ContainsKey(type))
            {
                throw new UnknownDepencyException();
            }

            var resultType = _itemsList[type];
            return (T)CreateObject(resultType.GetProperties().Select(property => property.Name).ToArray(),
                                          resultType.GetProperties().Select(property => property.PropertyType).ToArray()
                                          ,$"{resultType.Name}");
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

        public object CreateObject(string[] propertyNames, Type[] types, string typeName)
        {
            if (propertyNames.Length != types.Length)
            {
                Console.WriteLine("Properties number is no the same as types number!");
            }

            TypeBuilder dynamicClass = CreateClass(typeName);
            CreateConstructor(dynamicClass);
            for (int i = 0; i < propertyNames.Count(); i++)
                CreateProperty(dynamicClass, propertyNames[i], types[i]);
            Type type = dynamicClass.CreateType();

            var resultInstance = Activator.CreateInstance(type);
            var objectProperties = type.GetProperties().ToList();
            foreach (var property in objectProperties)
            {
                if (!_itemsList.ContainsKey(property.PropertyType))
                {
                    throw new UnknownDepencyException();
                }

                property.SetValue(resultInstance, Activator.CreateInstance(_itemsList[property.PropertyType]));
            }

            return resultInstance;
        }

        private TypeBuilder CreateClass(string typeName)
        {
            var assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(new AssemblyName(Guid.NewGuid().ToString()),
                AssemblyBuilderAccess.Run);
            var moduleBuilder = assemblyBuilder.DefineDynamicModule("ReflectionModule");
            TypeBuilder typeBuilder = moduleBuilder.DefineType(typeName
                , TypeAttributes.Public |
                  TypeAttributes.Class |
                  TypeAttributes.AutoClass |
                  TypeAttributes.AnsiClass |
                  TypeAttributes.BeforeFieldInit |
                  TypeAttributes.AutoLayout
                , null);

            return typeBuilder;
        }

        private void CreateConstructor(TypeBuilder typeBuilder)
        {
            if (typeBuilder == null)
            {
                throw new ArgumentNullException(nameof(typeBuilder));
            }

            typeBuilder.DefineDefaultConstructor(MethodAttributes.Public | MethodAttributes.SpecialName |
                                                 MethodAttributes.RTSpecialName);
        }

        private void CreateProperty(TypeBuilder typeBuilder, string propertyName, Type propertyType)
        {
            FieldBuilder fieldBuilder =
                typeBuilder.DefineField($"_{propertyName}", propertyType, FieldAttributes.Private);

            PropertyBuilder propertyBuilder =
                typeBuilder.DefineProperty(propertyName, PropertyAttributes.HasDefault, propertyType, null);
            MethodBuilder getPropMthdBldr = typeBuilder.DefineMethod($"get_{propertyName}",
                MethodAttributes.Public |
                MethodAttributes.SpecialName |
                MethodAttributes.HideBySig, propertyType,
                Type.EmptyTypes);
            ILGenerator getIl = getPropMthdBldr.GetILGenerator();

            getIl.Emit(OpCodes.Ldarg_0);
            getIl.Emit(OpCodes.Ldfld, fieldBuilder);
            getIl.Emit(OpCodes.Ret);

            MethodBuilder setPropMthdBldr = typeBuilder.DefineMethod($"set_{propertyName}",
                MethodAttributes.Public |
                MethodAttributes.SpecialName |
                MethodAttributes.HideBySig,
                null, new[] {propertyType});

            ILGenerator setIl = setPropMthdBldr.GetILGenerator();
            Label modifyProperty = setIl.DefineLabel();
            Label exitSet = setIl.DefineLabel();

            setIl.MarkLabel(modifyProperty);
            setIl.Emit(OpCodes.Ldarg_0);
            setIl.Emit(OpCodes.Ldarg_1);
            setIl.Emit(OpCodes.Stfld, fieldBuilder);

            setIl.Emit(OpCodes.Nop);
            setIl.MarkLabel(exitSet);
            setIl.Emit(OpCodes.Ret);

            propertyBuilder.SetGetMethod(getPropMthdBldr);
            propertyBuilder.SetSetMethod(setPropMthdBldr);
        }

        #endregion
    }
}