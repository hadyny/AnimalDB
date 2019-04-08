using   AnimalDBCore.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace   AnimalDBCore.Core.Interfaces
{
    public interface IEthicsNumber
    {
        IEnumerable<EthicsNumber> GetEthicsNumbers();

        IEnumerable<EthicsNumber> GetArchivedNumbers();

        Task CreateEthicsNumber(EthicsNumber ethicsNumber);

        Task<EthicsNumber> GetEthicsNumberById(int id);

        IEnumerable<EthicsNumberHistory> GetEthicsNumberHistoryByEthicsId(int ethicsId);

        Task UpdateEthicsNumber(EthicsNumber ethicsNumber);

        Task DeleteEthicsNumber(EthicsNumber ethicsNumber);

        Task ArchiveEthics(EthicsNumber ethicsNumber);

        Task<EthicsNumber> GetEthicsNumberByName(string name);
    }
}
