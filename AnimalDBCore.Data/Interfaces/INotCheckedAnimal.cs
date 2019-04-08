using   AnimalDBCore.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace   AnimalDBCore.Core.Interfaces
{
    public interface INotCheckedAnimal
    {
        IEnumerable<NotCheckedAnimal> GetNotCheckedAnimals();

        Task CreateNotCheckedAnimal(NotCheckedAnimal notCheckedAnimal);

        Task<NotCheckedAnimal> GetNotCheckedAnimalById(int id);

        Task UpdateNotCheckedAnimal(NotCheckedAnimal notCheckedAnimal);

        Task DeleteNotCheckedAnimal(NotCheckedAnimal notCheckedAnimal);
    }
}
