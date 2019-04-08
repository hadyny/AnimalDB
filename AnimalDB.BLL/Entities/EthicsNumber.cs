namespace AnimalDB.Repo.Entities
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class EthicsNumber
    {
        public EthicsNumber()
        {
            EthicsNumberHistory = new HashSet<EthicsNumberHistory>();
        }
    
        public int Id { get; set; }
        [Required, Display(Name = "AUP Number")]
        public string Text { get; set; }

        [Required, Display(Name = "First Year of Ethics Period")]
        public int? StartYear { get; set; }

        public string Investigator_Id { get; set; }
        [ForeignKey("Investigator_Id")]
        public virtual Investigator Investigator { get; set; }

        public int? Species_Id { get; set; }
        [ForeignKey("Species_Id")]
        public virtual Species Species { get; set; }

        public bool Archived { get; set; } = false;

        public virtual ICollection<EthicsNumberHistory> EthicsNumberHistory { get; set; }
    }
}
