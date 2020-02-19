using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PWEB_TP.Models;
using Microsoft.AspNet.Identity;

namespace PWEB_TP.Controllers
{
    public class EstagiosController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: EstagiosByUser
        [Authorize(Roles = "Docente,Administrador,Empresa")]
        public ActionResult Index()
        {
            var Id = User.Identity.GetUserId();

            var List = db.Estagio
                .Where(a => a.User_Id == Id)
                .ToList();
            return View(List);
        }

        public ActionResult TodosEstagios(string option, string search, int? pageNumber)
        {
            if (search != "")
            {
                if (option == "Regiao")
                {
                    //Index action method will return a view with a student records based on what a user specify the value in textbox  
                    return View(db.Estagio.Where(x => x.Local.Contains(search) || search == null).ToList());
                }
                else if (option == "Ramo")
                {
                    return View(db.Estagio.Where(x => x.Ramo.ToString().Contains(search) || search == null).ToList());
                }
                else if (option == "Contactos")
                {
                    return View(db.Estagio.Where(x => x.Contacto.ToString().Contains(search) || search == null).ToList());
                }

            }
            return View(db.Estagio.ToList());
        }

        // GET: Candidaturas from Estagio
        [Authorize(Roles = "Docente,Administrador,Empresa")]
        public ActionResult CandidaturasDoEstagio(int? id)
        {
            var candidaturas = db.Candidatura
                .Include(q => q.ApplicationUser)
                .Where(q => q.EstagioId == id)
                .ToList();
            List<SelectListItem> list = new List<SelectListItem>();
            var count = candidaturas.Count();
            for (var lista = 1; lista <= 5; lista++)
                list.Add(new SelectListItem() { Value = lista.ToString(), Text = lista.ToString() });
            ViewBag.Ordenacao = list;
            return View(candidaturas);
        }

        [HttpPost, ActionName("CandidaturasDoEstagio")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Docente,Administrador,Empresa")]
        public ActionResult CandidaturasDoEstagioImportancia(List<Candidatura> list)
        {
            if (ModelState.IsValid)
            {
                    foreach (var candidadatura in list)
                    {
                        var candidaturadb = db.Candidatura.Find(candidadatura.CandidaturaId);
                        candidaturadb.Importancia = candidadatura.Importancia;
                        db.Entry(candidaturadb).State = EntityState.Modified;
                        db.SaveChanges();
                    }
            
            }
            return RedirectToAction("CandidaturasDoEstagio");
        }
        // GET: Candidaturas from Estagio
        [Authorize(Roles = "Docente,Administrador,Empresa")]
        public ActionResult FinalizarEstagio(int? id)
        {
            var estagio = db.Estagio.Find(id);
            return View(estagio);
        }

        [HttpPost, ActionName("FinalizarEstagio")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Docente,Administrador,Empresa")]
        public ActionResult FinalizarEstagioConfirm(Estagio estagio)
        {
            if (ModelState.IsValid)
            {
                var estagiodb = db.Estagio.Find(estagio.EstagioId);
                estagiodb.AvaliacaoED = estagio.AvaliacaoED;
                estagiodb.Estado = Estado.Defesa;
                db.SaveChanges();
                return RedirectToAction("Index");

            }
            return null;

        }

        // GET: Estagios/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Estagio estagio = db.Estagio.Find(id);
            if (estagio == null)
            {
                return HttpNotFound();
            }
            return View(estagio);
        }

        // GET: Estagios/Create
        [Authorize(Roles = "Docente,Administrador,Empresa")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Estagios/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Docente,Administrador,Empresa")]

        public ActionResult Create([Bind(Include = "ID,Nome,Ramo,Enquandramento,Objetivos,CondicoesDeAcesso,Local,Contacto,Estado,Observacoes,User_Id")] Estagio estagio)
        {
            if (ModelState.IsValid)
            {
                estagio.DataDeCriacao = DateTime.Now;
                string currentUserId = User.Identity.GetUserId();
                estagio.ApplicationUser = db.Users.FirstOrDefault(x => x.Id == currentUserId);
                db.Estagio.Add(estagio);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(estagio);
        }

        // GET: Estagios/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Estagio estagio = db.Estagio.Find(id);
            if (estagio == null)
            {
                return HttpNotFound();
            }
            return View(estagio);
        }

        // POST: Estagios/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Nome,Ramo,Enquandramento,Objetivos,CondicoesDeAcesso,Local,Contacto,Estado,Observacoes,User_Id")] Estagio estagio)
        {
            if (ModelState.IsValid)
            {
                db.Entry(estagio).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(estagio);
        }

        // GET: Estagios/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Estagio estagio = db.Estagio.Find(id);
            if (estagio == null)
            {
                return HttpNotFound();
            }
            return View(estagio);
        }

        // POST: Estagios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Estagio estagio = db.Estagio.Find(id);
            db.Estagio.Remove(estagio);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult ListaOrientandos()
        {
            var Id = User.Identity.GetUserId();
            var candidaturas = db.Candidatura.Include(c => c.Estagio)
                .Where(a => a.Orientador_FK == Id);

            return View(candidaturas.ToList());
        }
    }
}
