using AnimalDB.Repo.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AnimalDB.Repo.Interfaces
{
    public interface IEthicsNumberService
    {
        Task<IEnumerable<EthicsNumber>> GetEthicsNumbers();

        Task<IEnumerable<EthicsNumber>> GetArchivedNumbers();

        Task CreateEthicsNumber(EthicsNumber ethicsNumber);

        Task<EthicsNumber> GetEthicsNumberById(int id);

        Task<IEnumerable<EthicsNumberHistory>> GetEthicsNumberHistoryByEthicsId(int ethicsId);

        Task UpdateEthicsNumber(EthicsNumber ethicsNumber);

        Task DeleteEthicsNumber(EthicsNumber ethicsNumber);

        Task ArchiveEthics(EthicsNumber ethicsNumber);

        Task<EthicsNumber> GetEthicsNumberByName(string name);
    }
}
