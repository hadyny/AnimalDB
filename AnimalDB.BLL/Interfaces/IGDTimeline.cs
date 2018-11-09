using AnimalDB.Repo.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AnimalDB.Repo.Interfaces
{
    public interface IGDTimeline
    {
        IEnumerable<GDTimeline> GetGDTimelines();

        Task CreateGDTimeline(GDTimeline gDTimeline);

        Task<GDTimeline> GetGDTimelineById(int id);

        Task UpdateGDTimeline(GDTimeline gDTimeline);

        Task DeleteGDTimeline(GDTimeline gDTimeline);

        List<string> GetUnusedTimelineList();

        bool CheckIfTimeLineExists(GDTimeline gDTimeline);
    }
}
