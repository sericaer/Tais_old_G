using System;
using System.IO;
namespace Modder.UnitTest
{
    public class ModFileSystem
    {
        public static string path = "../../test_data/";

        public string name;
        public string modPath
        {
            get
            {
                return path + name + "/";
            }
        }

        internal void AddCommonEvent(string fileName, string fileContent)
        {
            var commonPath = modPath + "events/common/";
            Directory.CreateDirectory(commonPath);

            File.WriteAllText(commonPath + fileName, fileContent);
        }

        internal void AddDepartEvent(string fileName, string fileContent)
        {
            var commonPath = modPath + "events/depart/";
            Directory.CreateDirectory(commonPath);

            File.WriteAllText(commonPath + fileName, fileContent);
        }

        public static ModFileSystem Generate(string modName)
        {
            return new ModFileSystem() { name = modName };
        }

        internal static void Clear()
        {
            foreach(var dir in Directory.EnumerateDirectories(path))
            {
                Directory.Delete(dir, true);
            }
            
        }
    }
}
