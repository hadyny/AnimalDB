using AnimalDB.Repo.Contexts;
using AnimalDB.Repo.Entities;
using AnimalDB.Repo.Repositories.Abstract;
using AnimalDB.Repositories.Concrete;

namespace AnimalDB.Repo.Repositories.Concrete
{
    public class SourceRepository : Repository<Source>, ISourceRepository
    {
        public SourceRepository(AnimalDBContext context) : base(context)
        {
        }
    }
}
