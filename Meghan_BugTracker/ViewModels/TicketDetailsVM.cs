﻿using Meghan_BugTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Meghan_BugTracker.ViewModels
{
    public class TicketDetailsVM
    {
        public Ticket Ticket { get; set; }
        public TicketComment TicketComment { get; set; }
        public TicketAttachment TicketAttachment { get; set; }
    }
}