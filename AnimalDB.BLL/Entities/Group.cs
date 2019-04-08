namespace AnimalDB.Repo.Entities
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Group
    {
        public Group()
        {
            Animals = new HashSet<Animal>();
        }

        public int Id { get; set; }
        [Required, Display(Name = "Group")]
        public string Description { get; set; }

        public int? FeedingGroup_Id { get; set; }
        [ForeignKey("FeedingGroup_Id")]
        public virtual FeedingGroup FeedingGroup { get; set; }

        public virtual ICollection<Animal> Animals { get; set; }
    }
}
