namespace AnimalDB.Repo.Entities
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    
    public class MedicationType
    {
        public MedicationType()
        {
            this.Medication = new HashSet<Medication>();
        }
    
        public int Id { get; set; }
        [Required, Display(Name = "Medication")]
        public string Description { get; set; }
    
        public virtual ICollection<Medication> Medication { get; set; }
    }
}
