using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Webdiyer.WebControls.Mvc;

namespace GDKQ.Web.Areas.Livelihood.Controllers
{
    public class AffairController : Controller
    {
        // GET: Livelihood/Affair
        public ActionResult List(string bh, int pageindex = 1)
        {
            DAL.GDKQContext dal = new DAL.GDKQContext();
            Model.Category ca = dal.Category.SingleOrDefault(x => x.bh == bh);
            if (ca == null)
            {
                return Content("没有该分类的新闻！");
            }
            int pagesize = 10;//每页显示条数
            int total = 0;//总数
            total = dal.News.Count(x => x.bh == ca.bh && x.IsDeleted == true);
            if (total < 1)
            {
                return Content("数据库中没有该新闻！");
            }
            List<Model.News> list0 = dal.News.Where(
                x => x.bh == ca.bh && x.IsDeleted == true).OrderByDescending(a=>a.CreateTime).Take(6).ToList<Model.News>();
            PagedList<GDKQ.Model.News> list = new PagedList<Model.News>(list0, pageindex, pagesize, total);
            ViewBag.Title = ca.CaName;
            ViewBag.bh = ca.bh;

            return View(list);
        }

        public ActionResult Advice(string Body)
        {
            string FromName;
            if (string.IsNullOrEmpty(Body))
            {
                return Json(new { status = "n", info = "请填写建议！" });
            }
            DAL.GDKQContext dal = new DAL.GDKQContext();
            Model.User u = Session["user"] as Model.User;
            if (u == null)
            {
                FromName = "匿名";
            }
            else {
                FromName = u.Nickname;
            }
            dal.Advice.Add(new Model.Advice()
            {
                Body = Body,
                CreateTime = DateTime.Now,
                IsRead = true,
                From_Name=FromName
            });
            dal.SaveChanges();
            return Json(new { status = "y", info = "感谢您的建议，我们会尽快改进的！" });
        }
    }
}