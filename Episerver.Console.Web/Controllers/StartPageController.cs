using System.Web.Mvc;
using Episerver.Console.Domain.Pages;
using EPiServer.Web.Mvc;

namespace Episerver.Console.Web.Controllers
{
    public class StartPageController : PageController<StartPage>
    {
        public ActionResult Index(StartPage currentPage)
        {
            return Content(currentPage.Name);
        }
    }
}