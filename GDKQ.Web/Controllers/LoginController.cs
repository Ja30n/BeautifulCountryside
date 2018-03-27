using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Common;
using System.Text.RegularExpressions;

namespace GDKQ.Web.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login & Register
        public ActionResult Index()
        {
            Session.Abandon();
            return View();
        }

        #region
        public ActionResult SecurityCode()
        {
            string oldcode = TempData["SecurityCode"] as string;
            string code = CreateRandomCode(4); //验证码的字符为4个
            TempData["SecurityCode"] = code; //验证码存放在TempData中
            return File(CreateValidateGraphic(code), "image/Jpeg");
        }

        /// <summary>
        /// 生成随机的字符串
        /// </summary>
        /// <param name="codeCount"></param>
        /// <returns></returns>
        public string CreateRandomCode(int codeCount)
        {
            string allChar = "0,1,2,3,4,5,6,7,8,9,A,B,C,D,E,a,b,c,d,e,f,g,h,i,g,k,l,m,n,o,p,q,r,F,G,H,I,G,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z,s,t,u,v,w,x,y,z";
            string[] allCharArray = allChar.Split(',');
            string randomCode = "";
            int temp = -1;
            Random rand = new Random();
            for (int i = 0; i < codeCount; i++)
            {
                if (temp != -1)
                {
                    rand = new Random(i * temp * ((int)DateTime.Now.Ticks));
                }
                int t = rand.Next(35);
                if (temp == t)
                {
                    return CreateRandomCode(codeCount);
                }
                temp = t;
                randomCode += allCharArray[t];
            }
            return randomCode;
        }

        /// <summary>
        /// 创建验证码图片
        /// </summary>
        /// <param name="validateCode">验证码字符串</param>
        /// <returns>验证码图片</returns>
        public byte[] CreateValidateGraphic(string validateCode)
        {
            Bitmap image = new Bitmap((int)Math.Ceiling(validateCode.Length * 16.0), 27);
            Graphics g = Graphics.FromImage(image);
            try
            {
                //生成随机生成器
                Random random = new Random();
                //清空图片背景色
                g.Clear(Color.White);
                //画图片的干扰线
                for (int i = 0; i < 25; i++)
                {
                    int x1 = random.Next(image.Width);
                    int x2 = random.Next(image.Width);
                    int y1 = random.Next(image.Height);
                    int y2 = random.Next(image.Height);
                    g.DrawLine(new Pen(Color.Silver), x1, x2, y1, y2);
                }
                Font font = new Font("Arial", 13, (FontStyle.Bold | FontStyle.Italic));
                LinearGradientBrush brush = new LinearGradientBrush(new Rectangle(0, 0, image.Width, image.Height), Color.Blue, Color.DarkRed, 1.2f, true);
                g.DrawString(validateCode, font, brush, 3, 2);

                //画图片的前景干扰线
                for (int i = 0; i < 100; i++)
                {
                    int x = random.Next(image.Width);
                    int y = random.Next(image.Height);
                    image.SetPixel(x, y, Color.FromArgb(random.Next()));
                }
                //画图片的边框线
                g.DrawRectangle(new Pen(Color.Silver), 0, 0, image.Width - 1, image.Height - 1);

                //保存图片数据
                MemoryStream stream = new MemoryStream();
                image.Save(stream, ImageFormat.Jpeg);

                //输出图片流
                return stream.ToArray();
            }
            finally
            {
                g.Dispose();
                image.Dispose();
            }
        }
        #endregion


        /// <summary>
        /// 登录验证
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        /// <param name="SecurityCode">验证码</param>
        /// <returns></returns>
        [ValidateAntiForgeryToken()]
        [HttpPost]
        public ActionResult Admin(string username, string password, string SecurityCode)
        {
            if (string.IsNullOrEmpty(SecurityCode))//验证验证码校验
            {
                return Json(new { status = "n", info = "请输入验证码" });
            }
            if (TempData["SecurityCode"]==null)
            {
                return Content("出现错误了，重新登录试试");
            }
            if ((SecurityCode.ToLower())!=((TempData["SecurityCode"] as string).ToLower()))
            {
                return Json(new { status = "n", info = "验证码错误,请重新输入" });
            }

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))//用户名和密码校验
            {
                return Json(new { status = "n", info = "用户名或密码不能为空" });
            }
            password += "_GDKQ";
            password = MD5.MD5Encrypt16(password);
            Model.Admin a = new DAL.GDKQContext().Admin.SingleOrDefault(x => x.UserName == username && x.Password == password);
            if (a == null)
            {
                return Json(new { status = "n", info = "用户名或密码错误" });
            }
            a.LastLoginIP = GetIPadress.GetHostAddress();
            a.LastLoginTime = DateTime.Now;
            Session["admin"] = a;//创建Session
            return Json(new { status = "y", info = "登录成功" ,NextUrl= "/Adm1n/Home/Index" });
        }
        //↑管理员
        public ActionResult Villager(string username, string password, string SecurityCode)
        {
            if (string.IsNullOrEmpty(SecurityCode))//验证验证码校验
            {
                return Json(new { status = "n", info = "请输入验证码" });
            }
            if ((SecurityCode.ToLower()) != ((TempData["SecurityCode"] as string).ToLower()))
            {
                return Json(new { status = "n", info = "验证码错误,请重新输入" });
            }

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))//用户名和密码校验
            {
                return Json(new { status = "n", info = "用户名或密码不能为空" });
            }
            password += "_GDKQ";
            password = MD5.MD5Encrypt16(password);
            Model.Villager v = new DAL.GDKQContext().Villager.SingleOrDefault(x => x.UserName == username && x.Password == password);
            if (v == null)
            {
                return Json(new { status = "n", info = "用户名或密码错误" });
            }
            if (v.Enabled!=true)
            {
                return Json(new { status = "n", info = "账号被冻结，请联系管理员" });
            }
            v.LastLoginIP = GetIPadress.GetHostAddress();
            v.LastLoginTime = DateTime.Now;
            Session["villager"] = v;//创建Session
            return Json(new { status = "y", info = "登录成功", NextUrl = "/Home/Index" });
        }
        //↑村民
        public new ActionResult User(string username, string password, string SecurityCode)
        {
            if (string.IsNullOrEmpty(SecurityCode))//验证验证码校验
            {
                return Json(new { status = "n", info = "请输入验证码" });
            }
            if ((SecurityCode.ToLower()) != ((TempData["SecurityCode"] as string).ToLower()))
            {
                return Json(new { status = "n", info = "验证码错误,请重新输入" });
            }

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))//用户名和密码校验
            {
                return Json(new { status = "n", info = "用户名或密码不能为空" });
            }
            password += "_GDKQ";
            password=MD5.MD5Encrypt16(password);
            DAL.GDKQContext dal = new DAL.GDKQContext();
            Model.User u = dal.User.SingleOrDefault(x => x.UserName == username && x.Password == password);
            if (u == null)
            {
                return Json(new { status = "n", info = "用户名或密码错误" });
            }
            if (u.Enabled != true)
            {
                return Json(new { status = "n", info = "账号被冻结，请联系管理员" });
            }
            u.LastLoginIP = GetIPadress.GetHostAddress();
            u.LastLoginTime = DateTime.Now;
            Session["user"] = u;//创建Session
            return Json(new { status = "y", info = "登录成功", NextUrl = "/User/Home" });
        }
        //↑用户


        //用户注册
        public ActionResult Register(string usernamer, string passwordr, string passwordr2, string mail)
        {
            //if()//用户名是否重复
            //
            
            //用户名和密码校验
            if (string.IsNullOrEmpty(usernamer) || string.IsNullOrEmpty(passwordr)||string.IsNullOrEmpty(passwordr2)||string.IsNullOrEmpty(mail))
            {
                return Json(new { status = "n", info = "请填写完整的注册信息" });
            }

            if (passwordr2 != passwordr)
            {
                return Json(new { status = "n", info = "两次输入的密码不一致，请重新输入" });
            }

            Regex Verification = new Regex("^[a-zA-Z]\\w{5,15}$");
            if (!Verification.IsMatch(usernamer))
            {
                return Json(new { status = "n", info = "请注意用户名格式" });
            }
            Verification = new Regex("\\w{5,15}$");
            if (!Verification.IsMatch(passwordr))
            {
                return Json(new { status = "n", info = "请注意密码格式" });
            }
            
            Verification = new Regex("^\\s*([A-Za-z0-9_-]+(\\.\\w+)*@(\\w+\\.)+\\w{2,5})\\s*$");
            if (!Verification.IsMatch(mail))
            {
                return Json(new { status = "n", info = "请输入正确的邮箱格式" });
            }

            //验证用户名是否已经注册
            DAL.GDKQContext dal = new DAL.GDKQContext();
            Model.User u = dal.User.SingleOrDefault(x => x.Password == passwordr);
            if (u!=null)
            {
                return Json(new { status = "n", info = "该用户名已经被注册了，请更换用户名" });
            }

            //数据库添加用户
            passwordr += "_GDKQ";
            passwordr = MD5.MD5Encrypt16(passwordr);
            dal.User.Add(new Model.User()
            {
                UserName = usernamer,
                Password = passwordr,
                CreatTime = DateTime.Now,
                LastLoginTime = DateTime.Now,
                LastLoginIP = GetIPadress.GetHostAddress(),
                Enabled = true,
                IsDeleted = true,
            });
            dal.SaveChanges();
            Model.User uu = dal.User.SingleOrDefault(x => x.UserName == usernamer && x.Password == passwordr);
            if (uu==null)
            {
                return Json(new { status = "n", info = "bug" });
            }

            dal.UserInfo.Add(new Model.UserInfo()
            {
                UserID = uu.ID,
                Nickname = "萌新"+uu.ID,
                Hobby = "暂无",
                Gender = "男",
                Description = "这家伙很懒，啥都没写",
                Photo = "/Content/assets/img/toux1.jpg",
                RealName = "保密",
            });
            uu.Nickname += uu.ID;
            dal.SaveChanges();
            //创建session
            Session["user"] = uu;
            return Json(new { status = "y", info = "注册成功", NextUrl = "/User/Home/Index" });
        }
    }
    
}
