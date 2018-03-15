//Author Kevin Haggerty
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace back_end_api
{
    public class User: IdentityUser
    {

        public string FirstName { get; set; }
        
        public string LastName { get; set; }
        
        public bool isStaff { get; set; }    

        public virtual ICollection<DailyCheck> DailyCheck { get; set; } 
        public virtual ICollection<Goals> Goals { get; set; } 
        public virtual ICollection<Post> Post { get; set; } 
    }
}