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
    public class FeedRepo : IFeed
    {
        private readonly AnimalDBContext db;

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
