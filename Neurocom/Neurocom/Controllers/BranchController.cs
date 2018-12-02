using Neurocom.BL.Services;
using Neurocom.DAO.Interfaces;
using Neurocom.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using Microsoft.AspNet.Identity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Neurocom.DAO.Repositories;

namespace Neurocom.Controllers
{
    public class BranchController : Controller
    {
        private IUnitOfWork db;

        public BranchController(IUnitOfWork database)
        {
            db = database;
        }

        public ActionResult Index()
        {
            return View();
        }

        public FileContentResult GetImage(int taskId)
        {
            TaskNetwork task = db.TaskNetworks.Get(taskId);
            if (task != null)
            {
                return File(task.ImageData, task.ImageMimeType);
            }
            else
            {
                return null;
            }
        }

        public FileContentResult GetAvatar(string userId)
        {
            var db = new ApplicationUserRepository(new Neurocom.Models.ApplicationDbContext());
            var user = db.Get(userId);
            if (user != null)
            {
                return File(user.ImageData, user.ImageMimeType);
            }
            else
            {
                return null;
            }
        }

        public FileContentResult GetCaptcha()
        {
            string code = new Random(DateTime.Now.Millisecond).Next(1111, 9999).ToString();
            Session["code"] = code;
            CaptchaImage captcha = new CaptchaImage(code, 110, 50);
            return File(ToByteArray(captcha.Image, ImageFormat.Jpeg), "Jpeg");

        }

        public static byte[] ToByteArray(Image image, ImageFormat format)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                image.Save(ms, format);
                return ms.ToArray();
            }
        }
    }
}