using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace obligP3parte2.Models.Dominio
{
    [Table("EtapasCumplidas")]
    public class EtapaCumplida
    {
        public virtual Etapa Etapa { get; set; }

        
        public virtual Expediente Expediente { get; set; }

        
        public Funcionario Funcionario { get; set; }

        
        public string Documento { get; set; }


        [DisplayName("Fecha de fin")]
        public DateTime FechaFin { get; set; }


        [DisplayName("Supera el lapso")]
        public bool SuperaLapsoMax { get; set; }


        [Key]
        public int Id { get; set; }


        public bool CalcularTiempoOk()
        {
            bool supera = false;
            if (this.FechaFin > this.Expediente.FechaInic.AddDays(this.Etapa.TiempoMax) )
            {
                supera = true;
            }
            return supera;
        }




    }
}