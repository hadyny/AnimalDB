namespace AnimalDB.Repo.Entities
{
    using System.ComponentModel.DataAnnotations;
    
    public class Colour
    {
        [UIHint("Colour")]
        public int Id { get; set; }
        [Required, Display(Name = "Colour")]
        public string Description { get; set; }
    }
}
