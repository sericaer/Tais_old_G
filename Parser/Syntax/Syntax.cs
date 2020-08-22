using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Parser.Syntax
{
    public class SyntaxItem : Value
    {
        public static int line;

        public static SyntaxItem RootParse(String raw)
        {

            line = 0;

            List<Value> synatxNodes = new List<Value>();

            var trim = Regex.Replace(raw, @"[\f\r\t\v ]+", "");

            int charIndex = 0;
            while (charIndex < trim.Length)
            {
                var node = new SyntaxItem(trim, ref charIndex);
                synatxNodes.Add(node);
            }

            return new SyntaxItem("root", synatxNodes);
        }

        public string key;
        public List<Value> values;

        internal SyntaxItem(string key, List<Value> values)
        {
            this.key = key;
            this.values = values;
        }

        internal SyntaxItem(string raw, ref int charIndex)
        {
            int start = charIndex;
            int end = start;

            while (end < raw.Length)
            {
                var elemType = Value.RegexMatch(raw, start, out end);

                switch (elemType)
                {
                    case ELEM_TYPE.KEY:
                        {
                            if (key != null)
                            {
                                throw new Exception("Syntax error");
                            }

                            key = raw.Substring(start, end - start - 1);
                            start = end;
                        }
                        break;
                    case ELEM_TYPE.CR:
                        {
                            line++;
                            start = end;
                            continue;
                        }
                    case ELEM_TYPE.STRING:
                        {
                            if (key == null)
                            {
                                throw new Exception();
                            }

                            values = new List<Value>();
                            values.Add(new StringValue(raw, start, end));
                            charIndex = end;
                        }
                        return;
                    case ELEM_TYPE.BRACE_OPEN:
                        {
                            if (key == null)
                            {
                                throw new Exception();
                            }

                            values = Value.LoadList(raw, ref end);
                            charIndex = end;

                        }
                        return;
                    default:
                        throw new Exception();

                }
            }
        }

        public override string ToString()
        {
            if (values.Count == 1)
            {
                return $"{key} ={{ {values[0]} }}";
            }
            else
            {
                return $"{key} =\n{{ \n {String.Join("\n", values.Select(x => x.ToString()))} \n}}";
            }
        }

        public SyntaxItem Find(string key)
        {
            foreach (var value in values)
            {
                SyntaxItem item = value as SyntaxItem;
                if(item == null)
                {
                    continue;
                }
                if(item.key == key)
                {
                    return item;
                }
            }

            return null;
        }

        public IEnumerable<SyntaxItem> Finds(string key)
        {
            var rslt = new List<SyntaxItem>();

            foreach (var value in values)
            {
                SyntaxItem item = value as SyntaxItem;
                if (item == null)
                {
                    continue;
                }
                if (item.key == key)
                {
                    rslt.Add(item);
                }
            }

            return rslt;
        }

    }

   

    public class StringValue : Value
    {
        String data;
        public StringValue(string raw, int start, int end)
        {
            data = raw.Substring(start, end - start);
        }

        public override string ToString()
        {
            return data;
        }
    }

    public class DigitValue : Value
    {

    }

    internal enum ELEM_TYPE
    {

        CR,
        BRACE_OPEN,
        BRACE_CLOSE,
        STRING,
        KEY,
        COMMA,
    }

}
