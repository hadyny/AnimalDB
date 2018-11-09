using AnimalDB.Repo.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AnimalDB.Repo.Interfaces
{
    public interface IRackEntry
    {
        IEnumerable<RackEntry> GetRackEntries();

        Task CreateRackEntry(RackEntry rackEntry);

        Task<RackEntry> GetRackEntryById(int id);

        IEnumerable<RackEntry> GetRackEntriesByAnimalId(int animalId);

        Task UpdateRackEntry(RackEntry rackEntry);

        Task DeleteRackEntry(RackEntry rackEntry);
    }
}
