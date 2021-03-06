﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AnimalDB.Repo.Entities
{
    public class EthicsDocument
    {
        public int Id { get; set; }

        [Required]
        public string Description { get; set; }

        [StringLength(255)]
        public string FileName { get; set; }

        public byte[] Content { get; set; }

        [Display(Name = "Date Uploaded")]
        public DateTime DateUploaded { get; set; }

        [Display(Name = "Investigator")]
        public string Investigator_Id { get; set; }
        [ForeignKey("Investigator_Id")]
        public Investigator Investigator { get; set; }
    }
}
