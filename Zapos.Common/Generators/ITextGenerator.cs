namespace Zapos.Common.Generators
{
    public interface ITextGenerator
    {
        string Generate<T>(string filePath, T model);
    }
}