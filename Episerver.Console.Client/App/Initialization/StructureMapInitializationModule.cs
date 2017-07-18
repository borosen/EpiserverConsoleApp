using Episerver.Console.Client.App.Migrations;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.ServiceLocation;
using Mediachase.Commerce.Initialization;

namespace Episerver.Console.Client.App.Initialization
{
    [ModuleDependency(typeof(CommerceInitialization))]
    public class StructureMapInitializationModule : IConfigurableModule
    {
        public void ConfigureContainer(ServiceConfigurationContext context)
        {
            context.Services
                .AddTransient<ISetupCmsContentMigration, SetupCmsContentMigration>()
                .AddTransient<ISetupCommerceContentMigration, SetupCommerceContentMigration>()
                .AddTransient<IApplication, Application>();

            Program.Container = context.StructureMap();
        }

        public void Initialize(InitializationEngine context)
        {
        }

        public void Uninitialize(InitializationEngine context)
        {
        }
    }
}