using AnimalDBCore.Infrastructure.Contexts;
using AnimalDBCore.Core.Entities;
using AnimalDBCore.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic; 
using System.Linq;
using System.Threading.Tasks;

namespace AnimalDBCore.Infrastructure.Data
{
    public class SurgicalWelfareScoreRepo : ISurgicalWelfareScore, IDisposable
    {
        private AnimalDBContext db;

        public SurgicalWelfareScoreRepo()
        {
            this.db = new AnimalDBContext();
        }
        public SurgicalWelfareScoreRepo(AnimalDBContext db)
        {
            this.db = db;
        }

        public async Task CreateSurgicalWelfareScore(SurgicalWelfareScore surgicalWelfareScore)
        {
            db.SurgicalWelfareScores.Add(surgicalWelfareScore);
            await db.SaveChangesAsync();
        }

        public async Task DeleteSurgicalWelfareScore(SurgicalWelfareScore surgicalWelfareScore)
        {
            if (db.Entry(surgicalWelfareScore).State == EntityState.Detached)
            {
                db.SurgicalWelfareScores.Attach(surgicalWelfareScore);
            }
            db.SurgicalWelfareScores.Remove(surgicalWelfareScore);
            await db.SaveChangesAsync();
        }

        public void Dispose()
        {
            ((IDisposable)db).Dispose();
        }

        public async Task<SurgicalWelfareScore> GetSurgicalWelfareScoreById(int id)
        {
            return await db.SurgicalWelfareScores.FindAsync(id);
        }

        public IEnumerable<SurgicalWelfareScore> GetSurgicalWelfareScores()
        {
            return db.SurgicalWelfareScores.ToList();
        }

        public async Task UpdateSurgicalWelfareScore(SurgicalWelfareScore surgicalWelfareScore)
        {
            db.Entry(surgicalWelfareScore).State = EntityState.Modified;
            await db.SaveChangesAsync();
        }
    }
}
