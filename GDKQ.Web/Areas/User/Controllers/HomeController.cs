using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GDKQ.Web.Areas.User.Controllers
{
    public class HomeController : Controller
    {
        [Filter.UserLoginFilter]
        // GET: User/Home
        public ActionResult Index()
        {
            Model.User u = Session["user"] as Model.User;
            if (u == null)
            {
                return Content("没有该用户的信息");
            }
            DAL.GDKQContext dal = new DAL.GDKQContext();
            Model.UserInfo ui = dal.UserInfo.SingleOrDefault(x => x.UserID == u.ID);
            if (ui == null)
            {
                return Content("没有该用户的信息");
            }
            List<Model.Article> art = dal.Article.Where(x => x.AuthorID == u.ID && x.Enable == true && x.IsDeleted == true).OrderByDescending(x=>x.CreateTime).Take(5).ToList<Model.Article>();
            viewModel vm = new viewModel(ui, art);
            return View(vm);
        }

        public class viewModel
        {
            public Model.UserInfo ui { get; set; }//用户信息
            public List<Model.Article> Article { get; set; }//个人文章

            public viewModel(Model.UserInfo _ui, List<Model.Article> _ArticleList)
            {
                this.ui = _ui;
                this.Article = _ArticleList;
            }
        }


    }
}
