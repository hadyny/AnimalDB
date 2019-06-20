using AnimalDB.Repo.Contexts;
using AnimalDB.Repo.Entities;
using AnimalDB.Repo.Repositories.Abstract;
using AnimalDB.Repositories.Concrete;

namespace AnimalDB.Repo.Repositories.Concrete
{
    public class SpeciesRepository : Repository<Species>, ISpeciesRepository
    {
        public SpeciesRepository(AnimalDBContext context) : base(context)
        {
        }

    }
}
