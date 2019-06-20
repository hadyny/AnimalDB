using AnimalDB.Repo.Contexts;
using AnimalDB.Repo.Entities;
using AnimalDB.Repo.Repositories.Abstract;
using AnimalDB.Repositories.Concrete;

namespace AnimalDB.Repo.Repositories.Concrete
{
    public class ColourRepository : Repository<Colour>, IColourRepository
    {
        public ColourRepository(AnimalDBContext context) : base(context)
        {
        }
    }
}
