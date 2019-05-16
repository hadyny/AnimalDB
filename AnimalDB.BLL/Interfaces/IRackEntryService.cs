using AnimalDB.Repo.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AnimalDB.Repo.Interfaces
{
    public interface IRackEntryService
    {
        Task<IEnumerable<RackEntry>> GetRackEntries();

        Task CreateRackEntry(RackEntry rackEntry);

        Task<RackEntry> GetRackEntryById(int id);

        Task<IEnumerable<RackEntry>> GetRackEntriesByAnimalId(int animalId);

        Task UpdateRackEntry(RackEntry rackEntry);

        Task DeleteRackEntry(RackEntry rackEntry);
    }
}
