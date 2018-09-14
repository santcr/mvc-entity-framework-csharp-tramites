using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace obligP3parte2.Models.Dominio
{
    [Table("Etapas")]
    public class Etapa
    {
        [DisplayName("Tiempo máximo")]
        [Required(ErrorMessage = "El campo es requerido")]
        [Range(1, int.MaxValue, ErrorMessage = "El numero debe ser positivo")]
        public int TiempoMax { get; set; }

        
        [DisplayName("Descripción")]
        [StringLength(200, MinimumLength = 4)]
        [Required(ErrorMessage = "El campo es requerido")]
        public string Descripcion { get; set; }
        
        [Key]
        [DisplayName("Código alfanumerico")]
        [StringLength(50, MinimumLength = 4)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string CodigoAlfa { get; set; }

        public virtual Tramite MiTramite { get; set; }

    }
}