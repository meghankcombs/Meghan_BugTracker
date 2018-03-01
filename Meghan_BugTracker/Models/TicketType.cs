using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Meghan_BugTracker.Models
{
    public class TicketType
    {
        public int Id { get; set; }
        [Display(Name = "Type")]
        public string Name { get; set; }

        //Navigation
        public virtual ICollection<Ticket> Tickets { get; set; }

        public TicketType()
        {
            this.Tickets = new HashSet<Ticket>();
        }
    }
}