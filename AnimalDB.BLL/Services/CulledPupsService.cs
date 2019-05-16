using AnimalDB.Repo.Entities;
using AnimalDB.Repo.Interfaces;
using AnimalDB.Repo.Repositories.Abstract;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnimalDB.Repo.Services
{
    public class CulledPupsService : ICulledPupsService
    {
        private readonly IRepository<CulledPups> _culledPups;

        public CulledPupsService(IRepository<CulledPups> culledPups)
        {
            this._culledPups = culledPups;
        }

        public async Task CreateCulledPups(CulledPups culledPups)
        {
            _culledPups.Insert(culledPups);
            await _culledPups.Save();
        }

        public async Task DeleteCulledPups(CulledPups culledPups)
        {
            await _culledPups.Delete(culledPups);
            await _culledPups.Save();
        }

        public async Task<CulledPups> GetCulledPupsById(int id)
        {
            return await _culledPups.GetById(id);
        }

        public async Task<IEnumerable<CulledPups>> GetCulledPups()
        {
            return await _culledPups.GetAll();
        }

        public async Task UpdateCulledPups(CulledPups culledPups)
        {
            _culledPups.Update(culledPups);
            await _culledPups.Save();
        }

        public async Task<IEnumerable<CulledPups>> GetCulledPupsByAnimalId(int animalId)
        {
            return await _culledPups.GetAll(m => m.AnimalId == animalId);
        }
    }
}
