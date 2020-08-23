﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

using Parser.Syntax;

namespace Parser.Semantic
{
    public class SemanticParser
    {
        public static T ParserFile<T>(string path)
        {
            try
            {
                var syntaxRoot = SyntaxItem.RootParse(File.ReadAllText(path));

                return DoParser<T>(syntaxRoot);
            }
            catch (Exception e)
            {
                throw new Exception($"Parse error in script:{path}", e);
            }
        }

        public static T DoParser<T>(SyntaxItem syntaxRoot)
        {
            object rslt = Activator.CreateInstance<T>();

            var fields = typeof(T).GetFields(BindingFlags.Public|BindingFlags.NonPublic | BindingFlags.Instance);
            foreach (var field in fields)
            {
                var Properties = field.GetCustomAttributes(typeof(SemanticProperty), false);
                if (Properties.Count() != 0)
                {
                    ParseSemanticProperty(syntaxRoot, Properties.First() as SemanticProperty, field, ref rslt);
                    continue;
                }

                Properties = field.GetCustomAttributes(typeof(SemanticPropertyArray), false);
                if (Properties.Count() != 0)
                {
                    ParseSemanticPropertyList(syntaxRoot, Properties.First() as SemanticPropertyArray, field, ref rslt);
                }
            }

            return (T)rslt;
        }

        private static void ParseSemanticProperty(SyntaxItem syntaxRoot, SemanticProperty property, FieldInfo field, ref object obj)
        {
            var item = syntaxRoot.Find(property.key);
            if (item == null)
            {
                return;
                //throw new Exception($"can not find key:{property.key}");
            }

            field.SetValue(obj, ConvertItem(item, field.FieldType));
        }

        private static void ParseSemanticPropertyList(SyntaxItem syntaxRoot, SemanticPropertyArray property, FieldInfo field, ref object obj)
        {
            var items = syntaxRoot.Finds(property.key);
            if (!items.Any())
            {
                return;
                //throw new Exception($"can not find key:{property.key}");
            }


            if (!field.FieldType.IsGenericType || field.FieldType.GetGenericTypeDefinition() != typeof(List<>))
            {
                throw new Exception($"field type not list! {field.FieldType.FullName}");
            }

            dynamic list = Activator.CreateInstance(field.FieldType);
            
            Type[] listParameters = field.FieldType.GetGenericArguments();

            foreach (var item in items)
            {
                list.Add((dynamic)ConvertItem(item, listParameters[0]));
            }

            field.SetValue(obj, list);
        }

        private static object ConvertItem(SyntaxItem item, Type type)
        {
            if(type.IsValueType)
            {
                if (item.values.Count() != 1 || !(item.values[0] is StringValue))
                {
                    throw new Exception($"type {type.FullName} not support key-value:{item}");
                }

                var strValue = item.values[0] as StringValue;
                if (type == typeof(string))
                {
                    return strValue.ToString();
                }
                else if (type == typeof(double))
                {
                    return double.Parse(strValue.ToString());
                }
                else if (type == typeof(int))
                {
                    return int.Parse(strValue.ToString());
                }
                else
                {
                    throw new Exception($"can not support type {type} with key:{item.key}");
                }
            }
            else
            {
                var ParseMethod = type.GetMethod("Parse", BindingFlags.Static | BindingFlags.Public);
                if (ParseMethod == null)
                {
                    throw new Exception($"can not support type {type}");
                }

                return ParseMethod.Invoke(null, new object[] { item });
            }

        }
    }
}
