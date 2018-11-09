using AnimalDB.Repo.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AnimalDB.Repo.Interfaces
{
    public interface ICleaningTask
    {
        IEnumerable<CleaningTask> GetCleaningTasks();

        Task CreateCleaningTask(CleaningTask cleaningTask);

        Task<CleaningTask> GetCleaningTaskById(int id);

        Task UpdateCleaningTask(CleaningTask cleaningTask);

        Task DeleteCleaningTask(CleaningTask cleaningTask);
    }
}
