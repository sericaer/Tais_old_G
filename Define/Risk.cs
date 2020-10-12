using System;
using System.Collections.Generic;
using System.IO;
using Parser.Semantic;

namespace Define
{
    public class Risk
    {
        [SemanticProperty("cost_days")]
        public double cost_days;

        private string file;

        public string key
        {
            get
            {
                return Path.GetFileNameWithoutExtension(file);
            }
        }

        internal static List<Risk> Load(string path)
        {
            List<Risk> rslt = new List<Risk>();

            if (!Directory.Exists(path))
            {
                return rslt;
            }

            foreach (var file in Directory.EnumerateFiles(path, "*.txt"))
            {
                var risk = DefElementLoader.Load<Risk>(file, File.ReadAllText(file));
                risk.file = file;

                rslt.Add(risk);
            }

            return rslt;
        }
    }
}
