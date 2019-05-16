using AnimalDB.Repo.Entities;
using AnimalDB.Repo.Interfaces;
using AnimalDB.Repo.Repositories.Abstract;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnimalDB.Repo.Services
{
    public class CageLocationHistoryService : ICageLocationHistoryService
    {
        private readonly IRepository<CageLocationHistory> _cageLocationHistory;

        public CageLocationHistoryService(IRepository<CageLocationHistory> cageLocationHistory)
        {
            _cageLocationHistory = cageLocationHistory;
        }

        public async Task CreateCageLocationHistory(CageLocationHistory cageLocationHistory)
        {
            _cageLocationHistory.Insert(cageLocationHistory);
            await _cageLocationHistory.Save();
        }

        public async Task DeleteCageLocationHistory(CageLocationHistory cageLocationHistory)
        {
            await _cageLocationHistory.Delete(cageLocationHistory);
            await _cageLocationHistory.Save();
        }

        public async Task<IEnumerable<CageLocationHistory>> GetCageLocationHistories()
        {
            return await _cageLocationHistory.GetAll();
        }

        public async Task<CageLocationHistory> GetCageLocationHistoryById(int id)
        {
            return await _cageLocationHistory.GetById(id);
        }

        public async Task<IEnumerable<CageLocationHistory>> GetCageLocationHistoryByAnimalId(int animalId)
        {
            var histories = await _cageLocationHistory.GetAll(m => m.Animal_Id == animalId);
            return histories
                    .OrderByDescending(m => m.Timestamp);
        }
        
        public async Task UpdateCageLocationHistory(CageLocationHistory cageLocationHistory)
        {
            _cageLocationHistory.Update(cageLocationHistory);
            await _cageLocationHistory.Save();
        }
    }
}
