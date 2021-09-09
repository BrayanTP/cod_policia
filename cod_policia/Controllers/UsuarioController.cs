using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using cod_policia.Models;
using System.Web.Security;

namespace cod_policia.Controllers
{
    public class UsuarioController : Controller
    {
        // GET: Usuario
        public ActionResult Login(string message="")
        {
            ViewBag.Message = message;
            return View();
        }

        [HttpPost]
        public ActionResult Login(string user, string password)
        {
            using (var db = new codigo_policiaEntities())
            {
                var userLogin = db.usuario.FirstOrDefault(e => e.correo == user && e.contraseña == password);
                if (userLogin != null)
                {
                    FormsAuthentication.SetAuthCookie(userLogin.correo, true);
                    Session["User"] = userLogin;
                    return RedirectToAction("Index");
                }
                else
                {
                    return Login("Verifique sus datos");
                }
            }
        }

        [Authorize]
        public ActionResult CloseSession()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }
    }
}