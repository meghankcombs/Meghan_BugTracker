using Meghan_BugTracker.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Meghan_BugTracker.Helpers
{
    public class TicketsHelper
    {
        private static ApplicationDbContext db = new ApplicationDbContext(); //default scope = private
        private static ProjectsHelper projHelper = new ProjectsHelper();

        public List<Ticket> ListUserTickets(string userId) //return list of tickets associated with this specific user
        {
            var userTickets = db.Tickets.AsNoTracking().Where(t => t.AssignedToUserId == userId);
            return userTickets.ToList();
        }

        public List<Ticket> SubmitterTickets()
        {
            var userId = HttpContext.Current.User.Identity.GetUserId();
            return (db.Tickets.Where(t => t.OwnerUserId == userId)).ToList();
        }

        public static bool IsUserOnTicket(string userId, int ticketId)
        {
            //Given as TicketId, pull back the AssignedToUserId
            return db.Tickets.Find(ticketId).AssignedToUserId == userId;
        }

        public static bool DoesUserOwnTicket(string userId, int ticketId)
        {
            //Given as TicketId, pull back the AssignedToUserId
            return db.Tickets.Find(ticketId).OwnerUserId == userId;
        }

        public void AddUserToTicket(string userId, int ticketId) 
        {
            if (!IsUserOnTicket(userId, ticketId))
            {
                Ticket tix = db.Tickets.Find(ticketId); 
                tix.AssignedToUserId = userId; //adding user to ticket
                db.Entry(tix).State = EntityState.Modified;
                db.SaveChanges();
            }
        }

        public void RemoveUserFromTicket(string userId, int ticketId) //remove user from ticket
        {
            if (IsUserOnTicket(userId, ticketId))
            {
                Ticket tix = db.Tickets.Find(ticketId);
                tix.AssignedToUserId = null; //removing user from ticket
                db.Entry(tix).State = EntityState.Modified; //modifies existing record; usually only used in an edit...
                db.SaveChanges();
            }
        }

        public bool IsTicketOnPMProject(int ticketId)
        {
            var userId = HttpContext.Current.User.Identity.GetUserId();
            var projectId = db.Tickets.Find(ticketId).ProjectId;
            return projHelper.IsUserOnProject(userId, projectId);
        }

        public List<TicketComment> ListTicketsComments(int ticketId) //return list of comments associated with specific Ticket
        {
            var ticketComments = db.TicketComments.AsNoTracking().Where(t => t.TicketId == ticketId);
            return ticketComments.ToList();
        }

        public List<TicketAttachment> ListTicketsAttachments(int ticketId) //return list of comments associated with specific Ticket
        {
            var ticketAttachments = db.TicketAttachments.AsNoTracking().Where(t => t.TicketId == ticketId);
            return ticketAttachments.ToList();
        }

        public void AddTicketHistory(Ticket oldTicket, Ticket newTicket)
        {
            List<TicketHistory> histories = new List<TicketHistory>();

            if (oldTicket.Title != newTicket.Title)
            {
                histories.Add(new TicketHistory()
                {
                    OldValue = oldTicket.Title,
                    NewValue = newTicket.Title,
                    Property = "Title"
                });
            }
            
            if(oldTicket.Description != newTicket.Description)
            {
                histories.Add(new TicketHistory()
                {
                    OldValue = oldTicket.Description,
                    NewValue = newTicket.Description,
                    Property = "Description"
                });
            }

            if(oldTicket.AssignedToUserId != newTicket.AssignedToUserId)
            {
                histories.Add(new TicketHistory()
                {
                    OldValue = db.Users.Find(oldTicket.AssignedToUserId).DisplayName,
                    NewValue = db.Users.Find(newTicket.AssignedToUserId).DisplayName,
                    Property = "Assigned User"
                });
            }

            if(oldTicket.TicketTypeId != newTicket.TicketTypeId)
            {
                histories.Add(new TicketHistory()
                {
                    OldValue = db.TicketTypes.Find(oldTicket.TicketTypeId).Name,
                    NewValue = db.TicketTypes.Find(newTicket.TicketTypeId).Name,
                    Property = "Ticket Type"
                });
            }

            if(oldTicket.TicketPriorityId != newTicket.TicketPriorityId)
            {
                histories.Add(new TicketHistory()
                {
                    OldValue = db.TicketPriorities.Find(oldTicket.TicketPriorityId).Name,
                    NewValue = db.TicketPriorities.Find(newTicket.TicketPriorityId).Name,
                    Property = "Ticket Priority"
                });
            }

            if(oldTicket.TicketStatusId != newTicket.TicketStatusId)
            {
                histories.Add(new TicketHistory()
                {
                    OldValue = db.TicketStatus.Find(oldTicket.TicketStatusId).Name,
                    NewValue = db.TicketStatus.Find(newTicket.TicketStatusId).Name,
                    Property = "Ticket Status"
                });
            }

            for(int i = 0; i < histories.Count; i++)
            {
                histories[i].TicketId = newTicket.Id;
                histories[i].UserId = HttpContext.Current.User.Identity.GetUserId();
                histories[i].ChangedDate = DateTime.Now;
                db.TicketHistories.Add(histories[i]);
            }

            db.SaveChanges();
        }

        public List<TicketHistory> ListTicketsHistories(int ticketId)
        {
            var ticketHistories = db.TicketHistories.AsNoTracking().Where(t => t.TicketId == ticketId);
            return ticketHistories.ToList();
        }

        //Overloaded method to send email to user who was assigned to a ticket
        public async Task AddTicketNotification(int ticketId, string oldAssignedToId, string newAssignedToId)
        {
            if(String.IsNullOrEmpty(oldAssignedToId) && String.IsNullOrEmpty(newAssignedToId))
                return;

            //If user assigned to ticket
            if(String.IsNullOrEmpty(oldAssignedToId) && !String.IsNullOrEmpty(newAssignedToId))
            {
                //Get copy of Ticket
                var ticket = db.Tickets.AsNoTracking().Include("Project").FirstOrDefault(t => t.Id == ticketId);
                var user = db.Users.FirstOrDefault(u => u.Id == newAssignedToId);
                if(user != null)
                {
                    //Create new Notification
                    var userId = HttpContext.Current.User.Identity.GetUserId();
                    var ticketNotification = new TicketNotification();
                    ticketNotification.SenderId = userId;
                    ticketNotification.Created = DateTime.Now;
                    ticketNotification.TicketId = ticketId;
                    ticketNotification.RecipientId = newAssignedToId;

                    //Assemble notification body
                    var msgBody = new StringBuilder();
                    msgBody.AppendFormat("Hello {0}", user.FirstName);
                    msgBody.AppendLine("");
                    msgBody.AppendLine("You have been assigned to ticket: " + ticket.Title + ", for project: " + ticket.Project.Name);

                    //Set Body
                    ticketNotification.Body = msgBody.ToString();

                    db.TicketNotifications.Add(ticketNotification);
                    db.SaveChanges();

                    //Send email
                    var from = "Kink Fix<meghankcombs@gmail.com>";
                    var to = db.Users.Find(newAssignedToId).Email;
                    var email = new MailMessage(from, to)
                    {
                        Subject = "Kink Fix: You have a new notification",
                        Body = "You have been assigned to a ticket. Please sign into your Kink Fix account to view details.",
                        IsBodyHtml = true
                    };

                    var svc = new PersonalEmail();
                    await svc.SendAsync(email);
                }
            }

            //if user unassigned
            if (!String.IsNullOrEmpty(oldAssignedToId) && String.IsNullOrEmpty(newAssignedToId))
            {
                //Get copy of Ticket
                var ticket = db.Tickets.AsNoTracking().Include("Project").FirstOrDefault(t => t.Id == ticketId);
                var user = db.Users.FirstOrDefault(u => u.Id == newAssignedToId);
                if (user != null)
                {
                    //Create new Notification
                    var userId = HttpContext.Current.User.Identity.GetUserId();
                    var ticketNotification = new TicketNotification();
                    ticketNotification.SenderId = userId;
                    ticketNotification.Created = DateTime.Now;
                    ticketNotification.TicketId = ticketId;
                    ticketNotification.RecipientId = oldAssignedToId;

                    //Assemble notification body
                    var msgBody = new StringBuilder();
                    msgBody.AppendFormat("Hello {0}", user.FirstName);
                    msgBody.AppendLine("");
                    msgBody.AppendLine("You have been unassigned from ticket: " + ticket.Title + ", for project: " + ticket.Project.Name);

                    //Set Body
                    ticketNotification.Body = msgBody.ToString();

                    db.TicketNotifications.Add(ticketNotification);
                    db.SaveChanges();

                    //Send email
                    var from = "Kink Fix<meghankcombs@gmail.com>";
                    var to = db.Users.Find(newAssignedToId).Email;
                    var email = new MailMessage(from, to)
                    {
                        Subject = "Kink Fix: You have a new notification",
                        Body = "You have been unassigned from a ticket. Please sign into your Kink Fix account to view details.",
                        IsBodyHtml = true
                    };

                    var svc = new PersonalEmail();
                    await svc.SendAsync(email);
                }
            }

        }

        //Overloaded method to send email to user when attachment added to ticket
        public async Task AddTicketNotification(TicketAttachment attachment)
        {
            var myTicket = db.Tickets.AsNoTracking().Include("Project").FirstOrDefault(t => t.Id == attachment.TicketId);
            var user = db.Users.FirstOrDefault(u => u.Id == myTicket.AssignedToUserId);
            if (user != null)
            {
                //Create new notification
                var ticketNotification = new TicketNotification();
                ticketNotification.SenderId = attachment.UserId;
                ticketNotification.Created = DateTime.Now;
                ticketNotification.TicketId = attachment.TicketId;

                ticketNotification.RecipientId = myTicket.AssignedToUserId;

                //Assemble notification body
                var msgBody = new StringBuilder();
                msgBody.AppendFormat("Hello {0}", user.FirstName);
                msgBody.AppendLine("");
                msgBody.AppendLine("An attachment has been added to ticket: " + myTicket.Title + ", for project: " + myTicket.Project.Name);

                //Set Body
                ticketNotification.Body = msgBody.ToString();

                db.TicketNotifications.Add(ticketNotification);
                db.SaveChanges();

                //Send email
                var from = "Kink Fix<meghankcombs@gmail.com>";
                var to = db.Users.Find(myTicket.AssignedToUserId).Email;
                var email = new MailMessage(from, to)
                {
                    Subject = "Kink Fix: You have a new notification",
                    Body = "An attachment has been added to one of your assigned tickets. Please sign into your Kink Fix account to view details.",
                    IsBodyHtml = true
                };

                var svc = new PersonalEmail();
                await svc.SendAsync(email);
            }
        }

        //Overloaded method to send email to user when comment added to ticket
        public async Task AddTicketNotification(TicketComment comment)
        {
            var myComment = db.Tickets.AsNoTracking().Include("Project").FirstOrDefault(t => t.Id == comment.TicketId);
            var user = db.Users.FirstOrDefault(u => u.Id == myComment.AssignedToUserId);
            if (user != null)
            {
                //Create new notification
                var ticketNotification = new TicketNotification();
                ticketNotification.SenderId = comment.UserId;
                ticketNotification.Created = DateTime.Now;
                ticketNotification.TicketId = comment.TicketId;


                ticketNotification.RecipientId = myComment.AssignedToUserId;

                //Assemble notification body
                var msgBody = new StringBuilder();
                msgBody.AppendFormat("Hello {0}", user.FirstName);
                msgBody.AppendLine("");
                msgBody.AppendLine("A comment has been added to ticket: " + myComment.Title + ", for project: " + myComment.Project.Name);

                //Set Body
                ticketNotification.Body = msgBody.ToString();

                db.TicketNotifications.Add(ticketNotification);
                db.SaveChanges();

                //Send email
                var from = "Kink Fix<meghankcombs@gmail.com>";
                var to = db.Users.Find(myComment.AssignedToUserId).Email;
                var email = new MailMessage(from, to)
                {
                    Subject = "Kink Fix: You have a new notification",
                    Body = "A comment has been added to one of your assigned tickets. Please sign into your Kink Fix account to view details.",
                    IsBodyHtml = true
                };

                var svc = new PersonalEmail();
                await svc.SendAsync(email);
            }
        }

        public List<TicketNotification> ListUserNotifications(string userId)
        {
            var userNotifications = db.TicketNotifications.AsNoTracking().Where(t => t.RecipientId == userId);
            return userNotifications.ToList();
        }

        public static int NotificationCount()
        {
            var userId = HttpContext.Current.User.Identity.GetUserId();
            var myCount = 0;
            var myNotifications = db.TicketNotifications.AsNoTracking().Where(c => c.RecipientId == userId).ToList();
            if (myNotifications.Count > 0)
            {
                myCount = myNotifications.Count;
            }
            return myCount;
        }

        public static List<TicketNotification> GetFirstFour()
        {
            return db.TicketNotifications.AsNoTracking().OrderByDescending(n => n.Created).Take(4).ToList();
        }

        public List<Ticket> GetMyProjectTickets(string userId)
        {
            var myTickets = new List<Ticket>();
            foreach(var project in projHelper.ListUserProjects(userId))
            {
                foreach(var ticket in project.Tickets)
                {
                    myTickets.Add(ticket);
                }
            }
            return myTickets;
        }

    }
}

