using AnimalDB.Repo.Entities;
using AnimalDB.Repo.Interfaces;
using AnimalDB.Repo.Repositories.Abstract;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AnimalDB.Repo.Services
{
    public class NotCheckedAnimalService : INotCheckedAnimalService
    {
        private readonly IRepository<NotCheckedAnimal> _notCheckedAnimals;

        public NotCheckedAnimalService(IRepository<NotCheckedAnimal> notCheckedAnimals)
        {
            this._notCheckedAnimals = notCheckedAnimals;
        }

        public async Task CreateNotCheckedAnimal(NotCheckedAnimal notCheckedAnimal)
        {
            _notCheckedAnimals.Insert(notCheckedAnimal);
            await _notCheckedAnimals.Save();
        }

        public async Task DeleteNotCheckedAnimal(NotCheckedAnimal notCheckedAnimal)
        {
            await _notCheckedAnimals.Delete(notCheckedAnimal.Id);
            await _notCheckedAnimals.Save();
        }

        public async Task<NotCheckedAnimal> GetNotCheckedAnimalById(int id)
        {
            return await _notCheckedAnimals.GetById(id);
        }

        public async Task<IEnumerable<NotCheckedAnimal>> GetNotCheckedAnimals()
        {
            return await _notCheckedAnimals.GetAll();
        }

        public async Task UpdateNotCheckedAnimal(NotCheckedAnimal notCheckedAnimal)
        {
            _notCheckedAnimals.Update(notCheckedAnimal);
            await _notCheckedAnimals.Save();
        }
    }
}
