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
    public class EthicsNumberRepo : IEthicsNumber, IDisposable
    {
        private AnimalDBContext db;

        public EthicsNumberRepo()
        {
            this.db = new AnimalDBContext();
        }
        public EthicsNumberRepo(AnimalDBContext db)
        {
            this.db = db;
        }

        public async Task CreateEthicsNumber(EthicsNumber ethicsNumber)
        {
            db.EthicsNumbers.Add(ethicsNumber);
            await db.SaveChangesAsync();
        }

        public async Task DeleteEthicsNumber(EthicsNumber ethicsNumber)
        {
            if (db.Entry(ethicsNumber).State == EntityState.Detached)
            {
                db.EthicsNumbers.Attach(ethicsNumber);
            }
            db.EthicsNumbers.Remove(ethicsNumber);
            await db.SaveChangesAsync();
        }

        public void Dispose()
        {
            ((IDisposable)db).Dispose();
        }

        public async Task<EthicsNumber> GetEthicsNumberById(int id)
        {
            return await db.EthicsNumbers.FindAsync(id);
        }

        public IEnumerable<EthicsNumber> GetEthicsNumbers()
        {
            return db.EthicsNumbers.Where(m => !m.Archived).ToList();
        }

        public async Task UpdateEthicsNumber(EthicsNumber ethicsNumber)
        {
            db.Entry(ethicsNumber).State = EntityState.Modified;
            await db.SaveChangesAsync();
        }

        public IEnumerable<EthicsNumberHistory> GetEthicsNumberHistoryByEthicsId(int ethicsId)
        {
            return db.EthicsNumberHistories
                    .Where(m =>
                            m.Ethics_Id == ethicsId &&
                            m.Animal
                                .EthicsNumbers
                                .OrderByDescending(n => n.Timestamp)
                                .FirstOrDefault().Id == m.Id);
        }

        public async Task ArchiveEthics(EthicsNumber ethicsNumber)
        {
            ethicsNumber.Archived = true;
            await db.SaveChangesAsync();
        }

        public IEnumerable<EthicsNumber> GetArchivedNumbers()
        {
            return db.EthicsNumbers.Where(m => m.Archived).ToList();
        }

        public async Task<EthicsNumber> GetEthicsNumberByName(string name)
        {
            return await db.EthicsNumbers.SingleOrDefaultAsync(m => m.Text == name);
        }
    }
}
