using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using obligP3parte2.Models.Dominio;
using System.ComponentModel;

namespace obligP3parte2.Models
{
    public class CrearExpedienteViewModel
    {
        public Expediente Expediente { get; set; }


        [DisplayName("Seleccione un trámite")]
        public SelectList Tramites { get; set; }


        public int TramiteSeleccionado { get; set; }


        [DisplayName("Cédula del solicitante")]
        public string CedulaSolicitante { get; set; }


        public Solicitante Solicitante { get; set; }





    }
}