//-----------OLD ADD TICKET HISTORY CODE-----------------
//if (property.Name != null && property.Name == "Title"
//    || property.Name == "Description"
//    || property.Name == "AssignedToUserId"
//    || property.Name == "TicketTypeId"
//    || property.Name == "TicketPriorityId"
//    || property.Name == "TicketStatusId")
//{
//var oldInt = oldTicket.GetType().GetProperty(property).GetValue(oldTicket);
//var newInt = newTicket.GetType().GetProperty(property).GetValue(newTicket);
//var oldValue = oldTicket.GetType().GetProperty(property).GetValue(oldTicket).ToString();
//var newValue = newTicket.GetType().GetProperty(property).GetValue(newTicket).ToString();

//if (newValue != oldValue)
//{
//    var newTicketHistory = new TicketHistory();
//    newTicketHistory.UserId = HttpContext.Current.User.Identity.GetUserId();
//    newTicketHistory.ChangedDate = DateTime.Now;
//    newTicketHistory.TicketId = newTicket.Id;
//    newTicketHistory.Property = property;

//    if (property == "Title")
//    {
//        oldValue = db.Tickets.Find(oldInt).Title;
//        newValue = db.Tickets.Find(newInt).Title;
//    }
//    if (property == "Description")
//    {
//        oldValue = db.Tickets.Find(oldInt).Description;
//        newValue = db.Tickets.Find(newInt).Description;
//    }
//    if (property == "AssignedToUserId")
//    {
//        oldValue = db.Tickets.Find(oldInt).AssignedToUser.FirstName;
//        newValue = db.Tickets.Find(newInt).AssignedToUser.FirstName;
//    }
//    if (property == "TicketTypeId")
//    {
//        oldValue = db.Tickets.Find(oldInt).TicketType.Name;
//        newValue = db.Tickets.Find(newInt).TicketType.Name;
//    }
//    if (property == "TicketPriorityId")
//    {
//        oldValue = db.Tickets.Find(oldInt).TicketPriority.Name;
//        newValue = db.Tickets.Find(newInt).TicketPriority.Name;
//    }
//    if (property == "TicketStatusId")
//    {
//        oldValue = db.Tickets.Find(oldInt).TicketStatus.Name;
//        newValue = db.Tickets.Find(newInt).TicketStatus.Name;
//    }

//    newTicketHistory.OldValue = oldValue;
//    newTicketHistory.NewValue = newValue;

//    db.TicketHistories.Add(newTicketHistory);
//    db.SaveChanges();