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
    public class FeedingGroupRepo : IFeedingGroup
    {
        private readonly AnimalDBContext db;

        public FeedingGroupRepo()
        {
            this.db = new AnimalDBContext();
        }
        public FeedingGroupRepo(AnimalDBContext db)
        {
            this.db = db;
        }


        public async Task CreateFeedingGroup(FeedingGroup feedingGroup)
        {
            db.FeedingGroups.Add(feedingGroup);
            await db.SaveChangesAsync();
        }

        public async Task DeleteFeedingGroup(FeedingGroup feedingGroup)
        {
            foreach (var animal in feedingGroup.Animals.ToList())
            {
                animal.FeedingGroup_Id = null;
                animal.Group_Id = null;
                db.Entry(animal).State = EntityState.Modified;
            }

            if (db.Entry(feedingGroup).State == EntityState.Detached)
            {
                db.FeedingGroups.Attach(feedingGroup);
            }

            db.FeedingGroups.Remove(feedingGroup);
            
            await db.SaveChangesAsync();
        }

        public async Task<FeedingGroup> GetFeedingGroupById(int id)
        {
            return await db.FeedingGroups.FindAsync(id);
        }

        public IEnumerable<FeedingGroup> GetFeedingGroups()
        {
            return db.FeedingGroups.ToList();
        }

        public async Task UpdateFeedingGroup(FeedingGroup feedingGroup)
        {
            db.Entry(feedingGroup).State = EntityState.Modified;
            await db.SaveChangesAsync();
        }

        public IEnumerable<FeedingGroup> GetFeedingGroupsByRoomId(int roomId)
        {
            return db.FeedingGroups.Where(m => m.Room_Id == roomId).ToList();
        }

        public async Task UpdateFeedingGroupPage(FeedingGroup model)
        {
            foreach (var animal in model.Animals)
            {
                foreach (var feed in animal.Feeds.OrderByDescending(m => m.Date).Distinct())
                {
                    //if (feed.Id == 0)
                    if (db.Feeds.Count(m => m.Animal_Id == feed.Animal_Id && m.Date == feed.Date) == 0)
                    {
                        if (feed.FeedAmount != null || feed.Weight != null)
                        {
                            db.Feeds.Add(feed);
                        }
                    }
                    else
                    {
                        //var oldFeed = db.Feeds.Find(feed.Id);
                        var oldFeed = db.Feeds.First(m => m.Animal_Id == feed.Animal_Id && m.Date == feed.Date);

                        if (feed.FeedAmount == null && feed.Weight == null)
                        {
                            db.Feeds.Remove(oldFeed);
                        }
                        else
                        {
                            oldFeed.CombinedFeed = feed.CombinedFeed;
                            oldFeed.FeedAmount = feed.FeedAmount;
                            oldFeed.Weight = feed.Weight;
                        }
                    }
                }

                db.Animals.Find(animal.Id).LastChecked = DateTime.Now.Date;
            }
            await db.SaveChangesAsync();
        }

        public ICollection<Animal> GetAnimalsInFeedingGroup(int feedingGroupId)
        {
            return db.Animals
                    .Where(m => m.FeedingGroup_Id == feedingGroupId)
                    .ToList();
        }

        public async Task RemoveAnimalFromFeedingGroup(int animalId)
        {
            var animal = await db.Animals.FindAsync(animalId);
            int? group = animal.FeedingGroup_Id;
            animal.FeedingGroup_Id = null;
            animal.Group_Id = null;
            db.Entry(animal).State = EntityState.Modified;
            await db.SaveChangesAsync();
        }

        public int? GetFeedingGroupByAnimalId(int animalId)
        {
            return db.Animals.Find(animalId)?.FeedingGroup_Id;
        }
    }
}
