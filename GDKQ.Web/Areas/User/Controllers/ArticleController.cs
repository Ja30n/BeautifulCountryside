using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Webdiyer.WebControls.Mvc;

namespace GDKQ.Web.Areas.User.Controllers
{
    [Filter.UserLoginFilter]
    public class ArticleController : Controller
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pageindex">当前第几页</param>
        /// <returns></returns>
        public ActionResult List(int pageindex=1)
        {
            int pagesize = 10;//每页显示条数
            int total = 0;//总数
            Model.User u = Session["user"] as Model.User;
            if (u == null)
            {
                return Content("重新登录");
            }
            DAL.GDKQContext dal = new DAL.GDKQContext();
            Model.UserInfo ui = dal.UserInfo.SingleOrDefault(x => x.UserID == u.ID);
            if (ui==null)
            {
                return Content("找不到该用户的资料");
            }
            total = dal.Article.Count(a => a.AuthorID==u.ID && a.IsDeleted == true && a.Enable == true);
            List<Model.Article> list0 = dal.Article.Where(
                a => a.AuthorID ==u.ID ).OrderBy(a => a.AuthorID==u.ID&&a.IsDeleted==true&&a.Enable==true).Skip((pageindex - 1) * pagesize).Take(pagesize).ToList<Model.Article>();
            PagedList<GDKQ.Model.Article> list =new  PagedList<Model.Article>(list0, pageindex, pagesize,total);
            ViewBag.username = ui.Nickname;
            return View(list);
        }


        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Delect(int id)
        {
            DAL.GDKQContext dal = new DAL.GDKQContext();
            Model.Article art = dal.Article.SingleOrDefault(x => x.ID == id);
            if (art==null)
            {
                return Json(new { status = "n", info = "数据库中没有该文章" });
            }
            art.IsDeleted = false;
            dal.SaveChanges();
            return Json(new { status = "y", info = "删除成功" });
        }
    }
}