using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace PWEB_TP.Models
{
    public class CandidaturasController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        protected UserManager<ApplicationUser> UserManager { get; set; }

 
        public ActionResult Index()
        {
            var candidatura = db.Candidatura.Include(c => c.Estagio);
            return View(candidatura.ToList());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Candidatura candidatura = db.Candidatura.Find(id);
            if (candidatura == null)
            {
                return HttpNotFound();
            }
            return View(candidatura);
        }

        [Authorize(Roles = "Aluno")]
        public ActionResult Create()
        {
            var createModel = new CreateCandidaturaViewModel
            {
                Disciplinas = db.Disciplina
                 .Select(m => new DisciplinasViewModel { Nome = m.Nome })
                 .ToList()

            };
            List<SelectListItem> list = new List<SelectListItem>();
            for (var lista = 10; lista <= 20; lista++)
                list.Add(new SelectListItem() { Value = lista.ToString(), Text = lista.ToString() });
            list.Add(new SelectListItem() { Value ="Por Concluir", Text = "Por Concluir" });
            list.Add(new SelectListItem() { Value = "Não inscrito" , Text = "Não inscrito"});
            list.Add(new SelectListItem() { Value = "Á espera de nota", Text = "Á espera de nota" });
        
      
            ViewBag.Classificacao = list;
            ViewBag.EstagioId = new SelectList(db.Estagio, "EstagioId", "Nome");
            return View(createModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CandidaturaId,Ramo,Disciplinas,Importancia,EstagioId,User_Id")] Candidatura candidatura, CreateCandidaturaViewModel candid)
        {
            if (ModelState.IsValid)
            {
                string currentUserId = User.Identity.GetUserId();
                candidatura.User_Id = currentUserId;
                candidatura.Disciplinas += candid.Disciplinas[0].Nome + ":" + candid.Disciplinas[0].Classificacao;
                for (int i = 1; i < candid.Disciplinas.Count; i++)
                {
                    candidatura.Disciplinas += "," + candid.Disciplinas[i].Nome;
                    candidatura.Disciplinas += ":" + candid.Disciplinas[i].Classificacao;
                }
                db.Candidatura.Add(candidatura);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            
            ViewBag.EstagioId = new SelectList(db.Estagio, "EstagioId", "Nome", candidatura.EstagioId);
            return View(candidatura);
        }

        [Authorize(Roles = "Administrador")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Candidatura candidatura = db.Candidatura.Find(id);
            if (candidatura == null)
            {
                return HttpNotFound();
            }
            ViewBag.EstagioId = new SelectList(db.Estagio, "EstagioId", "Nome");
            return View(candidatura);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public ActionResult Edit([Bind(Include = "Id,Ramo,Disciplinas,Importancia,EstagioId")] Candidatura candidatura)
        {
            if (ModelState.IsValid)
            {
                db.Entry(candidatura).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.EstagioId = new SelectList(db.Estagio, "EstagioId", "Nome", candidatura.EstagioId);
            return View(candidatura);
        }

        [Authorize(Roles = "Administrador")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Candidatura candidatura = db.Candidatura.Find(id);
            if (candidatura == null)
            {
                return HttpNotFound();
            }
            return View(candidatura);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public ActionResult DeleteConfirmed(int id)
        {
            Candidatura candidatura = db.Candidatura.Find(id);
            db.Candidatura.Remove(candidatura);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult MinhasCandidaturas()
        {
            var Id = User.Identity.GetUserId();
            var candidatura = db.Candidatura.Include(c => c.Estagio)
                .Where(a => a.User_Id == Id);

            return View(candidatura.ToList());


        }
        
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
