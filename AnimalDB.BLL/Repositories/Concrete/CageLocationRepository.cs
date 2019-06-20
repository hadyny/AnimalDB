using AnimalDB.Repo.Contexts;
using AnimalDB.Repo.Entities;
using AnimalDB.Repo.Repositories.Abstract;
using AnimalDB.Repositories.Concrete;

namespace AnimalDB.Repo.Repositories.Concrete
{
    public class CageLocationRepository : Repository<CageLocation>, ICageLocationRepository
    {
        public CageLocationRepository(AnimalDBContext context) : base(context)
        {
        }
    }
}
