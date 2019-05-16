using AnimalDB.Repo.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AnimalDB.Repo.Interfaces
{
    public interface IEthicsNumberHistoryService
    {
        Task<IEnumerable<EthicsNumberHistory>> GetEthicsNumberHistories();

        Task CreateEthicsNumberHistory(EthicsNumberHistory ethicsNumberHistory);

        Task<EthicsNumberHistory> GetEthicsNumberHistoryById(int id);

        Task UpdateEthicsNumberHistory(EthicsNumberHistory ethicsNumberHistory);

        Task DeleteEthicsNumberHistory(EthicsNumberHistory ethicsNumberHistory);

        Task<IEnumerable<EthicsNumberHistory>> GetEthicsNumberHistoriesByAnimal(int animal_Id);
    }
}
