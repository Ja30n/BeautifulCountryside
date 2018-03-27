using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Webdiyer.WebControls.Mvc;

namespace GDKQ.Web.Controllers
{
    /// <summary>
    /// 新闻控制器
    /// </summary>
    public class NewsController : Controller
    {
        /// <summary>
        /// 逆旅千古
        /// </summary>
        /// <param name="bh">11</param>
        /// <returns></returns>
        public ActionResult OnePage(string bh)
        {
            Model.News n = new DAL.GDKQContext().News.SingleOrDefault(a => a.bh == bh);
            if (n==null)
            {
                return Content("暂无该新闻");
            }
            ViewBag.Title = n.CaName;
            return View(n); 
        }

        /// <summary>
        /// 先哲今贤
        /// </summary>
        /// <param name="bh">12</param>
        /// <returns></returns>
        public ActionResult List(string bh,int pageindex=1)
        {
            int pagesize = 10;//每页显示条数
            int total = 0;//总数
            DAL.GDKQContext dal = new DAL.GDKQContext();
            total = dal.News.Count(n=>n.bh == bh);
            if (total <1)
            {
                return Content("数据库中没有相关新闻");
            }
            List<Model.News> list0 = dal.News.Where(n => n.bh == bh).OrderBy(n => n.CreateTime).Skip((pageindex - 1) * pagesize).Take(pagesize).ToList<Model.News>();
            
            PagedList<GDKQ.Model.News> list = new PagedList<Model.News>(list0, pageindex, pagesize, total);
            return View(list);
        }


        /// <summary>
        /// 文化遗产保护
        /// </summary>
        /// <param name="bh"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult OnePage1(string bh, int? id)
        {
            DAL.GDKQContext dal = new DAL.GDKQContext();
            Model.News n = dal.News.SingleOrDefault(x => x.ID == id && x.IsDeleted == true);
            if (n==null)
            {
                return Content("数据库中没有该新闻");
            }
            ViewBag.Title = n.CaName;
            return View(n);
        }


        public ActionResult List1(string bh, int pageindex = 1)
        {
            int pagesize = 10;//每页显示条数
            int total = 0;//总数
            DAL.GDKQContext dal = new DAL.GDKQContext();
            total = dal.News.Count(n => n.bh == bh);
            if (total < 1)
            {
                return Content("数据库中没有相关新闻");
            }
            List<Model.News> list0 = dal.News.Where(n => n.bh == bh).OrderBy(n => n.CreateTime).Skip((pageindex - 1) * pagesize).Take(pagesize).ToList<Model.News>();

            PagedList<GDKQ.Model.News> list = new PagedList<Model.News>(list0, pageindex, pagesize, total);
            return View(list);
        }
        /// <summary>
        /// 走进孔坵
        /// </summary>
        /// <returns></returns>
        public ActionResult KQDetail()
        {
            DAL.GDKQContext dal = new DAL.GDKQContext();
            
            return View();
        }



        /// <summary>
        /// 孔坵全景
        /// </summary>
        /// <returns></returns>
        public ActionResult OnePage_qj()
        {
            return View();
        }

        /// <summary>
        /// 山明水秀
        /// </summary>
        /// <returns></returns>
        public ActionResult OnePage_sm()
        {
            return View();
        }

        /// <summary>
        /// 守望瑰宝
        /// </summary>
        /// <returns></returns>
        public ActionResult OnePage_sw()
        {
            return View();
        }

        /// <summary>
        /// 曲径通幽
        /// </summary>
        /// <returns></returns>
        public ActionResult OnePage_qjty()
        {
            return View();
        }

        /// <summary>
        /// 众筹与活动（列表）
        /// </summary>
        /// <returns></returns>
        public ActionResult List_zc(string bh)
        {
            DAL.GDKQContext dal = new DAL.GDKQContext();
            List<Model.News> zc = dal.News.Where(x => x.bh == bh && x.IsDeleted == true).OrderByDescending(x=>x.CreateTime).Take(3).ToList<Model.News>();
            return View(zc);
        }

        /// <summary>
        /// 众筹活动（单页）
        /// </summary>
        /// <param name="bh"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult OnePage_zc(string bh, int? id)
        {
            DAL.GDKQContext dal = new DAL.GDKQContext();
            Model.News zc = dal.News.SingleOrDefault(x => x.bh == bh & x.ID == id);
            if (zc==null)
            {
                return Content("数据库中没有该活动");
            }
            ViewBag.Title=zc.Author;
            return View(zc);
        }

        /// <summary>
        /// 摄影比赛
        /// </summary>
        /// <returns></returns>
        public ActionResult photo_contest()
        {
            //DAL.GDKQContext dal = new DAL.GDKQContext();
            //List<Model.Photo> p = dal.Photo.OrderByDescending(x => x.CreateTime).Take(16).ToList<Model.Photo>();
            //if (p==null)
            //{
            //    return Content("暂无比赛");
            //}
            ViewBag.Title = "摄影比赛";
            return View();
        }


        public ActionResult join_contest()
        {
            return View();
        }
    }
}