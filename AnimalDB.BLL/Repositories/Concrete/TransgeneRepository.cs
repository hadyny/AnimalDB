using AnimalDB.Repo.Contexts;
using AnimalDB.Repo.Entities;
using AnimalDB.Repo.Repositories.Abstract;
using AnimalDB.Repositories.Concrete;

namespace AnimalDB.Repo.Repositories.Concrete
{
    public class TransgeneRepository : Repository<Transgene>, ITransgeneRepository
    {
        public TransgeneRepository(AnimalDBContext context) : base(context)
        {
        }
    }
}
