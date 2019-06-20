using AnimalDB.Repo.Contexts;
using AnimalDB.Repo.Entities;
using AnimalDB.Repo.Repositories.Abstract;
using AnimalDB.Repositories.Concrete;
using System.Collections.Generic;
using System.Linq;

namespace AnimalDB.Repo.Repositories.Concrete
{
    public class EthicsNumberHistoryRepository : Repository<EthicsNumberHistory>, IEthicsNumberHistoryRepository
    {
        public EthicsNumberHistoryRepository(AnimalDBContext context) : base(context)
        {
        }

        public IEnumerable<EthicsNumberHistory> GetByAnimal(int animal_Id)
        {
            return Context
                .EthicsNumberHistories
                .Where(m => m.Animal_Id == animal_Id)
                .OrderByDescending(m => m.Timestamp)
                .ToList();
        }

        public AnimalDBContext Context
        {
            get { return base.db; }
        }
    }
}
