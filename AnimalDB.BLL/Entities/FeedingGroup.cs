namespace AnimalDB.Repo.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class FeedingGroup
    {
        public FeedingGroup()
        {
            this.Animals = new HashSet<Animal>();
            this.Groups = new HashSet<Group>();
        }

        public int Id { get; set; }
        [Required, Display(Name = "Feeding Group")]
        public string Description { get; set; }

        public DateTime? FirstFeed { get; set; }

        public string Investigator_Id { get; set; }
        [ForeignKey("Investigator_Id")]
        public virtual Investigator Investigator { get; set; }

        public virtual ICollection<Animal> Animals { get; set; }

        public virtual ICollection<Group> Groups { get; set; }

        [Required, Display(Name = "Room")]
        public int? Room_Id { get; set; }
        [ForeignKey("Room_Id")]
        public virtual Room Room { get; set; }
    }

}
