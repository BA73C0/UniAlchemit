using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Pruebas2.Models;
using Pruebas2.Models.Clases;

namespace Pruebas2.Controllers
{
    public class ProfesorController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        [HttpGet]
        public ActionResult AsignarNota(Guid id)
        {
            NotaViewModel examen = new NotaViewModel { Id = id, Fecha = "00/00/0000", Nota = 0 };
            return View(examen);
        }

        [HttpPost]
        public ActionResult AsignarNota(NotaViewModel examen)
        {
            ValidarNota(examen);

            if (ModelState.IsValid)
            {
                Inscripcion inscripcion = db.Inscripcion.First(i => i.Id == examen.Id);
                inscripcion.Fecha = examen.Fecha;
                inscripcion.Nota = examen.Nota;
                db.SaveChanges();

                return RedirectToAction("VerExamenes", new { id = inscripcion.IDMateria.Trim() });
            }
            return View(examen);
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View(new MateriaLogInViewModel());
        }

        [HttpPost]
        public ActionResult Index(MateriaLogInViewModel materiaid)
        {
            ValidarMateria(materiaid);

            if (ModelState.IsValid)
            {
                return RedirectToAction("VerExamenes", new { id = materiaid.ID });
            }
            return View(materiaid);
        }

        [HttpGet]
        public ActionResult VerExamenes(string id)
        {
            List<Inscripcion> materias = new List<Inscripcion>();

            materias = db.Inscripcion.Where(i => i.IDMateria == id).ToList();

            List<ReporteViewModel> reporte = new List<ReporteViewModel>();

            if (materias.Count == 0)
            {
                List<ReporteViewModel> reporteVacio = new List<ReporteViewModel>();
                ReporteViewModel a = new ReporteViewModel
                {
                    IdAlumno = 0,
                    IDMateria = id,
                    Id = new Guid(),
                    Materia = "Ejemplo",
                    Nombre = "X",
                    Apellido = "X",
                    Semestre = 0,
                };
                reporteVacio.Add(a);

                return View(reporteVacio);
            }

            foreach (Inscripcion materia in materias)
            {
                ReporteViewModel reporteModel = new ReporteViewModel(materia)
                {
                    Materia = db.Materias.First(i => i.ID == materia.IDMateria).Materia,
                    Nombre = db.Alumnos.First(i => i.ID == materia.IdAlumno).Nombre,
                    Apellido = db.Alumnos.First(i => i.ID == materia.IdAlumno).Apellido
                };

                reporte.Add(reporteModel);
            }
            return View(reporte);
        }


        private void ValidarMateria(MateriaLogInViewModel model)
        {
            List<MateriaC> materias = db.Materias.Where(d => d.ID == model.ID).ToList();

            if (materias.Count() == 0)
            {
                ModelState.AddModelError(nameof(model.ID), "El ID no corresponde a una materia");
            }
        }
        private void ValidarNota(NotaViewModel model)
        {
            if (model.Nota < -1 && model.Nota > 10)
            {
                ModelState.AddModelError(nameof(model.Nota), "La nota solo puede tener valores entre -1 y 10");
            }
        }
    }
}