using   AnimalDBCore.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace   AnimalDBCore.Core.Interfaces
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
