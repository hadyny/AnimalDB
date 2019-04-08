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
    public class NotificationEmailRepo : INotificationEmail, IDisposable
    {
        private AnimalDBContext db;

        public NotificationEmailRepo()
        {
            this.db = new AnimalDBContext();
        }
        public NotificationEmailRepo(AnimalDBContext db)
        {
            this.db = db;
        }

        public async Task CreateNotificationEmail(NotificationEmail notificationEmail)
        {
            db.NotificationEmails.Add(notificationEmail);
            await db.SaveChangesAsync();
        }

        public async Task DeleteNotificationEmail(NotificationEmail notificationEmail)
        {
            if (db.Entry(notificationEmail).State == EntityState.Detached)
            {
                db.NotificationEmails.Attach(notificationEmail);
            }
            db.NotificationEmails.Remove(notificationEmail);
            await db.SaveChangesAsync();
        }

        public void Dispose()
        {
            ((IDisposable)db).Dispose();
        }

        public async Task<NotificationEmail> GetNotificationEmailById(int id)
        {
            return await db.NotificationEmails.FindAsync(id);
        }

        public IEnumerable<NotificationEmail> GetNotificationEmails()
        {
            return db.NotificationEmails.ToList();
        }

        public async Task UpdateNotificationEmail(NotificationEmail notificationEmail)
        {
            db.Entry(notificationEmail).State = EntityState.Modified;
            await db.SaveChangesAsync();
        }
    }
}
