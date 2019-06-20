using AnimalDB.Repo.Contexts;
using AnimalDB.Repo.Entities;
using AnimalDB.Repo.Repositories.Abstract;
using AnimalDB.Repositories.Concrete;

namespace AnimalDB.Repo.Repositories.Concrete
{
    public class NotCheckedRoomRepository : Repository<NotCheckedRoom>, INotCheckedRoomRepository
    {
        public NotCheckedRoomRepository(AnimalDBContext context) : base(context)
        {
        }
    }
}
