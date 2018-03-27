using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using Webdiyer.WebControls.Mvc;

namespace GDKQ.Web.Areas.Adm1n.Controllers
{
    [Filter.AdminLoginFilter]
    public class VoteController : Controller
    {
        /// <summary>
        /// 投票分页
        /// </summary>
        /// <param name="bh"></param>
        /// <param name="pageindex"></param>
        /// <returns></returns>
        public ActionResult List( int pageindex = 1)
        {
            int pagesize = 10;//每页显示条数
            int total = 0;//总数
            DAL.GDKQContext dal = new DAL.GDKQContext();
            List<Model.Vote_Main> lvm = dal.Vote_Main.Where(x => x.IsDeleted == true).OrderByDescending(
                x=>x.CreateTime).Skip((pageindex - 1) * pagesize).Take(pagesize).ToList<Model.Vote_Main>();
            total = dal.Vote_Main.Count(n=> n.IsDeleted == true);
            if (total < 1)
            {
                return Content("数据库中没有相关投票");
            }
            PagedList<GDKQ.Model.Vote_Main> list = new PagedList<Model.Vote_Main>(lvm, pageindex, pagesize, total);
            ViewBag.Title = "投票管理";
            return View(list);
        }

        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken()]
        public ActionResult Add(string title,string body,string endtime)
        {
            if (string.IsNullOrEmpty(title) || string.IsNullOrEmpty(body) || string.IsNullOrEmpty(endtime) )
            {
                return Json(new { status = "n", info = "请填写完整的信息" });
            }
            endtime += " 00:00:00";
            if (IsDate(endtime))
            {
                return Json(new { status = "n", info = "请输入正确的日期格式1" });
            }
            DateTime dt = Convert.ToDateTime(endtime);
            if (dt==null)
            {
                return Json(new { status = "n", info = "请输入正确的日期格式2" });
            }
            DAL.GDKQContext dal = new DAL.GDKQContext();
            if (dal.Vote_Main.Count(x=>x.Enabled==true)>0)
            {
                return Json(new { status = "n", info = "同一时间只能发起一个投票，请确认是否有投票仍在进行" });
            }
            dal.Vote_Main.Add(new Model.Vote_Main()
            {
                Title = title,
                Body = body,
                CreateTime = DateTime.Now,
                EndTime = dt,
                Enabled=true,
                IsDeleted=true
            });
            dal.SaveChanges();
            return Json(new { status = "y", info = "添加成功" });
        }


        public ActionResult VoteResult(int id)
        {
            if (id<1)
            {
                return Json(new { status="n",info = "数据库中没有该投票" });
            }
            DAL.GDKQContext dal = new DAL.GDKQContext();
            Model.Vote_Main vm = dal.Vote_Main.SingleOrDefault(x => x.ID == id);
            if (vm==null)
            {
                return Json(new { status = "n", info = "数据库中没有该投票" });
            }
            double total = dal.Vote_Result.Count(x => x.VoteID == id);
            double choice = dal.Vote_Result.Count(x => x.VoteID == id && x.Agree == true);
            ViewBag.Percent = (choice / total).ToString("P");
            return View(vm);
        }

        /// <summary>
        /// 是否为日期型字符串
        /// </summary>
        /// <param name="StrSource">日期字符串(2008-05-08)</param>
        /// <returns></returns>
        public static bool IsDate(string StrSource)
        {
            return Regex.IsMatch(StrSource, @"^((((1[6-9]|[2-9]\d)\d{2})-(0?[13578]|1[02])-(0?[1-9]|[12]\d|3[01]))|(((1[6-9]|[2-9]\d)\d{2})-(0?[13456789]|1[012])-(0?[1-9]|[12]\d|30))|(((1[6-9]|[2-9]\d)\d{2})-0?2-(0?[1-9]|1\d|2[0-9]))|(((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))-0?2-29-))$");
        }


        //public class viewModel{
        //    public List<Model.News> news001 { get; set; }//公示公告
        //    public List<Model.News> news002 { get; set; }//建设农村
        //    public List<Model.News> news003 { get; set; }//村内事务
        //    public List<Model.News> news007 { get; set; }//时事要点

        //    public viewModel(List<Model.News> newsList001, List<Model.News> newsList002, List<Model.News> newsList003, List<Model.News> newsList007)
        //    {
        //        this.news001 = newsList001;
        //        this.news002 = newsList002;
        //        this.news003 = newsList003;
        //        this.news007 = newsList007;

        //    }

        //}
    }
}