using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

namespace DataVisit
{
    public class Visitor
    {
        static Dictionary<Type, List<ReflectionInfo>> dictRef;

        static FieldReflectionInfo rootReflection;

        public class Pos
        {
            internal int index;
            internal object obj;
        }

        public static object Get(string raw)
        {
            int intRslt;
            if (int.TryParse(raw, out intRslt))
            {
                return intRslt;
            }

            double doubleRslt;
            if (double.TryParse(raw, out doubleRslt))
            {
                return doubleRslt;
            }

            if (!raw.Contains("."))
            {
                return raw;
            }

            var splits = raw.Split('.');
            if (splits.Length < 2)
            {
                throw new Exception("error value " + raw);
            }

            return VisitGet(raw);
        }

        public static object Get(string raw, Pos pos)
        {
            object rslt = pos.obj;
            foreach (var field in dictMap[raw])
            {
                rslt = field.GetValue(rslt);
            }

            return rslt;
        }

        public static void Set(string raw, object value)
        {
            object rslt = rootObj;

            var fields = dictMap[raw];
            if (enumerateObj != null && fields[0].GetDeclaringType() == enumerateObj.GetType())
            {
                rslt = enumerateObj;
            }

            for (int i=0; i< fields.Length-1; i++)
            {
                rslt = fields[i].GetValue(rslt);
            }

            fields[fields.Length - 1].SetValue(rslt, value);
        }

        public static void SetVisitData(object data)
        {
            rootObj = data;
        }

        public static void InitVisitMap(Type type)
        {
            rootReflection = new FieldReflectionInfo(type);
        }

        public static bool EnumerateVisit(string key, ref Pos pos)
        {
            var objs = VisitGet(key);
            var list = objs as IList;
            if(list == null)
            {
                throw new Exception($"EnumerateVisit error, {key} must be List<>");
            }

            if (pos == null)
            {
                pos = new Pos() { index = 0 };
            }
            else
            {
                pos.index++;
            }

            if (pos.index + 1 > list.Count)
            {
                enumerateObj = null;
                return false;
            }

            pos.obj = list[pos.index];
            enumerateObj = pos.obj;

            return true;
        }


        private static Dictionary<string, ReflectionInfo[]> AnaylizeDataVisitorProperty(Type type, ReflectionInfo[] parents)
        {
            var rslt = new Dictionary<string, ReflectionInfo[]>();

            var fieldVisitDict = AnaylizeFields(type, parents);
            var propertyVisitDict = AnaylizeProperties(type, parents);

            foreach (var elem in fieldVisitDict)
            {
                rslt.Add(elem.Key, elem.Value);
            }

            foreach (var elem in propertyVisitDict)
            {
                rslt.Add(elem.Key, elem.Value);
            }

            return rslt;
        }

        private static Dictionary<string, ReflectionInfo[]> AnaylizeProperties(Type type, ReflectionInfo[] parents)
        {
            var rslt = new Dictionary<string, ReflectionInfo[]>();

            var properties = type.GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance);
            foreach (var property in properties)
            {
                var visitPropery = (DataVisitorProperty)Attribute.GetCustomAttribute(property, typeof(DataVisitorProperty));
                if (visitPropery == null)
                {
                    continue;
                }

                if (visitPropery.key == "")
                {
                    throw new Exception("DataVisitorProperty of property must have vaild key!");
                }

                var reflectionInfos = new List<ReflectionInfo>(parents);
                reflectionInfos.Add(new PropertyReflectionInfo(property));

                rslt.Add(visitPropery.key, reflectionInfos.ToArray());
            }

            return rslt;
        }

        private static Dictionary<string, ReflectionInfo[]> AnaylizeFields(Type type, ReflectionInfo[] parents)
        {
            var rslt = new Dictionary<string, ReflectionInfo[]>();

            var fields = type.GetFields(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance);

            foreach (var field in fields)
            {
                var visitArrayPropery = (DataVisitorPropertyArray)Attribute.GetCustomAttribute(field, typeof(DataVisitorPropertyArray));
                if (visitArrayPropery != null)
                {
                    var subDict = AnaylizeDataVisitorPropertyArray(field, visitArrayPropery, parents);
                    foreach (var set in subDict)
                    {
                        rslt.Add(set.Key, set.Value);
                    }
                    continue;
                }

                var visitPropery = (DataVisitorProperty)Attribute.GetCustomAttribute(field, typeof(DataVisitorProperty));
                if (visitPropery == null)
                {
                    continue;
                }

                if (visitPropery.key != "")
                {
                    var reflectionInfos = new List<ReflectionInfo>(parents);
                    reflectionInfos.Add(new FieldReflectionInfo(field));

                    rslt.Add(visitPropery.key, reflectionInfos.ToArray());
                }
                else
                {
                    var reflectionInfos = new List<ReflectionInfo>(parents);
                    reflectionInfos.Add(new FieldReflectionInfo(field));

                    var subDict = AnaylizeDataVisitorProperty(field.FieldType, reflectionInfos.ToArray());
                    foreach (var set in subDict)
                    {
                        rslt.Add(set.Key, set.Value);
                    }
                }
            }

            return rslt;
        }

