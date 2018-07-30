using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace SolidityFlattener
{
    public sealed class Flattener
    {
        private readonly List<string> _imports;
        private readonly IEnumerable<string> _searchPaths;
        private readonly string _sourceDirectory;

        public Flattener(IEnumerable<string> searchPaths, string sourceDirectory)
        {
            this._imports = new List<string>();
            this._searchPaths = searchPaths;
            this._sourceDirectory = sourceDirectory;
        }

        public string Flatten(string contents, string statement)
        {
            var utility = new PathUtility(this._searchPaths, this._sourceDirectory);
            string path = utility.GetPath(statement);
            bool contains = this._imports.Contains(path);

            if (!string.IsNullOrWhiteSpace(path) && !contains)
            {
                this._imports.Add(path);
                string import = File.ReadAllText(path);

                Console.WriteLine($"Importing {path}");

                var pattern = new Regex("pragma.*?;", RegexOptions.IgnoreCase);
                import = pattern.Replace(import, "");


                contents = contents.Replace(statement, import);
            }

            if (contains)
            {
                contents = contents.Replace(statement, "");
            }

            return contents;
        }
    }
}