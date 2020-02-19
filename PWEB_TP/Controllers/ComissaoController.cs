using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using PWEB_TP.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace PWEB_TP.Controllers
{

    public class ComissaoController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            return View();
        }

        // GET: AprovarEstagios
        public ActionResult EstagiosPorAprovar()
        {

            var estagio = db.Estagio.Where(m => m.Estado == 0).ToList();
            if (estagio == null)
            {
                return HttpNotFound();
            }
            return View(estagio);
        }
        // GET: Comissao/AvaliarEstagio/id
        public ActionResult AvaliarEstagio(int? id)
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
        // POST: Comissao/AvaliarEstagio/id
        [HttpPost, ActionName("AvaliarEstagio")]
        [ValidateAntiForgeryToken]
        public ActionResult AvaliarEstagioConfirm(int? id, string SubmitButton, Estagio estagio)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (ModelState.IsValid)
            {
                Estado estadoAux;
                Estagio EstagioaAux = db.Estagio.First(m => m.EstagioId == id);
                if (SubmitButton == "Aprovado")
                    estadoAux = Estado.Aprovado;
                else estadoAux = Estado.Recusado;
                EstagioaAux.Estado = estadoAux;
                EstagioaAux.Observacoes = estagio.Observacoes;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(estagio);
        }

        // GET: Comissao
        public ActionResult CandidaturasPorAprovar()
        {

            var candidaturaList = db.Candidatura.Where(m => m.Estado == 0).ToList();
            if (candidaturaList == null)
            {
                return HttpNotFound();
            }
            return View(candidaturaList);
        }

        // GET: Estagios1/Edit/5
        public ActionResult AvaliarCandidatura(int? id)
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
        [HttpPost, ActionName("AvaliarCandidatura")]
        [ValidateAntiForgeryToken]
        public ActionResult AvaliarCandidaturaConfirm(int? id, string SubmitButton, Candidatura model)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (ModelState.IsValid)
            {
                Estado estadoAux;
                Candidatura candidaturaAux = db.Candidatura.First(m => m.CandidaturaId == id);
                if (SubmitButton == "Aprovado")
                    estadoAux = Estado.Aprovado;
                else estadoAux = Estado.Recusado;
                candidaturaAux.Estado = estadoAux;
                candidaturaAux.Observacoes = model.Observacoes;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(model);
        }


        // GET: Estagios1/Edit/5
        public ActionResult EscolherEstagioParaEnviarParaEntrevista()
        {
            var estagios = db.Estagio.Where(d => d.Estado == Estado.Aprovado);

            return View(estagios.ToList());
        }
        // GET: Estagios1/Edit/5
        public ActionResult EnviarParaEntrevista(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var candidaturas = db.Candidatura
            .Where(q => q.EstagioId == id)
            .Where(q => q.Estado == Estado.Aprovado);

            return View(candidaturas.ToList());
        }
        [HttpPost, ActionName("EnviarParaEntrevista")]
        [ValidateAntiForgeryToken]
        public ActionResult EnviarParaEntrevistaConfirm(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (ModelState.IsValid)
            {
                Candidatura candidaturaAux = db.Candidatura.First(m => m.CandidaturaId == id);
                candidaturaAux.Estado = Estado.Entrevista;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        // GET: AprovarEstagios
        public ActionResult EstagiosPorAtribuir()
        {

            var estagio = db.Estagio.Where(m => m.Estado == Estado.Entrevista || m.Estado == Estado.Aprovado).ToList();
            if (estagio == null)
            {
                return HttpNotFound();
            }
            return View(estagio);
        }
        // GET: AprovarEstagios
        public ActionResult CandidaturasPorAtribuir(int? id)
        {

            var candidaturas = db.Candidatura
                .Where(m => m.EstagioId == id)
                .Where(m => m.Estado == Estado.Entrevista || m.Estado == Estado.Aprovado)
                .ToList();
            if (candidaturas == null)
            {
                return HttpNotFound();
            }
            return View(candidaturas);
        }
        // GET: Comissao/AvaliarEstagio/id
        public ActionResult AtribuirCandidatura(int? id)
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
            var roleDocente = db.Roles.Where(m => m.Name == "Docente").Select(m => m.Id).SingleOrDefault();
            var listaDocentes = db.Users
              .Where(u => u.Roles.Any(r => r.RoleId == roleDocente))
              .Select(m => new { Value = m.Id, Text = m.UserName }).ToList();

            ViewBag.docentes = new SelectList(listaDocentes, "Value", "Text"); ;
            return View(candidatura);
        }
        // POST: Comissao/AvaliarEstagio/id
        [HttpPost, ActionName("AtribuirCandidatura")]
        [ValidateAntiForgeryToken]
        public ActionResult AtribuirCandidaturaConfirm(int? id, Candidatura candidatura)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (ModelState.IsValid)
            {

                Candidatura CandidaturaAux = db.Candidatura.First(m => m.CandidaturaId == id);
                CandidaturaAux.Estado = Estado.Atribuido;
                CandidaturaAux.Orientador_FK = candidatura.Orientador_FK;
                db.SaveChanges();

                Estagio estagioAux = db.Estagio.First(m => m.EstagioId == CandidaturaAux.EstagioId);
                estagioAux.Estado = Estado.Atribuido;
                estagioAux.Aluno = CandidaturaAux.ApplicationUser;
                db.SaveChanges();

                List<Candidatura> listaCandidatura = db.Candidatura.Where(m => m.EstagioId == estagioAux.EstagioId).ToList();
                foreach (var candidaturas in listaCandidatura)
                {
                    if (candidaturas.Estado != Estado.Atribuido)
                    {
                        candidaturas.Estado = Estado.NãoAtribuido;
                        db.SaveChanges();
                    }
                }
                return RedirectToAction("Index");
            }
            return View(candidatura);
        }
        // GET: Comissao/AvaliarEstagio/id
        public ActionResult EstagiosDefesasEConcluir()
        {
            var estagios = db.Estagio.Where(d => d.Estado == Estado.Defesa).ToList();
            return View(estagios);
        }
        // POST: Comissao/AvaliarEstagio/id
        [HttpPost, ActionName("EstagiosDefesasEConcluir")]
        [ValidateAntiForgeryToken]
        public ActionResult EstagiosDefesasEConcluirConfirm(int? id)
        {
            if (ModelState.IsValid)
            {
                var estagio = db.Estagio.Find(id);
                estagio.Estado = Estado.Concluido;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            var estagios = db.Estagio.Where(d => d.Estado == Estado.Defesa).ToList();
            return View(estagios);
        }
        // GET: Comissao/AvaliarEstagio/id
        public ActionResult EscolherDataDefesaEstagio(int? id)
        {
            Estagio estagios = db.Estagio.Find(id);
            estagios.DataDaDefesa = DateTime.Now;
            return View(estagios);
        }
        // POST: Comissao/AvaliarEstagio/id
        [HttpPost, ActionName("EscolherDataDefesaEstagio")]
        [ValidateAntiForgeryToken]
        public ActionResult EscolherDataDefesaEstagioConfirm(Estagio estagio)
        {
            if (ModelState.IsValid)
            {
                var estagiodb = db.Estagio.Find(estagio.EstagioId);
                estagiodb.DataDaDefesa = estagio.DataDaDefesa;
              //  estagio.Estado = Estado.Concluido;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(estagio);
        }
    }

}
