using AnimalDB.Repo.Contexts;
using AnimalDB.Repo.Entities;
using AnimalDB.Repo.Repositories.Abstract;
using AnimalDB.Repositories.Concrete;
using System.Collections.Generic;
using System.Linq;

namespace AnimalDB.Repo.Repositories.Concrete
{
    public class SopRepository : Repository<Sop>, ISopRepository
    {
        public SopRepository(AnimalDBContext context) : base(context)
        {
        }

        public IEnumerable<Sop> GetByCategoryId(int categoryId)
        {
            return Context.Sops.Where(m => m.Category_Id == categoryId).ToList();
        }

        public bool FileNameExists(string fileName)
        {
            return Context.Sops.Count(m => m.FileName == fileName) != 0;
        }

        public AnimalDBContext Context
        {
            get { return base.db; }
        }
    }
}
