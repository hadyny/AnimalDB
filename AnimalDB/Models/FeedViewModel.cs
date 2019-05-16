using System.Collections.Generic;

namespace AnimalDB.Web.Models
{
    public class FeedViewModel
    {
        public FeedViewModel()
        {
            Rooms = new HashSet<FeedRow>();
        }

        public ICollection<FeedRow> Rooms { get; set; }

        public int TotalAnimals { get; set; }

    }

    public class FeedRow
    {
        public string RoomName { get; set; }

        public int RoomId { get; set; }

        public string ClassList { get; set; }

        public int TotalAnimals { get; set; }

        public int StockAnimals { get; set; }

        public int GMOAnimals { get; set; }
    }
}