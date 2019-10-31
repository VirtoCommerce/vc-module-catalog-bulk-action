namespace VirtoCommerce.CatalogBulkActionsModule.Tests
{
    using System.Collections.Generic;

    using FluentAssertions;

    using Moq;

    using VirtoCommerce.BulkActionsModule.Core;
    using VirtoCommerce.CatalogBulkActionsModule.Data.Actions.PropertiesUpdate;
    using VirtoCommerce.CatalogBulkActionsModule.Data.Services;
    using VirtoCommerce.Domain.Catalog.Model;
    using VirtoCommerce.Domain.Catalog.Services;

    using Xunit;

    public class BulkPropertyUpdateManagerTests
    {
        private readonly PropertiesUpdateBulkActionContext _context;

        private readonly Mock<IDataSource> _dataSource;

        private readonly Mock<IDataSourceFactory> _dataSourceFactory;

        private readonly Mock<IItemService> _itemService;

        private readonly BulkPropertyUpdateManager _manager;

        public BulkPropertyUpdateManagerTests()
        {
            _dataSourceFactory = new Mock<IDataSourceFactory>();
            _itemService = new Mock<IItemService>();
            _manager = new BulkPropertyUpdateManager(_dataSourceFactory.Object, _itemService.Object);
            _context = new PropertiesUpdateBulkActionContext();
            _dataSource = new Mock<IDataSource>();
        }

        [Fact]
        public void GetProperties_Invoke_DataSource_Fetch_Method()
        {
            // arrange
            _dataSourceFactory.Setup(t => t.Create(_context)).Returns(_dataSource.Object);

            // act
            _manager.GetProperties(_context);

            // assert
            _dataSource.Verify(t => t.Fetch());
        }

        [Fact]
        public void GetProperties_Invoke_ItemService_GetByIds_Method()
        {
            // arrange
            var productId = "fakeProductId";
            var group = ItemResponseGroup.ItemInfo | ItemResponseGroup.ItemProperties;
            var productIds = new[] { productId };
            var properties = new List<Property>();
            var product = Mock.Of<CatalogProduct>(
                t => t.Id == productId && t.Properties == properties,
                MockBehavior.Loose);
            var products = new List<CatalogProduct> { product };
            _dataSourceFactory.Setup(t => t.Create(_context)).Returns(_dataSource.Object);
            _dataSource.SetupSequence(t => t.Fetch()).Returns(true).Returns(false);
            _dataSource.Setup(t => t.Items).Returns(products);
            _itemService.Setup(t => t.GetByIds(productIds, group, null)).Returns(products.ToArray());

            // act
            _manager.GetProperties(_context);

            // assert
            _itemService.Verify(t => t.GetByIds(productIds, group, null));
        }

        [Fact]
        public void GetProperties_Result_Should_HaveCount_GreaterThan()
        {
            // arrange
            _dataSourceFactory.Setup(t => t.Create(_context)).Returns(_dataSource.Object);

            // act
            var result = _manager.GetProperties(_context);

            // assert
            result.Should().HaveCountGreaterThan(1);
        }
    }
}