using Meghan_BugTracker.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Meghan_BugTracker.Helpers
{
    public class ProjectsHelper
    {
        ApplicationDbContext db = new ApplicationDbContext();

        public bool IsUserOnProject (string userId, int projectId)
        {
            var project = db.Projects.Find(projectId); //var project becomes type Project and is assigned to this ENTIRE project record
            var flag = project.Users.Any(u => u.Id == userId); //do any Users associated with this projectId match the passed in userId
            return (flag); //return true or false to above statement
        }

        public ICollection<Project> ListUserProjects (string userId) //return list of projects associated with this specific user
        {
            ApplicationUser user = db.Users.Find(userId);
            var projects = user.Projects.ToList();
            return (projects);
        }

        public void AddUserToProject(string userId, int projectId) //add new user to project, if he/she not on project already
        {
            if (!IsUserOnProject(userId, projectId))
            {
                Project proj = db.Projects.Find(projectId); //assigns entire record to proj, NOT just projectId
                var newUser = db.Users.Find(userId);

                proj.Users.Add(newUser);
                db.Entry(proj).State = EntityState.Modified; //modifies existing Project record
                db.SaveChanges();
            }
        }

        public void RemoveUserFromProject(string userId, int projectId) //remove user from project
        {
            if(IsUserOnProject(userId, projectId))
            {
                Project proj = db.Projects.Find(projectId);
                var delUser = db.Users.Find(userId);

                proj.Users.Remove(delUser);
                db.Entry(proj).State = EntityState.Modified; //modifies existing Project record
                db.SaveChanges();
            }
        }

        public ICollection<ApplicationUser> UsersOnProject(int projectId) //lists users on certain project
        {
            return db.Projects.Find(projectId).Users;
        }

        public ICollection<ApplicationUser> UsersNotOnProject(int projectId) //lists users NOT on certain project
        {
            return db.Users.Where(u => u.Projects.All(p => p.Id != projectId)).ToList();
        }
    }
}