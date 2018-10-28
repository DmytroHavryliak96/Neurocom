using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Neurocom.BL.Interfaces;
using Neurocom.Models;
using Neurocom.ViewModels.AdminViewModels;
using Neurocom.DAO.Interfaces;
using Neurocom.DAO.Repositories;
using AutoMapper;

namespace Neurocom.BL.Services.ControllerServices.AdminControllerServices
{
    public class AdminService : IAdminService
    {
        private IUnitOfWork Database { get; set; }

        public AdminService(IUnitOfWork db)
        {
            Database = db;
        }

        public void CreateUser(RegisterViewModel model)
        {
            ApplicationUser user = new ApplicationUser();
            user.UserName = model.UserName;
            user.Address = model.Address;
            user.PhoneNumber = model.Phone;
            user.Email = model.Email;
            string userPWD = model.Password;
            user.RegistrationDate = DateTime.Now;
            user.LastLogin = DateTime.Now;

            var repository = (ApplicationUserRepository)Database.Users;
            repository.Create(user, userPWD);
            Database.Save();
        }

        public ApplicationUser DeleteUser(string userId)
        {
            var repository = (ApplicationUserRepository)Database.Users;
            var user = repository.Delete(userId);
            Database.Save();
            return user;
        }

        public IEnumerable<ApplicationUserViewModel> GetAllUsers()
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<ApplicationUser, ApplicationUserViewModel>()).CreateMapper();
            return mapper.Map<IEnumerable<ApplicationUser>, List<ApplicationUserViewModel>>(Database.Users.GetAll());
        }

        public ApplicationUserViewModel GetUser(string id)
        {
            var repository = (ApplicationUserRepository)Database.Users;
            var user = repository.Get(id);
            return new ApplicationUserViewModel { Id = user.Id, UserName = user.UserName, Address = user.Address, Email = user.Email};
        }

        public void UpdateUser(ApplicationUserViewModel user, HttpPostedFileBase image)
        {
            if (image != null)
            {
                user.ImageMimeType = image.ContentType;
                user.ImageData = new byte[image.ContentLength];
                image.InputStream.Read(user.ImageData, 0, image.ContentLength);
            }

            ApplicationUser appUser = new ApplicationUser
            {
                Id = user.Id,
                Address = user.Address,
                Email = user.Email,
                UserName = user.UserName,
                ImageData = user.ImageData,
                ImageMimeType = user.ImageMimeType
            };

            Database.Users.Update(appUser);
            Database.Save();
        }
    }
}