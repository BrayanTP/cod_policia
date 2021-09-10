using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using cod_policia.Models;
using System.Web.Security;
using System.Text;
using System.Threading.Tasks;
using Fluent.Infrastructure.FluentModel;

namespace cod_policia.Controllers
{
    public class UsuarioController : Controller
    {

        [Authorize]
        public ActionResult Index()
        {
            using (var db = new codigo_policiaEntities())
            {
                return View(db.usuario.ToList());

            }

        }

        public ActionResult Registro()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Registro(usuario usuario)
        {
            if (!ModelState.IsValid)
                return View();

            try
            {
                using (var db = new codigo_policiaEntities())
                {
                    usuario.contraseña = UsuarioController.HashSHA1(usuario.contraseña);
                    db.usuario.Add(usuario);

                    db.SaveChanges();

                    return RedirectToAction("Index","Home");
                }

            }
            catch (Exception)
            {
                ModelState.AddModelError("", $"Verique que todos los campos fueron rellenados");
                return View();
            }
        }

        public static string HashSHA1(string value)
        {
            var sha1 = System.Security.Cryptography.SHA1.Create();
            var inputBytes = Encoding.ASCII.GetBytes(value);
            var hash = sha1.ComputeHash(inputBytes);

            var sb = new StringBuilder();
            for (var i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }
            return sb.ToString();
        }


        // GET: Usuario
        public ActionResult Login(string message="")
        {
            ViewBag.Message = message;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string user, string password)
        {
            string passEncrip = UsuarioController.HashSHA1(password);
            using (var db = new codigo_policiaEntities())
            {
                var userLogin = db.usuario.FirstOrDefault(e => e.correo == user && e.contraseña == passEncrip);
                if (userLogin != null)
                {
                    FormsAuthentication.SetAuthCookie(userLogin.correo, true);
                    Session["User"] = userLogin;
                    return RedirectToAction("Index", "Home");
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