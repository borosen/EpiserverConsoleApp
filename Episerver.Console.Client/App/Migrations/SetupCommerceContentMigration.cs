using System;
using System.Linq;
using EPiServer;
using EPiServer.Commerce.Catalog.ContentTypes;
using EPiServer.Commerce.SpecializedProperties;
using EPiServer.DataAbstraction;
using EPiServer.DataAccess;
using EPiServer.Security;
using Mediachase.Commerce;
using Mediachase.Commerce.Catalog;
using Mediachase.Commerce.Core;
using Mediachase.Commerce.Markets;
using Mediachase.Commerce.Pricing;

namespace Episerver.Console.Client.App.Migrations
{
    public class SetupCommerceContentMigration : ISetupCommerceContentMigration
    {
        private readonly IContentRepository _contentRepository;
        private readonly ReferenceConverter _referenceConverter;
        private readonly ILanguageBranchRepository _languageBranchRepository;
        private readonly ICurrentMarket _currentMarket;
        private readonly IMarketService _marketService;
        private readonly IPriceService _priceService;

        public SetupCommerceContentMigration(IContentRepository contentRepository,
            ReferenceConverter referenceConverter,
            ILanguageBranchRepository languageBranchRepository,
            ICurrentMarket currentMarket,
            IMarketService marketService,
            IPriceService priceService)
        {
            _contentRepository = contentRepository;
            _referenceConverter = referenceConverter;
            _languageBranchRepository = languageBranchRepository;
            _currentMarket = currentMarket;
            _marketService = marketService;
            _priceService = priceService;
        }

        public void Dump()
        {
            var applicationId = AppContext.Current.ApplicationId;
            var languageBranch = _languageBranchRepository.ListAll().First();

            var currentMarket = _currentMarket.GetCurrentMarket();
            var market = (MarketImpl)_marketService.GetMarket(currentMarket.MarketId);
            market.DefaultCurrency = Currency.EUR;
            market.DefaultLanguage = languageBranch.Culture;
            _marketService.UpdateMarket(market);

            var rootLink = _referenceConverter.GetRootLink();
            var catalog = _contentRepository.GetDefault<CatalogContent>(rootLink, languageBranch.Culture);
            catalog.Name = "Catalog";
            catalog.DefaultCurrency = market.DefaultCurrency;
            catalog.CatalogLanguages = new ItemCollection<string> { languageBranch.LanguageID };
            catalog.DefaultLanguage = "en";
            catalog.WeightBase = "kg";
            catalog.LengthBase = "cm";
            var catalogRef = _contentRepository.Save(catalog, SaveAction.Publish, AccessLevel.NoAccess);

            var category = _contentRepository.GetDefault<NodeContent>(catalogRef);
            category.Name = "Category";
            category.DisplayName = "Category";
            category.Code = "category";
            var categoryRef = _contentRepository.Save(category, SaveAction.Publish, AccessLevel.NoAccess);

            var product = _contentRepository.GetDefault<ProductContent>(categoryRef);
            product.Name = "Product";
            product.DisplayName = "Product";
            product.Code = "product";
            var productRef = _contentRepository.Save(product, SaveAction.Publish, AccessLevel.NoAccess);

            var variant = _contentRepository.GetDefault<VariationContent>(productRef);
            variant.Name = "Variant";
            variant.DisplayName = "Variant";
            variant.Code = "test";
            variant.MinQuantity = 1;
            variant.MaxQuantity = 100;
            _contentRepository.Save(variant, SaveAction.Publish, AccessLevel.NoAccess);

            var price = new PriceValue
            {
                UnitPrice = new Money(100, market.DefaultCurrency),
                CatalogKey = new CatalogKey(applicationId, variant.Code),
                MarketId = market.MarketId,
                ValidFrom = DateTime.Today.AddYears(-1),
                ValidUntil = DateTime.Today.AddYears(1),
                CustomerPricing = CustomerPricing.AllCustomers,
                MinQuantity = 0
            };

            _priceService.SetCatalogEntryPrices(price.CatalogKey, new[] { price });
        }
    }
}