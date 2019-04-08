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
    public class ApprovalNumberRepo : IApprovalNumber, IDisposable
    {
        private AnimalDBContext db;

        public ApprovalNumberRepo()
        {
            this.db = new AnimalDBContext();
        }
        public ApprovalNumberRepo(AnimalDBContext db)
        {
            this.db = db;
        }

        public async Task CreateApprovalNumber(ApprovalNumber approvalNumber)
        {
            db.ApprovalNumbers.Add(approvalNumber);
            await db.SaveChangesAsync();
        }

        public async Task DeleteApprovalNumber(ApprovalNumber approvalNumber)
        {
            if (db.Entry(approvalNumber).State == EntityState.Detached)
            {
                db.ApprovalNumbers.Attach(approvalNumber);
            }
            db.ApprovalNumbers.Remove(approvalNumber);
            await db.SaveChangesAsync(); ;
        }

        public void Dispose()
        {
            ((IDisposable)db).Dispose();
        }

        public async Task<ApprovalNumber> GetApprovalNumberById(int id)
        {
            return await db.ApprovalNumbers.FindAsync(id);
        }

        public IEnumerable<ApprovalNumber> GetApprovalNumbers()
        {
            return db.ApprovalNumbers.ToList();
        }

        public async Task UpdateApprovalNumber(ApprovalNumber approvalNumber)
        {
            db.Entry(approvalNumber).State = EntityState.Modified;
            await db.SaveChangesAsync();
        }
    }
}
