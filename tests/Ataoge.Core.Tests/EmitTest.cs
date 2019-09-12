using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using Xunit;

namespace Ataoge.Core.Tests
{
    public class EmitTest
    {
        [Fact]
        public void TestTypeBuild()
        {
            // specify a new assembly name
            var assemblyName = new AssemblyName("Pets");

    
            // create assembly builder
            var assemblyBuilder =AssemblyBuilder.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.RunAndCollect);

            // create module builder
            var moduleBuilder = assemblyBuilder.DefineDynamicModule("PetsModule");

            // create type builder for a class
            var typeBuilder = moduleBuilder.DefineType("Kitty", TypeAttributes.Public);


            // define two fields
            var fieldId = typeBuilder.DefineField(
                "_id", typeof(int), FieldAttributes.Private);
            var fieldName = typeBuilder.DefineField(
                "_name", typeof(string), FieldAttributes.Private);

            // define constructor
            Type objType = Type.GetType("System.Object");
            ConstructorInfo objCtor = objType.GetConstructor(new Type[0]);

            Type[] constructorArgs = { typeof(int), typeof(string) };

            var constructorBuilder = typeBuilder.DefineConstructor(
                MethodAttributes.Public, CallingConventions.Standard, constructorArgs);
            ILGenerator ilOfCtor = constructorBuilder.GetILGenerator();

            ilOfCtor.Emit(OpCodes.Ldarg_0);
            ilOfCtor.Emit(OpCodes.Call, objCtor);
            ilOfCtor.Emit(OpCodes.Ldarg_0);
            ilOfCtor.Emit(OpCodes.Ldarg_1);
            ilOfCtor.Emit(OpCodes.Stfld, fieldId);
            ilOfCtor.Emit(OpCodes.Ldarg_0);
            ilOfCtor.Emit(OpCodes.Ldarg_2);
            ilOfCtor.Emit(OpCodes.Stfld, fieldName);
            ilOfCtor.Emit(OpCodes.Ret);

            // Define Property
            var methodGetId = typeBuilder.DefineMethod(
      "GetId", MethodAttributes.Public, typeof(int), null);
            var methodSetId = typeBuilder.DefineMethod(
              "SetId", MethodAttributes.Public, null, new Type[] { typeof(int) });

            var ilOfGetId = methodGetId.GetILGenerator();
            ilOfGetId.Emit(OpCodes.Ldarg_0); // this
            ilOfGetId.Emit(OpCodes.Ldfld, fieldId);
            ilOfGetId.Emit(OpCodes.Ret);

            var ilOfSetId = methodSetId.GetILGenerator();
            ilOfSetId.Emit(OpCodes.Ldarg_0); // this
            ilOfSetId.Emit(OpCodes.Ldarg_1); // the first one in arguments list
            ilOfSetId.Emit(OpCodes.Stfld, fieldId);
            ilOfSetId.Emit(OpCodes.Ret);

            // create Id property
            var propertyId = typeBuilder.DefineProperty(
              "Id", PropertyAttributes.None, typeof(int), null);
            propertyId.SetGetMethod(methodGetId);
            propertyId.SetSetMethod(methodSetId);

            var methodGetName = typeBuilder.DefineMethod(
              "GetName", MethodAttributes.Public, typeof(string), null);
            var methodSetName = typeBuilder.DefineMethod(
              "SetName", MethodAttributes.Public, null, new Type[] { typeof(string) });

            var ilOfGetName = methodGetName.GetILGenerator();
            ilOfGetName.Emit(OpCodes.Ldarg_0); // this
            ilOfGetName.Emit(OpCodes.Ldfld, fieldName);
            ilOfGetName.Emit(OpCodes.Ret);

            var ilOfSetName = methodSetName.GetILGenerator();
            ilOfSetName.Emit(OpCodes.Ldarg_0); // this
            ilOfSetName.Emit(OpCodes.Ldarg_1); // the first one in arguments list
            ilOfSetName.Emit(OpCodes.Stfld, fieldName);
            ilOfSetName.Emit(OpCodes.Ret);

