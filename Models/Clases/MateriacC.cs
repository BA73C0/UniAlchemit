﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Pruebas2.Models.Clases
{
    [Table("Materias")]
    public class MateriaC
    {
        [Key]
        public string ID { get; set; }

        public string Materia { get; set; }

        public string IDcarrera { get; set; }

        public string IDcorrelativa1 { get; set; }

        public string IDcorrelativa2 { get; set; }

        public int Semestre { get; set; }
    }
}