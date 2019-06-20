using AnimalDB.Repo.Contexts;
using AnimalDB.Repo.Entities;
using AnimalDB.Repo.Repositories.Abstract;
using AnimalDB.Repositories.Concrete;

namespace AnimalDB.Repo.Repositories.Concrete
{
    public class VirusTypeRepository : Repository<VirusType>, IVirusTypeRepository
    {
        public VirusTypeRepository(AnimalDBContext context) : base(context)
        {
        }
    }
}