            // create Name property
            var propertyName = typeBuilder.DefineProperty(
              "Name", PropertyAttributes.None, typeof(string), null);
            propertyName.SetGetMethod(methodGetName);
            propertyName.SetSetMethod(methodSetName);

            // create ToString() method
            var methodToString = typeBuilder.DefineMethod(
              "ToString",
              MethodAttributes.Virtual | MethodAttributes.Public,
              typeof(string),
              null);

            var ilOfToString = methodToString.GetILGenerator();
            var local = ilOfToString.DeclareLocal(typeof(string)); // create a local variable
            ilOfToString.Emit(OpCodes.Ldstr, "Id:[{0}], Name:[{1}]");
            ilOfToString.Emit(OpCodes.Ldarg_0); // this
            ilOfToString.Emit(OpCodes.Ldfld, fieldId);
            ilOfToString.Emit(OpCodes.Box, typeof(int)); // boxing the value type to object
            ilOfToString.Emit(OpCodes.Ldarg_0); // this
            ilOfToString.Emit(OpCodes.Ldfld, fieldName);
            ilOfToString.Emit(OpCodes.Call,
              typeof(string).GetMethod("Format",
              new Type[] { typeof(string), typeof(object), typeof(object) }));
            ilOfToString.Emit(OpCodes.Stloc, local); // set local variable
            ilOfToString.Emit(OpCodes.Ldloc, local); // load local variable to stack
            ilOfToString.Emit(OpCodes.Ret);

            var classType = typeBuilder.CreateType();
            object obj = Activator.CreateInstance(classType,1,"aa");
            MethodInfo methodInfo = classType.GetMethod("ToString");

            var aa = methodInfo.Invoke(obj, null);
            
        }

        [Fact]
        public void TestEmitSetter()
        {
          Model model = new Model();
          var emitSetter=EmitSetter<Model>("Name");

          for (int i = 0; i < 100000; i++)
          {
              emitSetter(model, "测试");
          }
        }

        public static Action<T, object> EmitSetter<T>(string propertyName)
        {
           var type = typeof(T);
           var dynamicMethod = new DynamicMethod("EmitCallable", null, new[] { type, typeof(object) }, type.Module);
            var iLGenerator = dynamicMethod.GetILGenerator();


            var callMethod = type.GetMethod("set_" + propertyName, BindingFlags.Instance | BindingFlags.IgnoreCase | BindingFlags.Public);
            var parameterInfo = callMethod.GetParameters()[0];
            var local = iLGenerator.DeclareLocal(parameterInfo.ParameterType, true);


            iLGenerator.Emit(OpCodes.Ldarg_1);
            if (parameterInfo.ParameterType.IsValueType)
            {
                // 如果是值类型，拆箱
                iLGenerator.Emit(OpCodes.Unbox_Any, parameterInfo.ParameterType);
            }
            else
            {
                // 如果是引用类型，转换
                iLGenerator.Emit(OpCodes.Castclass, parameterInfo.ParameterType);
            }


            iLGenerator.Emit(OpCodes.Stloc, local);
            iLGenerator.Emit(OpCodes.Ldarg_0);
            iLGenerator.Emit(OpCodes.Ldloc, local);


            iLGenerator.EmitCall(OpCodes.Callvirt, callMethod, null);
            iLGenerator.Emit(OpCodes.Ret);


            return dynamicMethod.CreateDelegate(typeof(Action<T, object>)) as Action<T, object>;
        }

        [Fact]
        public void TestGetSetter()
        {
          List<Book> result = new List<Book>();
            string sql = "select * from book";
            //SQLiteCommand command = new SQLiteCommand(sql, conn);
            var factory = DbProviderFactories.GetFactory("");
            var connection = factory.CreateConnection();

            var command = factory.CreateCommand();
            command.CommandText = sql;
            command.Connection = connection;
            var reader = command.ExecuteReader();
            var readerMap = new ReaderMap(reader);
            var propertyMap = new PropertyMap(readerMap, typeof(Book));
 
            var func = GetSetter<Book>(reader, propertyMap);
 
            while (reader.Read())
            {
                Book r = (Book)func(reader);
                result.Add(r);
            }
 
         


        }

