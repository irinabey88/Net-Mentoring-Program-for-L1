using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using Exceptions.Exceptions;
using IocContainer.Interfaces;

namespace ReflectionObjectCreator
{
    public class ReflectionEmitActivator : IObjectCreator
    {
        private readonly string _assemblyName;

        public ReflectionEmitActivator(string assemblyName)
        {
            _assemblyName = string.IsNullOrWhiteSpace(assemblyName)
                ? throw new ArgumentNullException(nameof(assemblyName))
                : assemblyName;
        }

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

            if (!container.ContainsKey(type))
            {
                throw new UnknownDepencyException();
            }

            var resultType = container[type];
            var res = CreateObject(GetPropertiesName(resultType)
                                   ,GetProperties(resultType)
                                   ,$"{resultType.Namespace}.{resultType.Name}"
                                   ,container);

            return res;
        }

        public T CreateObject<T>(Dictionary<Type, Type> container)
        {
            var type = typeof(T);
            return (T)CreateObject(type, container);
        }

        #endregion


        #region Private methods

        public object CreateObject(string[] propertyNames, Type[] types, string typeName, Dictionary<Type, Type> container)
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
                if (!container.ContainsKey(property.PropertyType))
                {
                    throw new UnknownDepencyException();
                }

                property.SetValue(resultInstance, Activator.CreateInstance(container[property.PropertyType]));
            }

            return resultInstance;
        }

        private TypeBuilder CreateClass(string typeName)
        {
            var assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(new AssemblyName(_assemblyName),
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
                null, new[] { propertyType });

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

        private Type[] GetProperties(Type type)
        {
            var properties = type.GetProperties().Select(property => property.PropertyType).ToArray();
            return properties;
        }

        private string[] GetPropertiesName(Type type)
        {
            var properties = type.GetProperties().Select(property => property.Name).ToArray();
            return properties;
        }

        #endregion
    }
}