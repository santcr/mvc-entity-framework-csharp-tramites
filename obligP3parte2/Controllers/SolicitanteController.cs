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
    public class SolicitanteController : Controller
    {
        private obligP3parte2Context db = new obligP3parte2Context();

        // GET: Solicitante
        public ActionResult Index()
        {
            if (Session["UsuarioMail"] == null || Session["UsuarioMail"].ToString() == "")
                return RedirectToAction("SinAcceso", "Home");

            return View(db.Solicitantes.ToList());
        }

        // GET: Solicitante/Details/5
        public ActionResult Details(string id)
        {
            if (Session["UsuarioMail"] == null || Session["UsuarioMail"].ToString() == "")
                return RedirectToAction("SinAcceso", "Home");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Solicitante solicitante = db.Solicitantes.Find(id);
            if (solicitante == null)
            {
                return HttpNotFound();
            }
            return View(solicitante);
        }

        // GET: Solicitante/Create
        public ActionResult Create()
        {
            if (Session["UsuarioMail"] == null || Session["UsuarioMail"].ToString() == "")
                return RedirectToAction("SinAcceso", "Home");

            string cedula = TempData["cedula"] as string;
            Solicitante solicitante = new Solicitante { Cedula = cedula };
            return View(solicitante);
        }

        // POST: Solicitante/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Cedula,Nombre,Apellido,Tel,Mail")] Solicitante solicitante)
        {
            if (ModelState.IsValid)
            {
                db.Solicitantes.Add(solicitante);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(solicitante);
        }

        // GET: Solicitante/Edit/5
        public ActionResult Edit(string id)
        {
            if (Session["UsuarioMail"] == null || Session["UsuarioMail"].ToString() == "")
                return RedirectToAction("SinAcceso", "Home");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Solicitante solicitante = db.Solicitantes.Find(id);
            if (solicitante == null)
            {
                return HttpNotFound();
            }
            return View(solicitante);
        }

        // POST: Solicitante/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Cedula,Nombre,Apellido,Tel,Mail")] Solicitante solicitante)
        {
            if (ModelState.IsValid)
            {
                db.Entry(solicitante).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(solicitante);
        }

        // GET: Solicitante/Delete/5
        public ActionResult Delete(string id)
        {
            if (Session["UsuarioMail"] == null || Session["UsuarioMail"].ToString() == "")
                return RedirectToAction("SinAcceso", "Home");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Solicitante solicitante = db.Solicitantes.Find(id);
            if (solicitante == null)
            {
                return HttpNotFound();
            }
            return View(solicitante);
        }

        // POST: Solicitante/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Solicitante solicitante = db.Solicitantes.Find(id);
            db.Solicitantes.Remove(solicitante);
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





        [HttpGet]
        public ActionResult BuscarParaNuevoExpediente()
        {
            return View();
        }

        [HttpPost]
        public ActionResult BuscarParaNuevoExpediente(string cedula)
        {
            if (cedula == null)
            {
                return RedirectToAction("Create");
            }
            Solicitante solicitante = db.Solicitantes.Find(cedula);
            if (solicitante == null)
            {
                TempData["cedula"] = cedula;
                return RedirectToAction("Create");
            }
            TempData["SolicitanteCedula"] = solicitante.Cedula;
            return RedirectToAction("Create", "Expediente");

        }











    }
}
