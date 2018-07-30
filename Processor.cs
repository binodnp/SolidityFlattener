using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace SolidityFlattener
{
    internal sealed class Processor
    {
        private readonly Regex _pattern = new Regex("import.*?;", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        private readonly IEnumerable<string> _searchPaths;
        private readonly string _source;

        public Processor(string source, IEnumerable<string> searchPaths)
        {
            this._source = source;
            this._searchPaths = searchPaths;
        }

        public string Process()
        {
            string sourceDirectory = new FileInfo(this._source).DirectoryName;
            string contents = File.ReadAllText(this._source);

            var flattener = new Flattener(this._searchPaths, sourceDirectory);

            while (this._pattern.Match(contents).Success)
            {
                string matched = this._pattern.Match(contents).Value;
                contents = flattener.Flatten(contents, matched);
            }

            return contents;
        }
    }
}