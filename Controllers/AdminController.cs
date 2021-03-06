using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Pruebas2.Models;
using Pruebas2.Models.Clases;

namespace Pruebas2.Controllers
{
    public class AdminController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        [HttpGet]
        public ActionResult ExitoRegMateria(string nombre)
        {
            ViewBag.Message = nombre;

            return View();
        }

        [HttpGet]
        public ActionResult CargaMateria()
        {
            return View(new MateriaViewModel());
        }

        [HttpPost]
        public ActionResult CargaMateria(MateriaViewModel model)
        {
            ValidarCarrera(model);
            ValidarMateria(model);
            ValidarSemestre(model);

            if (ModelState.IsValid)
            {
                db.Materias.Add(new MateriaC { ID = model.ID, Materia = model.Materia, IDcarrera = model.IDcarrera, IDcorrelativa1 = model.IDcorrelativa1, IDcorrelativa2 = model.IDcorrelativa2, Semestre = model.Semestre });
                db.SaveChanges();

                return RedirectToAction("ExitoRegMateria", new { nombre = model.Materia });
            }
            return View(model);
        }

        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.Message = "Bienvenido/a Admin";

            return View(new CarreraViewModel());
        }

        [HttpPost]
        public ActionResult Index(CarreraViewModel model)
        {
            ValidarCarrera1(model);
            if (ModelState.IsValid)
            {
                var db = new ApplicationDbContext();

                db.Carreras.Add(new CarreraC { ID = model.ID, Carrera = model.Carrera });
                db.SaveChanges();

                return RedirectToAction("ExitoRegistroCarrera", new { nombre = model.Carrera });
            }
            return View(model);
        }

        [HttpGet]
        public ActionResult ExitoRegistroCarrera(string nombre)
        {
            ViewBag.Message = nombre;

            return View();
        }


        private void ValidarCarrera(MateriaViewModel model)
        {
            try
            {
                CarreraC carrera = db.Carreras.First(d => d.ID == model.IDcarrera);
            }
            catch
            {
                ModelState.AddModelError(nameof(model.IDcarrera), "La carrera no existe");
            }
        }
        private void ValidarCarrera1(CarreraViewModel model)
        {
            List<CarreraC> carreras = db.Carreras.ToList();

            foreach (CarreraC carrera in carreras)
            {
                if (model.ID == carrera.ID.Trim())
                {
                    ModelState.AddModelError(nameof(model.ID), "La carrera ya existe");
                    break;
                }
            }
        }
        private void ValidarMateria(MateriaViewModel model)
        {
            List<MateriaC> materias = db.Materias.ToList();

            foreach (MateriaC materia in materias)
            {
                if (model.ID == materia.ID.Trim())
                {
                    ModelState.AddModelError(nameof(model.ID), "La materia ya existe");
                    break;
                }
            }
        }
        private void ValidarSemestre(MateriaViewModel model)
        {
            if (model.Semestre > 2 && model.Semestre < 1) 
            {
                ModelState.AddModelError(nameof(model.Semestre), "Solamente hay dos semestres");
            }
        }
    }
}