﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Meghan_BugTracker.Models
{
    public class TicketHistory
    {
        public int Id { get; set; }
        [Display(Name = "History")]
        public int TicketId { get; set; }
        public string Property { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
        public DateTime ChangedDate { get; set; }
        public string UserId { get; set; }

        //Navigation
        public virtual Ticket Ticket { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}