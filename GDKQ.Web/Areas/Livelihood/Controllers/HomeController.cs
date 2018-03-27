using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Webdiyer.WebControls.Mvc;

namespace GDKQ.Web.Areas.Livelihood.Controllers
{
    public class HomeController : Controller
    {
        public class viewModel
        {
            public List<Model.News> news001 { get; set; }//公示公告
            public List<Model.News> news002 { get; set; }//建设农村
            public List<Model.News> news003 { get; set; }//村内事务
            public List<Model.News> news007 { get; set; }//时事要点

            public viewModel(List<Model.News> newsList001, List<Model.News> newsList002, List<Model.News> newsList003, List<Model.News> newsList007)
            {
                this.news001 = newsList001;
                this.news002 = newsList002;
                this.news003 = newsList003;
                this.news007 = newsList007;

            }
        }
        // 村务首页
        public ActionResult Index()
        {
            DAL.GDKQContext dal = new DAL.GDKQContext();
            var vm = new viewModel(
                dal.News.Where(x => x.bh == "001" && x.IsDeleted == true).OrderByDescending(x => x.CreateTime).Take(3).ToList<Model.News>(),
                dal.News.Where(x => x.bh == "002" && x.IsDeleted == true).OrderByDescending(x => x.CreateTime).Take(3).ToList<Model.News>(),
                dal.News.Where(x => x.bh == "003" && x.IsDeleted == true).OrderByDescending(x => x.CreateTime).Take(5).ToList<Model.News>(),
                dal.News.Where(x => x.bh == "007" && x.IsDeleted == true).OrderByDescending(x => x.CreateTime).Take(8).ToList<Model.News>()
                );
            return View(vm);
        }


        [HttpPost]
        public ActionResult AddLike(int? id)
        {
            DAL.GDKQContext dal = new DAL.GDKQContext();
            Model.Article a = dal.Article.SingleOrDefault(x => x.ID == id);
            if (a == null)
            {
                return Json(new { status = "n", info = "数据库中没有该文章" });
            }
            a.like_count++;
            dal.SaveChanges();
            return Json(new { status = "y", info = "点赞成功" });
        }


    }
}