using System.Web.Mvc;

namespace Sample.Controllers
{
    public class HomeController : Controller {

        public ActionResult Index() {
            return View();
        }
        public ActionResult GridViewPartial(bool? resizing) {
            ViewData["resizing"] = resizing;
            return PartialView();
        }
    }
}