using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Standard;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Webdiyer.WebControls.Mvc;
using Common;

namespace GDKQ.Web.Areas.BBS.Controllers
{
    public class NewsController : Controller
    {
        /// <summary>
        /// 常见问题(list)
        /// </summary>
        /// <returns></returns>
        public ActionResult ListQ()
        {
            return View();
        }



        /// <summary>
        /// 精华文章bh=04   按照点赞数排序
        /// 最新文章bh=01   按时间
        /// 活动提倡bh=02   按时间
        /// 其它精彩bh=03   按时间
        /// </summary>
        /// <returns></returns>
        public ActionResult List(string bh, int pageindex = 1)
        {
            DAL.GDKQContext dal = new DAL.GDKQContext();
            Model.UA_Category ca = dal.UA_Category.SingleOrDefault(x => x.bh == bh);
            if (ca == null)
            {
                return Content("请选择正确的分类");
            }
            int pagesize = 10;//每页显示条数
            int total = 0;//总数
            total = dal.Article.Count(x => x.CategoryID == ca.bh && x.IsDeleted == true);
            if (total < 1)
            {
                return Content("暂无相关文章");
            }
            List<Model.Article> list0;
            if (bh=="04")
            {
                list0 = dal.Article.Where(x => x.CategoryID == ca.bh && x.IsDeleted == true).OrderByDescending(x=>x.like_count).OrderByDescending(x=>x.CreateTime).Skip(
                (pageindex - 1) * pagesize).Take(pagesize).ToList<Model.Article>();
            }
            else
            {
                list0 = dal.Article.Where(x => x.CategoryID == ca.bh && x.IsDeleted == true).OrderByDescending(x=>x.CreateTime).OrderBy(a => a.ID).Skip(
                (pageindex - 1) * pagesize).Take(pagesize).ToList<Model.Article>();
            }
            PagedList<GDKQ.Model.Article> list = new PagedList<Model.Article>(list0, pageindex, pagesize, total);
            foreach (var item in list)
            {
                item.Body = HtmlFilter.ReplaceHtmlTag(item.Body); 
            }
            ViewBag.Title = ca.CaName;
            ViewBag.bh = ca.bh;
            return View(list);
        }




        /// <summary>
        /// 用户发帖
        /// </summary>
        /// <returns></returns>
        public ActionResult Add()
        {
            return View();
        }


        [ValidateAntiForgeryToken()]
        [HttpPost]
        public ActionResult Add(string bh, string Label, string Title, string Body)
        {
            if (Session["user"] == null)
            {
                return Json(new { status = "n", info = "亲，记得先登录哦~", NextUrl = "/Login" });
            }
            Model.User u = Session["user"] as Model.User;
            if (string.IsNullOrEmpty(bh) || string.IsNullOrEmpty(Label) || string.IsNullOrEmpty(Title) || string.IsNullOrEmpty(Body))
            {
                return Json(new { status = "n", info = "请填写完整的信息", NextUrl = "/BBS/News/Add" });
            }

            DAL.GDKQContext dal = new DAL.GDKQContext();
            Model.UA_Category uac = dal.UA_Category.SingleOrDefault(x => x.bh == bh);
            if (uac == null)
            {
                return Json(new { status = "n", info = "没有该分类,请重新选择", NextUrl = "/BBS/News/Add" });
            }
            dal.Article.Add(new Model.Article()
            {
                AuthorID = u.ID,
                AuthorName = u.UserName,
                Body = Body,
                Title = Title,
                CaName = uac.CaName,
                CategoryID = uac.bh,
                Comments = 0,
                CreateTime = DateTime.Now,
                IsDeleted = true,
                lab = Label + Title,
                like_count = 0,
                ModTime = DateTime.Now,
                VisitNum = 0,
                Enable = false,
                Photo = null
            });

            dal.SaveChanges();

            return Json(new { status = "y", info = "发布成功，请等待管理员审核", NextUrl = "/BBS/Home" });
        }

        /// <summary>
        /// 文章模型 文章&评论
        /// </summary>
        public class viewModel
        {
            public Model.Article Art { get; set; }//文章
            public List<Model.Comment> Comment { get; set; }//评论

            public viewModel(Model.Article _Art, List<Model.Comment> _Comment)
            {
                this.Art = _Art;
                this.Comment = _Comment;
            }
        }
        /// <summary>
        /// 文章单页
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult OnePage(int id)
        {
            DAL.GDKQContext dal = new DAL.GDKQContext();
            Model.Article art = dal.Article.SingleOrDefault(x => x.ID == id);
            if (art == null)
            {
                return Content("数据库中没有该文章");
            }
            List<Model.Comment> com = dal.Comment.Where(x => x.ArtcleID == art.ID).ToList<Model.Comment>();
            viewModel vm = new viewModel(art, com);
            ViewBag.Title = art.CaName;
            ViewBag.CategoryID = art.CategoryID;
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult AddComments(string body, int? id)
        {
            if (!id.HasValue)
            {
                return Json(new { status = "n", info = "该文章不存在" });
            }
            if (string.IsNullOrEmpty(body))
            {
                return Json(new { status = "n", info = "评论不能为空" });
            }
            if (Session == null)
            {
                return Json(new { status = "n1", info = "只有登录后才能评论哦，请先登录吧" });
            }
            Model.User u = Session["user"] as Model.User;

            if (u == null)
            {
                return Json(new { status = "n1", info = "只有登录后才能评论哦，请先登录吧" });
            }
            DAL.GDKQContext dal = new DAL.GDKQContext();
            Model.UserInfo ui = dal.UserInfo.SingleOrDefault(x => x.UserID == u.ID);
            dal.Comment.Add(new Model.Comment()
            {
                Body = body,
                ArtcleID = id.Value,
                UserID = u.ID,
                UserName = u.UserName,
                Photo = ui.Photo,
                CreateTime = DateTime.Now
            });
            dal.SaveChanges();
            return Json(new { status = "y", info = "评论成功" });
        }



        public ActionResult ReturnResult()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ReturnResult(string keywords,int pageindex = 1)
        {
            int total = 0;
            int pagesize = 5;
            if (string.IsNullOrEmpty(keywords))
            {
                return Json(new { status = "n", info = "搜索关键字不能为空哦~", NextUrl = "/BBS/Home" });
            }
            DAL.GDKQContext dal = new DAL.GDKQContext();
            total = dal.Article.Count(x => x.lab.IndexOf(keywords) > -1 && x.IsDeleted == true && x.Enable == true);
            if (total < 1)
            {
                return Json(new { status = "n", info = "暂无相关的文章~", NextUrl = "/BBS/Home" });
            }
            List<Model.Article> list0 = dal.Article.Where(x => x.lab.IndexOf(keywords) > -1 && x.IsDeleted == true && x.Enable == true).OrderByDescending(a => a.CreateTime).Skip(
                (pageindex - 1) * pagesize).Take(pagesize).ToList<Model.Article>();
            PagedList<GDKQ.Model.Article> list = new PagedList<Model.Article>(list0, pageindex, pagesize, total);
            ViewBag.Title = "搜索结果";

            return View("ReturnResult",list);
        }

        /// <summary>
        /// 点赞
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult AddLikeCount(int id)
        {
            DAL.GDKQContext dal = new DAL.GDKQContext();
            Model.Article art = dal.Article.SingleOrDefault(x => x.ID == id);
            if (art==null)
            {
                return Json(new { status = "n", info = "数据库中没有该文章" });
            }
            art.like_count++;
            dal.SaveChanges();
            return Json(new { status = "y", info = "点赞成功" });
        }
    }
}