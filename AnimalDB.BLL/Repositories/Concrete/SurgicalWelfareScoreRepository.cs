using AnimalDB.Repo.Contexts;
using AnimalDB.Repo.Entities;
using AnimalDB.Repo.Repositories.Abstract;
using AnimalDB.Repositories.Concrete;

namespace AnimalDB.Repo.Repositories.Concrete
{
    public class SurgicalWelfareScoreRepository : Repository<SurgicalWelfareScore>, ISurgicalWelfareScoreRepository
    {
        public SurgicalWelfareScoreRepository(AnimalDBContext context) : base(context)
        {
        }
    }
}
