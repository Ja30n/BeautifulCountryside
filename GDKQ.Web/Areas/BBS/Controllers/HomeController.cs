using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Webdiyer.WebControls.Mvc;

namespace GDKQ.Web.Areas.BBS.Controllers
{
    public class HomeController : Controller
    {
        public class viewModel
        {
            public List<Model.Article> Article01 { get; set; }//最新文章
            public List<Model.Article> Article04 { get; set; }//精华文章

            public viewModel(List<Model.Article> ArticleList01, List<Model.Article> ArticleList04)
            {
                this.Article01 = ArticleList01;
                this.Article04 = ArticleList04;
            }
        }
        /// <summary>
        /// 论坛首页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            DAL.GDKQContext dal = new DAL.GDKQContext();
            var vm = new viewModel(
                dal.Article.Where(x => x.CategoryID == "01" && x.IsDeleted == true && x.Enable == true).OrderByDescending(x => x.CreateTime).Take(5).ToList<Model.Article>(),
                dal.Article.Where(x => x.CategoryID == "04" && x.IsDeleted == true && x.Enable == true).OrderByDescending(x=>x.like_count).OrderByDescending(x => x.CreateTime).Take(5).ToList<Model.Article>()
                );
            ViewBag.Title = "论坛首页";
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