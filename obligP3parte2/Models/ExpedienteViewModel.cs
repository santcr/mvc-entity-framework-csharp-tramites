using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using obligP3parte2.Models.Dominio;

namespace obligP3parte2.Models
{
    public class ExpedienteViewModel
    {
        public virtual Solicitante Solicitante { get; set; }
        
        public virtual Tramite Tramite { get; set; }
        
        public string MailFunc { get; set; }
        
        public DateTime FechaInic { get; set; }
        
        public int Id { get; set; }
        
        public virtual ICollection<EtapaCumplida> EtapasCumplidas { get; set; }
        
        public virtual Tramite MiTramite { get; set; }






    }
}