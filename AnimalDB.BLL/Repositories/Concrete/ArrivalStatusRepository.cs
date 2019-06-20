using AnimalDB.Repo.Contexts;
using AnimalDB.Repo.Entities;
using AnimalDB.Repo.Repositories.Abstract;
using AnimalDB.Repositories.Concrete;

namespace AnimalDB.Repo.Repositories.Concrete
{
    public class ArrivalStatusRepository : Repository<ArrivalStatus>, IArrivalStatusRepository
    {
        public ArrivalStatusRepository(AnimalDBContext context) : base(context)
        {
        }
    }
}
