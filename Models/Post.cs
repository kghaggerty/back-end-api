//Author Kevin Haggerty
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace back_end_api
{
    public class Post
    {
        [Key]
        public int PostId { get; set; }

        [Required]
        public string Text { get; set; }

        public DateTime DateCreated { get; set; }

        [Required]
        public int UserId { get; set; }

        public User User { get; set; }
         
    }
}