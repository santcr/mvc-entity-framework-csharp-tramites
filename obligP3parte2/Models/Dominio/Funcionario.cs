using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace obligP3parte2.Models.Dominio
{
    [Table("Funcionarios")]
    public class Funcionario
    {
        [Key]
        [EmailAddress]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Mail { get; set; }


        [DataType(DataType.Password)]
        [Required(ErrorMessage = "El campo es requerido")]
        public string Password { get; set; }

        
        [Required(ErrorMessage = "El campo es requerido")]
        public string Nombre { get; set; }

        
        public virtual Grupo MiGrupo { get; set; }





    }
}