        private static Dictionary<string, ReflectionInfo[]> AnaylizeDataVisitorPropertyArray(FieldInfo field, DataVisitorPropertyArray visitArrayPropery, ReflectionInfo[] parents)
        {
            var rslt = new Dictionary<string, ReflectionInfo[]>();

            if (visitArrayPropery.key == "")
            {
                throw new Exception("DataVisitorPropertyArray of property must have vaild key!");
            }

            var reflectionInfos = new List<ReflectionInfo>(parents);
            reflectionInfos.Add(new FieldReflectionInfo(field));

            rslt.Add(visitArrayPropery.key, reflectionInfos.ToArray());


            Type[] listParameters = field.FieldType.GetGenericArguments();

            var subDict = AnaylizeDataVisitorProperty(listParameters[0], new ReflectionInfo[] { });
            foreach (var set in subDict)
            {
                rslt.Add(set.Key, set.Value);
            }

            return rslt;
        }

        private static object VisitGet(string raw)
        {
            var splits = raw.Split('.');

            dictRef.Values.Single(x=>x.)

            // object rslt = obj;

            // var fields = dictMap[raw];
            // if(enumerateObj != null && fields[0].GetDeclaringType() == enumerateObj.GetType())
            // {
            //     rslt = enumerateObj;
            // }

            // foreach (var field in dictMap[raw])
            // {
            //     rslt = field.GetValue(rslt);
            // }

            // return rslt;
        }

        private static Dictionary<string, ReflectionInfo[]> dictMap;
        private static object rootObj;
        private static object enumerateObj;
    }

    internal abstract class ReflectionInfo
    {
        internal string name;

        internal abstract object GetValue(object rslt);

        internal abstract void SetValue(object rslt, object value);

        internal abstract Type GetDataType();

        internal abstract Type GetDeclaringType();

        internal abstract (ReflectionInfo reflectionInfo, object obj) GetChild(string name, object currObj);
    }

    internal class FieldReflectionInfo : ReflectionInfo
    {
        internal FieldReflectionInfo(string name, FieldInfo field)
        {
            var fields = AnaylizeFields(field.FieldType);
            var propertys = AnaylizeProperties(field.FieldType);

            this.name = name;
            this.field = field;

            subReflectionInfos.Add(fields.Select(x => new FieldReflectionInfo(x.name, x.field)));
            subReflectionInfos.Add(propertys.Select(x => new PropertyReflectionInfo(x.name, x.field)));
        }

        internal FieldReflectionInfo(FieldInfo field)
        {
            this.field = field;
        }

        internal override object GetValue(object obj)
        {
            return field.GetValue(obj);
        }

        internal override void SetValue(object obj, object value)
        {
            field.SetValue(obj, value);
        }

        internal override Type GetDataType()
        {
            return field.FieldType;
        }

        internal override Type GetDeclaringType()
        {
            return field.DeclaringType;
        }

        internal override (ReflectionInfo reflectionInfo, object obj) GetChild(string name, object currObj)
        {
            var obj = GetValue(currObj);
            var reflectionInfo = subReflectionInfos.Single(x => x.name == name);
            return (reflectionInfo, obj);

        }

        internal List<ReflectionInfo> subReflectionInfos = new List<ReflectionInfo>();
        private FieldInfo field;

    }

    internal class PropertyReflectionInfo : ReflectionInfo
    {
        internal PropertyReflectionInfo(string name, PropertyInfo field)
        {
            this.name = name;
            this.property = field;
        }


        internal PropertyReflectionInfo(PropertyInfo property)
        {
            this.property = property;
        }

        internal override object GetValue(object obj)
        {
            return property.GetValue(obj);
        }

        internal override void SetValue(object obj, object value)
        {
            property.SetValue(obj, value);
        }

        internal override Type GetDataType()
        {
            return property.PropertyType;
        }

        internal override Type GetDeclaringType()
        {
            return property.DeclaringType;
        }

        internal override (ReflectionInfo reflectionInfo, object obj) GetChild(string name, object currObj)
        {
            var obj = GetValue(currObj);
            return (null, obj);
        }

        private PropertyInfo property;
    }

    [AttributeUsage(AttributeTargets.Field|AttributeTargets.Property)]
    public class DataVisitorProperty : Attribute
    {
        public DataVisitorProperty(string key)
        {
            this.key = key;
        }

        public DataVisitorProperty()
        {
            this.key = "";
        }

        internal string key;
    }

    [AttributeUsage(AttributeTargets.Field)]
    public class DataVisitorPropertyArray : Attribute
    {
        public DataVisitorPropertyArray(string key)
        {
            this.key = key;
        }

        public DataVisitorPropertyArray()
        {
            this.key = "";
        }

        internal string key;
    }
}
