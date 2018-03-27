using Common;
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
    /// 角色管理器
    /// </summary>
    public class RoleManageController : Controller
    {

        #region
        /// <summary>
        /// 添加村民
        /// </summary>
        /// <returns></returns>
        public ActionResult AddVillager(int? id)
        {
            DAL.GDKQContext dal = new DAL.GDKQContext();
            Model.Villager v = dal.Villager.SingleOrDefault(x => x.ID == id);
            if (id == null)//添加村民
            {
                v = new Model.Villager()
                {
                    CreatTime = DateTime.Now,
                    Enabled = true,
                    IsDeleted = true,
                    LastLoginIP = GetIPadress.GetHostAddress(),
                    LastLoginTime = DateTime.Now,
                };
            }
            else
            {//修改村民信息

                if (v == null)
                {
                    return Json(new { status = "n", info = "数据库中没有该村民" });
                }
            }
            return View(v);
        }
        [ValidateAntiForgeryToken()]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult AddVillager(Model.Villager v)
        {
            DAL.GDKQContext dal = new DAL.GDKQContext();
            if (v.ID == 0)
            {
                //添加
                dal.Villager.Add(new Model.Villager()
                {
                    UserName = v.UserName,
                    Password = v.Password,
                    RealName = v.RealName,
                    Mobile = v.Mobile,
                    CreatTime=v.CreatTime,
                    Enabled=v.Enabled,
                    IsDeleted=v.IsDeleted,
                    LastLoginIP=v.LastLoginIP,
                    LastLoginTime=v.LastLoginTime
                });
                dal.SaveChanges();
                return Json(new { status = "y", info = "添加成功" });
            }
            else
            {
                //编辑
                Model.Villager v_indbb = dal.Villager.SingleOrDefault(a => a.ID == v.ID);
                if (v_indbb == null)
                {
                    return Json(new { status = "n", info = "数据库中没有该用户" });
                }
                v_indbb.UserName = v.UserName;
                v_indbb.Password = v.Password;
                v_indbb.RealName = v.RealName;
                v_indbb.Mobile = v.Mobile;
                dal.SaveChanges();
                return Json(new { status = "y", info = "修改成功" });
            }
        }


        /// <summary>
        /// 村民列表
        /// </summary>
        /// <returns></returns>
        public ActionResult Villager(int pageindex = 1)
        {
            int pagesize = 10;
            int total = 0;//总数
            DAL.GDKQContext dal = new DAL.GDKQContext();
            total = dal.Villager.Count();
            List<Model.Villager> list0 = dal.Villager.OrderBy(a => a.ID).Skip((pageindex - 1) * pagesize).Take(pagesize).ToList<Model.Villager>();
            PagedList<GDKQ.Model.Villager> list = new PagedList<Model.Villager>(list0, pageindex, pagesize, total);
            ViewBag.Title = "村民管理";
            return View(list);
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Forbidden(int id)
        {
            DAL.GDKQContext dal = new DAL.GDKQContext();
            Model.Villager v = dal.Villager.SingleOrDefault(x => x.ID == id);
            if (v == null)
            {
                return Json(new { status = "n", info = "数据库中没有该用户" });
            }
            v.Enabled = !v.Enabled;
            dal.SaveChanges();
            return Json(new { status = "y", info = "操作成功" });
        }

        #endregion

        //↑村民管理


        /// <summary>
        /// 游客列表
        /// </summary>
        /// <param name="pageindex"></param>
        /// <returns></returns>
        public new ActionResult User(int pageindex = 1)
        {
            int pagesize = 10;
            int total = 0;//总数
            DAL.GDKQContext dal = new DAL.GDKQContext();
            total = dal.User.Count();
            List<Model.User> list0 = dal.User.OrderBy(a => a.ID).Skip((pageindex - 1) * pagesize).Take(pagesize).ToList<Model.User>();
            PagedList<GDKQ.Model.User> list = new PagedList<Model.User>(list0, pageindex, pagesize, total);
            ViewBag.Title = "游客管理";
            return View(list);
        }


        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Forbidden1(int id)
        {
            DAL.GDKQContext dal = new DAL.GDKQContext();
            Model.User v = dal.User.SingleOrDefault(x => x.ID == id);
            if (v == null)
            {
                return Json(new { status = "n", info = "数据库中没有该用户" });
            }
            v.Enabled = !v.Enabled;
            dal.SaveChanges();
            return Json(new { status = "y", info = "操作成功" });
        }

    }
}