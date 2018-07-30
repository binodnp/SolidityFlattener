using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SolidityFlattener
{
    public sealed class PathUtility
    {
        private readonly IEnumerable<string> _searchPaths;
        private readonly string _sourceDirectory;

        public PathUtility(IEnumerable<string> searchPaths, string sourceDirectory)
        {
            this._searchPaths = searchPaths;
            this._sourceDirectory = sourceDirectory;
        }

        public string GetPath(string statement)
        {
            string path = statement.Replace("import", "").Replace("\"", "").Replace("'", "").Replace(";", "").Trim();

            foreach (string search in this._searchPaths)
            {
                string file = Path.Combine(this._sourceDirectory, search, path);

                if (File.Exists(file))
                {
                    return new FileInfo(file).FullName;
                }
                string fileName = Path.GetFileName(path);
                string directory = Path.Combine(this._sourceDirectory, search);

                var files = Directory.EnumerateFiles(directory, fileName, SearchOption.AllDirectories).ToList();
                string result = files.FirstOrDefault();

                if (result != null)
                {
                    return new FileInfo(result).FullName;
                }
            }

            return string.Empty;
        }
    }
}