namespace AnimalDB.Repo.Entities
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    
    public class Species
    {
        public Species()
        {
            Strains = new HashSet<Strain>();
        }
    
        public int Id { get; set; }
        [Required, Display(Name = "Species")]
        public string Description { get; set; }
    
        public virtual ICollection<Strain> Strains { get; set; }
    }
}
