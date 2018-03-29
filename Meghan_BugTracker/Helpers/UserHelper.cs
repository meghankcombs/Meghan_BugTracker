using Meghan_BugTracker.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Meghan_BugTracker.Helpers
{
    public class UserHelper
    {
        private static ApplicationDbContext db = new ApplicationDbContext();
        private static ApplicationUser user = new ApplicationUser();
        private static UserRolesHelper roleHelper = new UserRolesHelper();

        public static string UserFirstName()
        {
            if(HttpContext.Current.User != null)
            {
                var userId = HttpContext.Current.User.Identity.GetUserId();
                var userFName = db.Users.FirstOrDefault(n => n.Id == userId).FirstName;
                return (userFName);
            }
            return "";
        }

        public static string UserLastName()
        {
            if (HttpContext.Current.User != null)
            {
                var userId = HttpContext.Current.User.Identity.GetUserId();
                var userLName = db.Users.FirstOrDefault(n => n.Id == userId).LastName;
                return (userLName);
            }
            return "";
        }

        public static string UserEmail()
        {
            if (HttpContext.Current.User != null)
            {
                var userId = HttpContext.Current.User.Identity.GetUserId();
                var userEmail = db.Users.FirstOrDefault(n => n.Id == userId).Email;
                return (userEmail);
            }
            return "";
        }

        public static string UserDisplayName()
        {
            if (HttpContext.Current.User != null)
            {
                var userId = HttpContext.Current.User.Identity.GetUserId();
                var userDN = db.Users.FirstOrDefault(n => n.Id == userId).DisplayName;
                return (userDN);
            }
            return "";
        }

        public static string UserPhotoRender()
        {
            if (HttpContext.Current.User != null)
            {
                var userId = HttpContext.Current.User.Identity.GetUserId();
                var userPhoto = db.Users.FirstOrDefault(n => n.Id == userId).UserPhoto;
                return (userPhoto);
            }
            return "";
        }

        public static string ShowRole()
        {
            if (HttpContext.Current.User != null)
            {
                var userId = HttpContext.Current.User.Identity.GetUserId();
                var userRole = roleHelper.ListUserRoles(userId).FirstOrDefault();
                return (userRole);
            }
            return "";
        }
    }
}