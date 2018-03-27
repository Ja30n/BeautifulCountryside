using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GDKQ.Model;

namespace GDKQ.Web.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult Home()
        {
            DAL.GDKQContext dal = new DAL.GDKQContext();
            List<Model.News> n = dal.News.Where(x => x.bh == "22" && x.IsDeleted == true).OrderByDescending(x => x.CreateTime).Take(3).ToList<Model.News>();
            List<Model.News> n1 = dal.News.Where(x => x.bh == "411" && x.IsDeleted == true).OrderByDescending(x => x.CreateTime).Take(2).ToList<Model.News>();
            List<Model.Article> a = dal.Article.Where(x => x.Enable == true).OrderByDescending(x => x.CreateTime).Take(11).ToList<Model.Article>();
            List<Model.News> n2 = dal.News.Where(x => x.bh == "121" && x.IsDeleted == true).OrderByDescending(x => x.CreateTime).Take(2).ToList<Model.News>();
            viewModel vm = new viewModel(n, n1, a, n2);
            return View(vm);
        }
        public class viewModel
        {
            public List<Article> a { get; set; }
            public List<News> n { get; set; }
            public List<News> n1 { get; set; }
            public List<News> n2 { get; set; }

            public viewModel(List<News> n, List<News> n1, List<Article> a, List<News> n2)
            {
                this.n = n;
                this.n1 = n1;
                this.a = a;
                this.n2 = n2;
            }
        }

        [HttpPost]
        public ActionResult AddLike(int? id)
        {
            DAL.GDKQContext dal = new DAL.GDKQContext();
            Model.Article a= dal.Article.SingleOrDefault(x => x.ID == id);
            if (a==null)
            {
                return Json(new { status = "n", info = "数据库中没有该文章" });
            }
            a.like_count++;
            dal.SaveChanges();
            return Json(new { status = "y", info = "点赞成功" });
        }
    }
}