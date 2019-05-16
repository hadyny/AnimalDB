using AnimalDB.Repo.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AnimalDB.Repo.Interfaces
{
    public interface INotificationService
    {
        Task<IEnumerable<Notification>> GetNotifications();

        Task CreateNotification(Notification notification);

        Task<Notification> GetNotificationById(int id);

        Task UpdateNotification(Notification notification);

        Task DeleteNotification(Notification notification);

        Task<IEnumerable<Notification>> GetNotificationsByMedicationId(int medicationId);

        Task<IEnumerable<Notification>> GetNotificationByInvestigatorUsername(string username);

        Task<IEnumerable<Notification>> GetPastNotifications();

        Task<IEnumerable<Notification>> GetFutureNotifications();
    }
}
