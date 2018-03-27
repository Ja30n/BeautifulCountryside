using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GDKQ.Model;
using Common;

namespace GDKQ.Web.Areas.Adm1n.Controllers
{
    public class HomeController : Controller
    {
        [Filter.AdminLoginFilter]
        //后台主页
        public ActionResult Index()
        {
            DAL.GDKQContext dal = new DAL.GDKQContext();
            int Advice = dal.Advice.Count(x => x.IsRead == true);
            int VotePercent = 62;
            int ArtcleNum= dal.Article.Count(x => x.Enable == false);
            int VisitNum = 654;
            List<Model.Article> listA = dal.Article.Where(x => x.Enable == false && x.IsDeleted == true).OrderByDescending(x 
                => x.CreateTime).Take(10).ToList<Model.Article>();
            List<Model.Advice> listAD= dal.Advice.Where(x =>x.IsRead==true).OrderByDescending(x
                 => x.CreateTime).Take(10).ToList<Model.Advice>();
            viewModel vm = new viewModel(Advice, VotePercent, ArtcleNum, VisitNum,listA,listAD);
            return View(vm);
        }

        public class viewModel
        {
            public List<Advice> listAD { get; set; }//建议列表
            public int Advice { get; set; }//建议
            public int VotePercent { get; set; }//投票
            public int ArtcleNum { get; set; }//文章审核
            public int VisitNum { get; set; }//访问统计
            public List<Model.Article> listA { get; set; }//文章列表

            public viewModel(int advice, int votePercent, int artcleNum, int visitNum, List<Article> listA, List<Advice> listAD)
            {
                this.Advice = advice;
                this.VotePercent = votePercent;
                this.ArtcleNum = artcleNum;
                this.VisitNum = visitNum;
                this.listA = listA;
                this.listAD = listAD;
            }
        }

        /// <summary>
        /// 上传列表图片
        /// </summary>
        /// <returns></returns>
        public ActionResult SaveMedia()
        {
            return Json(new { status = "y", info = "上传成功" });
            //HttpPostedFileBase imgFile = Request.Files["imgFile"];
        }
        /// <summary>
        /// Kindeditor在线上传
        /// </summary>
        /// <returns></returns>
        public ActionResult Kindeditor_Upload()
        {
            //文件保存目录路径
            String savePath = "/upload/";


            //文件保存目录URL
            String saveUrl = "/upload/";

            //定义允许上传的文件扩展名
            Hashtable extTable = new Hashtable();
            extTable.Add("image", "gif,jpg,jpeg,png,bmp");
            extTable.Add("flash", "swf,flv");
            extTable.Add("media", "swf,flv,mp3,wav,wma,wmv,mid,avi,mpg,asf,rm,rmvb");
            extTable.Add("file", "doc,docx,xls,xlsx,ppt,htm,html,txt,zip,rar,gz,bz2");

            //最大文件大小
            int maxSize = 1000000;

            HttpPostedFileBase imgFile = Request.Files["imgFile"];
            if (imgFile == null)
            {
                showError("请选择文件。");
            }

            String dirPath = Server.MapPath(savePath);
            if (!Directory.Exists(dirPath))
            {
                showError("上传目录不存在。");
            }

            String dirName = Request.QueryString["dir"];
            if (String.IsNullOrEmpty(dirName))
            {
                dirName = "image";
            }
            if (!extTable.ContainsKey(dirName))
            {
                showError("目录名不正确。");
            }

            String fileName = imgFile.FileName;
            String fileExt = Path.GetExtension(fileName).ToLower();

            if (imgFile.InputStream == null || imgFile.InputStream.Length > maxSize)
            {
                showError("上传文件大小超过限制。");
            }

            if (String.IsNullOrEmpty(fileExt) || Array.IndexOf(((String)extTable[dirName]).Split(','), fileExt.Substring(1).ToLower()) == -1)
            {
                showError("上传文件扩展名是不允许的扩展名。\n只允许" + ((String)extTable[dirName]) + "格式。");
            }

            //创建文件夹
            dirPath += dirName + "/";
            saveUrl += dirName + "/";
            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }
            String ymd = DateTime.Now.ToString("yyyyMMdd", DateTimeFormatInfo.InvariantInfo);
            dirPath += ymd + "/";
            saveUrl += ymd + "/";
            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }

            String newFileName = DateTime.Now.ToString("yyyyMMddHHmmss_ffff", DateTimeFormatInfo.InvariantInfo) + fileExt;
            String filePath = dirPath + newFileName;

            imgFile.SaveAs(filePath);

            String fileUrl = saveUrl + newFileName;

            Hashtable hash = new Hashtable();
            hash["error"] = 0;
            hash["url"] = fileUrl;
            Response.AddHeader("Content-Type", "text/html; charset=UTF-8");
            Response.Write(JsonMapper.ToJson(hash));
            Response.End();
            return null;
        }

        private void showError(string message)
        {
            Hashtable hash = new Hashtable();
            hash["error"] = 1;
            hash["message"] = message;
            Response.AddHeader("Content-Type", "text/html; charset=UTF-8");
            Response.Write(JsonMapper.ToJson(hash));
            Response.End();
        }
    }
}