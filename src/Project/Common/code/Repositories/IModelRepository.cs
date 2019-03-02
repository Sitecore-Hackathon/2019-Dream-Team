namespace DreamTeam.Project.Common.Repositories
{
    using DreamTeam.Project.Common.Services;
    using Sitecore.Mvc.Presentation;

    public interface IModelRepository : IService
    {
        RenderingModel GetModel();
    }
}
