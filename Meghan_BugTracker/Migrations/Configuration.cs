namespace Meghan_BugTracker.Migrations
{
    using Meghan_BugTracker.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Meghan_BugTracker.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(Meghan_BugTracker.Models.ApplicationDbContext context)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            #region Creating Roles
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

            //ADMIN
            if (!context.Roles.Any(r => r.Name == "Admin"))
            {
                roleManager.Create(new IdentityRole { Name = "Admin" });
            }

            //PROJECT MANAGER
            if (!context.Roles.Any(r => r.Name == "Project Manager"))
            {
                roleManager.Create(new IdentityRole { Name = "Project Manager" });
            }

            //DEVELOPER
            if (!context.Roles.Any(r => r.Name == "Developer"))
            {
                roleManager.Create(new IdentityRole { Name = "Developer" });
            }

            //SUBMITTER
            if (!context.Roles.Any(r => r.Name == "Submitter"))
            {
                roleManager.Create(new IdentityRole { Name = "Submitter" });
            }
            #endregion
            
            #region Creating Users
            //ADMIN
            if (!context.Users.Any(u => u.Email == "meghankcombs@gmail.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "meghankcombs@gmail.com",
                    Email = "meghankcombs@gmail.com",
                    FirstName = "Meghan (Admin)",
                    LastName = "Combs",
                    DisplayName = "Admin"
                }, "password");
            }
            
            //PROJECT MANAGER 1
            if (!context.Users.Any(u => u.Email == "ProjectManager1@mailinator.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "ProjectManager1@mailinator.com",
                    Email = "ProjectManager1@mailinator.com",
                    FirstName = "Project Manager 1",
                    LastName = "Dernbach",
                    DisplayName = "PM1"
                }, "password");
            }

            //PROJECT MANAGER 2
            if (!context.Users.Any(u => u.Email == "ProjectManager2@mailinator.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "ProjectManager2@mailinator.com",
                    Email = "ProjectManager2@mailinator.com",
                    FirstName = "Project Manager 2",
                    LastName = "Derenbach",
                    DisplayName = "PM2"
                }, "password");
            }

            //DEVELOPER 1
            if (!context.Users.Any(u => u.Email == "Developer1@mailinator.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "Developer1@mailinator.com",
                    Email = "Developer1@mailinator.com",
                    FirstName = "Developer1",
                    LastName = "Harrington",
                    DisplayName = "Developer1"
                }, "password");
            }

            //DEVELOPER 2
            if (!context.Users.Any(u => u.Email == "Developer2@mailinator.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "Developer2@mailinator.com",
                    Email = "Developer2@mailinator.com",
                    FirstName = "Developer2",
                    LastName = "Harrington",
                    DisplayName = "Developer2"
                }, "password");
            }

            //DEVELOPER 3
            if (!context.Users.Any(u => u.Email == "Developer3@mailinator.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "Developer3@mailinator.com",
                    Email = "Developer3@mailinator.com",
                    FirstName = "Developer3",
                    LastName = "Harrington",
                    DisplayName = "Developer3"
                }, "password");
            }

            //DEVELOPER 4
            if (!context.Users.Any(u => u.Email == "Developer4@mailinator.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "Developer4@mailinator.com",
                    Email = "Developer4@mailinator.com",
                    FirstName = "Developer4",
                    LastName = "Harrington",
                    DisplayName = "Developer4"
                }, "password");
            }

            //SUBMITTER 1
            if (!context.Users.Any(u => u.Email == "Submitter1@mailinator.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "Submitter1@mailinator.com",
                    Email = "Submitter1@mailinator.com",
                    FirstName = "Submitter1",
                    LastName = "Riddle",
                    DisplayName = "Submitter1"
                }, "password");
            }

            //SUBMITTER 2
            if (!context.Users.Any(u => u.Email == "Submitter2@mailinator.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "Submitter2@mailinator.com",
                    Email = "Submitter2@mailinator.com",
                    FirstName = "Submitter2",
                    LastName = "Riddle",
                    DisplayName = "Submitter2"
                }, "password");
            }

            //SUBMITTER 3
            if (!context.Users.Any(u => u.Email == "Submitter3@mailinator.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "Submitter3@mailinator.com",
                    Email = "Submitter3@mailinator.com",
                    FirstName = "Submitter3",
                    LastName = "Riddle",
                    DisplayName = "Submitter3"
                }, "password");
            }
            #endregion

            #region Assigning Users to Roles
            //ADMIN (1)
            var userId = userManager.FindByEmail("meghankcombs@gmail.com").Id;
            userManager.AddToRole(userId, "Admin");

            //PROJECT MANAGERS (2)
            var userId2 = userManager.FindByEmail("ProjectManager1@mailinator.com").Id;
            userManager.AddToRole(userId2, "Project Manager");

            var userId3 = userManager.FindByEmail("ProjectManager2@mailinator.com").Id;
            userManager.AddToRole(userId3, "Project Manager");

            //DEVELOPERS (4)
            var userId4 = userManager.FindByEmail("Developer1@mailinator.com").Id;
            userManager.AddToRole(userId4, "Developer");

            var userId5 = userManager.FindByEmail("Developer2@mailinator.com").Id;
            userManager.AddToRole(userId5, "Developer");

            var userId6 = userManager.FindByEmail("Developer3@mailinator.com").Id;
            userManager.AddToRole(userId6, "Developer");

            var userId7 = userManager.FindByEmail("Developer4@mailinator.com").Id;
            userManager.AddToRole(userId7, "Developer");

            //SUBMITTERS (3)
            var userId8 = userManager.FindByEmail("Submitter1@mailinator.com").Id;
            userManager.AddToRole(userId8, "Submitter");

            var userId9 = userManager.FindByEmail("Submitter2@mailinator.com").Id;
            userManager.AddToRole(userId9, "Submitter");

            var userId10 = userManager.FindByEmail("Submitter3@mailinator.com").Id;
            userManager.AddToRole(userId10, "Submitter");
            #endregion

            #region Creating Demo Users
            if (!context.Users.Any(u => u.Email == "DemoAdmin@mailinator.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "DemoAdmin@mailinator.com",
                    Email = "DemoAdmin@mailinator.com",
                    FirstName = "Demo Admin",
                    LastName = "DA",
                    DisplayName = "DemoAdmin"
                }, "password");
            }

            if (!context.Users.Any(u => u.Email == "DemoProjectManager@mailinator.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "DemoProjectManager@mailinator.com",
                    Email = "DemoProjectManager@mailinator.com",
                    FirstName = "Demo Project Manager",
                    LastName = "DemoPM",
                    DisplayName = "DemoPM"
                }, "password");
            }

            if (!context.Users.Any(u => u.Email == "DemoDeveloper@mailinator.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "DemoDeveloper@mailinator.com",
                    Email = "DemoDeveloper@mailinator.com",
                    FirstName = "Demo Developer",
                    LastName = "DemoDev",
                    DisplayName = "DemoDev"
                }, "password");
            }

            if (!context.Users.Any(u => u.Email == "DemoSubmitter@mailinator.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "DemoSubmitter@mailinator.com",
                    Email = "DemoSubmitter@mailinator.com",
                    FirstName = "Demo Submitter",
                    LastName = "DemoSub",
                    DisplayName = "DemoSub"
                }, "password");
            }
            #endregion

            #region Assigning Demo Users to Roles
            //DEMO ADMIN
            var userId11 = userManager.FindByEmail("DemoAdmin@mailinator.com").Id;
            userManager.AddToRole(userId11, "Admin");

            //DEMO PROJECT MANAGER
            var userId12 = userManager.FindByEmail("DemoProjectManager@mailinator.com").Id;
            userManager.AddToRole(userId12, "Project Manager");

            //DEMO DEVELOPER
            var userId13 = userManager.FindByEmail("DemoDeveloper@mailinator.com").Id;
            userManager.AddToRole(userId13, "Developer");

            //DEMO SUBMITTER
            var userId14 = userManager.FindByEmail("DemoSubmitter@mailinator.com").Id;
            userManager.AddToRole(userId14, "Submitter");
            #endregion

            #region Creating Projects
            context.Projects.AddOrUpdate(
                p => p.Name,
                new Project
                {
                    Id = 10010,
                    Name = "Project One",
                    Description = "This is the first project"
                },
                new Project
                {
                    Id = 10020,
                    Name = "Project Two",
                    Description = "This is the second project"
                },
                new Project
                {
                    Id = 10030,
                    Name = "Project Three",
                    Description = "This is the third project"
                },
                new Project
                {
                    Id = 10040,
                    Name = "Project Four",
                    Description = "This is the fourth project"
                },
                new Project
                {
                    Id = 10050,
                    Name = "Project Five",
                    Description = "This is the fifth project"
                }
                );
            #endregion

            #region Ticket Priority
            context.TicketPriorities.AddOrUpdate(
                t => t.Name,
                new TicketPriority { Id = 100, Name = "Low" },
                new TicketPriority { Id = 200, Name = "Medium" },
                new TicketPriority { Id = 300, Name = "High" }
                );
            #endregion

            #region Ticket Status
            context.TicketStatus.AddOrUpdate(
                t => t.Name,
                new TicketStatus { Id = 100, Name = "Unassigned" },
                new TicketStatus { Id = 200, Name = "Assigned/In Progress" },
                new TicketStatus { Id = 300, Name = "Resolved" },
                new TicketStatus { Id = 400, Name = "Archived" }
                );
            #endregion

            #region Ticket Type
            context.TicketTypes.AddOrUpdate(
                t => t.Name,
                new TicketType { Id = 100, Name = "Bug" },
                new TicketType { Id = 200, Name = "Task" },
                new TicketType { Id = 300, Name = "Update" }
                );
            #endregion
            
            #region Creating Tickets
            context.Tickets.AddOrUpdate(
                t => t.Title,
                new Ticket
                {
                    Title = "Bug Fix",
                    Created = DateTime.Now,
                    Description = "Description",
                    OwnerUserId = userManager.FindByEmail("Submitter1@mailinator.com").Id,
                    TicketPriorityId = 100,
                    TicketStatusId = 100,
                    TicketTypeId = 100,
                    ProjectId = 10010
                });
            context.Tickets.AddOrUpdate(
                t => t.Title,
                new Ticket
                {
                    Title = "Error Detection",
                    Created = DateTime.Now,
                    Description = "Description",
                    OwnerUserId = userManager.FindByEmail("Submitter2@mailinator.com").Id,
                    TicketPriorityId = 100,
                    TicketStatusId = 200,
                    TicketTypeId = 100,
                    ProjectId = 10010
                });
            context.Tickets.AddOrUpdate(
                t => t.Title,
                new Ticket
                {
                    Title = "Task Update",
                    Created = DateTime.Now,
                    Description = "Description",
                    OwnerUserId = userManager.FindByEmail("Submitter3@mailinator.com").Id,
                    TicketPriorityId = 200,
                    TicketStatusId = 300,
                    TicketTypeId = 100,
                    ProjectId = 10020
                });
            context.Tickets.AddOrUpdate(
                t => t.Title,
                new Ticket
                {
                    Title = "Ticket Update",
                    Created = DateTime.Now,
                    Description = "Description",
                    OwnerUserId = userManager.FindByEmail("Submitter1@mailinator.com").Id,
                    TicketPriorityId = 300,
                    TicketStatusId = 400,
                    TicketTypeId = 100,
                    ProjectId = 10020
                });
            context.Tickets.AddOrUpdate(
                t => t.Title,
                new Ticket
                {
                    Title = "Bug Reported",
                    Created = DateTime.Now,
                    Description = "Description",
                    OwnerUserId = userManager.FindByEmail("Submitter2@mailinator.com").Id,
                    TicketPriorityId = 300,
                    TicketStatusId = 100,
                    TicketTypeId = 200,
                    ProjectId = 10030
                });
            context.Tickets.AddOrUpdate(
                t => t.Title,
                new Ticket
                {
                    Title = "Error Reported",
                    Created = DateTime.Now,
                    Description = "Description",
                    OwnerUserId = userManager.FindByEmail("Submitter3@mailinator.com").Id,
                    TicketPriorityId = 300,
                    TicketStatusId = 100,
                    TicketTypeId = 300,
                    ProjectId = 10030
                });
            context.Tickets.AddOrUpdate(
                t => t.Title,
                new Ticket
                {
                    Title = "System Glitch",
                    Created = DateTime.Now,
                    Description = "Description",
                    OwnerUserId = userManager.FindByEmail("Submitter1@mailinator.com").Id,
                    TicketPriorityId = 100,
                    TicketStatusId = 400,
                    TicketTypeId = 100,
                    ProjectId = 10040
                });
            context.Tickets.AddOrUpdate(
                t => t.Title,
                new Ticket
                {
                    Title = "System Error",
                    Created = DateTime.Now,
                    Description = "Description",
                    OwnerUserId = userManager.FindByEmail("Submitter2@mailinator.com").Id,
                    TicketPriorityId = 300,
                    TicketStatusId = 300,
                    TicketTypeId = 300,
                    ProjectId = 10040
                });
            context.Tickets.AddOrUpdate(
                t => t.Title,
                new Ticket
                {
                    Title = "System Bug",
                    Created = DateTime.Now,
                    Description = "Ticket description",
                    OwnerUserId = userManager.FindByEmail("Submitter3@mailinator.com").Id,
                    TicketPriorityId = 200,
                    TicketStatusId = 200,
                    TicketTypeId = 200,
                    ProjectId = 10050
                });
            context.Tickets.AddOrUpdate(
                t => t.Title,
                new Ticket
                {
                    Title = "System Task",
                    Created = DateTime.Now,
                    Description = "Description",
                    OwnerUserId = userManager.FindByEmail("Submitter1@mailinator.com").Id,
                    TicketPriorityId = 300,
                    TicketStatusId = 400,
                    TicketTypeId = 300,
                    ProjectId = 10050
                });
            #endregion
        }
    }
}
