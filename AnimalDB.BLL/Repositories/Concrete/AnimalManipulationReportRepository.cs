using AnimalDB.Repo.Contexts;
using AnimalDB.Repo.Entities;
using AnimalDB.Repo.Repositories.Abstract;
using AnimalDB.Repositories.Concrete;

namespace AnimalDB.Repo.Repositories.Concrete
{
    public class AnimalManipulationReportRepository : Repository<AnimalManipulationReport>, IAnimalManipulationReportRepository
    {

        public AnimalManipulationReportRepository(AnimalDBContext context) : base(context)
        {
        }
    }
}
