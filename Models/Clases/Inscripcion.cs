using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Pruebas2.Models.Clases
{
    [Table("Inscripcion")]
    public class Inscripcion
    {
        [Key]
        public Guid Id { get; set; }

        public int IdAlumno { get; set; }

        public string IDMateria { get; set; }

        public string Fecha { get; set; }

        public int Nota { get; set; }
    }
}