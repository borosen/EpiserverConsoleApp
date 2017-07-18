using Episerver.Console.Client.App.Migrations;

namespace Episerver.Console.Client
{
    public class Application : IApplication
    {
        private readonly ISetupCmsContentMigration _setupCmsContentMigration;
        private readonly ISetupCommerceContentMigration _setupCommerceContentMigration;

        public Application(ISetupCmsContentMigration setupCmsContentMigration, ISetupCommerceContentMigration setupCommerceContentMigration)
        {
            _setupCmsContentMigration = setupCmsContentMigration;
            _setupCommerceContentMigration = setupCommerceContentMigration;
        }

        public void Run()
        {
            _setupCmsContentMigration.Dump();
            _setupCommerceContentMigration.Dump();
        }
    }
}