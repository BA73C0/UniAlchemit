using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using Pruebas2.Models;
using Pruebas2.Models.Clases;

namespace Pruebas2.Controllers
{
    public class AlumnoController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        [HttpGet]
        public ActionResult BajaCarrera(int id)
        {
            List<Inscripcion> inscripciones = db.Inscripcion.Where(i => i.IdAlumno == id).ToList();

            foreach (Inscripcion inscripcion in inscripciones)
            {
                db.Inscripcion.Remove(inscripcion);
            }

            Alumno alumno = db.Alumnos.First(i => i.ID == id);

            db.Alumnos.Remove(alumno);
            db.SaveChanges();

            return View();
        }

        [HttpGet]
        public ActionResult MateriasRendidas(int id)
        {
            List<Inscripcion> materias = new List<Inscripcion>();

            materias = db.Inscripcion.Where(i => i.IdAlumno == id && i.Nota > 0).ToList();

            List<ReporteViewModel> reporte = new List<ReporteViewModel>();

            if (materias.Count == 0)
            {
                List<ReporteViewModel> reporteVacio = new List<ReporteViewModel>();
                ReporteViewModel a = new ReporteViewModel
                {
                    IdAlumno = id,
                    IDMateria = "X01",
                    Id = new Guid(),
                    Semestre = 0,
                    Fecha = " ",
                    Nota = 4,
                    Materia = "Ejemplo"
                };
                reporteVacio.Add(a);

                return View(reporteVacio);
            }

            foreach (Inscripcion materia in materias)
            {
                ReporteViewModel reporteModel = new ReporteViewModel(materia)
                {
                    Materia = db.Materias.First(i => i.ID == materia.IDMateria).Materia
                };
                reporte.Add(reporteModel);
            }
            return View(reporte);
        }

        [HttpGet]
        public ActionResult MateriasAprobadas(int id)
        {
            List<Inscripcion> materias = new List<Inscripcion>();

            materias = db.Inscripcion.Where(i => i.IdAlumno == id && i.Nota > 3).ToList();

            List<ReporteViewModel> reporte = new List<ReporteViewModel>();

            if (materias.Count == 0)
            {
                List<ReporteViewModel> reporteVacio = new List<ReporteViewModel>();
                ReporteViewModel a = new ReporteViewModel
                {
                    IdAlumno = id,
                    IDMateria = "X01",
                    Id = new Guid(),
                    Semestre = 0,
                    Fecha = " ",
                    Nota = 4,
                    Materia = "Ejemplo"
                };
                reporteVacio.Add(a);

                return View(reporteVacio);
            }

            foreach (Inscripcion materia in materias)
            {
                ReporteViewModel reporteModel = new ReporteViewModel(materia)
                {
                    Materia = db.Materias.First(i => i.ID == materia.IDMateria).Materia
                };
                reporte.Add(reporteModel);
            }
            return View(reporte);
        }

        [HttpGet]
        public ActionResult CancelarIns(Guid id)
        {
            Inscripcion inscripcion = db.Inscripcion.First(i => i.Id == id);
            db.Inscripcion.Remove(inscripcion);
            db.SaveChanges();

            return RedirectToAction("MateriasAlu", new { id = inscripcion.IdAlumno });
        }

        [HttpGet]
        public ActionResult NuevaInscripcion(int id)
        {
            InscripcionViewModel model = new InscripcionViewModel { IdAlumno = id };

            return View(model);
        }

        [HttpPost]
        public ActionResult NuevaInscripcion(InscripcionViewModel model)
        {
            ValidarYaInscripto(model);
            ValidarMateriaExiste(model);
            Validar7materias(model);
            ValidarCorrelativas(model);
            ValidarMateriaConCarrera(model);

            if (ModelState.IsValid)
            {
                db.Inscripcion.Add(model.AgregarId());
                db.SaveChanges();
                return RedirectToAction("MateriasAlu", new { id = model.IdAlumno });
            }
            return View(model);
        }

        [HttpGet]
        public ActionResult LogIn()
        {
            return View(new LogInViewModel());
        }

