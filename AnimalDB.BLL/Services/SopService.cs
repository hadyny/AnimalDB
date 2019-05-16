using AnimalDB.Repo.Entities;
using AnimalDB.Repo.Interfaces;
using AnimalDB.Repo.Repositories.Abstract;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnimalDB.Repo.Services
{
    public class SopService : ISopService
    {
        private readonly IRepository<Sop> _sops;

        public SopService(IRepository<Sop> sops)
        {
            this._sops = sops;
        }

        public async Task CreateSop(Sop sop)
        {
            _sops.Insert(sop);
            await _sops.Save();
        }

        public async Task DeleteSop(Sop sop)
        {
            await _sops.Delete(sop);
            await _sops.Save();
        }

        public async Task<IEnumerable<Sop>> GetSopsByCategoryId(int categoryId)
        {
            return await _sops.GetAll(m => m.Category_Id == categoryId);
        }

        public async Task<Sop> GetSopById(int id)
        {
            return await _sops.GetById(id);
        }

        public async Task<IEnumerable<Sop>> GetSops()
        {
            return await _sops.GetAll();
        }

        public async Task UpdateSop(Sop sop)
        {
            _sops.Update(sop);
            await _sops.Save();
        }

        public async Task<bool> DoesSopFileNameExist(string fileName)
        {
            var sops = await _sops.GetAll(m => m.FileName == fileName);
            return sops.Count() != 0;
        }
    }
}
