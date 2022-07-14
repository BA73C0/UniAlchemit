using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Pruebas2.Models
{
    public class NotaViewModel
    {
        [Key]
        public Guid Id { get; set; }

        public string Fecha { get; set; }

        public int Nota { get; set; }
    }
}