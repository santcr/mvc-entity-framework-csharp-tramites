using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using obligP3parte2.Models.Dominio;
using obligP3parte2.Models;
using System.Linq;

namespace obligP3parte2.Controllers
{
    public class ExpedienteController : Controller
    {
        private obligP3parte2Context db = new obligP3parte2Context();

        // GET: Expediente
        public ActionResult Index()
        {
            if (Session["UsuarioMail"] == null || Session["UsuarioMail"].ToString() == "")
                return RedirectToAction("SinAcceso", "Home");

            return View(db.Expedientes.ToList());
        }

        public ActionResult Details(int? id)
        {
            if (Session["UsuarioMail"] == null || Session["UsuarioMail"].ToString() == "")
                return RedirectToAction("SinAcceso", "Home");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Expediente expediente = db.Expedientes
                                            .Where( e => e.Codigo == id )
                                            .Include( e => e.EtapasCumplidas.Select(ee => ee.Funcionario))
                                            .Include(e => e.EtapasCumplidas.Select(eee => eee.Etapa))
                                            .FirstOrDefault();
            if (expediente == null)
            {
                return HttpNotFound();
            }
            return View(expediente);
        }

        // GET: Expediente/Create
        public ActionResult Create()
        {
            if (Session["UsuarioMail"] == null || Session["UsuarioMail"].ToString() == "")
                return RedirectToAction("SinAcceso", "Home");

            string cedula = TempData["SolicitanteCedula"] as string;
            if (cedula == null)
                return RedirectToAction("BuscarParaNuevoExpediente", "Solicitante");

            if (ModelState.IsValid)
            {
                Expediente nuevoExp = new Expediente();
                List<Tramite> tramites;
                Solicitante solicitante = new Solicitante();

                using (obligP3parte2Context db = new obligP3parte2Context())
                {
                    tramites = db.Tramites.ToList();
                    solicitante = db.Solicitantes.Find(cedula);
                }

                CrearExpedienteViewModel vm = new CrearExpedienteViewModel()
                {
                    Expediente = nuevoExp,
                    Tramites = new SelectList(tramites, "Codigo", "Titulo"),
                    CedulaSolicitante = cedula,
                    Solicitante = solicitante
                };
                return View(vm);
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        // POST: Expediente/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CrearExpedienteViewModel vm)
        {
            DateTime hoy = DateTime.Today;
            using (obligP3parte2Context db = new obligP3parte2Context())
            {
                vm.Expediente = new Expediente();
                vm.Expediente.Tramite = db.Tramites.Find(vm.TramiteSeleccionado);
                vm.Expediente.idSolicitante = vm.CedulaSolicitante;
                vm.Expediente.FechaInic = hoy;
                vm.Expediente.MailFunc = Session["UsuarioMail"].ToString();

                db.Expedientes.Add(vm.Expediente);
                db.SaveChanges();
                vm.Tramites = new SelectList(db.Tramites.ToList(), "Codigo", "Titulo");
            }

            return View("Details", vm.Expediente);
        }




        // GET: Expediente/Edit/5
        public ActionResult Edit(int? id)
        {
            if (Session["UsuarioMail"] == null || Session["UsuarioMail"].ToString() == "")
                return RedirectToAction("SinAcceso", "Home");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Expediente expediente = db.Expedientes.Find(id);
            if (expediente == null)
            {
                return HttpNotFound();
            }
            return View(expediente);
        }

        // POST: Expediente/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Codigo,MailFunc,FechaInic")] Expediente expediente)
        {
            if (ModelState.IsValid)
            {
                db.Entry(expediente).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(expediente);
        }

        // GET: Expediente/Delete/5
        public ActionResult Delete(int? id)
        {
            if (Session["UsuarioMail"] == null || Session["UsuarioMail"].ToString() == "")
                return RedirectToAction("SinAcceso", "Home");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Expediente expediente = db.Expedientes.Find(id);
            if (expediente == null)
            {
                return HttpNotFound();
            }
            return View(expediente);
        }

        // POST: Expediente/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Expediente expediente = db.Expedientes.Find(id);
            db.Expedientes.Remove(expediente);
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
        



        // GET Agregar Etapa
        [HttpGet]
        public ActionResult AgregarEtapa(int? id)
        {
            if (Session["UsuarioMail"] == null || Session["UsuarioMail"].ToString() == "")
                return RedirectToAction("SinAcceso", "Home");

            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            Expediente expediente = db.Expedientes.Find(id);
            if (expediente == null)
                return HttpNotFound();

            // cargar etapas sin que se repitan
            List<Etapa> etapasTodas = expediente.Tramite.Etapas.ToList();
            List<Etapa> etapasExpediente = new List<Etapa>();
            foreach (EtapaCumplida ec in expediente.EtapasCumplidas)
            {
                etapasExpediente.Add(ec.Etapa);
            }
            var ee = from e in etapasTodas
                     where !etapasExpediente.Contains(e)
                     select e;
            List < Etapa > etapas = ee.ToList<Etapa>();

            // cargar funcionarios correspondientes
            List<Funcionario> funcTodos = db.Funcionarios.ToList<Funcionario>();
            var ff = from f in funcTodos
                     join g in expediente.Tramite.GruposAsignados
                     on f.MiGrupo equals g
                     select f;
            List<Funcionario> funcionarios = ff.ToList<Funcionario>();

            // pasar todo al viewmodel
            AgregarEtapaViewModel vm = new AgregarEtapaViewModel()
            {
                Expediente = expediente,
                Etapas = new SelectList(etapas, "CodigoAlfa", "Descripcion"),
                Funcionarios = new SelectList(funcionarios, "Mail", "Nombre")
            };
            return View(vm);
        }

        // POST Agregar Etapa
        [HttpPost]
        public ActionResult AgregarEtapa(AgregarEtapaViewModel vm)
        {
            DateTime hoy = DateTime.Today;

            using (obligP3parte2Context db = new obligP3parte2Context())
            {
                // cargar el exp y todo las cosas que fueron seleccionadas
                Expediente exp = db.Expedientes.Find(vm.Expediente.Codigo);
                Etapa etapa = db.Etapas.Find(vm.EtapaSeleccionada);
                Funcionario func = db.Funcionarios.Find(vm.FuncSeleccionado);
                
                if (exp == null || etapa == null || func == null || !vm.MapearArchivo())
                    return HttpNotFound();

                // si está todo bien cargado se agregar la etapa cumplida con sus cosas
                EtapaCumplida etapaAgregar = new EtapaCumplida() {
                    Etapa = etapa,
                    Expediente = exp,
                    FechaFin = hoy,
                    Funcionario = func,
                    Documento = vm.ArchivoNombre
                };
                etapaAgregar.SuperaLapsoMax = etapaAgregar.CalcularTiempoOk();

                exp.EtapasCumplidas.Add(etapaAgregar);
                exp.RevisarEtapasCumplidas();
                db.SaveChanges();

                // cargar expediente con todo para mandarlo a details
                Expediente expConTodo = db.Expedientes
                                            .Where(e => e.Codigo == vm.Expediente.Codigo)
                                            .Include(t => t.Tramite)
                                            .Include(e => e.EtapasCumplidas.Select(ee => ee.Funcionario))
                                            .Include(e => e.EtapasCumplidas.Select(eee => eee.Etapa))
                                            .FirstOrDefault();
                return View("Details", expConTodo);
            }
        }

        // Buscar menu
        public ActionResult Buscar()
        {
            if (Session["UsuarioMail"] == null || Session["UsuarioMail"].ToString() == "")
                return RedirectToAction("SinAcceso", "Home");

            return View();
        }

        // GET: Expediente
        public ActionResult ResultadoBusqueda(List<Expediente> expedientes)
        {
            if (Session["UsuarioMail"] == null || Session["UsuarioMail"].ToString() == "")
                return RedirectToAction("SinAcceso", "Home");

            return View();
        }
        
        // GET BuscarPorNumero
        [HttpGet]
        public ActionResult BuscarPorNumero()
        {
            if (Session["UsuarioMail"] == null || Session["UsuarioMail"].ToString() == "")
                return RedirectToAction("SinAcceso", "Home");

            return View();
        }

        // POST BuscarPorNumero
        [HttpPost]
        public ActionResult BuscarPorNumero(string numero)
        {
            var expedientes = from e in db.Expedientes
                              select e;
            if (!String.IsNullOrEmpty(numero))
            {
                int num = Int32.Parse(numero);
                expedientes = expedientes.Where(e => e.Codigo == num);
            }
            return View("ResultadoBusqueda", expedientes.ToList<Expediente>());
        }

        // GET BuscarPorSolicitante
        [HttpGet]
        public ActionResult BuscarPorSolicitante()
        {
            if (Session["UsuarioMail"] == null || Session["UsuarioMail"].ToString() == "")
                return RedirectToAction("SinAcceso", "Home");

            return View();
        }

        // POST BuscarPorSolicitante
        [HttpPost]
        public ActionResult BuscarPorSolicitante(string cedula)
        {
            var expedientes = from e in db.Expedientes
                              select e;
            if (!String.IsNullOrEmpty(cedula))
            {
                expedientes = expedientes.Where(e => e.Solicitante.Cedula == cedula);
            }
            return View("ResultadoBusqueda", expedientes.ToList<Expediente>());
        }

        // GET BuscarPorFuncionario
        [HttpGet]
        public ActionResult BuscarPorFuncionario()
        {
            if (Session["UsuarioMail"] == null || Session["UsuarioMail"].ToString() == "")
                return RedirectToAction("SinAcceso", "Home");

            return View();
        }

        // POST BuscarPorSolicitante
        [HttpPost]
        public ActionResult BuscarPorFuncionario(string mail)
        {
            var expedientes = from e in db.Expedientes
                              select e;
            if (!String.IsNullOrEmpty(mail))
            {
                expedientes = expedientes.Where(e => e.MailFunc == mail);
            }
            return View("ResultadoBusqueda", expedientes.ToList<Expediente>());
        }


        // VerCulminados
        public ActionResult VerCulminados()
        {
            if (Session["UsuarioMail"] == null || Session["UsuarioMail"].ToString() == "")
                return RedirectToAction("SinAcceso", "Home");

            var expedientes = db.Expedientes.Where(e => e.Culminado == true);
            return View("ResultadoBusqueda", expedientes.ToList<Expediente>());
        }

        // VerSinCulminar
        public ActionResult VerSinCulminar()
        {
            if (Session["UsuarioMail"] == null || Session["UsuarioMail"].ToString() == "")
                return RedirectToAction("SinAcceso", "Home");

            var expedientes = db.Expedientes.Where(e => e.Culminado == false);
            return View("ResultadoBusqueda", expedientes.ToList<Expediente>());
        }

        // VerTodos
        public ActionResult VerTodosOrdenados()
        {
            if (Session["UsuarioMail"] == null || Session["UsuarioMail"].ToString() == "")
                return RedirectToAction("SinAcceso", "Home");

            var expedientes = db.Expedientes
                                .OrderBy(e => e.Solicitante.Cedula)
                                .ThenByDescending(e => e.FechaInic);
            return View("ResultadoBusqueda", expedientes.ToList<Expediente>());
        }










    }
}
