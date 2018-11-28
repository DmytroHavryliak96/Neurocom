using Neurocom.BL.Services;
using Neurocom.DAO.Interfaces;
using Neurocom.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Neurocom.Controllers
{
    public class BranchController : Controller
    {
        private IUnitOfWork db;


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

        public FileContentResult GetAvatar(int userId)
        {
            ApplicationUser user = db.Users.Get(userId);
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