using AnimalDB.Repo.Contexts;
using AnimalDB.Repo.Entities;
using AnimalDB.Repo.Repositories.Abstract;
using AnimalDB.Repositories.Concrete;

namespace AnimalDB.Repo.Repositories.Concrete
{
    public class SopCategoryRepository : Repository<SopCategory>, ISopCategoryRepository
    {
        public SopCategoryRepository(AnimalDBContext context) : base(context)
        {
        }
    }
}
