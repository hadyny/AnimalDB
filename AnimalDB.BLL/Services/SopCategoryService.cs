using AnimalDB.Repo.Entities;
using AnimalDB.Repo.Interfaces;
using AnimalDB.Repo.Repositories.Abstract;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AnimalDB.Repo.Services
{
    public class SopCategoryService : ISopCategoryService
    {
        private readonly IRepository<SopCategory> _sopCategories;

        public SopCategoryService(IRepository<SopCategory> sopCategories)
        {
            this._sopCategories = sopCategories;
        }

        public async Task CreateSopCategory(SopCategory sopCategory)
        {
            _sopCategories.Insert(sopCategory);
            await _sopCategories.Save();
        }

        public async Task DeleteSopCategory(SopCategory sopCategory)
        {
            await _sopCategories.Delete(sopCategory);
            await _sopCategories.Save();
        }

        public async Task<SopCategory> GetSopCategoryById(int id)
        {
            return await _sopCategories.GetById(id);
        }

        public async Task<IEnumerable<SopCategory>> GetSopCategories()
        {
            return await _sopCategories.GetAll();
        }

        public async Task UpdateSopCategory(SopCategory sopCategory)
        {
            _sopCategories.Update(sopCategory);
            await _sopCategories.Save();
        }
    }
}
