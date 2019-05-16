using AnimalDB.Repo.Entities;
using AnimalDB.Repo.Interfaces;
using AnimalDB.Repo.Repositories.Abstract;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnimalDB.Repo.Services
{
    public class EthicsNumberService : IEthicsNumberService
    {
        private readonly IRepository<EthicsNumber> _ethicsNumbers;
        private readonly IRepository<EthicsNumberHistory> _ethicsNumberHistories;

        public EthicsNumberService(IRepository<EthicsNumber> ethicsNumbers, IRepository<EthicsNumberHistory> ethicsNumberHistories)
        {
            this._ethicsNumbers = ethicsNumbers;
            this._ethicsNumberHistories = ethicsNumberHistories;
        }

        public async Task CreateEthicsNumber(EthicsNumber ethicsNumber)
        {
            _ethicsNumbers.Insert(ethicsNumber);
            await _ethicsNumbers.Save();
        }

        public async Task DeleteEthicsNumber(EthicsNumber ethicsNumber)
        {
            await _ethicsNumbers.Delete(ethicsNumber);
            await _ethicsNumbers.Save();
        }

        public async Task<EthicsNumber> GetEthicsNumberById(int id)
        {
            return await _ethicsNumbers.GetById(id);
        }

        public async Task<IEnumerable<EthicsNumber>> GetEthicsNumbers()
        {
            return await _ethicsNumbers.GetAll(m => !m.Archived);
        }

        public async Task UpdateEthicsNumber(EthicsNumber ethicsNumber)
        {
            _ethicsNumbers.Update(ethicsNumber);
            await _ethicsNumbers.Save();
        }

        public async Task<IEnumerable<EthicsNumberHistory>> GetEthicsNumberHistoryByEthicsId(int ethicsId)
        {
            var histories = await _ethicsNumberHistories.GetAll(m =>
                            m.Ethics_Id == ethicsId &&
                            m.Animal
                                .EthicsNumbers
                                .OrderByDescending(n => n.Timestamp)
                                .FirstOrDefault().Id == m.Id);
            return histories;
        }

        public async Task ArchiveEthics(EthicsNumber ethicsNumber)
        {
            ethicsNumber.Archived = true;
            _ethicsNumbers.Update(ethicsNumber);
            await _ethicsNumbers.Save();
        }

        public async Task<IEnumerable<EthicsNumber>> GetArchivedNumbers()
        {
            return await _ethicsNumbers.GetAll(m => m.Archived);
        }

        public async Task<EthicsNumber> GetEthicsNumberByName(string name)
        {
            var numbers = await _ethicsNumbers.GetAll(m => m.Text == name);
            return numbers.SingleOrDefault();
        }
    }
}
