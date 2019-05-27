using AnimalDB.Repo.Entities;
using AnimalDB.Repo.Interfaces;
using AnimalDB.Repo.Repositories.Abstract;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AnimalDB.Repo.Services
{
    public class CageLocationService : ICageLocationService
    {
        private readonly IRepository<CageLocation> _cageLocations;

        public CageLocationService(IRepository<CageLocation> cageLocations)
        {
            _cageLocations = cageLocations;
        }

        public async Task CreateCageLocation(CageLocation cageLocation)
        {
            _cageLocations.Insert(cageLocation);
            await _cageLocations.Save();
        }

        public async Task DeleteCageLocation(CageLocation cageLocation)
        {
            await _cageLocations.Delete(cageLocation.Id);
            await _cageLocations.Save();
        }

        public async Task<CageLocation> GetCageLocationById(int id)
        {
            return await _cageLocations.GetById(id);
        }

        public async Task<IEnumerable<CageLocation>> GetCageLocations()
        {
            return await _cageLocations.GetAll();
        }

        public async Task UpdateCageLocation(CageLocation cageLocation)
        {
            _cageLocations.Update(cageLocation);
            await _cageLocations.Save();
        }
    }
}
