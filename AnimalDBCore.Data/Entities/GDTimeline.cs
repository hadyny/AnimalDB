﻿using System.ComponentModel.DataAnnotations;

namespace  AnimalDBCore.Core.Entities
{
    public class GDTimeline
    {
        public int Id { get; set; }

        [Required]
        public string Description { get; set; }

        [Display(Name = "Days after Plug")]
        public int Offset { get; set; }
    }
}
