using System;
using System.IO;
using System.Linq;
using System.Text;

namespace SolidityFlattener
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            if (args.Length != 3)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("An error has occured. Expected parameters: Source File, Destination File, Search Paths.");
                Console.ReadLine();
                return;
            }

            string source = args[0];
            string destination = args[1];
            string searchPaths = args[2];

            var processor = new Processor(source, searchPaths.Split(',').Select(x => x.Trim()));
            string contents = processor.Process();

            File.WriteAllText(destination, contents, new UTF8Encoding(false));
        }
    }
}