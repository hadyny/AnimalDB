using AnimalDB.Repo.Contexts;
using AnimalDB.Repo.Entities;
using AnimalDB.Repo.Repositories.Abstract;
using AnimalDB.Repositories.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AnimalDB.Repo.Repositories.Concrete
{
    public class NotificationRepository : Repository<Notification>, INotificationRepository
    {
        public NotificationRepository(AnimalDBContext context) : base(context)
        {
        }

        public IEnumerable<Notification> GetByInvestigatorUsername(string username)
        {
            return Context
                    .Notifications
                    .Where(m => 
                            m.Animal.Investigator_Id != null && 
                            m.Animal.Investigator.UserName == username)
                    .OrderByDescending(m => m.NotificationDate)
                    .ToList();
                        
        }

        public IEnumerable<Notification> GetPast()
        {
            return Context.Notifications.Where(m => m.NotificationDate < DateTime.Now).ToList();
        }

        public IEnumerable<Notification> GetFuture()
        {
            return Context.Notifications.Where(m => m.NotificationDate > DateTime.Now).ToList();
        }

        public IEnumerable<Notification> GetByMedicationId(int medicationId)
        {
            return Context
                    .Notifications
                    .Where(m => 
                            m.Medication_Id == medicationId)
                    .OrderByDescending(m => m.NotificationDate)
                    .ToList();                    
        }

        public AnimalDBContext Context
        {
            get { return base.db; }
        }
    }
}
