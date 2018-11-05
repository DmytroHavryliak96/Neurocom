using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Neurocom.ViewModels.AdminViewModels;
using System.Web;
using Neurocom.Models;

namespace Neurocom.BL.Interfaces
{
    interface IAdminService
    {
        IEnumerable<ApplicationUserViewModel> GetAllUsers();

        void UpdateUser(ApplicationUserViewModel user, HttpPostedFileBase image);

        ApplicationUserViewModel GetUser(string id);

        void CreateUser(RegisterViewModel model);

        ApplicationUser DeleteUser(string userId);

        void Dispose();

    }
}
