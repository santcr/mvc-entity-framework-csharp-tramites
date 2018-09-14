using obligP3parte2.Models.Dominio;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Data.Entity;

namespace obligP3parte2.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CargarDatos()
        {
            CargarUsuarios();

            ViewBag.DatosCargados += "<strong> Grupos </strong> <br>";
            CargarGrupos();

            ViewBag.DatosCargados += "<br> <strong> Funcionarios </strong> <br>";
            CargarFuncionarios();

            ViewBag.DatosCargados += "<br> <strong> Tramites </strong> <br>";
            CargarTramites();

            CargarGrupoTramites();

            CargarTramitesEtapas();
            
            CargarSolicitantesExpedientes();
            
            return View();
        }

        private void CargarGrupos()
        {
            var db = new obligP3parte2Context();
            string ruta = HostingEnvironment.ApplicationPhysicalPath + @"archivos\grupos.txt";
            StreamReader sr = System.IO.File.OpenText(ruta);
            string line = sr.ReadLine();
            while (line != null)
            {
                string[] vecStr = line.Split(new char[] { '#' }, StringSplitOptions.RemoveEmptyEntries);
                Grupo g = ParsearGrupo(vecStr);
                var grupoDb = db.Grupos.Find(g.Codigo);
                if (g != null && grupoDb == null)
                {
                    db.Grupos.Add(g);
                    ViewBag.DatosCargados += "- " + g.Nombre + "<br>";
                }
                line = sr.ReadLine();
            }
            db.SaveChanges();
            sr.Close();
        }

        private Grupo ParsearGrupo(string[] vecStr)
        {
            try
            {
                int codigo = int.Parse(vecStr[0]);
                string nombre = vecStr[1];
                Grupo g = new Grupo() { Nombre = nombre, Codigo = codigo };
                return g;
            }
            catch (FormatException fe)
            {
                return null;
            }
        }

        private void CargarFuncionarios()
        {
            var db = new obligP3parte2Context();
            string ruta = HostingEnvironment.ApplicationPhysicalPath + @"archivos\funcionarios.txt";
            StreamReader sr = System.IO.File.OpenText(ruta);
            string line = sr.ReadLine();
            while (line != null)
            {
                string[] vecStr = line.Split(new char[] { '#' }, StringSplitOptions.RemoveEmptyEntries);
                Funcionario f = ParsearFuncionarios(vecStr);

                f.MiGrupo = db.Grupos.Find(f.MiGrupo.Codigo);

                var funcDb = db.Funcionarios.Find(f.Mail);
                if (f != null && funcDb == null)
                {
                    db.Entry(f.MiGrupo).State = System.Data.Entity.EntityState.Unchanged;
                    db.Funcionarios.Add(f);
                    ViewBag.DatosCargados += "- " + f.Mail + "<br>";
                }
                line = sr.ReadLine();
            }
            db.SaveChanges();
            sr.Close();
        }

        private Funcionario ParsearFuncionarios(string[] vecStr)
        {
            try
            {
                string mail = vecStr[0];
                string nombre = vecStr[1];
                int grupo = int.Parse(vecStr[2]);
                var db = new obligP3parte2Context();
                string pass = vecStr[3];
                Grupo g = new Grupo() { Codigo = grupo };
                Funcionario f = new Funcionario() { Mail = mail, Nombre = nombre, MiGrupo = g, Password = pass };
                return f;
            }
            catch (FormatException fe)
            {
                return null;
            }
        }

        private void CargarTramites()
        {
            var db = new obligP3parte2Context();
            string ruta = HostingEnvironment.ApplicationPhysicalPath + @"archivos\tramites.txt";
            StreamReader sr = System.IO.File.OpenText(ruta);
            string line = sr.ReadLine();
            while (line != null)
            {
                string[] vecStr = line.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                Tramite t = new Tramite() {
                    Codigo = int.Parse(vecStr[0]),
                    Titulo = vecStr[1],
                    Descripcion = vecStr[2],
                    Costo = double.Parse(vecStr[3]),
                    TiempoPrevisto = int.Parse(vecStr[4])
                };
                var tramiteDb = db.Tramites.Find(t.Codigo);
                if (t != null && tramiteDb == null)
                {
                    db.Tramites.Add(t);
                    ViewBag.DatosCargados += "- " + t.Titulo + "<br>";
                }
                line = sr.ReadLine();
            }
            db.SaveChanges();
            sr.Close();
        }

        private void CargarGrupoTramites()
        {
            var db = new obligP3parte2Context();
            string ruta = HostingEnvironment.ApplicationPhysicalPath + @"archivos\tramitesGrupos.txt";
            StreamReader sr = System.IO.File.OpenText(ruta);
            string line = sr.ReadLine();
            while (line != null)
            {
                string[] vecStr = line.Split(new char[] { '$' }, StringSplitOptions.RemoveEmptyEntries);

                Tramite t = db.Tramites.Find(int.Parse(vecStr[0]));
                Grupo g = db.Grupos.Find(int.Parse(vecStr[1]));

                if (t != null && g != null && !t.GruposAsignados.Contains(g))
                {
                    t.GruposAsignados.Add(g);
                    db.Entry(t.GruposAsignados.ElementAt(t.GruposAsignados.Count()-1)).State = System.Data.Entity.EntityState.Unchanged;
                    ViewBag.DatosCargados += "- tramite " + t.Titulo + " asignado al grupo " + g.Nombre + "<br>";
                }
                line = sr.ReadLine();
            }
            db.SaveChanges();
            sr.Close();
        }

        private void CargarTramitesEtapas()
        {
            var db = new obligP3parte2Context();
            string ruta = HostingEnvironment.ApplicationPhysicalPath + @"archivos\tramitesEtapas.txt";
            StreamReader sr = System.IO.File.OpenText(ruta);
            string line = sr.ReadLine();
            while (line != null)
            {
                string[] vecStr = line.Split(new char[] { '@' }, StringSplitOptions.RemoveEmptyEntries);

                Tramite t = db.Tramites.Find(int.Parse(vecStr[0]));

                Etapa e = new Etapa() {
                    CodigoAlfa = vecStr[1],
                    Descripcion = vecStr[2],
                    TiempoMax = int.Parse(vecStr[3])
                };
                var etapaDb = db.Etapas.Find(e.CodigoAlfa);
                if (t != null && etapaDb == null && !t.Etapas.Contains(etapaDb))
                {
                    t.Etapas.Add(e);
                    ViewBag.DatosCargados += "- tramite " + t.Titulo + " asignada etapa " + e.CodigoAlfa + "<br>";
                }
                line = sr.ReadLine();
            }
            db.SaveChanges();
            sr.Close();
        }

        private void CargarUsuarios()
        {
            var db = new obligP3parte2Context();

            if (db.Usuarios.Any()) return;

            Usuario u = new Usuario() { Mail = "func1@mail.com", Password = "123" };
            if (db.Usuarios.Find(u.Mail) == null) db.Usuarios.Add(u);
            u = new Usuario() { Mail = "func2@mail.com", Password = "123" };
            if (db.Usuarios.Find(u.Mail) == null) db.Usuarios.Add(u);
            u = new Usuario() { Mail = "func3@mail.com", Password = "123" };
            if (db.Usuarios.Find(u.Mail) == null) db.Usuarios.Add(u);
            u = new Usuario() { Mail = "func4@mail.com", Password = "123" };
            if (db.Usuarios.Find(u.Mail) == null) db.Usuarios.Add(u);
            u = new Usuario() { Mail = "func5@mail.com", Password = "123" };
            if (db.Usuarios.Find(u.Mail) == null) db.Usuarios.Add(u);

            db.SaveChanges();
        }

        private void CargarSolicitantesExpedientes()
        {
            var db = new obligP3parte2Context();

            if (db.Solicitantes.Any()) return;

            string comando = @"
                USE[ObligP3parte2]
                ";
            db.Database.ExecuteSqlCommand(comando);

            comando = @"
                INSERT[dbo].[Solicitantes] ([Cedula], [Nombre], [Apellido], [Tel], [Mail]) VALUES
                (N'1234', N'Lolo', N'Fernandes', N'654544', N'lololo@mail.com'),
                (N'123456', N'Pepita', N'Lopez', N'7654564', N'pepilo@mail.com'),
                (N'3333', N'Carlos', N'Pepito', N'123456789', N'carpepito@mail.com')
                ";
            db.Database.ExecuteSqlCommand(comando);

            comando = @"
                SET IDENTITY_INSERT[dbo].[Expedientes] ON
                INSERT[dbo].[Expedientes] ([Codigo], [idSolicitante], [MailFunc], [FechaInic], [Culminado], [Tramite_Codigo]) VALUES
                (1, N'3333', N'func1@mail.com', CAST(N'2018-06-27 00:00:00.000' AS DateTime), 1, 1),
                (2, N'1234', N'func2@mail.com', CAST(N'2018-06-27 00:00:00.000' AS DateTime), 1, 3),
                (3, N'123456', N'func3@mail.com', CAST(N'2018-06-27 00:00:00.000' AS DateTime), 0, 2),
                (4, N'3333', N'func1@mail.com', CAST(N'2018-06-27 00:00:00.000' AS DateTime), 0, 2),
                (5, N'1234', N'func1@mail.com', CAST(N'2018-06-27 00:00:00.000' AS DateTime), 0, 1)
                ";
            db.Database.ExecuteSqlCommand(comando);

            comando = @"
                SET IDENTITY_INSERT[dbo].[EtapasCumplidas] ON
                INSERT[dbo].[EtapasCumplidas] ([Id], [Documento], [FechaFin], [SuperaLapsoMax], [Etapa_CodigoAlfa], [Expediente_Codigo], [Funcionario_Mail]) VALUES
                (1, N'339px-Akt_mianowania_ppłk._Kazimierza_Makarewicza_na_stopień_pułkownika.jpg', CAST(N'2018-06-27 00:00:00.000' AS DateTime), 0, N'T1E01', 1, N'camilodiaz@mail.com'),
                (2, N'23938868.png', CAST(N'2018-06-27 00:00:00.000' AS DateTime), 0, N'T1E02', 1, N'camilodiaz@mail.com'),
                (3, N'a_german_eagle_o_55c5132ea2151.jpg', CAST(N'2018-06-27 00:00:00.000' AS DateTime), 0, N'T1E03', 1, N'admin@mail.com'),
                (4, N'flakdoc.jpg', CAST(N'2018-06-27 00:00:00.000' AS DateTime), 0, N'T3E01', 2, N'eu@sodaleselit.com'),
                (5, N'form427-giant.gif', CAST(N'2018-06-27 00:00:00.000' AS DateTime), 0, N'T3E03', 2, N'erat@et.edu'),
                (6, N'french war documents 001.jpg', CAST(N'2018-06-27 00:00:00.000' AS DateTime), 0, N'T3E02', 2, N'aliquet@elitpellentesque.net'),
                (7, N'french war documents 001.jpg', CAST(N'2018-06-27 00:00:00.000' AS DateTime), 0, N'T3E04', 2, N'sed.pede@Duisgravida.co.uk')
                ";
            db.Database.ExecuteSqlCommand(comando);
        }

        public ActionResult SinAcceso()
        {
            return View();
        }






    }
}