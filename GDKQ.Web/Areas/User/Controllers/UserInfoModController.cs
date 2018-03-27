using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GDKQ.Web.Areas.User.Controllers
{
    public class UserInfoModController : Controller
    {
        // GET: User/UserInfoMod
        public ActionResult Index()
        {
            Model.User u = Session["user"] as Model.User;
            if (u == null)
            {
                return Content("请重新登录");
            }
            DAL.GDKQContext dal = new DAL.GDKQContext();
            Model.UserInfo ui = dal.UserInfo.SingleOrDefault(x => x.UserID == u.ID);
            if (ui == null)
            {
                return Content("没有该用户的资料");
            }
            return View(ui);
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Index(string Nickname,string password, string password1,string password2,string Hobby,string Gender,string Description)
        {
            if (string.IsNullOrEmpty(Nickname)|| string.IsNullOrEmpty(password) || string.IsNullOrEmpty(password1) || string.IsNullOrEmpty(password2) || string.IsNullOrEmpty(Hobby) || string.IsNullOrEmpty(Gender) ||string.IsNullOrEmpty(Description))
            {
                return Json(new { status = "n", info = "请填写完整的修改信息" });
            }
            if (password1!=password2)
            {
               return Json(new { status = "n", info = "两次输入的新密码不一致" });
            }
            if (password == password1)
            {
                return Json(new { status = "n", info = "新密码不能与旧密码相同" });
            }
            Model.User u = Session["user"] as Model.User;
            if (u == null)
            {
                return Json(new { status = "n", info = "该用户不存在" });
            }
            password += "_GDKQ";
            password = MD5.MD5Encrypt16(password);
            if (u.Password.ToLower() != password.ToLower())
            {
                return Json(new { status = "n", info = "原密码错误，请重新输入" });
            }
            DAL.GDKQContext dal = new DAL.GDKQContext();
            Model.UserInfo ui = dal.UserInfo.SingleOrDefault(x => x.UserID == u.ID);
            if (ui == null)
            {
                return Json(new { status = "n", info = "找不到该用户资料" });
            }
            password1 += "_GDKQ";
            password1 = MD5.MD5Encrypt16(password1);
            u.Password = password1;
            ui.Nickname = Nickname;
            ui.Hobby = Hobby;
            ui.Gender = Gender;
            ui.Description = Description;
            u.Nickname = Nickname;
            dal.SaveChanges();
            return Json(new { status = "y", info = "修改成功", NextUrl = "/User/Home/Index" });
        }


       
    }
}