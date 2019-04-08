namespace  AnimalDBCore.Core.Entities
{
    using  AnimalDBCore.Core.Enums;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    
    public class Source
    {
        public Source()
        {
            this.Animals = new HashSet<Animal>();
        }
        
        public int Id { get; set; }
        [Required, Display(Name = "Source")]
        public string Description { get; set; }
        
        public virtual ICollection<Animal> Animals { get; set; }

        public SourceType Type { get; set; }
    }
}
