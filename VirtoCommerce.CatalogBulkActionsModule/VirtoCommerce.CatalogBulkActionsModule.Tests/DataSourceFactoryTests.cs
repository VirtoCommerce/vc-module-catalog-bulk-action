namespace VirtoCommerce.CatalogBulkActionsModule.Tests
{
    using System;

    using FluentAssertions;

    using Moq;

    using VirtoCommerce.BulkActionsModule.Core;
    using VirtoCommerce.CatalogBulkActionsModule.Core.Models;
    using VirtoCommerce.CatalogBulkActionsModule.Data.Actions;
    using VirtoCommerce.CatalogBulkActionsModule.Data.Actions.CategoryChange;
    using VirtoCommerce.CatalogBulkActionsModule.Data.Actions.PropertiesUpdate;
    using VirtoCommerce.CatalogBulkActionsModule.Data.DataSources;
    using VirtoCommerce.CatalogModule.Web.Services;

    using Xunit;

    public class DataSourceFactoryTests
    {
        private readonly IDataSourceFactory _dataSourceFactory;

        public DataSourceFactoryTests()
        {
            var searchService = new Mock<IListEntrySearchService>();
            _dataSourceFactory = new DataSourceFactory(searchService.Object);
        }

        [Fact]
        public void Create_ShouldBeCreated_BaseDataSource()
        {
            // arrange
            var dataQuery = new Mock<DataQuery> { DefaultValueProvider = DefaultValueProvider.Mock };
            var context = new CategoryChangeBulkActionContext { DataQuery = dataQuery.Object };

            // act
            var result = _dataSourceFactory.Create(context);

            // assert
            result.Should().BeOfType<BaseDataSource>();
        }

        [Fact]
        public void Create_ShouldBeCreated_ProductDataSource()
        {
            // arrange
            var dataQuery = new Mock<DataQuery> { DefaultValueProvider = DefaultValueProvider.Mock };
            var context = new PropertiesUpdateBulkActionContext { DataQuery = dataQuery.Object };

            // act
            var result = _dataSourceFactory.Create(context);

            // assert
            result.Should().BeOfType<ProductDataSource>();
        }

        [Fact]
        public void Create_ShouldThrow_ArgumentException()
        {
            // arrange
            var context = new BaseBulkActionContext();

            // act
            var action = new Action(
                () =>
                {
                    _dataSourceFactory.Create(context);
                });

            // assert
            action.Should().Throw<ArgumentException>();
        }
    }
}