using AnimalDB.Repo.Entities;
using AnimalDB.Repo.Interfaces;
using AnimalDB.Repo.Repositories.Abstract;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnimalDB.Repo.Services
{
    public class EthicsNumberHistoryService : IEthicsNumberHistoryService
    {
        private readonly IRepository<EthicsNumberHistory> _ethicsNumberHistories;

        public EthicsNumberHistoryService(IRepository<EthicsNumberHistory> ethicsNumberHistories)
        {
            this._ethicsNumberHistories = ethicsNumberHistories;
        }

        public async Task CreateEthicsNumberHistory(EthicsNumberHistory ethicsNumberHistory)
        {
            _ethicsNumberHistories.Insert(ethicsNumberHistory);
            await _ethicsNumberHistories.Save();
        }

        public async Task DeleteEthicsNumberHistory(EthicsNumberHistory ethicsNumberHistory)
        {
            await _ethicsNumberHistories.Delete(ethicsNumberHistory.Id);
            await _ethicsNumberHistories.Save();
        }

        public async Task<EthicsNumberHistory> GetEthicsNumberHistoryById(int id)
        {
            return await _ethicsNumberHistories.GetById(id);
        }

        public async Task<IEnumerable<EthicsNumberHistory>> GetEthicsNumberHistories()
        {
            return await _ethicsNumberHistories.GetAll();
        }

        public async Task UpdateEthicsNumberHistory(EthicsNumberHistory ethicsNumberHistory)
        {
            _ethicsNumberHistories.Update(ethicsNumberHistory);
            await _ethicsNumberHistories.Save();
        }

        public async Task<IEnumerable<EthicsNumberHistory>> GetEthicsNumberHistoriesByAnimal(int animal_Id)
        {
            var histories = await _ethicsNumberHistories.GetAll(m => m.Animal_Id == animal_Id);
            return histories
                        .OrderByDescending(m => m.Timestamp);
        }
    }
}
