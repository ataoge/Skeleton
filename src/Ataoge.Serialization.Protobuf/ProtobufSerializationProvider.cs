using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Ataoge.Data;
using ProtoBuf.Meta;

namespace Ataoge.Serialization
{
    public class ProtobufSerializationProvider : ISerializationProvider
    {
        public T DeSerialize<T>(byte[] bytes)
        {
            EnsureRegisterType(typeof(T));
            T t = default(T);
            using (var memoryStream = new MemoryStream(bytes))
            {
                t = ProtoBuf.Serializer.Deserialize<T>(memoryStream);
            }
            return t;
        }

        public byte[] Serialize<T>(T value)
        {
            EnsureRegisterType(typeof(T));
            byte[] bytes = null;
            using( var memoryStream = new MemoryStream())
            {
                ProtoBuf.Serializer.Serialize(memoryStream, value);
                bytes = memoryStream.ToArray();
            }
            return bytes;
        }

        private void EnsureRegisterType(Type type)
        {
            if (RuntimeTypeModel.Default.IsDefined(type))
                return;

            var meta = RuntimeTypeModel.Default.Add(type, false);

            var index = 1;
            foreach (var p in type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly).Where(x => x.GetSetMethod() != null))
            {
                var i = index++;
                meta.AddField(i, p.Name);

                if (typeof(IEntity).IsAssignableFrom(p.PropertyType))
                {
                    EnsureRegisterType(p.DeclaringType);
                }
            }
        }

        private static void EnsureRegisterTypes(string namePrefix, params Type[] types)
        {
            foreach (var t in types.Where(x => x.Namespace.Contains(namePrefix)))
            {
                //Console.WriteLine("Processing {0}", t.FullName);
                var meta = RuntimeTypeModel.Default.Add(t, false);
                var index = 1;

                // find any derived class for the entity
                foreach (var d in types.Where(x => x.IsSubclassOf(t)))
                {
                    var i = index++;
                    //Console.WriteLine("\tSubtype: {0} - #{1}", d.Name, i);
                    meta.AddSubType(i, d);
                }

                // then add the properties
                foreach (var p in t.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly).Where(x => x.GetSetMethod() != null))
                {
                    var i = index++;
                    //Console.WriteLine("\tProperty: {0} - #{1}", p.Name, i);
                    meta.AddField(i, p.Name);
                }
            }
        }
    }
}