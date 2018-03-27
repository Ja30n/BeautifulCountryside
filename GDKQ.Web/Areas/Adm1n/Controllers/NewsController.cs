using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Webdiyer.WebControls.Mvc;

namespace GDKQ.Web.Areas.Adm1n.Controllers
{
    [Filter.AdminLoginFilter]
    /// <summary>
    /// 新闻控制器
    /// </summary>
    public class NewsController : Controller
    {
        /// <summary>
        /// 单页新闻(读取)
        /// </summary>
        /// <param name="bh"></param>
        /// <returns></returns>
        public ActionResult OnePage(string bh)
        {
            DAL.GDKQContext dal = new DAL.GDKQContext();
            Model.Category ca = dal.Category.SingleOrDefault(a => a.bh == bh);
            if (ca == null)
            {
                return Content("没有该分类");
            }
            Model.News n = dal.News.Single(a => a.bh == bh);
            if (n == null)
            {
                dal.News.Add(new Model.News()
                {
                    Title = ca.CaName,
                    Body = ca.CaName + "的内容",
                    Author = null,
                    bh = ca.bh,
                    CaName = ca.CaName,
                    CreateTime = DateTime.Now,
                    VisitNum = 0,
                });
                dal.News.Add(n);
            }
            return View(n);
        }


        [ValidateAntiForgeryToken()]
        [HttpPost]
        [ValidateInput(false)]
        //单页新闻(编辑)
        public ActionResult OnePage(Model.News n)
        {
            DAL.GDKQContext dal = new DAL.GDKQContext();
            Model.News n_indb = dal.News.SingleOrDefault(a => a.ID == n.ID);
            if (n_indb == null)
            {
                return Json(new { status = "n", info = "数据库中无该新闻" });
            }
            n_indb.Title = n.Title;
            n_indb.Body = n.Body;
            n_indb.Url = n.Url;
            dal.SaveChanges();
            return Json(new { status = "y", info = "更新成功" });
        }







        /// <summary>
        /// 列表新闻(读取)
        /// </summary>
        /// <param name="bh">分类编号</param>
        /// <param name="pageindex">当前所在页面编号</param>
        /// <returns></returns>
        public ActionResult List(string bh, int pageindex = 1)
        {
            int pagesize = 10;//每页显示条数
            int total = 0;//总数
            DAL.GDKQContext dal = new DAL.GDKQContext();
            Model.Category ca = dal.Category.SingleOrDefault(x => x.bh == bh);
            total = dal.News.Count(n => n.bh == bh && n.IsDeleted==true);
            if (total < 1)
            {
                return Content("数据库中没有相关新闻");
            }
            List<Model.News> list0 = dal.News.Where(n => n.bh == bh && n.IsDeleted == true).OrderBy(
                n => n.CreateTime).Skip((pageindex - 1) * pagesize).Take(pagesize).ToList<Model.News>();
            PagedList<GDKQ.Model.News> list = new PagedList<Model.News>(list0, pageindex, pagesize, total);
            ViewBag.Title = ca.CaName;
            ViewBag.bh = ca.bh;
            return View(list);
        }

        
        /// <summary>
        /// 列表新闻(添加)
        /// </summary>
        /// <param name="bh"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Add(string bh, int? id)
        {
            DAL.GDKQContext dal = new DAL.GDKQContext();
            #region
            Model.Category ca = dal.Category.SingleOrDefault(x => x.bh == bh);
            if (ca == null)
            {
                return Content("没有该分类");
            }
            Model.News n = new Model.News();
            #endregion//验证是否有该新闻分类
            ViewBag.Title = ca.CaName;
            if (id == null)
            {
                //添加
                n = new Model.News()
                {
                    VisitNum = 0,
                    CaName = ca.CaName,
                    bh = ca.bh,
                    CreateTime = DateTime.Now
                };
            }
            else
            {
                //编辑
                n = dal.News.SingleOrDefault(a => a.ID == id);
                if (n == null)
                {
                    return Content("数据库中不存在该新闻");
                }
            }
            return View(n);
        }

