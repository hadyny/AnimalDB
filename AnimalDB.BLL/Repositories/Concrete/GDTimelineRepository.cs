using AnimalDB.Repo.Contexts;
using AnimalDB.Repo.Entities;
using AnimalDB.Repo.Repositories.Abstract;
using AnimalDB.Repositories.Concrete;
using System.Collections.Generic;
using System.Linq;

namespace AnimalDB.Repo.Repositories.Concrete
{
    public class GDTimelineRepository : Repository<GDTimeline>, IGDTimelineRepository
    {
        public GDTimelineRepository(AnimalDBContext context) : base(context)
        {
        }

        public List<string> GetUnused()
        {
            var notes = Context.SurgicalNotes;
            var timelines = Context.GDTimelines;
            return timelines
                        .Where(m => notes.Count(n => n.GDTimeline_Id == m.Id) == 0)
                        .Select(m => m.Description)
                        .ToList();
        }

        public bool Exists(GDTimeline gDTimeline)
        {
            return Context.GDTimelines.Count(m => m.Description == gDTimeline.Description && m.Id != gDTimeline.Id) != 0;
        }

        public AnimalDBContext Context
        {
            get { return base.db; }
        }
    }
}
