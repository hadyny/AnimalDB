using AnimalDB.Repo.Contexts;
using AnimalDB.Repo.Entities;
using AnimalDB.Repo.Repositories.Abstract;
using AnimalDB.Repositories.Concrete;

namespace AnimalDB.Repo.Repositories.Concrete
{
    public class StrainRepository : Repository<Strain>, IStrainRepository
    {
        public StrainRepository(AnimalDBContext context) : base(context)
        {
        }
    }
}
