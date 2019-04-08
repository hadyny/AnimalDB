using   AnimalDBCore.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace   AnimalDBCore.Core.Interfaces
{
    public interface ICageLocation
    {
        IEnumerable<CageLocation> GetCageLocations();

        Task CreateCageLocation(CageLocation cageLocation);

        Task<CageLocation> GetCageLocationById(int id);

        Task UpdateCageLocation(CageLocation cageLocation);

        Task DeleteCageLocation(CageLocation cageLocation);
    }
}
