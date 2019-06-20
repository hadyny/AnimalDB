using AnimalDB.Repo.Contexts;
using AnimalDB.Repo.Entities;
using AnimalDB.Repo.Repositories.Abstract;
using AnimalDB.Repositories.Concrete;

namespace AnimalDB.Repo.Repositories.Concrete
{
    public sealed class AdministratorRepository : UserRepository<Administrator>, IAdministratorRepository
    {
        public AdministratorRepository(AnimalDBContext context) : base(context)
        {
        }
    }
}