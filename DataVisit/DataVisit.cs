using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

namespace DataVisit
{
    public class Visitor
    {
        static Dictionary<Type, List<ReflectionInfo>> dictRef = new Dictionary<Type, List<ReflectionInfo>>();

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
            var splits = raw.Split('.');

            ReflectionInfo currReflection = dictRef[rootObj.GetType()].Single(x => x.name == splits[0]);
            var currObj = currReflection.GetValue(rootObj);

            for (int i = 1; i < splits.Length-1; i++)
            {
                currReflection = dictRef[currObj.GetType()].Single(x => x.name == splits[i]);
                currObj = currReflection.GetValue(currObj);
            }

            var leaf = dictRef[currReflection.GetDataType()].Single(x => x.name == splits[splits.Length - 1]);
            leaf.SetValue(currObj, value);
        }

        public static void SetVisitData(object data)
        {
            rootObj = data;
        }

        public static void InitVisitMap(List<Type> types)
        {
            foreach(var type in  types)
            {
                var reflectionList = new List<ReflectionInfo>();
                reflectionList.AddRange(AnaylizeFields(type));
                reflectionList.AddRange(AnaylizeProperties(type));

                dictRef.Add(type, reflectionList);
            }
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

        internal static List<FieldReflectionInfo> AnaylizeFields(Type type)
        {
            var rslt = new List<FieldReflectionInfo>();

            var fields = type.GetFields(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance);

            foreach (var field in fields)
            {
                var visitPropery = (DataVisitorProperty)Attribute.GetCustomAttribute(field, typeof(DataVisitorProperty));
                if (visitPropery == null)
                {
                    continue;
                }

                rslt.Add(new FieldReflectionInfo(visitPropery.key, field));
            }

            return rslt;
        }

        internal static List<PropertyReflectionInfo> AnaylizeProperties(Type type)
        {
            var rslt = new List<PropertyReflectionInfo>();

            var properties = type.GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance);

            foreach (var property in properties)
            {
                var visitPropery = (DataVisitorProperty)Attribute.GetCustomAttribute(property, typeof(DataVisitorProperty));
                if (visitPropery == null)
                {
                    continue;
                }

                rslt.Add(new PropertyReflectionInfo(visitPropery.key, property));
            }

            return rslt;
        }

        private static object VisitGet(string raw)
        {
            var splits = raw.Split('.');

            ReflectionInfo currReflection = dictRef[rootObj.GetType()].Single(x => x.name == splits[0]);
            var currObj = currReflection.GetValue(rootObj);

            for(int i=1; i<splits.Length; i++)
            {
                currReflection = dictRef[currObj.GetType()].Single(x => x.name == splits[i]);
                currObj = currReflection.GetValue(currObj);
            }

            return currObj;
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

        internal abstract ReflectionInfo GetChild(string name);

        internal abstract (ReflectionInfo reflectionInfo, object obj) GetChild(string name, object currObj);
    }

    internal class FieldReflectionInfo : ReflectionInfo
    {
        internal FieldReflectionInfo(string name, FieldInfo field)
        {
            var fields = Visitor.AnaylizeFields(field.FieldType);
            var propertys = Visitor.AnaylizeProperties(field.FieldType);

            this.name = name;
            this.field = field;
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

        internal override ReflectionInfo GetChild(string name)
        {
            throw new NotImplementedException();
        }

        internal override (ReflectionInfo reflectionInfo, object obj) GetChild(string name, object currObj)
        {
            throw new NotImplementedException();
        }

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

        internal override ReflectionInfo GetChild(string name)
        {
            throw new NotImplementedException();
        }

        internal PropertyInfo property;
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
