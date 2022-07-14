using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Pruebas2.Models
{
    public class LogInViewModel
    {
        public LogInViewModel() { }


        [Required(ErrorMessage = "Debe cargar su Id de estudiante")]
        public int Id { get; set; }
    }
}