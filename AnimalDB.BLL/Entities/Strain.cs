namespace AnimalDB.Repo.Entities
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    
    public class Strain
    {
        public Strain()
        {
            this.Animals = new HashSet<Animal>();
        }
    
        public int Id { get; set; }
        [Required]
        public string Description { get; set; }
        [Display(Name = "Species")]
        public int Species_Id { get; set; }
        [ForeignKey("Species_Id")]
        public virtual Species Species { get; set; }
        [Required, StringLength(2), Display(Name = "2 Character Code")]
        public string Code { get; set; }
    
        public virtual ICollection<Animal> Animals { get; set; }
    }
}
