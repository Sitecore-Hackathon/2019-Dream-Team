namespace DreamTeam.Foundation.AccessibilityChecker.Controllers
{
    using System.Web.Mvc;

    [Route("AccessibilityChecker")]
    public class AccessibilityCheckerController : Controller
    {
        [ActionName("Validate")]
        public ActionResult Validate()
        {
            return this.View();
        }
    }
}