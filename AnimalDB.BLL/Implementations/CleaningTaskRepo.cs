using AnimalDB.Repo.Contexts;
using AnimalDB.Repo.Entities;
using AnimalDB.Repo.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace AnimalDB.Repo.Implementations
{
    public class CleaningTaskRepo : ICleaningTask, IDisposable
    {
        private AnimalDBContext db;

        public CleaningTaskRepo()
        {
            this.db = new AnimalDBContext();
        }
        public CleaningTaskRepo(AnimalDBContext db)
        {
            this.db = db;
        }

        public async Task CreateCleaningTask(CleaningTask cleaningTask)
        {
            db.CleaningTasks.Add(cleaningTask);
            await db.SaveChangesAsync();
        }

        public async Task DeleteCleaningTask(CleaningTask cleaningTask)
        {
            if (db.Entry(cleaningTask).State == EntityState.Detached)
            {
                db.CleaningTasks.Attach(cleaningTask);
            }
            db.CleaningTasks.Remove(cleaningTask);
            await db.SaveChangesAsync();
        }

        public void Dispose()
        {
            ((IDisposable)db).Dispose();
        }

        public async Task<CleaningTask> GetCleaningTaskById(int id)
        {
            return await db.CleaningTasks.FindAsync(id);
        }

        public IEnumerable<CleaningTask> GetCleaningTasks()
        {
            return db.CleaningTasks.ToList();
        }

        public async Task UpdateCleaningTask(CleaningTask cleaningTask)
        {
            db.Entry(cleaningTask).State = EntityState.Modified;
            await db.SaveChangesAsync();
        }
    }
}
