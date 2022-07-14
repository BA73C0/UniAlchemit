using Pruebas2.Models.Clases;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Pruebas2.Models
{
    public class AlumnoViewModel
    {
        public AlumnoViewModel() { }


        [Required(ErrorMessage = "Debe cargar su Nombre")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "Debe cargar su Apellido")]
        public string Apellido { get; set; }

        [Required(ErrorMessage = "Debe cargar su Carrera")]
        public string IDcarrera { get; set; }


        public Alumno ToEntity(int ultimoAlu)
        {
            return new Alumno
            {
                Nombre = this.Nombre,
                Apellido = this.Apellido,
                IDcarrera = this.IDcarrera,
                ID = ultimoAlu + 1,
            };
        }
    }
}