using AnimalDB.Repo.Entities;
using AnimalDB.Repo.Interfaces;
using AnimalDB.Repo.Repositories.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnimalDB.Repo.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IRepository<Notification> _notifications;

        public NotificationService(IRepository<Notification> notifications)
        {
            this._notifications = notifications;
        }

        public async Task CreateNotification(Notification notification)
        {
            _notifications.Insert(notification);
            await _notifications.Save();
        }

        public async Task<IEnumerable<Notification>> GetNotificationByInvestigatorUsername(string username)
        {
            var notifications = await _notifications.GetAll(m => m.Animal.Investigator_Id != null && 
                                                                 m.Animal.Investigator.UserName == username);
            return notifications
                        .OrderByDescending(m => m.NotificationDate);
        }

        public async Task DeleteNotification(Notification notification)
        {
            await _notifications.Delete(notification);
            await _notifications.Save();
        }

        public async Task<IEnumerable<Notification>> GetPastNotifications()
        {
            return await _notifications.GetAll(m => m.NotificationDate < DateTime.Now);
        }

        public async Task<IEnumerable<Notification>> GetFutureNotifications()
        {
            return await _notifications.GetAll(m => m.NotificationDate > DateTime.Now);
        }

        public async Task<Notification> GetNotificationById(int id)
        {
            return await _notifications.GetById(id);
        }

        public async Task<IEnumerable<Notification>> GetNotifications()
        {
            var notifications = await _notifications.GetAll();
            return notifications
                        .OrderByDescending(m => m.NotificationDate);
        }

        public async Task UpdateNotification(Notification notification)
        {
            _notifications.Update(notification);
            await _notifications.Save();
        }

        public async Task<IEnumerable<Notification>> GetNotificationsByMedicationId(int medicationId)
        {
            var notifications = await _notifications.GetAll(m => m.Medication_Id == medicationId);
            return notifications
                    .OrderByDescending(m => m.NotificationDate);
        }
    }
}
