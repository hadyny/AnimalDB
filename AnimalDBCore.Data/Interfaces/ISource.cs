using   AnimalDBCore.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace   AnimalDBCore.Core.Interfaces
{
    public interface ISource
    {
        IEnumerable<Source> GetSources();

        Task CreateSource(Source source);

        Task<Source> GetSourceById(int id);

        Task UpdateSource(Source source);

        Task DeleteSource(Source source);
    }
}
