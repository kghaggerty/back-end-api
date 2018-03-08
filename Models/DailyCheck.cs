//Author Kevin Haggerty
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace back_end_api
{
    public class DailyCheck
    {
        [Key]
        public int DailyCheckId { get; set; }

        [Required]
        public string feeling { get; set; }

        [Required]
        public string actions { get; set; }

        public DateTime DateCreated { get; set; }

        [Required]
        public string NeedSupport { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
    }
}