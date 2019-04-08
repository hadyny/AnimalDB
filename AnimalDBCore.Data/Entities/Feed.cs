using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace  AnimalDBCore.Core.Entities
{
    public class Feed
    {
        public int Id { get; set; }

        public DateTime Date { get; set; }

        public int? Weight { get; set; }

        public int? FeedAmount { get; set; }

        [Display(Name = "Animal")]
        public int Animal_Id { get; set; }
        [ForeignKey("Animal_Id")]
        public virtual Animal Animal { get; set; }

        public int FeedingGroupId { get; set; }

        public bool CombinedFeed { get; set; }
    }
}