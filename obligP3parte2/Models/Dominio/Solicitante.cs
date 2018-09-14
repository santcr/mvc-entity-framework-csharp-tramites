using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace obligP3parte2.Models.Dominio
{
    [Table("Solicitantes")]
    public class Solicitante
    {
        [Key]
        [DisplayName("Cédula de identidad")]
        [StringLength(15, MinimumLength = 4)]
        public string Cedula { get; set; }


        [StringLength(50, MinimumLength = 4)]
        [Required(ErrorMessage = "El campo es requerido")]
        public string Nombre { get; set; }


        [StringLength(50, MinimumLength = 4)]
        [Required(ErrorMessage = "El campo es requerido")]
        public string Apellido { get; set; }


        [DisplayName("Teléfono")]
        [StringLength(50, MinimumLength = 4)]
        [Required(ErrorMessage = "El campo es requerido")]
        public string Tel { get; set; }


        [StringLength(50, MinimumLength = 4)]
        [Required(ErrorMessage = "El campo es requerido")]
        public string Mail { get; set; }





    }
}