        [ValidateAntiForgeryToken()]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Add(Model.News n)
        {
            DAL.GDKQContext dal = new DAL.GDKQContext();
            if (n.ID == 0)
            {
                //添加
                dal.News.Add(new Model.News()
                {
                    Author = n.Author,
                    bh = n.bh,
                    Body = n.Body,
                    CaName = n.CaName,
                    CreateTime = DateTime.Now,
                    IsDeleted = true,
                    Photo = n.Photo,
                    Title = n.Title,
                    VisitNum = 1,
                    Url= "https://"+n.Url
                });
                dal.SaveChanges();
                return Json(new { status = "y", info = "添加成功" ,bh=n.bh});
            }
            else
            {
                //编辑
                Model.News n_indbb = dal.News.SingleOrDefault(a => a.ID == n.ID);
                if (n_indbb == null)
                {
                    return Json(new { status = "n", info = "数据库中无该新闻" });
                }
                n_indbb.Title = n.Title;
                n_indbb.Body = n.Body;
                n_indbb.Photo = n.Photo;
                n_indbb.Author = n.Author;
                n_indbb.Url = n.Url;
                dal.SaveChanges();
                return Json(new { status = "y", info = "编辑成功",bh=n.bh });
            }
        }


        public ActionResult Delect(string bh,int id)
        {
            DAL.GDKQContext dal = new DAL.GDKQContext();
            Model.News n = dal.News.SingleOrDefault(x => x.ID == id);
            if (n==null)
            {
                return Content("没有该新闻");
            }
            n.IsDeleted = false;
            dal.SaveChanges();
            return Redirect("/Adm1n/News/List?bh="+bh);
        }

        #region
        /// <summary>
        /// 建议列表
        /// </summary>
        /// <param name="pageindex"></param>
        /// <returns></returns>
        public ActionResult AdviceList(int pageindex=1)
        {
            int pagesize = 10;//每页显示条数
            int total = 0;//总数
            DAL.GDKQContext dal = new DAL.GDKQContext();
            total = dal.Advice.Count();
            if (total < 1)
            {
                return Content("暂无建议");
            }
            List<Model.Advice> list0 = dal.Advice.Where(x=>x.IsRead==true).OrderByDescending(ad=>ad.CreateTime).Skip((pageindex - 1) * pagesize).Take(pagesize).ToList<Model.Advice>();
            PagedList<GDKQ.Model.Advice> list = new PagedList<Model.Advice>(list0, pageindex, pagesize, total);
            ViewBag.Title = "建议箱";
            return View(list);
        }

        /// <summary>
        /// 删除建议
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult DelectAdvice(int id)
        {
            DAL.GDKQContext dal = new DAL.GDKQContext();
            Model.Advice ad = dal.Advice.SingleOrDefault(x => x.ID == id);
            if (ad == null)
            {
                return Json(new { status = "n", info = "数据库中没有相关数据" });
            }
            ad.IsRead = false;
            dal.SaveChanges();
            return Json(new { status = "y", info = "删除成功" });
        }
        #endregion
        //↑查看建议与删除



        /// <summary>
        /// 所有文章(列表)
        /// </summary>
        /// <param name="pageindex"></param>
        /// <returns></returns>
        public ActionResult ShowArticle(int pageindex = 1)
        {
            int pagesize = 10;//每页显示条数
            int total = 0;//总数
            DAL.GDKQContext dal = new DAL.GDKQContext();
            total = dal.Article.Count(x => x.IsDeleted == true && x.Enable == true);
            if (total<1)
            {
                return Content("暂无文章");
            }
            List<Model.Article> list0 = dal.Article.Where(x => x.IsDeleted == true && x.Enable == true).OrderByDescending(x => x.CreateTime).Skip((pageindex - 1) * pageindex).Take(pagesize).ToList<Model.Article>();
            PagedList<Model.Article> list = new PagedList<Model.Article>(list0, pageindex, pagesize, total);
            ViewBag.Title = "所有文章";
            return View(list);
        }
        /// <summary>
        /// 列表新闻(添加)
        /// </summary>
        /// <param name="bh"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult AddArticle( int? id)
        {
            DAL.GDKQContext dal = new DAL.GDKQContext();
            #region
            Model.UA_Category ca = dal.UA_Category.SingleOrDefault(x => x.bh == "04");
            if (ca == null)
            {
                return Content("没有该分类");
            }
            Model.Article art = new Model.Article();
            #endregion//验证是否有该文章分类
            ViewBag.Title = ca.CaName;
            if (id == null)
            {
                //添加
                art = new Model.Article()
                {
                    AuthorID = 999,
                    AuthorName = "管理员",
                    CaName = ca.CaName,
                    CategoryID = ca.bh,
                    lab = null,
                };
            }
            else
            {
                //编辑
                art = dal.Article.SingleOrDefault(a => a.ID == id);
                if (art == null)
                {
                    return Content("数据库中不存在该新闻");
                }
            }
            return View(art);
        }

