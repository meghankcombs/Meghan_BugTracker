using Meghan_BugTracker.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Meghan_BugTracker.Helpers
{
    public class NameHelper
    {
        private static ApplicationDbContext db = new ApplicationDbContext();
        private static ApplicationUser user = new ApplicationUser();

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
    }
}