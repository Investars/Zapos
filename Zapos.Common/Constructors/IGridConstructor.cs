using Zapos.Common.DocumentModel;

namespace Zapos.Common.Constructors
{
    public interface IGridConstructor
    {
        Table CreateTable<TModel>(string filePath, TModel model);
    }
}