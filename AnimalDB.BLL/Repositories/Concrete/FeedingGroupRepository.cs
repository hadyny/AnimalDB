using AnimalDB.Repo.Contexts;
using AnimalDB.Repo.Entities;
using AnimalDB.Repo.Repositories.Abstract;
using AnimalDB.Repositories.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnimalDB.Repo.Repositories.Concrete
{
    public class FeedingGroupRepository : Repository<FeedingGroup>, IFeedingGroupRepository
    {
        public FeedingGroupRepository(AnimalDBContext context) : base(context)
        {
        }

        public override void Delete(FeedingGroup feedingGroup)
        {
            foreach (var animal in feedingGroup.Animals.ToList())
            {
                animal.FeedingGroup_Id = null;
                animal.Group_Id = null;
                Context.Entry(animal).State = System.Data.Entity.EntityState.Modified;
            }

            Context.FeedingGroups.Remove(feedingGroup);
        }

        public IEnumerable<FeedingGroup> GetByRoomId(int roomId)
        {
            return Context.FeedingGroups.Where(m => m.Room_Id == roomId).ToList();
        }

        public async Task UpdateFeedingGroupPage(FeedingGroup model)
        {
            foreach (var animal in model.Animals)
            {
                var allFeeds = Context.Feeds.Where(m => m.Animal_Id == animal.Id);

                foreach (var feed in animal.Feeds.OrderByDescending(m => m.Date).Distinct())
                {
                    //if (feed.Id == 0)
                    if (allFeeds.Count(m => m.Animal_Id == feed.Animal_Id && m.Date == feed.Date) == 0)
                    {
                        if (feed.FeedAmount != null || feed.Weight != null)
                        {
                            Context.Feeds.Add(feed);
                        }
                    }
                    else
                    {
                        //var oldFeed = db.Feeds.Find(feed.Id);
                        var oldFeed = allFeeds.First(m => m.Animal_Id == feed.Animal_Id && m.Date == feed.Date);

                        if (feed.FeedAmount == null && feed.Weight == null)
                        {
                            Context.Feeds.Remove(oldFeed);
                        }
                        else
                        {
                            oldFeed.CombinedFeed = feed.CombinedFeed;
                            oldFeed.FeedAmount = feed.FeedAmount;
                            oldFeed.Weight = feed.Weight;
                        }
                    }
                }

                var a = await Context.Animals.FindAsync(animal.Id);
                a.LastChecked = DateTime.Now.Date;
            }
        }

        public IEnumerable<Animal> GetAnimalsInFeedingGroup(int feedingGroupId)
        {
            return Context.Animals.Where(m => m.FeedingGroup_Id == feedingGroupId).ToList();
        }

        public async Task RemoveAnimalFromFeedingGroup(int animalId)
        {
            var animal = await Context.Animals.FindAsync(animalId);
            animal.FeedingGroup_Id = null;
            animal.Group_Id = null;
            Context.Entry(animal).State = System.Data.Entity.EntityState.Modified;
        }

        public async Task<int?> GetByAnimalId(int animalId)
        {
            var animal = await Context.Animals.FindAsync(animalId);
            return animal?.FeedingGroup_Id;
        }

        public AnimalDBContext Context
        {
            get { return base.db; }
        }
    }
}
