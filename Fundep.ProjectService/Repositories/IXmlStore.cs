using System.Collections.Generic;

namespace Fundep.ProjectService.Repositories
{
    public interface IXmlStore<T>
    {
        List<T> LoadAll();
        void SaveAll(List<T> items);
    }
}
