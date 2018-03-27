using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GDKQ.Web.Areas.Adm1n.Controllers
{
    public class PwdModController : Controller
    {
        [Filter.AdminLoginFilter]
        //修改密码
        public ActionResult Index()
        {
            return View();
        }


        [ValidateAntiForgeryToken()]
        [HttpPost]

        public ActionResult Index(string password,string password1,string password2)
        {
            if (string.IsNullOrEmpty(password) || string.IsNullOrEmpty(password1) || string.IsNullOrEmpty(password2))//密码校验
            {
                return Json(new { status = "n", info = "请填写完整的修改信息" });
            }
            if (password1!=password2)
            {
                return Json(new { status = "n", info = "两次输入的新密码不同，请重新输入" });
            }
            if (password==password1)
            {
                return Json(new { status = "n", info = "新密码不能与旧密码相同" });
            }

            Model.Admin a = Session["admin"] as Model.Admin;
            password += "_GDKQ";
            password = MD5.MD5Encrypt16(password);
            if (a.Password.ToLower()!=password.ToLower())
            {
                return Json(new { status = "n", info = "原密码错误，请重新输入" });
            }

            DAL.GDKQContext dal = new DAL.GDKQContext();
            Model.Admin aa = dal.Admin.SingleOrDefault(x=>x.ID==a.ID);
            if (aa==null)
            {
                return Json(new { status = "n", info = "数据库没有该管理员" });
            }
            password1 += "_GDKQ";
            password1 = MD5.MD5Encrypt16(password1);
            aa.Password = password1;
            dal.SaveChanges();
            return Json(new { status = "y", info = "修改成功", NextUrl = "/Login" });
        }
    }
}