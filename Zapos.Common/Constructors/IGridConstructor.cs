using System.Collections.Generic;
using Zapos.Common.DocumentModel;

namespace Zapos.Common.Constructors
{
    public interface IGridConstructor
    {
        void Init(IDictionary<string, object> config);

        IEnumerable<Table> CreateTables<TModel>(string filePath, TModel model);
    }
}