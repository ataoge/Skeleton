using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace Ataoge.Data.Mapper
{
    public static class MapperHelper
    {
        static MapperHelper()
        {
            typeMap = new Dictionary<Type, DbType>
            {
                [typeof(byte)] = DbType.Byte,
                [typeof(sbyte)] = DbType.SByte,
                [typeof(short)] = DbType.Int16,
                [typeof(ushort)] = DbType.UInt16,
                [typeof(int)] = DbType.Int32,
                [typeof(uint)] = DbType.UInt32,
                [typeof(long)] = DbType.Int64,
                [typeof(ulong)] = DbType.UInt64,
                [typeof(float)] = DbType.Single,
                [typeof(double)] = DbType.Double,
                [typeof(decimal)] = DbType.Decimal,
                [typeof(bool)] = DbType.Boolean,
                [typeof(string)] = DbType.String,
                [typeof(char)] = DbType.StringFixedLength,
                [typeof(Guid)] = DbType.Guid,
                [typeof(DateTime)] = DbType.DateTime,
                [typeof(DateTimeOffset)] = DbType.DateTimeOffset,
                [typeof(TimeSpan)] = DbType.Time,
                [typeof(byte[])] = DbType.Binary,
                [typeof(byte?)] = DbType.Byte,
                [typeof(sbyte?)] = DbType.SByte,
                [typeof(short?)] = DbType.Int16,
                [typeof(ushort?)] = DbType.UInt16,
                [typeof(int?)] = DbType.Int32,
                [typeof(uint?)] = DbType.UInt32,
                [typeof(long?)] = DbType.Int64,
                [typeof(ulong?)] = DbType.UInt64,
                [typeof(float?)] = DbType.Single,
                [typeof(double?)] = DbType.Double,
                [typeof(decimal?)] = DbType.Decimal,
                [typeof(bool?)] = DbType.Boolean,
                [typeof(char?)] = DbType.StringFixedLength,
                [typeof(Guid?)] = DbType.Guid,
                [typeof(DateTime?)] = DbType.DateTime,
                [typeof(DateTimeOffset?)] = DbType.DateTimeOffset,
                [typeof(TimeSpan?)] = DbType.Time,
                [typeof(object)] = DbType.Object
            };

            ResetTypeHandlers(false);
        }

        private static Dictionary<Type, DbType> typeMap;
        /// <summary>
        /// Clear the registered type handlers.
        /// </summary>
        public static void ResetTypeHandlers() => ResetTypeHandlers(true);

        private static void ResetTypeHandlers(bool clone)
        {
            typeHandlers = new Dictionary<Type, ITypeHandler>();
        }
        /// <summary>
        /// Gets type-map for the given type
        /// </summary>
        /// <returns>Type map instance, default is to create new instance of DefaultTypeMap</returns>
        public static Func<Type, ITypeMap> TypeMapProvider = (Type type) => new DefaultTypeMap(type);

        /// <summary>
        /// Gets type-map for the given <see cref="Type"/>.
        /// </summary>
        /// <param name="type">The type to get a map for.</param>
        /// <returns>Type map implementation, DefaultTypeMap instance if no override present</returns>
        public static ITypeMap GetTypeMap(Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));
            var map = (ITypeMap)_typeMaps[type];
            if (map == null)
            {
                lock (_typeMaps)
                {   // double-checked; store this to avoid reflection next time we see this type
                    // since multiple queries commonly use the same domain-entity/DTO/view-model type
                    map = (ITypeMap)_typeMaps[type];

                    if (map == null)
                    {
                        map = TypeMapProvider(type);
                        _typeMaps[type] = map;
                    }
                }
            }
            return map;
        }
        
        // use Hashtable to get free lockless reading
        private static readonly Hashtable _typeMaps = new Hashtable();

        private static Func<IDataReader, object> GetTypeDeserializerImpl(
            Type type, IDataReader reader, int startBound = 0, int length = -1, bool returnNullIfFirstMissing = false
        )
        {
           if (length == -1)
            {
                length = reader.FieldCount - startBound;
            }

            if (reader.FieldCount <= startBound)
            {
                throw new Exception("MultiMapException");
            }

            var returnType = type.IsValueType ? typeof(object) : type;
            var dm = new DynamicMethod("Deserialize" + Guid.NewGuid().ToString(), returnType, new[] { typeof(IDataReader) }, type, true);
            var il = dm.GetILGenerator();

            if (IsValueTuple(type))
            {
                GenerateValueTupleDeserializer(type, reader, startBound, length, il);
            }
            else
            {
                GenerateDeserializerFromMap(type, reader, startBound, length, returnNullIfFirstMissing, il);
            }

            var funcType = System.Linq.Expressions.Expression.GetFuncType(typeof(IDataReader), returnType);
            return (Func<IDataReader, object>)dm.CreateDelegate(funcType);
        }


        private static void GenerateDeserializerFromMap(Type type, IDataReader reader, int startBound, int length, bool returnNullIfFirstMissing, ILGenerator il)
        {
            var currentIndexDiagnosticLocal = il.DeclareLocal(typeof(int));
            var returnValueLocal = il.DeclareLocal(type);
            il.Emit(OpCodes.Ldc_I4_0);
            il.Emit(OpCodes.Stloc, currentIndexDiagnosticLocal);

            var names = Enumerable.Range(startBound, length).Select(i => reader.GetName(i)).ToArray();

            ITypeMap typeMap = GetTypeMap(type);

            int index = startBound;
            ConstructorInfo specializedConstructor = null;

            bool supportInitialize = false;
            Dictionary<Type, LocalBuilder> structLocals = null;
            if (type.IsValueType)
            {
                il.Emit(OpCodes.Ldloca, returnValueLocal);
                il.Emit(OpCodes.Initobj, type);
            }
            else
            {
                var types = new Type[length];
                for (int i = startBound; i < startBound + length; i++)
                {
                    types[i - startBound] = reader.GetFieldType(i);
                }

                var explicitConstr = typeMap.FindExplicitConstructor();
                if (explicitConstr != null)
                {
                    var consPs = explicitConstr.GetParameters();
                    foreach (var p in consPs)
                    {
                        if (!p.ParameterType.IsValueType)
                        {
                            il.Emit(OpCodes.Ldnull);
                        }
                        else
                        {
                            GetTempLocal(il, ref structLocals, p.ParameterType, true);
                        }
                    }

                    il.Emit(OpCodes.Newobj, explicitConstr);
                    il.Emit(OpCodes.Stloc, returnValueLocal);
                    supportInitialize = typeof(ISupportInitialize).IsAssignableFrom(type);
                    if (supportInitialize)
                    {
                        il.Emit(OpCodes.Ldloc, returnValueLocal);
                        il.EmitCall(OpCodes.Callvirt, typeof(ISupportInitialize).GetMethod(nameof(ISupportInitialize.BeginInit)), null);
                    }
                }
                else
                {
                    var ctor = typeMap.FindConstructor(names, types);
                    if (ctor == null)
                    {
                        string proposedTypes = "(" + string.Join(", ", types.Select((t, i) => t.FullName + " " + names[i]).ToArray()) + ")";
                        throw new InvalidOperationException($"A parameterless default constructor or one matching signature {proposedTypes} is required for {type.FullName} materialization");
                    }

                    if (ctor.GetParameters().Length == 0)
                    {
                        il.Emit(OpCodes.Newobj, ctor);
                        il.Emit(OpCodes.Stloc, returnValueLocal);
                        supportInitialize = typeof(ISupportInitialize).IsAssignableFrom(type);
                        if (supportInitialize)
                        {
                            il.Emit(OpCodes.Ldloc, returnValueLocal);
                            il.EmitCall(OpCodes.Callvirt, typeof(ISupportInitialize).GetMethod(nameof(ISupportInitialize.BeginInit)), null);
                        }
                    }
                    else
                    {
                        specializedConstructor = ctor;
                    }
                }
            }

            il.BeginExceptionBlock();
            if (type.IsValueType)
            {
                il.Emit(OpCodes.Ldloca, returnValueLocal); // [target]
            }
            else if (specializedConstructor == null)
            {
                il.Emit(OpCodes.Ldloc, returnValueLocal); // [target]
            }

            var members = (specializedConstructor != null
                ? names.Select(n => typeMap.GetConstructorParameter(specializedConstructor, n))
                : names.Select(n => typeMap.GetMember(n))).ToList();

            // stack is now [target]
            bool first = true;
            var allDone = il.DefineLabel();
            var stringEnumLocal = (LocalBuilder)null;
            var valueCopyDiagnosticLocal = il.DeclareLocal(typeof(object));
            bool applyNullSetting = false;//Settings.ApplyNullValues;
            foreach (var item in members)
            {
                if (item != null)
                {
                    if (specializedConstructor == null)
                        il.Emit(OpCodes.Dup); // stack is now [target][target]
                    Label finishLabel = il.DefineLabel();
                    Type memberType = item.MemberType;

                    // Save off the current index for access if an exception is thrown
                    EmitInt32(il, index);
                    il.Emit(OpCodes.Stloc, currentIndexDiagnosticLocal);

                    LoadReaderValueOrBranchToDBNullLabel(il, index, ref stringEnumLocal, valueCopyDiagnosticLocal, reader.GetFieldType(index), memberType, out var isDbNullLabel);

                    if (specializedConstructor == null)
                    {
                        // Store the value in the property/field
                        if (item.Property != null)
                        {
                            il.Emit(type.IsValueType ? OpCodes.Call : OpCodes.Callvirt, DefaultTypeMap.GetPropertySetter(item.Property, type));
                        }
                        else
                        {
                            il.Emit(OpCodes.Stfld, item.Field); // stack is now [target]
                        }
                    }

                    il.Emit(OpCodes.Br_S, finishLabel); // stack is now [target]

                    il.MarkLabel(isDbNullLabel); // incoming stack: [target][target][value]
                    if (specializedConstructor != null)
                    {
                        il.Emit(OpCodes.Pop);
                        LoadDefaultValue(il, item.MemberType);
                    }
                    else if (applyNullSetting && (!memberType.IsValueType || Nullable.GetUnderlyingType(memberType) != null))
                    {
                        il.Emit(OpCodes.Pop); // stack is now [target][target]
                        // can load a null with this value
                        if (memberType.IsValueType)
                        { // must be Nullable<T> for some T
                            GetTempLocal(il, ref structLocals, memberType, true); // stack is now [target][target][null]
                        }
                        else
                        { // regular reference-type
                            il.Emit(OpCodes.Ldnull); // stack is now [target][target][null]
                        }

                        // Store the value in the property/field
                        if (item.Property != null)
                        {
                            il.Emit(type.IsValueType ? OpCodes.Call : OpCodes.Callvirt, DefaultTypeMap.GetPropertySetter(item.Property, type));
                            // stack is now [target]
                        }
                        else
                        {
                            il.Emit(OpCodes.Stfld, item.Field); // stack is now [target]
                        }
                    }
                    else
                    {
                        il.Emit(OpCodes.Pop); // stack is now [target][target]
                        il.Emit(OpCodes.Pop); // stack is now [target]
                    }

                    if (first && returnNullIfFirstMissing)
                    {
                        il.Emit(OpCodes.Pop);
                        il.Emit(OpCodes.Ldnull); // stack is now [null]
                        il.Emit(OpCodes.Stloc, returnValueLocal);
                        il.Emit(OpCodes.Br, allDone);
                    }

                    il.MarkLabel(finishLabel);
                }
                first = false;
                index++;
            }
            if (type.IsValueType)
            {
                il.Emit(OpCodes.Pop);
            }
            else
            {
                if (specializedConstructor != null)
                {
                    il.Emit(OpCodes.Newobj, specializedConstructor);
                }
                il.Emit(OpCodes.Stloc, returnValueLocal); // stack is empty
                if (supportInitialize)
                {
                    il.Emit(OpCodes.Ldloc, returnValueLocal);
                    il.EmitCall(OpCodes.Callvirt, typeof(ISupportInitialize).GetMethod(nameof(ISupportInitialize.EndInit)), null);
                }
            }
            il.MarkLabel(allDone);
            il.BeginCatchBlock(typeof(Exception)); // stack is Exception
            il.Emit(OpCodes.Ldloc, currentIndexDiagnosticLocal); // stack is Exception, index
            il.Emit(OpCodes.Ldarg_0); // stack is Exception, index, reader
            il.Emit(OpCodes.Ldloc, valueCopyDiagnosticLocal); // stack is Exception, index, reader, value
            il.EmitCall(OpCodes.Call, typeof(MapperHelper).GetMethod(nameof(MapperHelper.ThrowDataException)), null);
            il.EndExceptionBlock();

            il.Emit(OpCodes.Ldloc, returnValueLocal); // stack is [rval]
            if (type.IsValueType)
            {
                il.Emit(OpCodes.Box, type);
            }
            il.Emit(OpCodes.Ret);
        }

        private static LocalBuilder GetTempLocal(ILGenerator il, ref Dictionary<Type, LocalBuilder> locals, Type type, bool initAndLoad)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));
            locals = locals ?? new Dictionary<Type, LocalBuilder>();
            if (!locals.TryGetValue(type, out LocalBuilder found))
            {
                found = il.DeclareLocal(type);
                locals.Add(type, found);
            }
            if (initAndLoad)
            {
                il.Emit(OpCodes.Ldloca, found);
                il.Emit(OpCodes.Initobj, type);
                il.Emit(OpCodes.Ldloca, found);
                il.Emit(OpCodes.Ldobj, type);
            }
            return found;
        }

        private static bool IsValueTuple(Type type) => type?.IsValueType == true && type.FullName.StartsWith("System.ValueTuple`", StringComparison.Ordinal);

        private static void GenerateValueTupleDeserializer(Type valueTupleType, IDataReader reader, int startBound, int length, ILGenerator il)
        {
            var currentValueTupleType = valueTupleType;

            var constructors = new List<ConstructorInfo>();
            var languageTupleElementTypes = new List<Type>();

            while (true)
            {
                var arity = int.Parse(currentValueTupleType.Name.Substring("ValueTuple`".Length), CultureInfo.InvariantCulture);
                var constructorParameterTypes = new Type[arity];
                var restField = (FieldInfo)null;

                foreach (var field in currentValueTupleType.GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly))
                {
                    if (field.Name == "Rest")
                    {
                        restField = field;
                    }
                    else if (field.Name.StartsWith("Item", StringComparison.Ordinal))
                    {
                        var elementNumber = int.Parse(
                            field.Name.Substring("Item".Length), CultureInfo.InvariantCulture);
                        constructorParameterTypes[elementNumber - 1] = field.FieldType;
                    }
                }

                var itemFieldCount = constructorParameterTypes.Length;
                if (restField != null) itemFieldCount--;

                for (var i = 0; i < itemFieldCount; i++)
                {
                    languageTupleElementTypes.Add(constructorParameterTypes[i]);
                }

                if (restField != null)
                {
                    constructorParameterTypes[constructorParameterTypes.Length - 1] = restField.FieldType;
                }

                constructors.Add(currentValueTupleType.GetConstructor(constructorParameterTypes));

                if (restField is null) break;

                currentValueTupleType = restField.FieldType;
                if (!IsValueTuple(currentValueTupleType))
                {
                    throw new InvalidOperationException("The Rest field of a ValueTuple must contain a nested ValueTuple of arity 1 or greater.");
                }
            }

            var stringEnumLocal = (LocalBuilder)null;

            for (var i = 0; i < languageTupleElementTypes.Count; i++)
            {
                var targetType = languageTupleElementTypes[i];

                if (i < length)
                {
                    LoadReaderValueOrBranchToDBNullLabel(
                        il,
                        startBound + i,
                        ref stringEnumLocal,
                        valueCopyLocal: null,
                        reader.GetFieldType(startBound + i),
                        targetType,
                        out var isDbNullLabel);

                    var finishLabel = il.DefineLabel();
                    il.Emit(OpCodes.Br_S, finishLabel);
                    il.MarkLabel(isDbNullLabel);
                    il.Emit(OpCodes.Pop);

                    LoadDefaultValue(il, targetType);

                    il.MarkLabel(finishLabel);
                }
                else
                {
                    LoadDefaultValue(il, targetType);
                }
            }

            for (var i = constructors.Count - 1; i >= 0; i--)
            {
                il.Emit(OpCodes.Newobj, constructors[i]);
            }

            il.Emit(OpCodes.Box, valueTupleType);
            il.Emit(OpCodes.Ret);
        }


        private static void LoadDefaultValue(ILGenerator il, Type type)
        {
            if (type.IsValueType)
            {
                var local = il.DeclareLocal(type);
                il.Emit(OpCodes.Ldloca, local);
                il.Emit(OpCodes.Initobj, type);
                il.Emit(OpCodes.Ldloc, local);
            }
            else
            {
                il.Emit(OpCodes.Ldnull);
            }
        }

        internal static bool HasTypeHandler(Type type) => typeHandlers.ContainsKey(type);
        private static Dictionary<Type, ITypeHandler> typeHandlers;
        
        private static void LoadReaderValueOrBranchToDBNullLabel(ILGenerator il, int index, ref LocalBuilder stringEnumLocal, LocalBuilder valueCopyLocal, Type colType, Type memberType, out Label isDbNullLabel)
        {
            isDbNullLabel = il.DefineLabel();
            il.Emit(OpCodes.Ldarg_0); // stack is now [...][reader]
            EmitInt32(il, index); // stack is now [...][reader][index]
            il.Emit(OpCodes.Callvirt, getItem); // stack is now [...][value-as-object]

            if (valueCopyLocal != null)
            {
                il.Emit(OpCodes.Dup); // stack is now [...][value-as-object][value-as-object]
                il.Emit(OpCodes.Stloc, valueCopyLocal); // stack is now [...][value-as-object]
            }

            if (memberType == typeof(char) || memberType == typeof(char?))
            {
                il.EmitCall(OpCodes.Call, typeof(MapperHelper).GetMethod(
                    memberType == typeof(char) ? nameof(MapperHelper.ReadChar) : nameof(MapperHelper.ReadNullableChar), BindingFlags.Static | BindingFlags.Public), null); // stack is now [...][typed-value]
            }
            else
            {
                il.Emit(OpCodes.Dup); // stack is now [...][value-as-object][value-as-object]
                il.Emit(OpCodes.Isinst, typeof(DBNull)); // stack is now [...][value-as-object][DBNull or null]
                il.Emit(OpCodes.Brtrue_S, isDbNullLabel); // stack is now [...][value-as-object]

                // unbox nullable enums as the primitive, i.e. byte etc

                var nullUnderlyingType = Nullable.GetUnderlyingType(memberType);
                var unboxType = nullUnderlyingType?.IsEnum == true ? nullUnderlyingType : memberType;

                if (unboxType.IsEnum)
                {
                    Type numericType = Enum.GetUnderlyingType(unboxType);
                    if (colType == typeof(string))
                    {
                        if (stringEnumLocal == null)
                        {
                            stringEnumLocal = il.DeclareLocal(typeof(string));
                        }
                        il.Emit(OpCodes.Castclass, typeof(string)); // stack is now [...][string]
                        il.Emit(OpCodes.Stloc, stringEnumLocal); // stack is now [...]
                        il.Emit(OpCodes.Ldtoken, unboxType); // stack is now [...][enum-type-token]
                        il.EmitCall(OpCodes.Call, typeof(Type).GetMethod(nameof(Type.GetTypeFromHandle)), null);// stack is now [...][enum-type]
                        il.Emit(OpCodes.Ldloc, stringEnumLocal); // stack is now [...][enum-type][string]
                        il.Emit(OpCodes.Ldc_I4_1); // stack is now [...][enum-type][string][true]
                        il.EmitCall(OpCodes.Call, enumParse, null); // stack is now [...][enum-as-object]
                        il.Emit(OpCodes.Unbox_Any, unboxType); // stack is now [...][typed-value]
                    }
                    else
                    {
                        FlexibleConvertBoxedFromHeadOfStack(il, colType, unboxType, numericType);
                    }

                    if (nullUnderlyingType != null)
                    {
                        il.Emit(OpCodes.Newobj, memberType.GetConstructor(new[] { nullUnderlyingType })); // stack is now [...][typed-value]
                    }
                }
                else if (memberType.FullName == LinqBinary)
                {
                    il.Emit(OpCodes.Unbox_Any, typeof(byte[])); // stack is now [...][byte-array]
                    il.Emit(OpCodes.Newobj, memberType.GetConstructor(new Type[] { typeof(byte[]) }));// stack is now [...][binary]
                }
                else
                {
                    TypeCode dataTypeCode = Type.GetTypeCode(colType), unboxTypeCode = Type.GetTypeCode(unboxType);
                    bool hasTypeHandler;
                    if ((hasTypeHandler = typeHandlers.ContainsKey(unboxType)) || colType == unboxType || dataTypeCode == unboxTypeCode || dataTypeCode == Type.GetTypeCode(nullUnderlyingType))
                    {
                        if (hasTypeHandler)
                        {
#pragma warning disable 618
                            il.EmitCall(OpCodes.Call, typeof(TypeHandlerCache<>).MakeGenericType(unboxType).GetMethod(nameof(TypeHandlerCache<int>.Parse)), null); // stack is now [...][typed-value]
#pragma warning restore 618
                        }
                        else
                        {
                            il.Emit(OpCodes.Unbox_Any, unboxType); // stack is now [...][typed-value]
                        }
                    }
                    else
                    {
                        // not a direct match; need to tweak the unbox
                        FlexibleConvertBoxedFromHeadOfStack(il, colType, nullUnderlyingType ?? unboxType, null);
                        if (nullUnderlyingType != null)
                        {
                            il.Emit(OpCodes.Newobj, unboxType.GetConstructor(new[] { nullUnderlyingType })); // stack is now [...][typed-value]
                        }
                    }
                }
            }
        }

         private static void FlexibleConvertBoxedFromHeadOfStack(ILGenerator il, Type from, Type to, Type via)
        {
            MethodInfo op;
            if (from == (via ?? to))
            {
                il.Emit(OpCodes.Unbox_Any, to); // stack is now [target][target][typed-value]
            }
            else if ((op = GetOperator(from, to)) != null)
            {
                // this is handy for things like decimal <===> double
                il.Emit(OpCodes.Unbox_Any, from); // stack is now [target][target][data-typed-value]
                il.Emit(OpCodes.Call, op); // stack is now [target][target][typed-value]
            }
            else
            {
                bool handled = false;
                OpCode opCode = default(OpCode);
                switch (Type.GetTypeCode(from))
                {
                    case TypeCode.Boolean:
                    case TypeCode.Byte:
                    case TypeCode.SByte:
                    case TypeCode.Int16:
                    case TypeCode.UInt16:
                    case TypeCode.Int32:
                    case TypeCode.UInt32:
                    case TypeCode.Int64:
                    case TypeCode.UInt64:
                    case TypeCode.Single:
                    case TypeCode.Double:
                        handled = true;
                        switch (Type.GetTypeCode(via ?? to))
                        {
                            case TypeCode.Byte:
                                opCode = OpCodes.Conv_Ovf_I1_Un; break;
                            case TypeCode.SByte:
                                opCode = OpCodes.Conv_Ovf_I1; break;
                            case TypeCode.UInt16:
                                opCode = OpCodes.Conv_Ovf_I2_Un; break;
                            case TypeCode.Int16:
                                opCode = OpCodes.Conv_Ovf_I2; break;
                            case TypeCode.UInt32:
                                opCode = OpCodes.Conv_Ovf_I4_Un; break;
                            case TypeCode.Boolean: // boolean is basically an int, at least at this level
                            case TypeCode.Int32:
                                opCode = OpCodes.Conv_Ovf_I4; break;
                            case TypeCode.UInt64:
                                opCode = OpCodes.Conv_Ovf_I8_Un; break;
                            case TypeCode.Int64:
                                opCode = OpCodes.Conv_Ovf_I8; break;
                            case TypeCode.Single:
                                opCode = OpCodes.Conv_R4; break;
                            case TypeCode.Double:
                                opCode = OpCodes.Conv_R8; break;
                            default:
                                handled = false;
                                break;
                        }
                        break;
                }
                if (handled)
                {
                    il.Emit(OpCodes.Unbox_Any, from); // stack is now [target][target][col-typed-value]
                    il.Emit(opCode); // stack is now [target][target][typed-value]
                    if (to == typeof(bool))
                    { // compare to zero; I checked "csc" - this is the trick it uses; nice
                        il.Emit(OpCodes.Ldc_I4_0);
                        il.Emit(OpCodes.Ceq);
                        il.Emit(OpCodes.Ldc_I4_0);
                        il.Emit(OpCodes.Ceq);
                    }
                }
                else
                {
                    il.Emit(OpCodes.Ldtoken, via ?? to); // stack is now [target][target][value][member-type-token]
                    il.EmitCall(OpCodes.Call, typeof(Type).GetMethod(nameof(Type.GetTypeFromHandle)), null); // stack is now [target][target][value][member-type]
                    il.EmitCall(OpCodes.Call, typeof(Convert).GetMethod(nameof(Convert.ChangeType), new Type[] { typeof(object), typeof(Type) }), null); // stack is now [target][target][boxed-member-type-value]
                    il.Emit(OpCodes.Unbox_Any, to); // stack is now [target][target][typed-value]
                }
            }
        }

        private static MethodInfo GetOperator(Type from, Type to)
        {
            if (to == null) return null;
            MethodInfo[] fromMethods, toMethods;
            return ResolveOperator(fromMethods = from.GetMethods(BindingFlags.Static | BindingFlags.Public), from, to, "op_Implicit")
                ?? ResolveOperator(toMethods = to.GetMethods(BindingFlags.Static | BindingFlags.Public), from, to, "op_Implicit")
                ?? ResolveOperator(fromMethods, from, to, "op_Explicit")
                ?? ResolveOperator(toMethods, from, to, "op_Explicit");
        }

        private static MethodInfo ResolveOperator(MethodInfo[] methods, Type from, Type to, string name)
        {
            for (int i = 0; i < methods.Length; i++)
            {
                if (methods[i].Name != name || methods[i].ReturnType != to) continue;
                var args = methods[i].GetParameters();
                if (args.Length != 1 || args[0].ParameterType != from) continue;
                return methods[i];
            }
            return null;
        }

        internal const string LinqBinary = "System.Data.Linq.Binary";

        private static readonly MethodInfo
                    enumParse = typeof(Enum).GetMethod(nameof(Enum.Parse), new Type[] { typeof(Type), typeof(string), typeof(bool) }),
                    getItem = typeof(IDataRecord).GetProperties(BindingFlags.Instance | BindingFlags.Public)
                        .Where(p => p.GetIndexParameters().Length > 0 && p.GetIndexParameters()[0].ParameterType == typeof(int))
                        .Select(p => p.GetGetMethod()).First();

        private static void EmitInt32(ILGenerator il, int value)
        {
            switch (value)
            {
                case -1: il.Emit(OpCodes.Ldc_I4_M1); break;
                case 0: il.Emit(OpCodes.Ldc_I4_0); break;
                case 1: il.Emit(OpCodes.Ldc_I4_1); break;
                case 2: il.Emit(OpCodes.Ldc_I4_2); break;
                case 3: il.Emit(OpCodes.Ldc_I4_3); break;
                case 4: il.Emit(OpCodes.Ldc_I4_4); break;
                case 5: il.Emit(OpCodes.Ldc_I4_5); break;
                case 6: il.Emit(OpCodes.Ldc_I4_6); break;
                case 7: il.Emit(OpCodes.Ldc_I4_7); break;
                case 8: il.Emit(OpCodes.Ldc_I4_8); break;
                default:
                    if (value >= -128 && value <= 127)
                    {
                        il.Emit(OpCodes.Ldc_I4_S, (sbyte)value);
                    }
                    else
                    {
                        il.Emit(OpCodes.Ldc_I4, value);
                    }
                    break;
            }
        }

        /// <summary>
        /// Internal use only.
        /// </summary>
        /// <param name="value">The object to convert to a character.</param>
        //[Browsable(false)]
        //[EditorBrowsable(EditorBrowsableState.Never)]
        //[Obsolete(ObsoleteInternalUsageOnly, false)]
        public static char ReadChar(object value)
        {
            if (value == null || value is DBNull) throw new ArgumentNullException(nameof(value));
            if (value is string s && s.Length == 1) return s[0];
            if (value is char c) return c;
            throw new ArgumentException("A single-character was expected", nameof(value));
        }

        /// <summary>
        /// Internal use only.
        /// </summary>
        /// <param name="value">The object to convert to a character.</param>
        //[Browsable(false)]
        //[EditorBrowsable(EditorBrowsableState.Never)]
        //[Obsolete(ObsoleteInternalUsageOnly, false)]
        public static char? ReadNullableChar(object value)
        {
            if (value == null || value is DBNull) return null;
            if (value is string s && s.Length == 1) return s[0];
            if (value is char c) return c;
            throw new ArgumentException("A single-character was expected", nameof(value));
        }

        /// <summary>
        /// Throws a data exception, only used internally
        /// </summary>
        /// <param name="ex">The exception to throw.</param>
        /// <param name="index">The index the exception occured at.</param>
        /// <param name="reader">The reader the exception occured in.</param>
        /// <param name="value">The value that caused the exception.</param>
        //[Obsolete(ObsoleteInternalUsageOnly, false)]
        public static void ThrowDataException(Exception ex, int index, IDataReader reader, object value)
        {
            Exception toThrow;
            try
            {
                string name = "(n/a)", formattedValue = "(n/a)";
                if (reader != null && index >= 0 && index < reader.FieldCount)
                {
                    name = reader.GetName(index);
                    try
                    {
                        if (value == null || value is DBNull)
                        {
                            formattedValue = "<null>";
                        }
                        else
                        {
                            formattedValue = Convert.ToString(value) + " - " + Type.GetTypeCode(value.GetType());
                        }
                    }
                    catch (Exception valEx)
                    {
                        formattedValue = valEx.Message;
                    }
                }
                toThrow = new DataException($"Error parsing column {index} ({name}={formattedValue})", ex);
            }
            catch
            { // throw the **original** exception, wrapped as DataException
                toThrow = new DataException(ex.Message, ex);
            }
            throw toThrow;
        }
     
    }
}