        public class Book
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public Book()
        {
            Id = 0;
            Name = "Name";
            Price = 11.9;
        }
 
        public override string ToString()
        {
            return $"Id; {Id}\tName: {Name}\tPrice: {Price}";
        }
    }



        static Func<IDataReader, object> GetSetter<T>(IDataReader reader, PropertyMap map)
        {
            Type bookType = typeof(T);
            var constructor = bookType.GetConstructors().FirstOrDefault();
            DynamicMethod setter = new DynamicMethod("setbook", bookType, new Type[] { typeof(IDataReader) }, bookType, true);
            
            //setter.DefineParameter(0, ParameterAttributes.In, "reader");
 
            var iLGenerator = setter.GetILGenerator();
 
            iLGenerator.DeclareLocal(bookType); //Ldloc_0 book
            iLGenerator.DeclareLocal(typeof(object));//Ldloc_1 reader.GetValue
 
            iLGenerator.Emit(OpCodes.Nop);
            iLGenerator.Emit(OpCodes.Newobj, constructor);
            iLGenerator.Emit(OpCodes.Stloc_0);
            iLGenerator.Emit(OpCodes.Nop);
 
            var getM = typeof(DbDataReader).GetMethod("GetValue");
 
            int len = map.Count;
            for (int i = 0; i < len; i++)
            {
                ///读数据
                iLGenerator.Emit(OpCodes.Ldarg_0);
                iLGenerator.Emit(OpCodes.Ldc_I4, i);
                iLGenerator.Emit(OpCodes.Callvirt, getM);
                iLGenerator.Emit(OpCodes.Stloc_1);
                iLGenerator.Emit(OpCodes.Nop);
 
                var tp = map[i];
                iLGenerator.Emit(OpCodes.Ldloc_0);
                iLGenerator.Emit(OpCodes.Ldloc_1);
                if (tp.PropertyType.IsValueType)
                    iLGenerator.Emit(OpCodes.Unbox_Any, tp.PropertyType);
                else
                    iLGenerator.Emit(OpCodes.Castclass, tp.PropertyType);
 
                var mt = tp.GetSetMethod();
 
                iLGenerator.Emit(OpCodes.Callvirt, mt);
                iLGenerator.Emit(OpCodes.Nop);
            }
 
            iLGenerator.Emit(OpCodes.Ldloc_0);
            iLGenerator.Emit(OpCodes.Ret);
 
            return (Func<IDataReader, object>)setter.CreateDelegate(typeof(Func<IDataReader,object>));
        }


      

        public class Model
        {
        public string Name { get; set; }
        }
      }

    public class PropertyMap
    {
        private PropertyInfo[] properties = null;
        public PropertyMap(ReaderMap readerMap,Type type)
        {
            int len = readerMap.Count;
            Count = len;
            var ps = type.GetProperties();
            properties = new PropertyInfo[len];
            for(int i = 0; i < len; i++)
            {
                var readerItem = readerMap[i];
                var tp = ps.FirstOrDefault(p => p.Name.ToUpper() == readerItem.Name.ToUpper());
                if (tp != null)
                {
                    if (tp.PropertyType.IsAssignableFrom(readerItem.Type))
                    {
                        properties[i] = tp;
                    }
                }                
            }
        }
        public PropertyInfo this[int i]
        {
            get
            {
                return properties[i];
            }
        }
        public int Count { get; private set; }
    }
    public class ReaderMap
    {
        private ReaderItem[] items = null; 
        public ReaderMap(IDataReader reader)
        {
            int len = reader.FieldCount;
            Count = len;
            items = new ReaderItem[len];
            for(int i = 0; i < len; i++)
            {
                items[i] = new ReaderItem
                {
                    Id = i,
                    Name = reader.GetName(i),
                    Type = reader.GetFieldType(i)
                };
            }
        }
        public int Count
        {
            get;
            private set;
        }
        public ReaderItem this[int i]
        {
            get
            {
                return items[i];
            }
        }
    }
    public class ReaderItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Type Type { get; set; }
    }



  
}