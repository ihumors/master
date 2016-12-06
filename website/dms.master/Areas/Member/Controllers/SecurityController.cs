using Dms.Master.Areas.Member.Models;
using Dms.Master.Filters;
using System.Web;
using System.Web.Mvc;

namespace Dms.Master.Areas.Member.Controllers
{
    public class SecurityController : Controller
    {
        string message = string.Empty;
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost, SignInValidateAntiForgeryToken]
        public JsonResult Login(LoginViewModel loginViewModel)
        {
            bool loginState = false;
            if (ModelState.IsValid)
            {
                Core.Cookies cookies = new Core.Cookies();
                cookies.Set("uid", "hn-man@live.cn", loginViewModel.RememberMe);
                loginState = true;
            }
            return Json(new
            {
                secucess = loginState,
                message = message
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult LogOff()
        {
            Core.Cookies cookies = new Core.Cookies();
            cookies.Clear();
            return RedirectToAction("Login", "Security");
        }

        public FileContentResult CodeImage()
        {
            Core.VerifyCodeModel model = new Core.VerifyCodeModel()
            {
                Noise = true,
                BackgroundColor = System.Drawing.Color.Transparent
            };
            string code = Core.VerifyCode.RandomCode(6, model);
            HttpContext.Response.Cookies.Add(new HttpCookie("SigninVerifyCode", code.ToUpper()));
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            System.Drawing.Bitmap image = Core.VerifyCode.CreateImageCode(code, model);
            image.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            ms.Close();
            image.Dispose();
            return File(ms.ToArray(), "image/png");
        }
    }
}