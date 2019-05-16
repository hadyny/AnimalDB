using AnimalDB.Repo.Entities;
using AnimalDB.Repo.Interfaces;
using AnimalDB.Repo.Repositories.Abstract;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AnimalDB.Repo.Services
{
    public class NotificationEmailService : INotificationEmailService
    {
        private readonly IRepository<NotificationEmail> _notificationEmails;

        public NotificationEmailService(IRepository<NotificationEmail> notificationEmails)
        {
            this._notificationEmails = notificationEmails;
        }

        public async Task CreateNotificationEmail(NotificationEmail notificationEmail)
        {
            _notificationEmails.Insert(notificationEmail);
            await _notificationEmails.Save();
        }

        public async Task DeleteNotificationEmail(NotificationEmail notificationEmail)
        {
            await _notificationEmails.Delete(notificationEmail);
            await _notificationEmails.Save();
        }

        public async Task<NotificationEmail> GetNotificationEmailById(int id)
        {
            return await _notificationEmails.GetById(id);
        }

        public async Task<IEnumerable<NotificationEmail>> GetNotificationEmails()
        {
            return await _notificationEmails.GetAll();
        }

        public async Task UpdateNotificationEmail(NotificationEmail notificationEmail)
        {
            _notificationEmails.Update(notificationEmail);
            await _notificationEmails.Save();
        }
    }
}
