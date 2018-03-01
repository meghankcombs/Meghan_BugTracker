using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Meghan_BugTracker.Models
{
    public class Project
    {
        public int Id { get; set; }
        [Display(Name = "Project")]
        public string Name { get; set; }
        public string Description { get; set; }

        //Navigation
        public virtual ICollection<Ticket> Tickets { get; set; }
        //many to many relationship, bc ICollection also in AppUser class
        public virtual ICollection<ApplicationUser> Users { get; set; } 

        public Project()
        {
            this.Tickets = new HashSet<Ticket>();
            this.Users = new HashSet<ApplicationUser>();

        }
    }
}