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
    public class EthicsNumberHistoryRepo : IEthicsNumberHistory, IDisposable
    {
        private AnimalDBContext db;

        public EthicsNumberHistoryRepo()
        {
            this.db = new AnimalDBContext();
        }
        public EthicsNumberHistoryRepo(AnimalDBContext db)
        {
            this.db = db;
        }

        public async Task CreateEthicsNumberHistory(EthicsNumberHistory ethicsNumberHistory)
        {
            db.EthicsNumberHistories.Add(ethicsNumberHistory);
            await db.SaveChangesAsync();
        }

        public async Task DeleteEthicsNumberHistory(EthicsNumberHistory ethicsNumberHistory)
        {
            if (db.Entry(ethicsNumberHistory).State == EntityState.Detached)
            {
                db.EthicsNumberHistories.Attach(ethicsNumberHistory);
            }
            db.EthicsNumberHistories.Remove(ethicsNumberHistory);
            await db.SaveChangesAsync();
        }

        public async Task<EthicsNumberHistory> GetEthicsNumberHistoryById(int id)
        {
            return await db.EthicsNumberHistories.FindAsync(id);
        }

        public IEnumerable<EthicsNumberHistory> GetEthicsNumberHistories()
        {
            return db.EthicsNumberHistories.ToList();
        }

        public async Task UpdateEthicsNumberHistory(EthicsNumberHistory ethicsNumberHistory)
        {
            db.Entry(ethicsNumberHistory).State = EntityState.Modified;
            await db.SaveChangesAsync();
        }

        public IEnumerable<EthicsNumberHistory> GetEthicsNumberHistoriesByAnimal(int animal_Id)
        {
            return db.EthicsNumberHistories.Where(m => m.Animal_Id == animal_Id).OrderByDescending(m => m.Timestamp).ToList();
        }

        public void Dispose()
        {
            ((IDisposable)db).Dispose();
        }
    }
}
