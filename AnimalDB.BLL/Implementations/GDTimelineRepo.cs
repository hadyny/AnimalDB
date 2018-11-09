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
    public class GDTimelineRepo : IGDTimeline, IDisposable
    {
        private AnimalDBContext db;

        public GDTimelineRepo()
        {
            this.db = new AnimalDBContext();
        }
        public GDTimelineRepo(AnimalDBContext db)
        {
            this.db = db;
        }

        public async Task CreateGDTimeline(GDTimeline gDTimeline)
        {
            db.GDTimelines.Add(gDTimeline);
            await db.SaveChangesAsync();
        }

        public List<string> GetUnusedTimelineList()
        {
            return db.GDTimelines
                        .Where(m => db.SurgicalNotes.Count(n => n.GDTimeline_Id == m.Id) == 0)
                        .Select(m => m.Description)
                        .ToList();
        }

        public async Task DeleteGDTimeline(GDTimeline gDTimeline)
        {
            if (db.Entry(gDTimeline).State == EntityState.Detached)
            {
                db.GDTimelines.Attach(gDTimeline);
            }
            db.GDTimelines.Remove(gDTimeline);
            await db.SaveChangesAsync();
        }

        public void Dispose()
        {
            ((IDisposable)db).Dispose();
        }

        public async Task<GDTimeline> GetGDTimelineById(int id)
        {
            return await db.GDTimelines.FindAsync(id);
        }

        public IEnumerable<GDTimeline> GetGDTimelines()
        {
            return db.GDTimelines.ToList();
        }

        public async Task UpdateGDTimeline(GDTimeline gDTimeline)
        {
            db.Entry(gDTimeline).State = EntityState.Modified;
            await db.SaveChangesAsync();
        }

        public bool CheckIfTimeLineExists(GDTimeline gDTimeline)
        {
            return db.GDTimelines.Count(m => m.Description == gDTimeline.Description) != 0;
        }
    }
}