        [HttpPost]
        public ActionResult LogIn(LogInViewModel logIn)
        {
            ValidarInscripcion(logIn);
            if (ModelState.IsValid)
            {
                return RedirectToAction("NuevaInscripcion", new { id = logIn.Id });
            }
            return View(logIn);
        }

        [HttpGet]
        public ActionResult MateriasAlu(int id)
        {
            List<Inscripcion> materias = new List<Inscripcion>();
            List<ReporteViewModel> reporte = new List<ReporteViewModel>();

            materias = db.Inscripcion.Where(i => i.IdAlumno == id).ToList();

            if (materias.Count == 0)
            {
                List<ReporteViewModel> reporteVacio = new List<ReporteViewModel>();
                ReporteViewModel a = new ReporteViewModel
                {
                    IdAlumno = id,
                    IDMateria = "X01",
                    Id = new Guid(),
                    Semestre = 0,
                    Fecha = " ",
                    Nota = 4,
                    Materia = "Ejemplo"
                };
                reporteVacio.Add(a);

                return View(reporteVacio);
            }

            foreach (Inscripcion materia in materias)
            {
                ReporteViewModel reporteModel = new ReporteViewModel(materia);

                reporteModel.Materia = db.Materias.First(i => i.ID == materia.IDMateria).Materia;
                reporte.Add(reporteModel);
            }
            return View(reporte);
        }

        [HttpGet]
        public ActionResult CrearAlu()
        {

            ViewBag.Message = "¿Estás registrado/a?";

            return View(new AlumnoViewModel());
        }

        [HttpPost]
        public ActionResult CrearAlu(AlumnoViewModel model)
        {
            if (ModelState.IsValid)
            {
                var db = new ApplicationDbContext();

                List<Alumno> alumno = db.Alumnos.ToList();

                int ultimoID = alumno.Count() + 1;

                Alumno a = model.ToEntity(ultimoID);

                db.Alumnos.Add(a);
                db.SaveChanges();

                return RedirectToAction("CreacionAluExitosa", new { id = a.ID });
            }
            return View(model);
        }

        [HttpGet]
        public ActionResult CreacionAluExitosa(int id)
        {
            ViewBag.Message = id;

            return View();
        }

        #region Validaciones

