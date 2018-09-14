using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace obligP3parte2.Models.Dominio
{
    [Table("Tramites")]
    public class Tramite
    {
        [Key]
        [DisplayName("Código")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Codigo { get; set; }


        [DisplayName("Título")]
        [StringLength(50, MinimumLength = 4)]
        [Required(ErrorMessage = "El campo es requerido")]
        public string Titulo { get; set; }


        [DisplayName("Descripción")]
        [StringLength(200, MinimumLength = 4)]
        [Required(ErrorMessage = "El campo es requerido")]
        public string Descripcion { get; set; }


        [Required(ErrorMessage = "El campo es requerido")]
        [Range(1, double.MaxValue, ErrorMessage = "El numero debe ser positivo")]
        public double Costo { get; set; }


        [DisplayName("Tiempo previsto")]
        [Required(ErrorMessage = "El campo es requerido")]
        [Range(1, int.MaxValue, ErrorMessage = "El numero debe ser positivo")]
        public int TiempoPrevisto { get; set; }


        [DisplayName("Grupos asignados")]
        public virtual ICollection<Grupo> GruposAsignados { get; set; }
        

        public virtual ICollection<Etapa> Etapas { get; set; }





    }
}