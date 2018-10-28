using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Neurocom.Models;
using Neurocom.DAO.Interfaces;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;

namespace Neurocom.DAO.Repositories
{
    public class ApplicationUserRepository : IRepository<ApplicationUser>
    {
        private ApplicationDbContext db;

        public ApplicationUserRepository(ApplicationDbContext context)
        {
            this.db = context;
        }

        public void Create(ApplicationUser user)
        {
            throw new NotImplementedException();
        }

        public void Create(ApplicationUser user, string pass)
        {
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            var checkUser = UserManager.Create(user, pass);

            if (checkUser.Succeeded)
            {
                var result = UserManager.AddToRole(user.Id, "User");
            }
        }

        public void Delete(int id)
        {
            ApplicationUser user = db.Users.Find(id);

            if (user != null)
            {
                db.Users.Remove(user);
            }
        }

        public ApplicationUser Delete(string id)
        {
            ApplicationUser appUser = db.Users.Find(id);
            if (appUser != null)
            {
                db.Users.Remove(appUser);
              
            }
            return appUser;

        }


        public IEnumerable<ApplicationUser> Find(Func<ApplicationUser, bool> predicate)
        {
            return db.Users.Where(predicate).ToList();
        }

        public ApplicationUser Get(int id)
        {
            return db.Users.Find(id);
        }

        public ApplicationUser Get(string id)
        {
            return db.Users.Find(id);
        }

        public IEnumerable<ApplicationUser> GetAll()
        {
            return db.Users;
        }

        public void Update(ApplicationUser user)
        {
            var dbEntry = GetAll();

            ApplicationUser userEntry = null;

            foreach (var item in dbEntry)
            {
                if (item.Id.Equals(user.Id))
                {
                    userEntry = item;
                }
            }


            if (userEntry != null)
            {
                userEntry.UserName = user.UserName;
                userEntry.Email = user.Email;
                userEntry.PhoneNumber = user.PhoneNumber;
                userEntry.Address = user.Address;
                userEntry.ImageData = user.ImageData;
                userEntry.ImageMimeType = user.ImageMimeType;
            }

        }
    }
}