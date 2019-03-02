namespace DreamTeam.Project.Common.Repositories
{
    using Sitecore.Mvc.Presentation;

    public interface IModelRepository
    {
        RenderingModel GetModel();
    }
}
