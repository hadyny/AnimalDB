using   AnimalDBCore.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace   AnimalDBCore.Core.Interfaces
{
    public interface IEthicsNumberHistory
    {
        IEnumerable<EthicsNumberHistory> GetEthicsNumberHistories();

        Task CreateEthicsNumberHistory(EthicsNumberHistory ethicsNumberHistory);

        Task<EthicsNumberHistory> GetEthicsNumberHistoryById(int id);

        Task UpdateEthicsNumberHistory(EthicsNumberHistory ethicsNumberHistory);

        Task DeleteEthicsNumberHistory(EthicsNumberHistory ethicsNumberHistory);

        IEnumerable<EthicsNumberHistory> GetEthicsNumberHistoriesByAnimal(int animal_Id);
    }
}
