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
    public class NotCheckedAnimalRepo : INotCheckedAnimal, IDisposable
    {
        private AnimalDBContext db;

        public NotCheckedAnimalRepo()
        {
            this.db = new AnimalDBContext();
        }
        public NotCheckedAnimalRepo(AnimalDBContext db)
        {
            this.db = db;
        }

        public async Task CreateNotCheckedAnimal(NotCheckedAnimal notCheckedAnimal)
        {
            db.NotCheckedAnimals.Add(notCheckedAnimal);
            await db.SaveChangesAsync();
        }

        public async Task DeleteNotCheckedAnimal(NotCheckedAnimal notCheckedAnimal)
        {
            if (db.Entry(notCheckedAnimal).State == EntityState.Detached)
            {
                db.NotCheckedAnimals.Attach(notCheckedAnimal);
            }
            db.NotCheckedAnimals.Remove(notCheckedAnimal);
            await db.SaveChangesAsync();
        }

        public void Dispose()
        {
            ((IDisposable)db).Dispose();
        }

        public async Task<NotCheckedAnimal> GetNotCheckedAnimalById(int id)
        {
            return await db.NotCheckedAnimals.FindAsync(id);
        }

        public IEnumerable<NotCheckedAnimal> GetNotCheckedAnimals()
        {
            return db.NotCheckedAnimals.ToList();
        }

        public async Task UpdateNotCheckedAnimal(NotCheckedAnimal notCheckedAnimal)
        {
            db.Entry(notCheckedAnimal).State = EntityState.Modified;
            await db.SaveChangesAsync();
        }
    }
}
