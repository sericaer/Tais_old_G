using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Modder;

namespace TaisGodot.Scripts
{
    static class TranslateServerEx
    {
        internal static void AddTranslate(List<Language> languages)
        {
            foreach (var language in languages)
            {
                Translation tran = GetTranslate(language.locale);

                foreach (var pair in language.dict)
                {
                    tran.AddMessage(pair.Key, pair.Value);
                }
            }
        }

        internal static string Translate(string format, params string[] pp)
        {
            var tranFormat = TranslationServer.Translate(format);
            if(pp.Count() == 0)
            {
                return tranFormat;
            }

            return String.Format(tranFormat, pp);
        }

        private static Translation GetTranslate(string locale)
        {
            Translation tran;
            if (!tranDict.TryGetValue(locale, out tran))
            {
                tran = new Translation();
                tran.Locale = locale;

                tranDict.Add(locale, tran);
                TranslationServer.AddTranslation(tran);
            }

            return tran;
        }

        private static Dictionary<string, Translation> tranDict = new Dictionary<string, Translation>();
    }
}
