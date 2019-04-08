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
    public class RosterRepo : IRoster, IDisposable
    {
        private AnimalDBContext db;

        public RosterRepo()
        {
            this.db = new AnimalDBContext();
        }

        public RosterRepo(AnimalDBContext db)
        {
            this.db = db;
        }

        public async Task CreateRoster(Roster roster)
        {
            db.Rosters.Add(roster);
            await db.SaveChangesAsync();
        }

        public async Task DeleteRoster(Roster roster)
        {
            if (db.Entry(roster).State == EntityState.Detached)
            {
                db.Rosters.Attach(roster);
            }
            db.Rosters.Remove(roster);
            await db.SaveChangesAsync();
        }

        public void Dispose()
        {
            ((IDisposable)db).Dispose();
        }

        public async Task<Roster> GetRosterById(int id)
        {
            return await db.Rosters.FindAsync(id);
        }

        public IEnumerable<Roster> GetRosters()
        {
            return db.Rosters.ToList();
        }

        public IEnumerable<Roster> GetRostersByRoomId(int id)
        {
            return db.Rosters.Where(m => m.Room_Id == id).ToList();
        }

        public async Task UpdateRoster(Roster roster)
        {
            db.Entry(roster).State = EntityState.Modified;
            await db.SaveChangesAsync();
        }

        
    }
}
