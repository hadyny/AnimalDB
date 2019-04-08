using   AnimalDBCore.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace   AnimalDBCore.Core.Interfaces
{
    public interface INotificationEmail
    {
        IEnumerable<NotificationEmail> GetNotificationEmails();

        Task CreateNotificationEmail(NotificationEmail notificationEmail);

        Task<NotificationEmail> GetNotificationEmailById(int id);

        Task UpdateNotificationEmail(NotificationEmail notificationEmail);

        Task DeleteNotificationEmail(NotificationEmail notificationEmail);
    }
}
