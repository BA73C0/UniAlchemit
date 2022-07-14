﻿using Pruebas2.Models.Clases;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Pruebas2.Models
{
    public class InscripcionViewModel
    {
        public InscripcionViewModel() { }
        public InscripcionViewModel(Inscripcion inscripcion)
        {
            this.IdAlumno = inscripcion.IdAlumno;
            this.IDMateria = inscripcion.IDMateria;
            this.Fecha = inscripcion.Fecha;
            this.Nota = inscripcion.Nota;
        }


        [Required(ErrorMessage = "Debe cargar su IdAlumno")]
        public int IdAlumno { get; set; }

        [Required(ErrorMessage = "Debe cargar Id de la Materia")]
        public string IDMateria { get; set; }

        public string Fecha { get; set; }

        public int Nota { get; set; }


        public Inscripcion AgregarId()
        {
            return new Inscripcion
            {
                IdAlumno = this.IdAlumno,
                IDMateria = this.IDMateria,
                Id = Guid.NewGuid(),
            };
        }
    }
}