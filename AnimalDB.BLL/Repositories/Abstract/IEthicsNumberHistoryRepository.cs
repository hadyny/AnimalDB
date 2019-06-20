using AnimalDB.Repo.Entities;
using System.Collections.Generic;

namespace AnimalDB.Repo.Repositories.Abstract
{
    public interface IEthicsNumberHistoryRepository : IRepository<EthicsNumberHistory>
    {
        IEnumerable<EthicsNumberHistory> GetByAnimal(int animal_Id);
    }
}
