using AnimalDB.Repo.Contexts;
using AnimalDB.Repo.Entities;
using AnimalDB.Repo.Repositories.Abstract;
using AnimalDB.Repositories.Concrete;

namespace AnimalDB.Repo.Repositories.Concrete
{
    public class SurgeryTypeRepository : Repository<SurgeryType>, ISurgeryTypeRepository
    {
        public SurgeryTypeRepository(AnimalDBContext context) : base(context)
        {
        }
    }
}
