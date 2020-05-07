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
        [Fact]
        public void Create_EmptyContext_ThrowArgumentException()
        {
            // arrange
            var dataSourceFactory = BuildDataSourceFactory();
            var context = new BaseBulkActionContext();

            // act
            var action = new Action(
                () =>
                {
                    dataSourceFactory.Create(context);
                });

            // assert
            action.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void Create_Result_BaseDataSource()
        {
            // arrange
            var dataSourceFactory = BuildDataSourceFactory();
            var dataQuery = new Mock<DataQuery> { DefaultValueProvider = DefaultValueProvider.Mock };
            var context = new CategoryChangeBulkActionContext { DataQuery = dataQuery.Object };

            // act
            var result = dataSourceFactory.Create(context);

            // assert
            result.Should().BeOfType<BaseDataSource>();
        }

        [Fact]
        public void Create_Result_ProductDataSource()
        {
            // arrange
            var dataSourceFactory = BuildDataSourceFactory();
            var dataQuery = new Mock<DataQuery> { DefaultValueProvider = DefaultValueProvider.Mock };
            var context = new PropertiesUpdateBulkActionContext { DataQuery = dataQuery.Object };

            // act
            var result = dataSourceFactory.Create(context);

            // assert
            result.Should().BeOfType<ProductDataSource>();
        }

        private IDataSourceFactory BuildDataSourceFactory()
        {
            var searchService = new Mock<IListEntrySearchService>();
            return new DataSourceFactory(searchService.Object);
        }
    }
}