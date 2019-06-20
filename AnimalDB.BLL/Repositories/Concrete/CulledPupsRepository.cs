using AnimalDB.Repo.Contexts;
using AnimalDB.Repo.Entities;
using AnimalDB.Repo.Repositories.Abstract;
using AnimalDB.Repositories.Concrete;
using System.Collections.Generic;
using System.Linq;

namespace AnimalDB.Repo.Repositories.Concrete
{
    public class CulledPupsRepository : Repository<CulledPups>, ICulledPupsRepository
    {
        public CulledPupsRepository(AnimalDBContext context) : base(context)
        {
        }

        public IEnumerable<CulledPups> GetByAnimalId(int animalId)
        {
            return Context.CulledPups.Where(m => m.AnimalId == animalId).ToList();
        }

        public AnimalDBContext Context
        {
            get { return base.db; }
        }
    }
}
