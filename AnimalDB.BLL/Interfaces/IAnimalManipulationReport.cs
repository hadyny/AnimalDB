using AnimalDB.Repo.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AnimalDB.Repo.Interfaces
{
    public interface IAnimalManipulationReport
    {
        IEnumerable<AnimalManipulationReport> GetAnimalManipulationReports();

        Task CreateAnimalManipulationReport(AnimalManipulationReport animalManipulationReport);

        Task<AnimalManipulationReport> GetAnimalManipulationReportById(int id);

        Task UpdateAnimalManipulationReport(AnimalManipulationReport animalManipulationReport);

        Task DeleteAnimalManipulationReport(AnimalManipulationReport animalManipulationReport);
    }
}
