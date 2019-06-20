using AnimalDB.Repo.Contexts;
using AnimalDB.Repo.Entities;
using AnimalDB.Repo.Repositories.Abstract;
using AnimalDB.Repositories.Concrete;

namespace AnimalDB.Repo.Repositories.Concrete
{
    public class VeterinarianRepository : UserRepository<Veterinarian>, IVeterinarianRepository
    {
        public VeterinarianRepository(AnimalDBContext context) : base(context)
        {
        }
    }
}
