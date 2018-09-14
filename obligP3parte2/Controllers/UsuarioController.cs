using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using obligP3parte2.Models.Dominio;

namespace obligP3parte2.Controllers
{
    public class UsuarioController : Controller
    {
        private obligP3parte2Context db = new obligP3parte2Context();

        // GET: Usuario
        public ActionResult Index()
        {
            if (Session["UsuarioMail"] == null || Session["UsuarioMail"].ToString() == "")
                return RedirectToAction("SinAcceso", "Home");

            return View(db.Usuarios.ToList());
        }

        // GET: Usuario/Details/5
        public ActionResult Details(string id)
        {
            if (Session["UsuarioMail"] == null || Session["UsuarioMail"].ToString() == "")
                return RedirectToAction("SinAcceso", "Home");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Usuario usuario = db.Usuarios.Find(id);
            if (usuario == null)
            {
                return HttpNotFound();
            }
            return View(usuario);
        }

        // GET: Usuario/Create
        public ActionResult Create()
        {
            if (Session["UsuarioMail"] == null || Session["UsuarioMail"].ToString() == "")
                return RedirectToAction("SinAcceso", "Home");

            return View();
        }

        // POST: Usuario/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Mail,Password")] Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                db.Usuarios.Add(usuario);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(usuario);
        }

        // GET: Usuario/Edit/5
        public ActionResult Edit(string id)
        {
            if (Session["UsuarioMail"] == null || Session["UsuarioMail"].ToString() == "")
                return RedirectToAction("SinAcceso", "Home");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Usuario usuario = db.Usuarios.Find(id);
            if (usuario == null)
            {
                return HttpNotFound();
            }
            return View(usuario);
        }

        // POST: Usuario/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Mail,Password")] Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                db.Entry(usuario).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(usuario);
        }

        // GET: Usuario/Delete/5
        public ActionResult Delete(string id)
        {
            if (Session["UsuarioMail"] == null || Session["UsuarioMail"].ToString() == "")
                return RedirectToAction("SinAcceso", "Home");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Usuario usuario = db.Usuarios.Find(id);
            if (usuario == null)
            {
                return HttpNotFound();
            }
            return View(usuario);
        }

        // POST: Usuario/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Usuario usuario = db.Usuarios.Find(id);
            db.Usuarios.Remove(usuario);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }





        // GET: Usuario
        public ActionResult Login()
        {
            return View();
        }
        
        // POST: Usuario/Login
        [HttpPost]
        public ActionResult Login(string EmailLogin, string PasswordLogin)
        {
            try
            {
                using (obligP3parte2Context db = new obligP3parte2Context())
                {
                    var usuario = db.Usuarios.SingleOrDefault
                        (u => u.Mail.ToUpper() == EmailLogin.ToUpper()
                        && u.Password == PasswordLogin);
                    if (usuario != null)
                    {
                        Session["UsuarioMail"] = usuario.Mail;
                    }
                    ModelState.AddModelError("LoginIncorrecto", "El mail o contraseña son inexistentes");
                    return RedirectToAction("Index", "Home");
                }
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Logout()
        {
            Session["UsuarioMail"] = null;
            return RedirectToAction("Index", "Home");
        }










    }
}
