using AnimalDB.Repo.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AnimalDB.Repo.Interfaces
{
    public interface ISourceService
    {
        Task<IEnumerable<Source>> GetSources();

        Task CreateSource(Source source);

        Task<Source> GetSourceById(int id);

        Task UpdateSource(Source source);

        Task DeleteSource(Source source);
    }
}
