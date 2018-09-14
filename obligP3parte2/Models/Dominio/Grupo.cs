using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace obligP3parte2.Models.Dominio
{
    [Table("Grupos")]
    public class Grupo
    {
        [StringLength(50, MinimumLength = 4)]
        [Required(ErrorMessage = "El campo es requerido")]
        public string Nombre { get; set; }


        [Key]
        [DisplayName("Código")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Codigo { get; set; }


        public virtual ICollection<Tramite> Tramites { get; set; }

        


    }
}