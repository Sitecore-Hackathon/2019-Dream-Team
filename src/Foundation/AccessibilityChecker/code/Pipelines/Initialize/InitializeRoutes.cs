namespace DreamTeam.Foundation.AccessibilityChecker.Pipelines.Initialize
{
    using System.Web.Mvc;
    using System.Web.Routing;
    using Sitecore.Pipelines;

    public class InitializeRoutes : Sitecore.Mvc.Pipelines.Loader.InitializeRoutes
    {
        public override void Process(PipelineArgs args)
        {
            this.RegisterRoutes(RouteTable.Routes, args);
        }

        protected override void RegisterRoutes(RouteCollection routes, PipelineArgs args)
        {
            routes.MapRoute(
                "AccessibilityChecker",
                "axe",
                new { controller = "AccessibilityChecker", action = "Validate" });
        }
    }
}