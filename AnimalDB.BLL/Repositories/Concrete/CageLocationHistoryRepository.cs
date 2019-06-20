using AnimalDB.Repo.Contexts;
using AnimalDB.Repo.Entities;
using AnimalDB.Repo.Repositories.Abstract;
using AnimalDB.Repositories.Concrete;
using System.Collections.Generic;
using System.Linq;

namespace AnimalDB.Repo.Repositories.Concrete
{
    public class CageLocationHistoryRepository : Repository<CageLocationHistory>, ICageLocationHistoryRepository
    {

        public CageLocationHistoryRepository(AnimalDBContext context) : base(context)
        {
        }

        public IEnumerable<CageLocationHistory> GetByAnimalId(int animalId)
        {
            var histories = Context.CageLocationHistories.Where(m => m.Animal_Id == animalId);
            return histories
                    .OrderByDescending(m => m.Timestamp)
                    .ToList();
        }

        public AnimalDBContext Context
        {
            get { return base.db; }
        }
    }
}
