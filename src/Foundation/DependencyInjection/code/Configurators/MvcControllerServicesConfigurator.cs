namespace DreamTeam.Foundation.DependencyInjection.Configurators
{
    using System;
    using System.Linq;
    using DreamTeam.Foundation.DependencyInjection.Extensions;
    using Microsoft.Extensions.DependencyInjection;
    using Sitecore.DependencyInjection;

    public class MvcControllerServicesConfigurator : IServicesConfigurator
    {
        public void Configure(IServiceCollection serviceCollection)
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies()
                .Where(assembly => assembly.FullName.StartsWith("DreamTeam.Feature") ||
                    assembly.FullName.StartsWith("DreamTeam.Foundation") ||
                    assembly.FullName.StartsWith("DreamTeam.Project"));
            serviceCollection.AddMvcControllers("DreamTeam.Feature");
            serviceCollection.AddMvcControllers("DreamTeam.Foundation");
            serviceCollection.AddMvcControllers("DreamTeam.Project");
        }
    }
}