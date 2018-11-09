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
    public class ArrivalStatusRepo : IArrivalStatus, IDisposable
    {
        private AnimalDBContext db;

        public ArrivalStatusRepo()
        {
            this.db = new AnimalDBContext();
        }
        public ArrivalStatusRepo(AnimalDBContext db)
        {
            this.db = db;
        }

        public async Task CreateArrivalStatus(ArrivalStatus arrivalStatus)
        {
            db.ArrivalStatus.Add(arrivalStatus);
            await db.SaveChangesAsync();
        }

        public async Task DeleteArrivalStatus(ArrivalStatus arrivalStatus)
        {
            if (db.Entry(arrivalStatus).State == EntityState.Detached)
            {
                db.ArrivalStatus.Attach(arrivalStatus);
            }
            db.ArrivalStatus.Remove(arrivalStatus);
            await db.SaveChangesAsync();
        }

        public void Dispose()
        {
            ((IDisposable)db).Dispose();
        }

        public IEnumerable<ArrivalStatus> GetArrivalStatus()
        {
            return db.ArrivalStatus.ToList();
        }

        public async Task<ArrivalStatus> GetArrivalStatusById(int id)
        {
            return await db.ArrivalStatus.FindAsync(id);
        }

        public async Task UpdateArrivalStatus(ArrivalStatus arrivalStatus)
        {
            db.Entry(arrivalStatus).State = EntityState.Modified;
            await db.SaveChangesAsync();
        }
    }
}
