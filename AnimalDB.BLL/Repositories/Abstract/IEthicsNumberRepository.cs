using AnimalDB.Repo.Entities;
using System.Collections.Generic;

namespace AnimalDB.Repo.Repositories.Abstract
{
    public interface IEthicsNumberRepository : IRepository<EthicsNumber>
    {
        IEnumerable<EthicsNumberHistory> GetByEthicsId(int ethicsId);
        void Archive(EthicsNumber ethicsNumber);
        EthicsNumber GetByName(string name);
        IEnumerable<EthicsNumber> GetArchived();
        IEnumerable<EthicsNumberHistory> GetHistoryByEthicsId(int ethicsId);
    }
}
