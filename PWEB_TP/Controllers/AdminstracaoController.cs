using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using PWEB_TP.Models;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
namespace PWEB_TP.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class AdminstracaoController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
    
        public ActionResult Index()
        {
            return View();
        }
        // GET: Adminstracao/ComissaoDeEstagios
        public ActionResult ComissaoDeEstagios()
        {
            var roleComissao = db.Roles.Where(m => m.Name == "ComissaoEstagios").Select(m => m.Id).SingleOrDefault();
            var roleDocente = db.Roles.Where(m => m.Name == "Docente").Select(m => m.Id).SingleOrDefault();
            var viewModel = new ComissaoDeEstagiosViewModel
            {
                Comissao = db.Users
                .Where(u => u.Roles.Any(r => r.RoleId == roleComissao))
                .Select(m => new AdminstracaoViewModel { Id = m.Id, UserName = m.UserName, Email = m.Email })
                .ToList()

            };
            var NaoComissaoList = db.Users
              .Where(u => u.Roles.Any(r => r.RoleId != roleComissao))
              .Where(u => u.Roles.Any(r => r.RoleId == roleDocente))
              .Select(m => new { Value = m.Id, Text = m.UserName }).ToList();

            ViewBag.NaoComissao = new SelectList(NaoComissaoList, "Value", "Text"); ;
            return View(viewModel);
        }

        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ComissaoDeEstagios(ComissaoDeEstagiosViewModel model)
        {

            if (ModelState.IsValid && model.escolhido != null)
            {
                await Request.GetOwinContext().GetUserManager<ApplicationUserManager>().AddToRoleAsync(model.escolhido, "ComissaoEstagios");

            }
            // If we got this far, something failed, redisplay form
            return RedirectToAction("ComissaoDeEstagios");
        }
        // GET: DeleteDocente/5
        public ActionResult ComissaoDeEstagiosRemove(string id)
        {
            if (id.Equals(""))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser user = db.Users.Where(m => m.Id.Equals(id)).SingleOrDefault();
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }


        // POST: /DeleteDocente
        [HttpPost, ActionName("ComissaoDeEstagiosRemove")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ComissaoDeEstagiosRemoveConfirmacao(string id)
        {

            if (ModelState.IsValid)
            {
                await Request.GetOwinContext().GetUserManager<ApplicationUserManager>().RemoveFromRoleAsync(id, "ComissaoEstagios");

            }
            return RedirectToAction("ComissaoDeEstagios");
        }
    }
}