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
        //Children of Project
        public virtual ICollection<Ticket> Tickets { get; set; }
        public virtual ICollection<ApplicationUser> Users { get; set; }

        public Project()
        {
            this.Tickets = new HashSet<Ticket>();
            this.Users = new HashSet<ApplicationUser>();

        }
    }
}