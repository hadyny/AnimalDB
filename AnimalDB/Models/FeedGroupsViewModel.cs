using AnimalDB.Repo.Entities;
using System;
using System.Collections.Generic;

namespace AnimalDB.Web.Models
{
    public class FeedGroupsViewModel
    {
        public int NumAppAnimals { get; set; }
        public int GMOCount { get; set; }
        public Room Room { get; set; }
        public bool AllChecked { get; set; }
        public bool DoneToday { get; set; }
        public DateTime startDay { get; set; }
        public IEnumerable<AnimalRoomCount> CountHistory { get; set; }
        public bool AnimalsNotInDB { get; set; }
        public bool LastRoomCheckToday { get; set; }
        public IEnumerable<FeedingGroup> groups { get; set; }
    }
}