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
    public class MensagensController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            return View();
        }


        // GET: Mensagem Recebidas
        public ActionResult Recebidas()
        {
            var Id = User.Identity.GetUserId();
            var mensagens = db.Mensagems.Where(d => d.Recebido_Id == Id)
                .Include(q => q.Enviado);


            return View(mensagens.ToList());
        }
        // GET: Mensagem Enviadas
        public ActionResult Enviadas()
        {
            var Id = User.Identity.GetUserId();
            var mensagens = db.Mensagems.Where(d => d.Enviado_Id == Id).Include(q => q.Recebido);
            return View(mensagens.ToList());
        }
        // GET: Mensagem/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Mensagem mensagem = db.Mensagems.Find(id);
            if (mensagem == null)
            {
                return HttpNotFound();
            }
            return View(mensagem);
        }

        // GET: Mensagem/Create
        public ActionResult Create(String mail)
        {
            if (mail == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var myid = User.Identity.GetUserId();

            Mensagem mensagem = new Mensagem
            {
                Enviado = db.Users.FirstOrDefault( x => x.Id == myid),
                Recebido = db.Users.FirstOrDefault(x => x.Email == mail),
        };
            mensagem.Enviado_Id = mensagem.Enviado.Id;
            mensagem.Recebido_Id = mensagem.Recebido.Id;

            return View(mensagem);
        }

        // POST: Mensagem/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MensagemID,Titulo,Conteudo,Enviado_Id,Recebido_Id,Recebido,Enviado")] Mensagem mensagem)
        {
            if (ModelState.IsValid)
            {
                
                mensagem.DataDeCriacao = DateTime.Now;
                db.Mensagems.Add(mensagem);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(mensagem);
        }

        // GET: Mensagem/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Mensagem mensagem = db.Mensagems.Find(id);
            if (mensagem == null)
            {
                return HttpNotFound();
            }
            return View(mensagem);
        }

        // POST: Mensagem/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MensagemID,Titulo,Conteudo,Enviado_Id,Recebido_Id")] Mensagem mensagem)
        {
            if (ModelState.IsValid)
            {
                db.Entry(mensagem).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(mensagem);
        }

        // GET: Mensagem/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Mensagem mensagem = db.Mensagems.Find(id);
            if (mensagem == null)
            {
                return HttpNotFound();
            }
            return View(mensagem);
        }

        // POST: Mensagem/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Mensagem mensagem = db.Mensagems.Find(id);
            db.Mensagems.Remove(mensagem);
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
    }
}
