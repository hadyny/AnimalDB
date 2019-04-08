using AnimalDBCore.Infrastructure.Contexts;
using  AnimalDBCore.Core.Entities;
using  AnimalDBCore.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic; 
using System.Linq;
using System.Threading.Tasks;

namespace AnimalDBCore.Infrastructure.Data
{
    public class NotificationRepo : INotification, IDisposable
    {
        private AnimalDBContext db;

        public NotificationRepo()
        {
            this.db = new AnimalDBContext();
        }
        public NotificationRepo(AnimalDBContext db)
        {
            this.db = db;
        }

        public async Task CreateNotification(Notification notification)
        {
            db.Notifications.Add(notification);
            await db.SaveChangesAsync();
        }

        public IEnumerable<Notification> GetNotificationByInvestigatorUsername(string username)
        {
            return db.Notifications
                        .Where(m => m.Animal.Investigator_Id != null && m.Animal.Investigator.UserName == username)
                        .OrderByDescending(m => m.NotificationDate)
                        .ToList();
        }

        public async Task DeleteNotification(Notification notification)
        {
            if (db.Entry(notification).State == EntityState.Detached)
            {
                db.Notifications.Attach(notification);
            }
            db.Notifications.Remove(notification);
            await db.SaveChangesAsync();
        }

        public IEnumerable<Notification> GetPastNotifications()
        {
            return db.Notifications.Where(m => m.NotificationDate < DateTime.Now);
        }

        public IEnumerable<Notification> GetFutureNotifications()
        {
            return db.Notifications.Where(m => m.NotificationDate > DateTime.Now);
        }

        public void Dispose()
        {
            ((IDisposable)db).Dispose();
        }

        public async Task<Notification> GetNotificationById(int id)
        {
            return await db.Notifications.FindAsync(id);
        }

        public IEnumerable<Notification> GetNotifications()
        {
            return db.Notifications
                        .OrderByDescending(m => m.NotificationDate)
                        .ToList();
        }

        public async Task UpdateNotification(Notification notification)
        {
            db.Entry(notification).State = EntityState.Modified;
            await db.SaveChangesAsync();
        }

        public IEnumerable<Notification> GetNotificationsByMedicationId(int medicationId)
        {
            return db.Notifications
                    .Where(m => m.Medication_Id == medicationId)
                    .OrderByDescending(m => m.NotificationDate)
                    .ToList();
        }
    }
}
