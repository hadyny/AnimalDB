using   AnimalDBCore.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace   AnimalDBCore.Core.Interfaces
{
    public interface IStrain
    {
        IEnumerable<Strain> GetStrains();

        Task CreateStrain(Strain strain);

        Task<Strain> GetStrainById(int id);

        Task UpdateStrain(Strain strain);

        Task DeleteStrain(Strain strain);
    }
}
