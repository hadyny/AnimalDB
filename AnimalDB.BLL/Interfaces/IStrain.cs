using AnimalDB.Repo.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AnimalDB.Repo.Interfaces
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