        private void ValidarMateriaConCarrera(InscripcionViewModel model)
        {
            bool materiacorrecta = false;

            Alumno alumno = db.Alumnos.First(d => d.ID == model.IdAlumno);

            MateriaC materia = db.Materias.First(d => d.ID == model.IDMateria);

            if(alumno.IDcarrera == materia.IDcarrera)
            {
                materiacorrecta = true;
            }
            if (materiacorrecta == false)
            {
                ModelState.AddModelError(nameof(model.IDMateria), "Esta materia no corresponde a tu carrera");
            }
        }
        private void ValidarInscripcion(LogInViewModel model)
        {
            bool alue = true;

            List<Alumno> alumno = db.Alumnos.Where(d => d.ID == model.Id).ToList();

            if (alumno.Count() == 0)
            {
                alue = false;
            }

            if (alue == false)
            {
                ModelState.AddModelError(nameof(model.Id), "El ID no corresponde a un alumno registrado/a");
            }
        }
        private void ValidarYaInscripto(InscripcionViewModel model)
        {
            bool materiaNoExiste = false;

            List<Inscripcion> inscripciones = db.Inscripcion.Where(d => d.IdAlumno == model.IdAlumno).ToList();

            foreach (Inscripcion inscripcion in inscripciones)
            {
                if (model.IDMateria == inscripcion.IDMateria.Trim() && (inscripcion.Nota > 3 || inscripcion.Nota == 0))
                {
                    materiaNoExiste = true;
                }
            }
            if (materiaNoExiste == true)
            {
                ModelState.AddModelError(nameof(model.IDMateria), "La inscripciona esta Materia ya existe");
            }
        }
        private void ValidarMateriaExiste(InscripcionViewModel model)
        {
            bool materiaExiste = false;
            List<MateriaC> materias = db.Materias.ToList();
            foreach (MateriaC materia in materias)
            {
                if (model.IDMateria == materia.ID.Trim())
                {
                    materiaExiste = true;
                }
            }
            if (materiaExiste == false)
            {
                ModelState.AddModelError(nameof(model.IDMateria), "La Materia no existe");
            }
        }
        private void Validar7materias(InscripcionViewModel model)
        {
            int materia7 = 0;
            
            List<Inscripcion> inscripciones = db.Inscripcion.Where(d => d.IdAlumno == model.IdAlumno).ToList();
            int semestre = db.Materias.First(i => i.ID == model.IDMateria).Semestre;

            foreach (Inscripcion inscripcion in inscripciones)
            {
                int semestreIns = db.Materias.First(i => i.ID == inscripcion.IDMateria).Semestre;
                if (inscripcion.Nota == 0 && semestre == semestreIns)
                {
                    materia7++;
                }
            }
            if (materia7 >= 7)
            {
                ModelState.AddModelError(nameof(model.IDMateria), "El número máximo de inscripciones por semestre es siete.");
            }
        }
        private bool ValidarCorrelativa1(InscripcionViewModel model)
        {
            bool correlativaAprovada1 = false;

            MateriaC materia = db.Materias.First(i => i.ID == model.IDMateria);

            if (materia.IDcorrelativa1 != null)
            {
                MateriaC correlativa1 = db.Materias.First(i => i.ID == materia.IDcorrelativa1);

                List<Inscripcion> inscripciones = db.Inscripcion.Where(d => d.IdAlumno == model.IdAlumno).ToList();

                foreach (Inscripcion inscripcion in inscripciones)
                {
                    if (inscripcion.IDMateria == correlativa1.ID && inscripcion.Nota > 3)
                    {
                        correlativaAprovada1 = true;
                    }
                }
                return correlativaAprovada1;
            }
            return correlativaAprovada1;
        }
        private bool ValidarCorrelativa2(InscripcionViewModel model)
        {
            bool correlativaAprovada2 = false;

            MateriaC materia = db.Materias.First(i => i.ID == model.IDMateria);

            if (materia.IDcorrelativa2 != null)
            {
                MateriaC correlativa2 = db.Materias.First(i => i.ID == materia.IDcorrelativa2);

                List<Inscripcion> inscripciones = db.Inscripcion.Where(d => d.IdAlumno == model.IdAlumno).ToList();

                foreach (Inscripcion inscripcion in inscripciones)
                {
                    if (inscripcion.IDMateria == correlativa2.ID && inscripcion.Nota > 3)
                    {
                        correlativaAprovada2 = true;
                    }
                }
                return correlativaAprovada2;
            }
            return correlativaAprovada2;
        }
        public void ValidarCorrelativas(InscripcionViewModel model)
        {
            MateriaC materia = db.Materias.First(i => i.ID == model.IDMateria);

            if (materia.IDcorrelativa1 != null && materia.IDcorrelativa2 != null && ValidarCorrelativa1(model) == false && ValidarCorrelativa2(model) == false)
            {
                MateriaC correlativa2 = db.Materias.First(i => i.ID == materia.IDcorrelativa2);
                MateriaC correlativa1 = db.Materias.First(i => i.ID == materia.IDcorrelativa1);
                ModelState.AddModelError(nameof(model.IDMateria), $"Falta aprobar {correlativa1.Materia} y {correlativa2.Materia}");
            }

            else if (materia.IDcorrelativa1 != null && ValidarCorrelativa1(model) == false)
            {
                MateriaC correlativa1 = db.Materias.First(i => i.ID == materia.IDcorrelativa1);
                ModelState.AddModelError(nameof(model.IDMateria), $"Falta aprobar {correlativa1.Materia}");
            }

            else if (materia.IDcorrelativa2 != null && ValidarCorrelativa2(model) == false)
            {
                MateriaC correlativa2 = db.Materias.First(i => i.ID == materia.IDcorrelativa2);
                ModelState.AddModelError(nameof(model.IDMateria), $"Falta aprobar {correlativa2.Materia}");
            }
        }

        #endregion
    }
}