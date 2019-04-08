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
    public class FeedRepo : IFeed, IDisposable
    {
        private AnimalDBContext db;

        public FeedRepo()
        {
            this.db = new AnimalDBContext();
        }
        public FeedRepo(AnimalDBContext db)
        {
            this.db = db;
        }

        public async Task CreateFeed(Feed feed)
        {
            db.Feeds.Add(feed);
            await db.SaveChangesAsync();
        }

        public async Task DeleteFeed(Feed feed)
        {
            if (db.Entry(feed).State == EntityState.Detached)
            {
                db.Feeds.Attach(feed);
            }
            db.Feeds.Remove(feed);
            await db.SaveChangesAsync();
        }

        public void Dispose()
        {
            ((IDisposable)db).Dispose();
        }

        public async Task<Feed> GetFeedById(int id)
        {
            return await db.Feeds.FindAsync(id);
        }

        public IEnumerable<Feed> GetFeeds()
        {
            return db.Feeds.ToList();
        }

        public async Task UpdateFeed(Feed feed)
        {
            db.Entry(feed).State = EntityState.Modified;
            await db.SaveChangesAsync();
        }
    }
}
