using AnimalDB.Repo.Entities;
using AnimalDB.Repo.Interfaces;
using AnimalDB.Repo.Repositories.Abstract;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AnimalDB.Repo.Services
{
    public class AnimalManipulationReportService : IAnimalManipulationReportService
    {
        private readonly IRepository<AnimalManipulationReport> animalManipulationReports;

        public AnimalManipulationReportService(IRepository<AnimalManipulationReport> animalManipulationReports)
        {
            this.animalManipulationReports = animalManipulationReports;
        }

        public async Task CreateAnimalManipulationReport(AnimalManipulationReport animalManipulationReport)
        {
            animalManipulationReports.Insert(animalManipulationReport);
            await animalManipulationReports.Save();
        }

        public async Task DeleteAnimalManipulationReport(AnimalManipulationReport animalManipulationReport)
        {
            await animalManipulationReports.Delete(animalManipulationReport);
            await animalManipulationReports.Save();
        }

        public async Task<AnimalManipulationReport> GetAnimalManipulationReportById(int id)
        {
            return await animalManipulationReports.GetById(id);
        }

        public async Task<IEnumerable<AnimalManipulationReport>> GetAnimalManipulationReports()
        {
            return await animalManipulationReports.GetAll();
        }

        public async Task UpdateAnimalManipulationReport(AnimalManipulationReport animalManipulationReport)
        {
            animalManipulationReports.Update(animalManipulationReport);
            await animalManipulationReports.Save();
        }
    }
}
