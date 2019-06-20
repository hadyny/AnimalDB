using AnimalDB.Repo.Contexts;
using AnimalDB.Repo.Entities;
using AnimalDB.Repo.Repositories.Abstract;
using AnimalDB.Repositories.Concrete;

namespace AnimalDB.Repo.Repositories.Concrete
{
    public class FeedRepository : Repository<Feed>, IFeedRepository
    {
        public FeedRepository(AnimalDBContext context) : base(context)
        {
        }
    }
}
