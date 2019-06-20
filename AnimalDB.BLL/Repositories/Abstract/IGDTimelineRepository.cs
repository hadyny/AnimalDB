using AnimalDB.Repo.Entities;
using System.Collections.Generic;

namespace AnimalDB.Repo.Repositories.Abstract
{
    public interface IGDTimelineRepository : IRepository<GDTimeline>
    {
        List<string> GetUnused();
        bool Exists(GDTimeline gDTimeline);
    }
}
