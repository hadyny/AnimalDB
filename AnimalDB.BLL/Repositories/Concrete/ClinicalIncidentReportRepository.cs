using AnimalDB.Repo.Contexts;
using AnimalDB.Repo.Entities;
using AnimalDB.Repo.Repositories.Abstract;
using AnimalDB.Repositories.Concrete;

namespace AnimalDB.Repo.Repositories.Concrete
{
    public class ClinicalIncidentReportRepository : Repository<ClinicalIncidentReport>, IClinicalIncidentReportRepository
    {
        public ClinicalIncidentReportRepository(AnimalDBContext context) : base(context)
        {
        }
    }
}
