using AnimalDB.Repo.Contexts;
using AnimalDB.Repo.Entities;
using AnimalDB.Repo.Repositories.Abstract;
using AnimalDB.Repositories.Concrete;

namespace AnimalDB.Repo.Repositories.Concrete
{
    public class MedicationTypeRepository : Repository<MedicationType>, IMedicationTypeRepository
    {
        public MedicationTypeRepository(AnimalDBContext context) : base(context)
        {
        }
    }
}
