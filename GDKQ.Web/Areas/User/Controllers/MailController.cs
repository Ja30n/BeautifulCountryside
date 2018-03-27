using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Webdiyer.WebControls.Mvc;

namespace GDKQ.Web.Areas.User.Controllers
{
    [Filter.UserLoginFilter]
    public class MailController : Controller
    {
        /// <summary>
        /// 邮件列表
        /// </summary>
        /// <returns></returns>
        public ActionResult List(int pageindex = 1)
        {
            int pagesize = 10;
            int total = 0;
            Model.User u = Session["user"] as Model.User;
            if (u == null)
            {
                return Content("请重新登录");
            }
            DAL.GDKQContext dal = new DAL.GDKQContext();
            Model.UserInfo ui = dal.UserInfo.SingleOrDefault(x => x.UserID == u.ID);
            if (ui == null)
            {
                return Content("没有该用户资料");
            }
            total = dal.Mail.Count(a => a.To_UID == u.ID&&a.IsDelect==true);
            List<Model.Mail> list0 = dal.Mail.Where(m => m.To_UID == u.ID && m.IsDelect == true).OrderBy(m=>m.IsRead).OrderBy(m => m.CreateTime).Skip((pageindex - 1) * pagesize).Take
                (pagesize).ToList<Model.Mail>();
            PagedList<GDKQ.Model.Mail> list = new PagedList<Model.Mail>(list0, pageindex, pagesize, total);
            ViewBag.username = ui.Nickname;
            return View(list);
        }

        
        public ActionResult Read(int Mail)
        {
            Model.User u = Session["user"] as Model.User;
            if (u==null)
            {
                return Content("请重新登录");
            }
            DAL.GDKQContext dal = new DAL.GDKQContext();
            Model.UserInfo ui = dal.UserInfo.SingleOrDefault(x => x.UserID == u.ID);
            if (ui==null)
            {
                return Content("没有该用户资料");
            }
            Model.Mail mail = dal.Mail.SingleOrDefault(x => x.ID == Mail);
            if (mail==null)
            {
                return Content("没有相关邮件信息");
            }
            mail.IsRead = false;
            dal.SaveChanges();
            return View(mail);
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Reply(string Mail,string Title,string Body)
        {
            int MailID= Convert.ToInt16(Mail);
            if (string.IsNullOrEmpty(Title)||string.IsNullOrEmpty(Body))
            {
                return Json(new { status = "n", info = "请填写完整的回信信息" });
            }
            Model.User u = Session["user"] as Model.User;
            if (u == null)
            {
                return Content("请重新登录");
            }
            DAL.GDKQContext dal = new DAL.GDKQContext();
            Model.UserInfo ui = dal.UserInfo.SingleOrDefault(x => x.UserID == u.ID);
            if (ui == null)
            {
                return Content("没有该用户资料");
            }
            Model.Mail mail = dal.Mail.SingleOrDefault(x => x.ID ==MailID);
            if (mail == null)
            {
                return Content("没有相关邮件信息");
            }
            dal.Mail.Add(new Model.Mail()
            {
                Title = Title,
                Body = Body,
                CreateTime = DateTime.Now,
                From_UID = mail.To_UID,
                To_UID=mail.From_UID,
                From_Name=mail.To_Name,
                To_Name=mail.From_Name,
                IsDelect=true,
                IsRead=true,
            });
            dal.SaveChanges();
            return Json(new { status = "y", info = "回信成功" });
        }


        public ActionResult Delect(string Mail)
        {
            int MailID= Convert.ToInt16(Mail);
            Model.User u = Session["user"] as Model.User;
            if (u == null)
            {
                return Content("请重新登录");
            }
            DAL.GDKQContext dal = new DAL.GDKQContext();
            Model.Mail m = dal.Mail.SingleOrDefault(x => x.ID == MailID);
            if (m==null)
            {
                return Content("没有相应邮件");
            }
            m.IsDelect = false;
            dal.SaveChanges();
            return RedirectToAction("List");
        }
    }
}