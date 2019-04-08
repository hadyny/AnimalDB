﻿using   AnimalDBCore.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace   AnimalDBCore.Core.Interfaces
{
    public interface INotification
    {
        IEnumerable<Notification> GetNotifications();

        Task CreateNotification(Notification notification);

        Task<Notification> GetNotificationById(int id);

        Task UpdateNotification(Notification notification);

        Task DeleteNotification(Notification notification);

        IEnumerable<Notification> GetNotificationsByMedicationId(int medicationId);

        IEnumerable<Notification> GetNotificationByInvestigatorUsername(string username);

        IEnumerable<Notification> GetPastNotifications();

        IEnumerable<Notification> GetFutureNotifications();
    }
}