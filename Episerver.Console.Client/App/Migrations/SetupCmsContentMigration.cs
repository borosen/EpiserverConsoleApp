using System;
using Episerver.Console.Domain.Pages;
using EPiServer;
using EPiServer.Core;
using EPiServer.DataAccess;
using EPiServer.Security;
using EPiServer.Web;

namespace Episerver.Console.Client.App.Migrations
{
    public class SetupCmsContentMigration : ISetupCmsContentMigration
    {
        private readonly IContentRepository _contentRepository;
        private readonly ISiteDefinitionRepository _siteDefinitionRepository;

        public SetupCmsContentMigration(IContentRepository contentRepository, ISiteDefinitionRepository siteDefinitionRepository)
        {
            _contentRepository = contentRepository;
            _siteDefinitionRepository = siteDefinitionRepository;
        }

        public void Dump()
        {
            var startPage = _contentRepository.GetDefault<StartPage>(ContentReference.RootPage);
            startPage.Name = "Start";
            var contentLink = _contentRepository.Save(startPage, SaveAction.Publish, AccessLevel.NoAccess);

            var site = new SiteDefinition
            {
                Name = "Start",
                StartPage = contentLink,
                SiteUrl = new Uri("http://localhost/")
            };

            site.Hosts.Add(new HostDefinition { Name = site.SiteUrl.Authority });
            site.Hosts.Add(new HostDefinition { Name = SiteDefinition.WildcardHostName });
            _siteDefinitionRepository.Save(site);
        }
    }
}