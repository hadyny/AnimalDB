using AnimalDB.Repo.Contexts;
using AnimalDB.Repo.Entities;
using AnimalDB.Repo.Repositories.Abstract;
using AnimalDB.Repositories.Concrete;

namespace AnimalDB.Repo.Repositories.Concrete
{
    public class ChargeCodeRepository : Repository<ChargeCode>, IChargeCodeRepository
    {
        public ChargeCodeRepository(AnimalDBContext context) : base(context)
        {
        }
    }
}
