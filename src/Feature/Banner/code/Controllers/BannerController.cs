namespace DreamTeam.Feature.Banner.Controllers
{
    using System.Web.Mvc;
    using DreamTeam.Feature.Banner.Repositories;

    public class BannerController : Controller
    {
        private readonly IBannerRepository bannerRepository;

        public BannerController()
        {
            this.bannerRepository = new BannerRepository();
        }

        public ActionResult Index()
        {
            return this.View(
                this.bannerRepository.GetModel());
        }
    }
}