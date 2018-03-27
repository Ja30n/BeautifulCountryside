using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GDKQ.Web.Areas.Livelihood.Controllers
{
    public class VoteController : Controller
    {
        public ActionResult Index()
        {
            DAL.GDKQContext dal = new DAL.GDKQContext();
            Model.Vote_Main vm = dal.Vote_Main.SingleOrDefault(x => x.Enabled == true);
            if (vm==null)
            {
                return Content("当前暂无投票");
            }
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Index(int? id,string Choice)
        {
            DAL.GDKQContext dal = new DAL.GDKQContext();
            Model.Vote_Main vm = dal.Vote_Main.SingleOrDefault(x => x.ID == id);
            if (vm==null)
            {
                return Json(new { status = "n", info = "数据库中没有该投票" });
            }
            
            Model.Villager v = Session["villager"] as Model.Villager;
            if (v==null)
            {
                return Json(new { status = "n", info = "只有村民才能投票哦" });
            }

            if (string.IsNullOrEmpty(Choice))
            {
                return Json(new { status = "n", info = "请选择同意或者反对" });
            }

            bool bo;
            if (Choice=="agree")
            {
                bo = true;
            }
            else
            {
                bo = false;
            }
           
            int total = dal.Vote_Result.Count(x => x.VoteID == id && x.VillagerID == v.ID);
            if (total>1)
            {
                return Json(new { status = "n", info = "不能重复投票哦" });
            }

            dal.Vote_Result.Add(new Model.Vote_Result()
            {
                Agree = bo,
                VoteID = vm.ID,
                VillagerID = v.ID,
                CreateTime = DateTime.Now
            });
            dal.SaveChanges();
            return Json(new { status = "y", info = "投票成功" });
        }

        [Filter.VillagerLoginFilter]
        public ActionResult VoteResult(int?id)
        {
            DAL.GDKQContext dal = new DAL.GDKQContext();
            Model.Vote_Main vom = dal.Vote_Main.SingleOrDefault(x => x.ID == id);
            if (vom==null)
            {
                return Content("没有该投票结果");
            }
            int agree_num = dal.Vote_Result.Count(x => x.VoteID == id && x.Agree == true);
            int disagree_num= dal.Vote_Result.Count(x => x.VoteID == id && x.Agree == false);
            viewModel vm = new viewModel(vom, agree_num, disagree_num);
            return View(vm);
        }
        public class viewModel{
            public Model.Vote_Main vom { get; set; }//投票模型
            public int agree_num { get; set; }//同意数量
            public int disagree_num { get; set; }//反对数量

            public viewModel(Model.Vote_Main _vom, int _agree_num, int _disagree_num)
            {
                this.vom = _vom;
                this.agree_num = _agree_num;
                this.disagree_num = _disagree_num;
            }

        }
    }
}