using System.IO;
using Zapos.Common.Generators;

namespace Zapos.Constructors.Razor.Generators
{
    public class RazorTextGenerator : ITextGenerator
    {
        public string Generate<T>(string filePath, T model)
        {
            var content = File.ReadAllText(filePath);

            var result = RazorEngine.Razor.Parse(content, model, filePath);

            return result;
        }
    }
}