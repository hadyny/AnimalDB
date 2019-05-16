using AnimalDB.Repo.Entities;
using AnimalDB.Repo.Interfaces;
using AnimalDB.Repo.Repositories.Abstract;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnimalDB.Repo.Services
{
    public class RackEntryService : IRackEntryService
    {
        private readonly IRepository<RackEntry> _rackEntries;

        public RackEntryService(IRepository<RackEntry> rackEntries)
        {
            this._rackEntries = rackEntries;
        }

        public async Task CreateRackEntry(RackEntry rackEntry)
        {
            _rackEntries.Insert(rackEntry);
            await _rackEntries.Save();
        }

        public async Task DeleteRackEntry(RackEntry rackEntry)
        {
            await _rackEntries.Delete(rackEntry);
            await _rackEntries.Save();
        }

        public async Task<RackEntry> GetRackEntryById(int id)
        {
            return await _rackEntries.GetById(id);
        }

        public async Task<IEnumerable<RackEntry>> GetRackEntriesByAnimalId(int animalId)
        {
            return await _rackEntries.GetAll(m => m.Animal_Id == animalId);
        }

        public async Task<IEnumerable<RackEntry>> GetRackEntries()
        {
            return await _rackEntries.GetAll();
        }

        public async Task UpdateRackEntry(RackEntry rackEntry)
        {
            _rackEntries.Update(rackEntry);
            await _rackEntries.Save();
        }
    }
}
