using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace obligP3parte2.Models
{
    public class LoginModel
    {
        [EmailAddress]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Correo electrónico")]


        public string EmailLogin { get; set; }
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]

        public string PasswordLogin { get; set; }



    }
}