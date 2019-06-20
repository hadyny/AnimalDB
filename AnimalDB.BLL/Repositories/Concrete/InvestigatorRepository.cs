using AnimalDB.Repo.Contexts;
using AnimalDB.Repo.Entities;
using AnimalDB.Repo.Repositories.Abstract;
using AnimalDB.Repositories.Concrete;

namespace AnimalDB.Repo.Repositories.Concrete
{
    public class InvestigatorRepository : UserRepository<Investigator>, IInvestigatorRepository
    {
        public InvestigatorRepository(AnimalDBContext context) : base(context)
        {
        }
    }
}
