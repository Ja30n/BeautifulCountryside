using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Webdiyer.WebControls.Mvc;

namespace GDKQ.Web.Areas.Livelihood.Controllers
{
    /// <summary>
    /// 新闻管理器（村务）
    /// </summary>
    public class NewsController : Controller
    {
        /// <summary>
        /// 新闻列表
        /// </summary>
        /// <param name="bh">分类编号</param>
        /// <param name="pageindex">当前页面页数</param>
        /// <returns></returns>
        public ActionResult List(string bh, int pageindex = 1)
        {
            DAL.GDKQContext dal = new DAL.GDKQContext();
            Model.Category ca = dal.Category.SingleOrDefault(x => x.bh == bh);
            if (ca==null)
            {
                return Content("没有该分类的新闻");
            }
            int pagesize = 10;//每页显示条数
            int total = 0;//总数
            total = dal.News.Count(x=>x.bh==ca.bh&&x.IsDeleted==true);
            if (total <1)
            {
                return Content("数据库中没有该新闻");
            }
            
            List<Model.News> list0 = dal.News.Where(
                x=>x.bh==ca.bh && x.IsDeleted == true).OrderBy(a => a.ID).Skip((pageindex - 1) * pagesize).Take(pagesize).ToList<Model.News>();
            PagedList<GDKQ.Model.News> list = new PagedList<Model.News>(list0, pageindex, pagesize, total);
            ViewBag.Title = ca.CaName;
            ViewBag.bh = ca.bh;
            
            return View(list);

        }


        /// <summary>
        /// 村务单页新闻(list下的)
        /// </summary>
        /// <param name="id">新闻ID</param>
        /// <returns></returns>
        public ActionResult OnePage(int id)
        {
            DAL.GDKQContext dal = new DAL.GDKQContext();
            Model.News n = dal.News.SingleOrDefault(x => x.ID == id);
            if (n==null)
            {
                return Content("数据库中没有该新闻");
            }
            n.VisitNum += 1;
            dal.SaveChanges();
            ViewBag.Title = n.CaName;
            return View(n);
        }

        public ActionResult Services()
        {
            ViewBag.Title = "便民服务";
            return View();
        }

    }
}