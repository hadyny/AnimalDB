using AnimalDB.Repo.Contexts;
using AnimalDB.Repo.Entities;
using AnimalDB.Repo.Repositories.Abstract;
using AnimalDB.Repositories.Concrete;

namespace AnimalDB.Repo.Repositories.Concrete
{
    public class NotificationEmailRepository : Repository<NotificationEmail>, INotificationEmailRepository
    {
        public NotificationEmailRepository(AnimalDBContext context) : base(context)
        {
        }
    }
}
