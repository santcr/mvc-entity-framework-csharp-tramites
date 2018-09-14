using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace obligP3parte2.Models.Dominio
{
    [Table("Expedientes")]
    public class Expediente
    {
        [ForeignKey("Solicitante")]
        [DisplayName("Cédula del solicitante")]
        public string idSolicitante { get; set; }
        public virtual Solicitante Solicitante { get; set; }


        [DisplayName("Trámite")]
        [Required(ErrorMessage = "El campo es requerido")]
        public virtual Tramite Tramite { get; set; }


        [DisplayName("Mail del funcionario")]
        [StringLength(50, MinimumLength = 4)]
        public string MailFunc { get; set; }


        [DisplayName("Fecha de inicio")]
        [Required(ErrorMessage = "El campo es requerido")]
        public DateTime FechaInic { get; set; }


        [Key]
        [DisplayName("Código")]
        public int Codigo { get; set; }


        [DisplayName("Etapas cumplidas")]
        public virtual ICollection<EtapaCumplida> EtapasCumplidas { get; set; }


        public bool Culminado { get; set; }


        public bool RevisarEtapasCumplidas()
        {
            if (this.Tramite.Etapas.Count() == this.EtapasCumplidas.Count())
            {
                this.Culminado = true;
                return true;
            }
            else
                return false;
        }






    }
}