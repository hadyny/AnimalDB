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
    public class EthicsDocumentRepo : IEthicsDocument, IDisposable
    {
        private AnimalDBContext db;

        public EthicsDocumentRepo()
        {
            this.db = new AnimalDBContext();
        }
        public EthicsDocumentRepo(AnimalDBContext db)
        {
            this.db = db;
        }

        public async Task CreateEthicsDocument(EthicsDocument ethicsDocument)
        {
            db.EthicsDocuments.Add(ethicsDocument);
            await db.SaveChangesAsync();
        }

        public async Task DeleteEthicsDocument(EthicsDocument ethicsDocument)
        {
            if (db.Entry(ethicsDocument).State == EntityState.Detached)
            {
                db.EthicsDocuments.Attach(ethicsDocument);
            }
            db.EthicsDocuments.Remove(ethicsDocument);
            await db.SaveChangesAsync();
        }

        public void Dispose()
        {
            ((IDisposable)db).Dispose();
        }

        public async Task<EthicsDocument> GetEthicsDocumentById(int id)
        {
            return await db.EthicsDocuments.FindAsync(id);
        }

        public IEnumerable<EthicsDocument> GetEthicsDocumentsByInvestigatorId(string investigatorId)
        {
            return db.EthicsDocuments.Where(m => m.Investigator_Id == investigatorId).ToList();
        }

        public IEnumerable<EthicsDocument> GetEthicsDocuments()
        {
            return db.EthicsDocuments.ToList();
        }

        public async Task UpdateEthicsDocument(EthicsDocument ethicsDocument)
        {
            db.Entry(ethicsDocument).State = EntityState.Modified;
            await db.SaveChangesAsync();
        }

        public bool CheckIfDocumentExists(string fileName)
        {
            return db.EthicsDocuments.Where(m => m.FileName == fileName).Count() != 0;
        }
    }
}
