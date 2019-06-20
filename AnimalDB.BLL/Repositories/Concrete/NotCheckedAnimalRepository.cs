using AnimalDB.Repo.Contexts;
using AnimalDB.Repo.Entities;
using AnimalDB.Repo.Repositories.Abstract;
using AnimalDB.Repositories.Concrete;

namespace AnimalDB.Repo.Repositories.Concrete
{
    public class NotCheckedAnimalRepository : Repository<NotCheckedAnimal>, INotCheckedAnimalRepository
    {
        public NotCheckedAnimalRepository(AnimalDBContext context) : base(context)
        {
        }
    }
}
