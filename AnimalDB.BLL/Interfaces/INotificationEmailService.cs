using AnimalDB.Repo.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AnimalDB.Repo.Interfaces
{
    public interface INotificationEmailService
    {
        Task<IEnumerable<NotificationEmail>> GetNotificationEmails();

        Task CreateNotificationEmail(NotificationEmail notificationEmail);

        Task<NotificationEmail> GetNotificationEmailById(int id);

        Task UpdateNotificationEmail(NotificationEmail notificationEmail);

        Task DeleteNotificationEmail(NotificationEmail notificationEmail);
    }
}
