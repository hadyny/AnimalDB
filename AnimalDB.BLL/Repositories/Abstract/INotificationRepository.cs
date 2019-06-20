using AnimalDB.Repo.Entities;
using System.Collections.Generic;

namespace AnimalDB.Repo.Repositories.Abstract
{
    public interface INotificationRepository : IRepository<Notification>
    {
        IEnumerable<Notification> GetByInvestigatorUsername(string username);
        IEnumerable<Notification> GetPast();
        IEnumerable<Notification> GetFuture();
        IEnumerable<Notification> GetByMedicationId(int medicationId);
    }
}
