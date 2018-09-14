using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc;
using obligP3parte2.Models.Dominio;
using System.ComponentModel;

namespace obligP3parte2.Models
{
    public class AgregarEtapaViewModel
    {
        public Expediente Expediente { get; set; }

        public SelectList Etapas { get; set; }

        public string EtapaSeleccionada { get; set; }

        public SelectList Funcionarios { get; set; }

        public string FuncSeleccionado { get; set; }

        public HttpPostedFileBase Archivo { get; set; }

        public string ArchivoNombre { get; set; }


        public bool MapearArchivo()
        {
            if (this.Archivo != null)
            {
                if (GuardarArchivo(Archivo))
                {
                    this.ArchivoNombre = Archivo.FileName;
                    return true;
                }
            }
            return false;
        }
        private bool GuardarArchivo(HttpPostedFileBase archivo)
        {
            if (archivo != null)
            {
                string ruta = System.IO.Path.Combine
                (AppDomain.CurrentDomain.BaseDirectory, "docs");
                if (!System.IO.Directory.Exists(ruta))
                    System.IO.Directory.CreateDirectory(ruta);

                ruta = System.IO.Path.Combine(ruta, archivo.FileName);
                archivo.SaveAs(ruta);
                return true;
            }
            else
                return false;
        }







    }
}