        [ValidateAntiForgeryToken()]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult AddArticle(Model.Article art)
        {
            DAL.GDKQContext dal = new DAL.GDKQContext();
            if (art.ID == 0)
            {
                //添加
                dal.Article.Add(new Model.Article()
                {
                    AuthorID = 999,
                    Body=art.Body,
                    Title=art.Title,
                    CaName=art.CaName,
                    CategoryID=art.CategoryID,
                    AuthorName="管理员",
                    lab=art.lab
                });
                dal.SaveChanges();
                return Json(new { status = "y", info = "添加成功"});
            }
            else
            {
                //编辑
                Model.Article art_indbb = dal.Article.SingleOrDefault(a => a.ID == art.ID);
                if (art_indbb == null)
                {
                    return Json(new { status = "n", info = "数据库中无该新闻" });
                }
                art_indbb.Title = art.Title;
                art_indbb.Body = art.Body;
                art_indbb.AuthorName = art.AuthorName;
                art_indbb.AuthorID = 999;
                art_indbb.CaName = art.CaName;
                art_indbb.CategoryID = art.CategoryID;
                art_indbb.lab = art.lab;
                dal.SaveChanges();
                return Json(new { status = "y", info = "编辑成功" });
            }
        }



        /// <summary>
        /// 文章审核
        /// </summary>
        /// <param name="pageindex"></param>
        /// <returns></returns>
        public ActionResult ExmArticle(int pageindex = 1)
        {
            int pagesize = 10;//每页显示条数
            int total = 0;//总数
            DAL.GDKQContext dal = new DAL.GDKQContext();
            total = dal.Article.Count(x=>x.Enable==false);
            if (total < 1)
            {
                return Content("暂无待审核文章");
            }
            List<Model.Article> list0 = dal.Article.Where(x => x.Enable == false&&x.IsDeleted==true).OrderByDescending(ad 
                => ad.CreateTime).Skip((pageindex - 1) * pagesize).Take(pagesize).ToList<Model.Article>();
            PagedList<GDKQ.Model.Article> list = new PagedList<Model.Article>(list0, pageindex, pagesize, total);
            ViewBag.Title = "文章审核";
            return View(list);
        }


        /// <summary>
        /// 审核通过
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Pass(int id)
        {
            DAL.GDKQContext dal = new DAL.GDKQContext();
            Model.Article art = dal.Article.SingleOrDefault(x => x.ID==id);
            if (art==null)
            {
                return Json(new { status = "n", info = "数据库中没有相关数据" });
            }
            art.Enable = true;
            dal.SaveChanges();
            return Json(new { status = "y", info = "审核通过" });
        }

        /// <summary>
        /// 审核不通过(软删除)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Delect(int id)
        {
            DAL.GDKQContext dal = new DAL.GDKQContext();
            Model.Article art = dal.Article.SingleOrDefault(x => x.ID == id);
            if (art == null)
            {
                return Json(new { status = "n", info = "数据库中没有相关数据" });
            }
            art.IsDeleted = false;
            dal.SaveChanges();
            return Json(new { status = "y", info = "删除成功" });
        }


    }
}