//Author Kevin Haggerty
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace back_end_api
{
    public class Goals
    {
        [Key]
        public int GoalsId { get; set; }

        [Required]
        public string Description { get; set; }

        public DateTime DateCreated { get; set; }

        [Required]
        public DateTime DateDue { get; set; }
        public bool isCompleted { get; set; }     

        [Required]
        public virtual User User { get; set; }
    }
}