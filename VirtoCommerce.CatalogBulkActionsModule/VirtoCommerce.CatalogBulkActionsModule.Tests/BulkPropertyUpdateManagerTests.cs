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
        [Fact]
        public void GetProperties_DataSource_InvokeFetch()
        {
            // arrange
            var context = new PropertiesUpdateBulkActionContext();
            var dataSource = new Mock<IDataSource>();
            var dataSourceFactory = new Mock<IDataSourceFactory>();
            var manager = BuildManager(dataSourceFactory);
            dataSourceFactory.Setup(t => t.Create(context)).Returns(dataSource.Object);

            // act
            manager.GetProperties(context);

            // assert
            dataSource.Verify(t => t.Fetch());
        }

        [Fact]
        public void GetProperties_ItemService_InvokeGetByIds()
        {
            // arrange
            var context = new PropertiesUpdateBulkActionContext();
            var dataSourceFactory = new Mock<IDataSourceFactory>();
            var itemService = new Mock<IItemService>();
            var manager = BuildManager(dataSourceFactory, itemService);
            var dataSource = new Mock<IDataSource>();
            var productId = "fakeProductId";
            var group = ItemResponseGroup.ItemInfo | ItemResponseGroup.ItemProperties;
            var productIds = new[] { productId };
            var properties = new List<Property>();
            var product = Mock.Of<CatalogProduct>(
                t => t.Id == productId && t.Properties == properties,
                MockBehavior.Loose);
            var products = new List<CatalogProduct> { product };
            dataSourceFactory.Setup(t => t.Create(context)).Returns(dataSource.Object);
            dataSource.SetupSequence(t => t.Fetch()).Returns(true).Returns(false);
            dataSource.Setup(t => t.Items).Returns(products);
            itemService.Setup(t => t.GetByIds(productIds, group, null)).Returns(products.ToArray());

            // act
            manager.GetProperties(context);

            // assert
            itemService.Verify(t => t.GetByIds(productIds, group, null));
        }

        [Theory]
        [InlineData(1)]
        public void GetProperties_Should_HaveCountGreaterThan(int count)
        {
            // arrange
            var context = new PropertiesUpdateBulkActionContext();
            var dataSource = new Mock<IDataSource>();
            var dataSourceFactory = new Mock<IDataSourceFactory>();
            var manager = BuildManager(dataSourceFactory);
            dataSourceFactory.Setup(t => t.Create(context)).Returns(dataSource.Object);

            // act
            var result = manager.GetProperties(context);

            // assert
            result.Should().HaveCountGreaterThan(count);
        }

        private IBulkPropertyUpdateManager BuildManager()
        {
            var dataSourceFactory = new Mock<IDataSourceFactory>();
            var itemService = new Mock<IItemService>();
            var manager = new BulkPropertyUpdateManager(dataSourceFactory.Object, itemService.Object);
            return manager;
        }

        private IBulkPropertyUpdateManager BuildManager(IMock<IDataSourceFactory> dataSourceFactory)
        {
            var itemService = new Mock<IItemService>();
            var manager = new BulkPropertyUpdateManager(dataSourceFactory.Object, itemService.Object);
            return manager;
        }

        private IBulkPropertyUpdateManager BuildManager(
            IMock<IDataSourceFactory> dataSourceFactory,
            IMock<IItemService> itemService)
        {
            var manager = new BulkPropertyUpdateManager(dataSourceFactory.Object, itemService.Object);
            return manager;
        }
    }
}